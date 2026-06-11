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

if '--tray-process' in sys.argv:
    import pystray
    from PIL import Image
    import subprocess
    import signal
    
    icon_path = os.path.join('assets', 'icon.png')
    if not os.path.exists(icon_path):
        base_dir = os.path.dirname(os.path.abspath(__file__))
        icon_path = os.path.join(base_dir, 'assets', 'icon.png')
        
    image = Image.open(icon_path)
    exec_path = sys.executable
    
    def show_window(icon, item):
        subprocess.Popen([exec_path])
        
    def quit_app(icon, item):
        # Kill the parent GTK process
        try:
            os.kill(os.getppid(), signal.SIGTERM)
        except Exception:
            pass
        icon.stop()
        
    menu = pystray.Menu(
        pystray.MenuItem("Show GT Control Center", show_window, default=True),
        pystray.MenuItem("Quit", quit_app)
    )
    
    icon = pystray.Icon("GTControlCenter", image, "GT Control Center", menu)
    icon.run()
    sys.exit(0)

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
    --hidden-import pystray \
    --hidden-import PIL.Image \
    run_app.py

echo "Build complete. Binary is located in the 'dist' folder."
