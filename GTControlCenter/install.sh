#!/bin/bash

if [ "$EUID" -ne 0 ]; then
  echo "Please run as root (sudo ./install.sh)"
  exit 1
fi

echo "Copying udev rules..."
cp 99-byd-keyboard.rules /etc/udev/rules.d/
udevadm control --reload-rules
udevadm trigger

echo "Setting up acpi_call permissions service..."
cp acpi-call-perms.service /etc/systemd/system/
systemctl daemon-reload
systemctl enable --now acpi-call-perms.service

echo "Done! GT Control Center should now work without sudo."
