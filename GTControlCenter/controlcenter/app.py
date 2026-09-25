import sys
import gi

gi.require_version('Gtk', '4.0')
gi.require_version('Adw', '1')
from gi.repository import Gtk, Adw, Gio, GLib

import subprocess
import os

from controlcenter.window import MainWindow


class ControlCenterApp(Adw.Application):
    def __init__(self, is_background=False):
        super().__init__(application_id='com.byd.controlcenter',
                         flags=Gio.ApplicationFlags.HANDLES_COMMAND_LINE)
        self.is_background = is_background
        self.first_activate = True
                         
        # Suppress Adwaita warning if user's environment has prefer-dark-theme set globally
        settings = Gtk.Settings.get_default()
        if settings:
            settings.set_property("gtk-application-prefer-dark-theme", False)
            
        self.win = None
        self.tray_proc = None

    def setup_tray(self):
        try:
            if getattr(sys, 'frozen', False):
                cmd = [sys.executable, '--tray-process']
            else:
                script_path = os.path.join(os.path.dirname(os.path.dirname(os.path.abspath(__file__))), 'run_app.py')
                cmd = [sys.executable, script_path, '--tray-process']
            self.tray_proc = subprocess.Popen(cmd)
        except Exception as e:
            print("Failed to start tray process:", e)

    def do_startup(self):
        Adw.Application.do_startup(self)
        Adw.StyleManager.get_default().set_color_scheme(Adw.ColorScheme.PREFER_DARK)
        
        self.hold()
        self.setup_tray()

    def get_backend(self):
        if not hasattr(self, 'backend'):
            from controlcenter.backend import AppBackend
            self.backend = AppBackend()
        return self.backend
        


    def do_command_line(self, command_line):
        args = command_line.get_arguments()
        
        mode = None
        max_fan = None
        
        for i, arg in enumerate(args):
            if arg == '--mode' and i + 1 < len(args):
                mode_str = args[i+1].lower()
                if mode_str == 'office': mode = 0
                elif mode_str == 'balanced': mode = 1
                elif mode_str == 'gaming': mode = 2
            elif arg == '--max-fan' and i + 1 < len(args):
                mf_str = args[i+1].lower()
                if mf_str == 'on': max_fan = True
                elif mf_str == 'off': max_fan = False
                elif mf_str == 'toggle':
                    current = self.get_backend().config_mgr.config.get("performance", {}).get("max_fan", False)
                    max_fan = not current
                    
        if mode is not None or max_fan is not None:
            if self.win:
                def force_ui_update():
                    if mode is not None:
                        self.win.set_performance_mode(mode, save=True)
                    if max_fan is not None:
                        self.win.set_max_fan(max_fan, save=True)
                    return False
                from gi.repository import GLib
                GLib.idle_add(force_ui_update)
            else:
                if mode is not None:
                    self.get_backend().config_mgr.config.setdefault("performance", {})["mode"] = mode
                if max_fan is not None:
                    self.get_backend().config_mgr.config.setdefault("performance", {})["max_fan"] = max_fan
                self.get_backend().config_mgr.save()
                self.get_backend().apply_performance()
                if mode is not None:
                    self.get_backend().apply_backzone()
            return 0
            
        self.activate()
        return 0

            
        self.activate()
        return 0

    def do_activate(self):
        if self.is_background and self.first_activate:
            self.first_activate = False
            self.get_backend().apply_all()
        else:
            if self.win and not self.win.get_visible() and not self.win.get_realized():
                self.win = None
            if not self.win:
                self.win = MainWindow(self, self.get_backend())
                self.win.set_hide_on_close(True)
            self.win.present()
            self.first_activate = False

def main():
    is_bg = '--background' in sys.argv
    if is_bg:
        sys.argv.remove('--background')
        
    app = ControlCenterApp(is_background=is_bg)
    return app.run(sys.argv)

if __name__ == '__main__':
    sys.exit(main())
