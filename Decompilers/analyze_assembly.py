#!/usr/bin/env python3
"""
.NET Assembly Metadata Reader
Reads type definitions, methods, and namespaces from .NET PE assemblies
by parsing the CLI metadata tables directly.
"""

import struct
import sys
import os


class DotNetMetadataReader:
    def __init__(self, filepath):
        with open(filepath, 'rb') as f:
            self.data = f.read()
        self.filepath = filepath
        self.sections = []
        self.metadata_rva = 0
        self.metadata_size = 0
        self.streams = {}
        self.string_heap = b''
        self.guid_heap = b''
        self.blob_heap = b''
        self.us_heap = b''  # User strings
        self.tables_data = b''
        self.table_rows = {}
        self.tables = {}
        
    def read_u16(self, offset):
        return struct.unpack_from('<H', self.data, offset)[0]
    
    def read_u32(self, offset):
        return struct.unpack_from('<I', self.data, offset)[0]
    
    def read_u64(self, offset):
        return struct.unpack_from('<Q', self.data, offset)[0]
    
    def rva_to_offset(self, rva):
        """Convert RVA to file offset using section table"""
        for sec in self.sections:
            if sec['va'] <= rva < sec['va'] + sec['vs']:
                return rva - sec['va'] + sec['raw_offset']
        return rva  # fallback
    
    def read_string_heap(self, index):
        """Read a string from the #Strings heap"""
        if index >= len(self.string_heap):
            return '<invalid>'
        end = self.string_heap.index(b'\x00', index)
        return self.string_heap[index:end].decode('utf-8', errors='replace')
    
    def parse(self):
        # PE header
        pe_offset = self.read_u32(60)
        
        # COFF header
        num_sections = self.read_u16(pe_offset + 6)
        opt_header_size = self.read_u16(pe_offset + 20)
        
        # Optional header
        opt_start = pe_offset + 24
        pe_magic = self.read_u16(opt_start)
        is_pe32plus = pe_magic == 0x20b
        
        # CLI header data directory (index 14)
        if is_pe32plus:
            dd_offset = opt_start + 112
        else:
            dd_offset = opt_start + 96
        
        cli_rva = self.read_u32(dd_offset + 14 * 8)
        cli_size = self.read_u32(dd_offset + 14 * 8 + 4)
        
        # Parse section table
        section_table_start = opt_start + opt_header_size
        for i in range(num_sections):
            sec_offset = section_table_start + i * 40
            name = self.data[sec_offset:sec_offset+8].rstrip(b'\x00').decode('ascii', errors='replace')
            vs = self.read_u32(sec_offset + 8)
            va = self.read_u32(sec_offset + 12)
            raw_size = self.read_u32(sec_offset + 16)
            raw_offset = self.read_u32(sec_offset + 20)
            self.sections.append({
                'name': name,
                'vs': vs,
                'va': va,
                'raw_size': raw_size,
                'raw_offset': raw_offset,
            })
        
        # Read CLI header
        cli_offset = self.rva_to_offset(cli_rva)
        cb = self.read_u32(cli_offset)
        major = self.read_u16(cli_offset + 4)
        minor = self.read_u16(cli_offset + 6)
        self.metadata_rva = self.read_u32(cli_offset + 8)
        self.metadata_size = self.read_u32(cli_offset + 12)
        flags = self.read_u32(cli_offset + 16)
        entry_token = self.read_u32(cli_offset + 20)
        
        # Parse metadata root
        md_offset = self.rva_to_offset(self.metadata_rva)
        md_sig = self.read_u32(md_offset)
        assert md_sig == 0x424A5342, f"Bad metadata signature: 0x{md_sig:08x}"
        
        md_major = self.read_u16(md_offset + 4)
        md_minor = self.read_u16(md_offset + 6)
        version_len = self.read_u32(md_offset + 12)
        version = self.data[md_offset+16:md_offset+16+version_len].rstrip(b'\x00').decode('utf-8', errors='replace')
        
        # Stream headers
        flags_offset = md_offset + 16 + version_len
        stream_flags = self.read_u16(flags_offset)
        num_streams = self.read_u16(flags_offset + 2)
        
        pos = flags_offset + 4
        for i in range(num_streams):
            stream_offset = self.read_u32(pos)
            stream_size = self.read_u32(pos + 4)
            pos += 8
            # Read null-terminated name, padded to 4 bytes
            name_start = pos
            while self.data[pos] != 0:
                pos += 1
            name = self.data[name_start:pos].decode('ascii')
            pos += 1
            # Align to 4 bytes
            pos = (pos + 3) & ~3
            
            abs_offset = md_offset + stream_offset
            self.streams[name] = {
                'offset': abs_offset,
                'size': stream_size,
            }
            
            if name == '#Strings':
                self.string_heap = self.data[abs_offset:abs_offset+stream_size]
            elif name == '#GUID':
                self.guid_heap = self.data[abs_offset:abs_offset+stream_size]
            elif name == '#Blob':
                self.blob_heap = self.data[abs_offset:abs_offset+stream_size]
            elif name == '#US':
                self.us_heap = self.data[abs_offset:abs_offset+stream_size]
            elif name == '#~':
                self.tables_data = self.data[abs_offset:abs_offset+stream_size]
        
        # Parse #~ stream (tables)
        self._parse_tables_stream()
        
        return version
    
    def _parse_tables_stream(self):
        """Parse the #~ metadata tables stream"""
        data = self.tables_data
        
        # Header
        reserved = self.read_u32(0)  # from tables_data perspective, use struct
        major = data[4]
        minor = data[5]
        heap_sizes = data[6]
        reserved2 = data[7]
        valid_tables = struct.unpack_from('<Q', data, 8)[0]
        sorted_tables = struct.unpack_from('<Q', data, 16)[0]
        
        self.string_index_size = 4 if (heap_sizes & 1) else 2
        self.guid_index_size = 4 if (heap_sizes & 2) else 2
        self.blob_index_size = 4 if (heap_sizes & 4) else 2
        
        # Row counts for each valid table
        pos = 24
        for i in range(64):
            if valid_tables & (1 << i):
                count = struct.unpack_from('<I', data, pos)[0]
                self.table_rows[i] = count
                pos += 4
            else:
                self.table_rows[i] = 0
        
        self.tables_start = pos
        
        # Now parse the tables we care about
        self._parse_type_defs(data, pos)
    
    def _get_coded_index_size(self, tables, bits):
        """Get size of a coded index based on referenced table row counts"""
        max_rows = max(self.table_rows.get(t, 0) for t in tables)
        if max_rows < (1 << (16 - bits)):
            return 2
        return 4
    
    def _parse_type_defs(self, data, start_pos):
        """Parse TypeDef table (table 0x02) and MethodDef table (table 0x06)"""
        
        # Calculate sizes of all preceding table rows to find TypeDef offset
        # Table order: Module(0), TypeRef(1), TypeDef(2), FieldPtr(3), Field(4), 
        # MethodPtr(5), MethodDef(6), ...
        
        # We need to calculate row sizes for each table to know offsets
        # This is complex - let's calculate the row size for each table type
        
        # TypeDef table (0x02): Flags(4) + TypeName(str) + TypeNamespace(str) + 
        #                        Extends(coded:TypeDefOrRef) + FieldList(index) + MethodList(index)
        
        # TypeDefOrRef coded index: TypeDef(0x02), TypeRef(0x01), TypeSpec(0x1B)
        typedef_or_ref_size = self._get_coded_index_size([0x02, 0x01, 0x1B], 2)
        
        # Field table index size
        field_index_size = 4 if self.table_rows.get(0x04, 0) > 0xFFFF else 2
        # Method table index size
        method_index_size = 4 if self.table_rows.get(0x06, 0) > 0xFFFF else 2
        
        typedef_row_size = (4 + self.string_index_size + self.string_index_size + 
                           typedef_or_ref_size + field_index_size + method_index_size)
        
        # MethodDef table (0x06): RVA(4) + ImplFlags(2) + Flags(2) + Name(str) + 
        #                          Signature(blob) + ParamList(index)
        param_index_size = 4 if self.table_rows.get(0x08, 0) > 0xFFFF else 2
        methoddef_row_size = (4 + 2 + 2 + self.string_index_size + 
                             self.blob_index_size + param_index_size)
        
        # MemberRef table (0x0A) - we won't parse this for now
        
        # Calculate offset to TypeDef table by summing all preceding tables
        # Table sizes for common tables
        table_defs = self._get_table_row_sizes()
        
        pos = start_pos
        
        # Skip tables before TypeDef (0x02)
        for table_id in range(0x02):
            if self.table_rows.get(table_id, 0) > 0:
                row_size = table_defs.get(table_id, 0)
                if row_size == 0:
                    print(f"WARNING: Unknown table 0x{table_id:02x} with {self.table_rows[table_id]} rows")
                    return
                pos += self.table_rows[table_id] * row_size
        
        # Parse TypeDef rows
        self.type_defs = []
        typedef_count = self.table_rows.get(0x02, 0)
        
        for i in range(typedef_count):
            row_start = pos
            flags = struct.unpack_from('<I', data, pos)[0]; pos += 4
            
            if self.string_index_size == 4:
                name_idx = struct.unpack_from('<I', data, pos)[0]; pos += 4
                ns_idx = struct.unpack_from('<I', data, pos)[0]; pos += 4
            else:
                name_idx = struct.unpack_from('<H', data, pos)[0]; pos += 2
                ns_idx = struct.unpack_from('<H', data, pos)[0]; pos += 2
            
            pos += typedef_or_ref_size  # extends
            
            if field_index_size == 4:
                field_list = struct.unpack_from('<I', data, pos)[0]; pos += 4
            else:
                field_list = struct.unpack_from('<H', data, pos)[0]; pos += 2
            
            if method_index_size == 4:
                method_list = struct.unpack_from('<I', data, pos)[0]; pos += 4
            else:
                method_list = struct.unpack_from('<H', data, pos)[0]; pos += 2
            
            name = self.read_string_heap(name_idx)
            namespace = self.read_string_heap(ns_idx)
            
            self.type_defs.append({
                'name': name,
                'namespace': namespace,
                'flags': flags,
                'field_list': field_list,
                'method_list': method_list,
            })
        
        # Now skip to MethodDef table
        methoddef_start = pos
        # Skip tables between TypeDef and MethodDef
        for table_id in range(0x03, 0x06):
            if self.table_rows.get(table_id, 0) > 0:
                row_size = table_defs.get(table_id, 0)
                if row_size == 0:
                    print(f"WARNING: Unknown table 0x{table_id:02x}")
                    return
                methoddef_start += self.table_rows[table_id] * row_size
        
        # Parse MethodDef rows
        self.method_defs = []
        methoddef_count = self.table_rows.get(0x06, 0)
        pos = methoddef_start
        
        for i in range(methoddef_count):
            rva = struct.unpack_from('<I', data, pos)[0]; pos += 4
            impl_flags = struct.unpack_from('<H', data, pos)[0]; pos += 2
            flags = struct.unpack_from('<H', data, pos)[0]; pos += 2
            
            if self.string_index_size == 4:
                name_idx = struct.unpack_from('<I', data, pos)[0]; pos += 4
            else:
                name_idx = struct.unpack_from('<H', data, pos)[0]; pos += 2
            
            pos += self.blob_index_size  # signature
            pos += param_index_size  # param list
            
            name = self.read_string_heap(name_idx)
            
            self.method_defs.append({
                'name': name,
                'rva': rva,
                'impl_flags': impl_flags,
                'flags': flags,
            })
    
    def _get_table_row_sizes(self):
        """Calculate row sizes for all metadata tables"""
        s = self.string_index_size
        g = self.guid_index_size
        b = self.blob_index_size
        
        # Coded index sizes
        typedef_or_ref = self._get_coded_index_size([0x02, 0x01, 0x1B], 2)
        has_constant = self._get_coded_index_size([0x04, 0x08, 0x17], 2)
        has_custom_attr = self._get_coded_index_size([
            0x06, 0x04, 0x01, 0x02, 0x08, 0x09, 0x0A, 0x00,
            0x0E, 0x17, 0x14, 0x11, 0x1A, 0x1B, 0x20, 0x23,
            0x26, 0x27, 0x28, 0x2A, 0x2C, 0x2B
        ], 5)
        has_field_marshal = self._get_coded_index_size([0x04, 0x08], 1)
        has_decl_security = self._get_coded_index_size([0x02, 0x06, 0x20], 2)
        member_ref_parent = self._get_coded_index_size([0x02, 0x01, 0x1A, 0x06, 0x1B], 3)
        has_semantics = self._get_coded_index_size([0x14, 0x17], 1)
        method_def_or_ref = self._get_coded_index_size([0x06, 0x0A], 1)
        member_forwarded = self._get_coded_index_size([0x04, 0x06], 1)
        implementation = self._get_coded_index_size([0x26, 0x23, 0x27], 2)
        custom_attr_type = self._get_coded_index_size([0x02, 0x02, 0x06, 0x0A, 0x02], 3)
        resolution_scope = self._get_coded_index_size([0x00, 0x1A, 0x23, 0x01], 2)
        type_or_method = self._get_coded_index_size([0x02, 0x06], 1)
        
        # Simple table index sizes
        def idx_size(table_id):
            return 4 if self.table_rows.get(table_id, 0) > 0xFFFF else 2
        
        field_idx = idx_size(0x04)
        method_idx = idx_size(0x06)
        param_idx = idx_size(0x08)
        event_idx = idx_size(0x14)
        property_idx = idx_size(0x17)
        typedef_idx = idx_size(0x02)
        generic_param_idx = idx_size(0x2A)
        module_ref_idx = idx_size(0x1A)
        
        sizes = {
            0x00: 2 + s + g + g + g,  # Module
            0x01: resolution_scope + s + s,  # TypeRef
            0x02: 4 + s + s + typedef_or_ref + field_idx + method_idx,  # TypeDef
            0x03: field_idx,  # FieldPtr
            0x04: 2 + s + b,  # Field
            0x05: method_idx,  # MethodPtr
            0x06: 4 + 2 + 2 + s + b + param_idx,  # MethodDef
            0x07: param_idx,  # ParamPtr
            0x08: 2 + 2 + s,  # Param
            0x09: typedef_idx + typedef_or_ref,  # InterfaceImpl
            0x0A: member_ref_parent + s + b,  # MemberRef
            0x0B: has_constant + 2 + b,  # Constant
            0x0C: has_custom_attr + custom_attr_type + b,  # CustomAttribute
            0x0D: has_field_marshal + b,  # FieldMarshal
            0x0E: has_decl_security + 2 + b,  # DeclSecurity
            0x0F: 2 + 4 + typedef_idx,  # ClassLayout
            0x10: 4 + field_idx,  # FieldLayout
            0x11: b,  # StandAloneSig
            0x12: typedef_idx + event_idx,  # EventMap
            0x13: event_idx,  # EventPtr
            0x14: 2 + s + typedef_or_ref,  # Event
            0x15: typedef_idx + property_idx,  # PropertyMap
            0x16: property_idx,  # PropertyPtr
            0x17: 2 + s + b,  # Property
            0x18: has_semantics + method_idx,  # MethodSemantics
            0x19: typedef_idx + method_def_or_ref + method_def_or_ref,  # MethodImpl
            0x1A: s,  # ModuleRef
            0x1B: b,  # TypeSpec
            0x1C: 2 + member_forwarded + s + module_ref_idx,  # ImplMap
            0x1D: 4 + field_idx,  # FieldRVA
            0x20: 4 + s + s + implementation,  # Assembly
            0x21: 4,  # AssemblyProcessor
            0x22: 4 + 4 + 4 + 4,  # AssemblyOS
            0x23: 2 + 2 + 2 + 2 + 4 + b + s + s + b,  # AssemblyRef
            0x24: 4 + 4 + 4 + 4 + b + s + s + b,  # AssemblyRefProcessor (unused usually)
            0x26: 4 + s + b,  # File
            0x27: 4 + typedef_idx + s + implementation,  # ExportedType
            0x28: 4 + 4 + implementation,  # ManifestResource
            0x29: typedef_idx + typedef_or_ref,  # NestedClass
            0x2A: 2 + 2 + type_or_method + s,  # GenericParam
            0x2B: method_def_or_ref + b,  # MethodSpec
            0x2C: generic_param_idx + typedef_or_ref,  # GenericParamConstraint
        }
        
        return sizes
    
    def get_types_with_methods(self):
        """Return a list of types with their methods"""
        if not hasattr(self, 'type_defs') or not hasattr(self, 'method_defs'):
            return []
        
        results = []
        for i, td in enumerate(self.type_defs):
            start_method = td['method_list'] - 1  # 1-indexed
            if i + 1 < len(self.type_defs):
                end_method = self.type_defs[i + 1]['method_list'] - 1
            else:
                end_method = len(self.method_defs)
            
            methods = []
            for j in range(start_method, min(end_method, len(self.method_defs))):
                methods.append(self.method_defs[j])
            
            fullname = f"{td['namespace']}.{td['name']}" if td['namespace'] else td['name']
            results.append({
                'name': td['name'],
                'namespace': td['namespace'],
                'fullname': fullname,
                'flags': td['flags'],
                'methods': methods,
            })
        
        return results


def analyze_assembly(filepath):
    """Analyze a .NET assembly and print its type/method structure"""
    reader = DotNetMetadataReader(filepath)
    
    try:
        version = reader.parse()
    except Exception as e:
        print(f"Error parsing {filepath}: {e}")
        return None
    
    types = reader.get_types_with_methods()
    
    return {
        'version': version,
        'types': types,
        'table_rows': reader.table_rows,
    }


def main():
    if len(sys.argv) < 2:
        dll_dir = 'extracted_bundle'
        # Analyze key application DLLs
        key_dlls = [
            'ControlCenter.dll',
            'BydCentral.Core.dll', 
            'BYDWmi2.dll',
            'CustomControlLibrary.dll',
        ]
        filepaths = [os.path.join(dll_dir, d) for d in key_dlls if os.path.exists(os.path.join(dll_dir, d))]
    else:
        filepaths = sys.argv[1:]
    
    for filepath in filepaths:
        print(f"\n{'='*80}")
        print(f"Assembly: {os.path.basename(filepath)}")
        print(f"{'='*80}")
        
        result = analyze_assembly(filepath)
        if not result:
            continue
        
        print(f"Runtime: {result['version']}")
        print(f"TypeDef count: {result['table_rows'].get(0x02, 0)}")
        print(f"MethodDef count: {result['table_rows'].get(0x06, 0)}")
        print(f"MemberRef count: {result['table_rows'].get(0x0A, 0)}")
        print(f"TypeRef count: {result['table_rows'].get(0x01, 0)}")
        
        # Group by namespace
        ns_types = {}
        for t in result['types']:
            ns = t['namespace'] or '<global>'
            if ns not in ns_types:
                ns_types[ns] = []
            ns_types[ns].append(t)
        
        print(f"\nNamespaces: {len(ns_types)}")
        for ns in sorted(ns_types.keys()):
            types_in_ns = ns_types[ns]
            total_methods = sum(len(t['methods']) for t in types_in_ns)
            print(f"\n  namespace {ns} ({len(types_in_ns)} types, {total_methods} methods)")
            
            for t in sorted(types_in_ns, key=lambda x: x['name']):
                # Decode visibility
                vis = t['flags'] & 0x07
                vis_names = {0: '', 1: 'public', 2: 'internal', 3: 'internal',
                            4: 'nested public', 5: 'nested private', 6: 'nested family',
                            7: 'nested assembly'}
                vis_str = vis_names.get(vis, '')
                
                # Check if abstract, sealed, interface
                is_abstract = bool(t['flags'] & 0x80)
                is_sealed = bool(t['flags'] & 0x100)
                is_interface = bool(t['flags'] & 0x20)
                
                kind = 'interface' if is_interface else 'static class' if (is_abstract and is_sealed) else 'abstract class' if is_abstract else 'sealed class' if is_sealed else 'class'
                
                print(f"    {vis_str} {kind} {t['name']} ({len(t['methods'])} methods)")
                
                for m in t['methods']:
                    m_vis = m['flags'] & 0x07
                    m_vis_names = {0: '', 1: 'private', 2: 'family and assembly', 
                                  3: 'assembly', 4: 'family', 5: 'family or assembly', 6: 'public'}
                    m_vis_str = m_vis_names.get(m_vis, '')
                    
                    is_static = bool(m['flags'] & 0x10)
                    is_virtual = bool(m['flags'] & 0x40)
                    is_abstract_m = bool(m['flags'] & 0x400)
                    
                    modifiers = []
                    if is_static: modifiers.append('static')
                    if is_virtual: modifiers.append('virtual')
                    if is_abstract_m: modifiers.append('abstract')
                    
                    mod_str = ' '.join(modifiers)
                    print(f"      - {m_vis_str} {mod_str} {m['name']}()")


if __name__ == '__main__':
    main()
