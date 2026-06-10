import sys
import gi

gi.require_version('Gtk', '4.0')
gi.require_version('Adw', '1')
from gi.repository import Gtk, Adw, Gio

from controlcenter.window import MainWindow


class ControlCenterApp(Adw.Application):
    def __init__(self):
        super().__init__(application_id='com.byd.controlcenter',
                         flags=Gio.ApplicationFlags.FLAGS_NONE)
                         
        # Suppress Adwaita warning if user's environment has prefer-dark-theme set globally
        settings = Gtk.Settings.get_default()
        if settings:
            settings.set_property("gtk-application-prefer-dark-theme", False)
            
        self.win = None

    def do_startup(self):
        Adw.Application.do_startup(self)
        Adw.StyleManager.get_default().set_color_scheme(Adw.ColorScheme.PREFER_DARK)

    def do_activate(self):
        if not self.win:
            self.win = MainWindow(self)
        self.win.present()

def main():
    app = ControlCenterApp()
    return app.run(sys.argv)

if __name__ == '__main__':
    sys.exit(main())
