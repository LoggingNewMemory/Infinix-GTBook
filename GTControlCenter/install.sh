#!/bin/bash

if [ "$EUID" -ne 0 ]; then
  echo "Please run as root (sudo ./install.sh)"
  exit 1
fi

echo "Installing GT Control Center to /opt/gt-controlcenter..."
mkdir -p /opt/gt-controlcenter
cp -r controlcenter assets run_app.py /opt/gt-controlcenter/

echo "Setting up isolated virtual environment..."
python3 -m venv /opt/gt-controlcenter/venv --system-site-packages
/opt/gt-controlcenter/venv/bin/pip install pyusb psutil pyserial sounddevice numpy pystray pillow nvidia-ml-py

echo "Creating launcher script..."
cat << 'EOF' > /usr/bin/gt-controlcenter
#!/bin/bash
cd /opt/gt-controlcenter
exec /opt/gt-controlcenter/venv/bin/python run_app.py "$@"
EOF
chmod +x /usr/bin/gt-controlcenter

echo "Creating Desktop Entry..."
cat << 'EOF' > /usr/share/applications/com.byd.controlcenter.desktop
[Desktop Entry]
Name=GT Control Center
Comment=Control Center for Infinix GT Book
Exec=gt-controlcenter
Icon=/opt/gt-controlcenter/assets/icon.png
Terminal=false
Type=Application
Categories=Settings;HardwareSettings;
StartupWMClass=com.byd.controlcenter
EOF

echo "Copying udev rules..."
cp 99-byd-keyboard.rules /etc/udev/rules.d/
udevadm control --reload-rules || true
udevadm trigger || true

echo "Setting up acpi_call permissions service..."
cp acpi-call-perms.service /etc/systemd/system/
systemctl daemon-reload || true
systemctl enable --now acpi-call-perms.service || true

echo "----------------------------------------"
echo "Installation complete!"
echo "You can now launch 'GT Control Center' from your application menu."
echo "Or run 'gt-controlcenter' from the terminal."
