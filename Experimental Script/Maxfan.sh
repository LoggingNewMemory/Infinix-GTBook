#!/bin/bash
# beast_fan.sh - Chain Beast Mode and Fan Full Mode

call_acpi() {
    echo "$1" | sudo tee /proc/acpi/call > /dev/null
}

read_status() {
    call_acpi '\_SB.AMW0.RSIO 0x300 1 0x94'
    sudo cat /proc/acpi/call | tr -d '\0'
    echo ""
}

echo "[*] Step 1: Forcing laptop into BEAST MODE..."
call_acpi '\_SB.AMW0.WSIO 0x300 1 0x90 0x00'
call_acpi '\_SB.AMW0.WSIO 0x300 1 0x91 0x40'  # 0x40 = Profile change command
call_acpi '\_SB.AMW0.WSIO 0x300 1 0x92 0x01'
call_acpi '\_SB.AMW0.WSIO 0x300 1 0xA0 0x03'  # 0x03 = Beast Mode
call_acpi '\_SB.AMW0.WSIO 0x300 1 0x93 0xA1'  # Trigger
sleep 0.1
echo "    Status: $(read_status)"

echo "[*] Step 2: Enabling Fan Full Mode..."
call_acpi '\_SB.AMW0.WSIO 0x300 1 0x90 0x00'
call_acpi '\_SB.AMW0.WSIO 0x300 1 0x91 0x41'  # 0x41 = Fan change command
call_acpi '\_SB.AMW0.WSIO 0x300 1 0x92 0x01'
call_acpi '\_SB.AMW0.WSIO 0x300 1 0xA0 0x01'  # 0x01 = Enable Fan Full
call_acpi '\_SB.AMW0.WSIO 0x300 1 0x93 0xA1'  # Trigger
sleep 0.1
echo "    Status: $(read_status)"

echo "[+] Done! You should hear the fans spinning up..."
