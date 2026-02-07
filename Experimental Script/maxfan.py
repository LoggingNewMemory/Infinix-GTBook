#!/usr/bin/env python3
import sys
import os
import struct
import time

# --- Configuration ---
EC_INDEX_PORT = 0x300
EC_DATA_PORT  = 0x301

# Addresses from BYD WMI .cs
ADDR_PERF_MODE   = 0x40  # 64
ADDR_FAN_BOOST   = 0x41  # 65
ADDR_TURBO       = 0x67  # 103
ADDR_CUSTOM_MODE = 0x69  # 105 (Key fix: Disable custom curves)

# Modes
MODE_OFFICE  = 0x00
MODE_BALANCE = 0x01
MODE_GAMING  = 0x02
MODE_TURBO   = 0x03  # Value 3 found in SetPerformanceMode switch case

def check_root():
    if os.geteuid() != 0:
        print("[-] Root privileges required.")
        sys.exit(1)

def ec_io(port_fd, index, value):
    try:
        # Write Index to 0x300
        port_fd.seek(EC_INDEX_PORT)
        port_fd.write(struct.pack('B', index))
        
        # Short delay between Index and Data is crucial for some ECs
        time.sleep(0.005) 
        
        # Write Value to 0x301
        port_fd.seek(EC_DATA_PORT)
        port_fd.write(struct.pack('B', value))
        
        # Safe delay for EC processing
        time.sleep(0.05) 
    except OSError as e:
        print(f"[-] I/O Error: {e}")

def ec_write_ram_cmd(port_fd, address, value):
    """
    Exact replication of ECWriteRamCMD from BYD WMI .cs
    Sequence: 0x94 -> 0x91 -> 0x92 -> 0x92(1) -> 0x90 -> 0x91(Addr) -> 0xA0(Val) -> 0x93(0xA1)
    """
    # 1. Initialization / Handshake
    ec_io(port_fd, 0x94, 0x00) 
    ec_io(port_fd, 0x91, 0x00) 
    ec_io(port_fd, 0x92, 0x00) 
    ec_io(port_fd, 0x92, 0x01) # Unlock/Bank Switch
    ec_io(port_fd, 0x90, 0x00) 
    
    # 2. Set Target Address
    ec_io(port_fd, 0x91, address) 
    
    # 3. Set Value
    ec_io(port_fd, 0xA0, value) 
    
    # 4. Trigger Execution (Commit)
    ec_io(port_fd, 0x93, 0xA1) 

def set_fan_max(enable: bool):
    try:
        with open("/dev/port", "rb+", buffering=0) as port:
            if enable:
                print("[*] Activating SKYROCKET Mode...")
                
                # 1. CRITICAL: Disable Custom Fan Mode (Addr 105 -> 0)
                # If this is 1, the EC ignores the Max Fan command and uses a curve.
                print("   -> Disabling Custom Fan Curves...")
                ec_write_ram_cmd(port, ADDR_CUSTOM_MODE, 0)
                
                # 2. Set Performance Mode to TURBO (Addr 64 -> 3)
                print("   -> Setting Performance Mode to Turbo...")
                ec_write_ram_cmd(port, ADDR_PERF_MODE, MODE_TURBO)
                
                # 3. Set Turbo Flag (Addr 103 -> 3)
                # Found in SetGBoxTurbo(true)
                print("   -> Enabling GBox Turbo...")
                ec_write_ram_cmd(port, ADDR_TURBO, 3)

                # 4. Set Fan Full Mode (Addr 65 -> 1)
                # This is the "Max Fan" override switch
                print("   -> Triggering MAX FAN...")
                ec_write_ram_cmd(port, ADDR_FAN_BOOST, 1)
                
                print("[+] DONE. Fan should skyrocket now.")
                
            else:
                print("[*] Deactivating MAX FAN Mode...")
                
                # 1. Disable Fan Full Mode
                ec_write_ram_cmd(port, ADDR_FAN_BOOST, 0)

                # 2. Disable Turbo Flag (Addr 103 -> 2)
                ec_write_ram_cmd(port, ADDR_TURBO, 2)
                
                # 3. Revert to Balance Mode
                ec_write_ram_cmd(port, ADDR_PERF_MODE, MODE_BALANCE)
                
                print("[+] FAN NORMALIZED")
            
    except FileNotFoundError:
        print("[-] Error: /dev/port not found. Ensure 'CONFIG_DEVPORT' is enabled in kernel.")
    except Exception as e:
        print(f"[-] Error: {e}")

if __name__ == "__main__":
    check_root()
    if len(sys.argv) < 2:
        print("Usage: sudo python3 maxfan_fixed.py [on|off]")
        sys.exit(1)
        
    mode = sys.argv[1].lower()
    if mode == "on":
        set_fan_max(True)
    elif mode == "off":
        set_fan_max(False)
    else:
        print("Invalid argument.")