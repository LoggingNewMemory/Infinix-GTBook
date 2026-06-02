#!/usr/bin/env python3
"""Extract embedded resources (images) from a .NET WPF DLL using pythonnet or raw binary scanning."""

import os
import sys
import struct
import zipfile

def extract_from_dll_raw(dll_path, output_dir):
    """
    Extract PNG/JPEG/BMP/GIF/ICO images from a .NET DLL by scanning for file signatures.
    This is a brute-force approach that works without needing .NET runtime.
    """
    os.makedirs(output_dir, exist_ok=True)
    
    with open(dll_path, 'rb') as f:
        data = f.read()
    
    # File signatures
    signatures = {
        'png': (b'\x89PNG\r\n\x1a\n', b'IEND\xaeB`\x82'),
        'jpg': (b'\xff\xd8\xff', b'\xff\xd9'),
        'gif': (b'GIF87a', None),
        'gif89': (b'GIF89a', None),
        'bmp': (b'BM', None),
        'ico': (b'\x00\x00\x01\x00', None),
    }
    
    found_count = 0
    
    # Extract PNGs
    print("Scanning for PNG images...")
    png_start = b'\x89PNG\r\n\x1a\n'
    png_end = b'IEND\xaeB`\x82'
    pos = 0
    while True:
        start = data.find(png_start, pos)
        if start == -1:
            break
        end = data.find(png_end, start)
        if end == -1:
            pos = start + 1
            continue
        end += len(png_end)
        img_data = data[start:end]
        
        # Try to determine name from nearby strings
        name = find_nearby_name(data, start, 'png')
        if not name:
            name = f"image_{found_count:04d}.png"
        
        filepath = os.path.join(output_dir, name)
        os.makedirs(os.path.dirname(filepath), exist_ok=True)
        with open(filepath, 'wb') as img_f:
            img_f.write(img_data)
        print(f"  Extracted: {name} ({len(img_data)} bytes)")
        found_count += 1
        pos = end
    
    # Extract JPEGs
    print("Scanning for JPEG images...")
    pos = 0
    while True:
        start = data.find(b'\xff\xd8\xff', pos)
        if start == -1:
            break
        end = data.find(b'\xff\xd9', start + 3)
        if end == -1:
            pos = start + 1
            continue
        end += 2
        img_data = data[start:end]
        if len(img_data) > 100:  # Skip tiny false positives
            name = find_nearby_name(data, start, 'jpg')
            if not name:
                name = f"image_{found_count:04d}.jpg"
            
            filepath = os.path.join(output_dir, name)
            os.makedirs(os.path.dirname(filepath), exist_ok=True)
            with open(filepath, 'wb') as img_f:
                img_f.write(img_data)
            print(f"  Extracted: {name} ({len(img_data)} bytes)")
            found_count += 1
        pos = start + 3
    
    print(f"\nTotal images extracted: {found_count}")
    return found_count


def find_nearby_name(data, pos, ext):
    """Try to find a resource name near the image data by looking for preceding path strings."""
    # Look backwards up to 500 bytes for a resource name pattern
    search_region = data[max(0, pos-500):pos]
    
    # Look for patterns like "image/name.png" or similar resource paths
    import re
    # Try to find a path-like string ending in .png/.jpg etc
    matches = list(re.finditer(rb'([\w/._-]+\.' + ext.encode() + rb')', search_region, re.IGNORECASE))
    if matches:
        name = matches[-1].group(1).decode('utf-8', errors='replace')
        # Clean up the name
        name = name.replace('/', os.sep)
        return name
    return None


def extract_baml_resources(dll_path, output_dir):
    """
    .NET WPF assemblies store resources in a special format.
    The resources are stored in the assembly manifest as .g.resources streams.
    We can parse these manually.
    """
    os.makedirs(output_dir, exist_ok=True)
    
    with open(dll_path, 'rb') as f:
        data = f.read()
    
    # Look for the .resources magic number: 0xCECECECE
    # .NET resource format: magic(4) + version(4) + ...
    resource_magic = b'\xce\xce\xce\xce'
    
    pos = 0
    resource_count = 0
    while True:
        idx = data.find(resource_magic, pos)
        if idx == -1:
            break
        print(f"Found .resources section at offset 0x{idx:08x}")
        resource_count += 1
        pos = idx + 4
    
    print(f"Found {resource_count} .resources sections")
    return resource_count


if __name__ == '__main__':
    dll_path = '/home/yamada/CODE/device_infinix_gtbook/extracted_bundle/ControlCenter.dll'
    output_dir = '/home/yamada/CODE/device_infinix_gtbook/GTControlCenter/assets/extracted'
    
    if not os.path.exists(dll_path):
        print(f"DLL not found at {dll_path}")
        sys.exit(1)
    
    print(f"Extracting resources from: {dll_path}")
    print(f"Output directory: {output_dir}")
    print("=" * 60)
    
    count = extract_from_dll_raw(dll_path, output_dir)
    
    if count == 0:
        print("\nNo images found via raw scanning. Trying .resources parsing...")
        extract_baml_resources(dll_path, output_dir)
