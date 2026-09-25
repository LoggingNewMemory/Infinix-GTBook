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
    if getattr(sys, 'frozen', False):
        exec_cmd = [sys.executable]
    else:
        exec_cmd = [sys.executable, os.path.abspath(__file__)]
    
    def show_window(icon, item):
        subprocess.Popen(exec_cmd)

    def set_mode(mode_name):
        def _callback(icon, item=None):
            subprocess.Popen(exec_cmd + ['--mode', mode_name])
        return _callback
        
    def quit_app(icon, item):
        # Kill the parent GTK process
        try:
            os.kill(os.getppid(), signal.SIGTERM)
        except Exception:
            pass
        icon.stop()
        
    menu = pystray.Menu(
        pystray.MenuItem("Show GT Control Center", show_window, default=True),
        pystray.Menu.SEPARATOR,
        pystray.MenuItem("Office Mode", set_mode("office")),
        pystray.MenuItem("Balanced Mode", set_mode("balanced")),
        pystray.MenuItem("Gaming Mode", set_mode("gaming")),
        pystray.Menu.SEPARATOR,
        pystray.MenuItem("Quit", quit_app)
    )
    
    icon = pystray.Icon("GTControlCenter", image, "GT Control Center", menu)
    
    
    
    
    def hotkey_listener():
        import sys
        import os
        import subprocess
        import evdev
        
        if getattr(sys, 'frozen', False):
            exec_cmd = [sys.executable]
        else:
            exec_cmd = [sys.executable, os.path.abspath(__file__)]
            
        def set_mode(mode_name):
            def _callback(icon, item=None):
                subprocess.Popen(exec_cmd + ['--mode', mode_name])
            return _callback

        try:
            devices = [evdev.InputDevice(path) for path in evdev.list_devices()]
            target = next((d for d in devices if 'BYD' in d.name and 'Keyboard' in d.name), None)
            if not target: return
            
            for event in target.read_loop():
                if event.type == evdev.ecodes.EV_KEY and event.value == 1:
                    with open("/tmp/evdev.log", "a") as f: f.write(f"Key pressed: {event.code}\n")
                    if event.code == evdev.ecodes.KEY_F13:
                        set_mode("office")(None, None)
                    elif event.code == evdev.ecodes.KEY_F14:
                        set_mode("balanced")(None, None)
                    elif event.code == evdev.ecodes.KEY_F15:
                        set_mode("gaming")(None, None)
        except Exception as e:
            print("Hotkey listener error:", e)

    import threading
    threading.Thread(target=hotkey_listener, daemon=True).start()

    icon.run()
    sys.exit(0)

from controlcenter.app import main

if __name__ == '__main__':
    main()
