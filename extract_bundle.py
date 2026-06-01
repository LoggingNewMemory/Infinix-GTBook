#!/usr/bin/env python3
"""
.NET Single-File Bundle Extractor
Extracts all embedded assemblies from a .NET 6+ single-file bundle.
"""

import struct
import os
import sys
import zlib


def read_7bit_encoded_int(data, pos):
    """Read a 7-bit encoded integer (BinaryReader.Read7BitEncodedInt format)"""
    result = 0
    shift = 0
    while True:
        byte = data[pos]
        pos += 1
        result |= (byte & 0x7F) << shift
        if (byte & 0x80) == 0:
            break
        shift += 7
    return result, pos


def read_string(data, pos):
    """Read a .NET serialized string (7-bit length prefix + UTF-8 bytes)"""
    length, pos = read_7bit_encoded_int(data, pos)
    s = data[pos:pos+length].decode('utf-8')
    return s, pos + length


def main():
    exe_path = sys.argv[1] if len(sys.argv) > 1 else 'ControlCenter.exe'
    output_dir = sys.argv[2] if len(sys.argv) > 2 else 'extracted_bundle'
    
    # Known manifest offset found by scanning
    MANIFEST_OFFSET = 192896311
    
    with open(exe_path, 'rb') as f:
        f.seek(0, 2)
        file_size = f.tell()
        
        # Read from manifest to end of file
        f.seek(MANIFEST_OFFSET)
        manifest_data = f.read(file_size - MANIFEST_OFFSET)
        
        pos = 0
        major = struct.unpack_from('<I', manifest_data, pos)[0]; pos += 4
        minor = struct.unpack_from('<I', manifest_data, pos)[0]; pos += 4
        count = struct.unpack_from('<I', manifest_data, pos)[0]; pos += 4
        bundle_id, pos = read_string(manifest_data, pos)
        
        print(f"Bundle version: {major}.{minor}")
        print(f"Bundle ID: {bundle_id}")
        print(f"File count: {count}")
        print(f"File size: {file_size:,} bytes")
        print("=" * 80)
        
        os.makedirs(output_dir, exist_ok=True)
        
        type_names = {
            0: 'Unknown', 1: 'Assembly', 2: 'NativeBinary', 
            3: 'DepsJson', 4: 'RuntimeConfigJson', 5: 'Symbols'
        }
        
        entries = []
        for i in range(count):
            try:
                entry_start = pos
                offset = struct.unpack_from('<Q', manifest_data, pos)[0]; pos += 8
                size = struct.unpack_from('<Q', manifest_data, pos)[0]; pos += 8
                compressed_size = struct.unpack_from('<Q', manifest_data, pos)[0]; pos += 8
                file_type = manifest_data[pos]; pos += 1
                name, pos = read_string(manifest_data, pos)
                
                # Validate entry
                if offset > file_size or size > file_size or file_type > 10:
                    print(f"  WARNING: Skipping invalid entry #{i} at manifest pos {entry_start}")
                    continue
                
                entries.append({
                    'name': name,
                    'offset': offset,
                    'size': size,
                    'compressed_size': compressed_size,
                    'file_type': file_type,
                })
            except (UnicodeDecodeError, struct.error, ValueError) as e:
                print(f"  WARNING: Error parsing entry #{i}: {e}")
                # Try to recover by searching for next valid entry
                # Look for next offset pattern (should be a reasonable file offset)
                recovery_pos = entry_start + 1
                while recovery_pos < len(manifest_data) - 25:
                    try:
                        test_offset = struct.unpack_from('<Q', manifest_data, recovery_pos)[0]
                        test_size = struct.unpack_from('<Q', manifest_data, recovery_pos + 8)[0]
                        if 0 < test_offset < file_size and 0 < test_size < file_size:
                            test_type = manifest_data[recovery_pos + 24]
                            if test_type <= 5:
                                test_name_len = manifest_data[recovery_pos + 25]
                                if 0 < test_name_len < 128:
                                    try:
                                        test_name = manifest_data[recovery_pos+26:recovery_pos+26+test_name_len].decode('utf-8')
                                        if '.' in test_name and all(c.isprintable() for c in test_name):
                                            pos = recovery_pos
                                            print(f"  Recovered at pos {recovery_pos}")
                                            break
                                    except UnicodeDecodeError:
                                        pass
                    except struct.error:
                        pass
                    recovery_pos += 1
                else:
                    print(f"  Could not recover, stopping at entry #{i}")
                    break
        
        # Extract files
        assemblies = []
        native_bins = []
        configs = []
        symbols = []
        others = []
        
        for entry in entries:
            name = entry['name']
            out_path = os.path.join(output_dir, name)
            out_dir = os.path.dirname(out_path)
            if out_dir:
                os.makedirs(out_dir, exist_ok=True)
            
            f.seek(entry['offset'])
            
            if entry['compressed_size'] > 0 and entry['compressed_size'] != entry['size']:
                raw = f.read(entry['compressed_size'])
                try:
                    data = zlib.decompress(raw, -15)
                except zlib.error:
                    try:
                        data = zlib.decompress(raw)
                    except zlib.error:
                        data = raw
                        print(f"  WARNING: Could not decompress {name}")
            else:
                data = f.read(entry['size'])
            
            with open(out_path, 'wb') as out:
                out.write(data)
            
            ft = type_names.get(entry['file_type'], f"Type{entry['file_type']}")
            size_kb = len(data) / 1024
            
            if entry['file_type'] == 1:
                assemblies.append(name)
            elif entry['file_type'] == 2:
                native_bins.append(name)
            elif entry['file_type'] in (3, 4):
                configs.append(name)
            elif entry['file_type'] == 5:
                symbols.append(name)
            else:
                others.append(name)
            
            comp_info = ""
            if entry['compressed_size'] > 0 and entry['compressed_size'] != entry['size']:
                ratio = entry['compressed_size'] / entry['size'] * 100
                comp_info = f" (compressed {ratio:.1f}%)"
            
            print(f"  [{ft:>15}] {name:<60} {size_kb:>10.1f} KB{comp_info}")
        
        print("=" * 80)
        print(f"\nExtraction Summary:")
        print(f"  .NET Assemblies:    {len(assemblies)}")
        print(f"  Native Binaries:    {len(native_bins)}")
        print(f"  Config Files:       {len(configs)}")
        print(f"  Symbol Files:       {len(symbols)}")
        print(f"  Other:              {len(others)}")
        print(f"  TOTAL:              {len(entries)}")
        print(f"\nFiles extracted to: {os.path.abspath(output_dir)}")
        
        # List key application assemblies
        app_dlls = [n for n in assemblies if not n.startswith('System.') 
                    and not n.startswith('Microsoft.') 
                    and not n.startswith('netstandard')
                    and not n.startswith('WindowsBase')
                    and not n.startswith('Presentation')
                    and not n.startswith('UIAutomation')]
        
        print(f"\n--- Key Application Assemblies ({len(app_dlls)}) ---")
        for name in sorted(app_dlls):
            print(f"  {name}")


if __name__ == '__main__':
    main()
