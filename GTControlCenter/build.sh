#!/bin/bash
cd "$(dirname "$0")"

if [ ! -d "venv" ]; then
    echo "Virtual environment not found. Creating one..."
    python3 -m venv venv --system-site-packages
    ./venv/bin/pip install pyusb psutil pyserial sounddevice numpy PyGObject pyinstaller
else
    # Ensure PyInstaller is installed in the existing venv
    ./venv/bin/pip install pyinstaller
fi

# Create an entry point script for PyInstaller
cat << 'EOF' > run_app.py
import sys
import os

# PyInstaller creates a temp folder and stores path in _MEIPASS
if hasattr(sys, '_MEIPASS'):
    os.chdir(sys._MEIPASS)

from controlcenter.app import main

if __name__ == '__main__':
    main()
EOF

echo "Compiling to single binary using PyInstaller..."
./venv/bin/pyinstaller --onefile --windowed --name gt-controlcenter \
    --add-data "assets:assets" \
    --add-data "controlcenter:controlcenter" \
    --hidden-import gi \
    --hidden-import gi.repository.Gtk \
    --hidden-import gi.repository.Adw \
    --hidden-import gi.repository.GLib \
    --hidden-import gi.repository.Gdk \
    --hidden-import sounddevice \
    --hidden-import numpy \
    --hidden-import serial \
    --hidden-import usb \
    run_app.py

echo "Build complete. Binary is located in the 'dist' folder."
