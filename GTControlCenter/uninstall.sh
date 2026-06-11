#!/bin/bash

if [ "$EUID" -ne 0 ]; then
  echo "Please run as root (sudo ./uninstall.sh)"
  exit 1
fi

echo "Removing GT Control Center from /opt..."
rm -rf /opt/gt-controlcenter

echo "Removing launcher and desktop entry..."
rm -f /usr/bin/gt-controlcenter
rm -f /usr/share/applications/gt-controlcenter.desktop

echo "Disabling and removing acpi_call service..."
systemctl disable --now acpi-call-perms.service 2>/dev/null || true
rm -f /etc/systemd/system/acpi-call-perms.service
systemctl daemon-reload || true

echo "Removing udev rules..."
rm -f /etc/udev/rules.d/99-byd-keyboard.rules
udevadm control --reload-rules || true
udevadm trigger || true

echo "----------------------------------------"
echo "Uninstallation complete!"
echo "GT Control Center has been successfully removed from your system."
