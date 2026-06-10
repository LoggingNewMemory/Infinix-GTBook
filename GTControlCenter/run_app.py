import sys
import os
import subprocess

# PyInstaller creates a temp folder and stores path in _MEIPASS
if hasattr(sys, '_MEIPASS'):
    os.chdir(sys._MEIPASS)
else:
    # Automatically use venv and sync dependencies
    base_dir = os.path.dirname(os.path.abspath(__file__))
    venv_dir = os.path.join(base_dir, 'venv')
    venv_python = os.path.join(venv_dir, 'bin', 'python')
    
    # Check if we are running inside the venv
    if sys.prefix == sys.base_prefix or not os.path.samefile(sys.prefix, venv_dir):
        if not os.path.exists(venv_python):
            print("Creating virtual environment...")
            subprocess.check_call([sys.executable, "-m", "venv", "--system-site-packages", "venv"], cwd=base_dir)
            
        print("Switching to virtual environment...")
        os.execv(venv_python, [venv_python] + sys.argv)
        
    try:
        import pynvml
        import psutil
        import serial
        import usb
        import gi
    except ImportError as e:
        print(f"Missing dependency: {e}. Syncing dependencies in venv...")
        try:
            subprocess.check_call([sys.executable, "-m", "pip", "install", "-e", base_dir])
            print("Dependencies synced. Restarting app...")
            os.execv(sys.executable, [sys.executable] + sys.argv)
        except subprocess.CalledProcessError as err:
            print(f"Failed to install dependencies: {err}")
            sys.exit(1)

from controlcenter.app import main

if __name__ == '__main__':
    main()
