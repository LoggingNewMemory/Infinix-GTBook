import sys
import os

# PyInstaller creates a temp folder and stores path in _MEIPASS
if hasattr(sys, '_MEIPASS'):
    os.chdir(sys._MEIPASS)
elif sys.prefix == sys.base_prefix:
    # Not in a virtual environment, try to use the local venv
    script_dir = os.path.dirname(os.path.abspath(__file__))
    venv_python = os.path.join(script_dir, 'venv', 'bin', 'python3')
    if os.path.exists(venv_python):
        os.execl(venv_python, venv_python, *sys.argv)

from controlcenter.app import main

if __name__ == '__main__':
    main()
