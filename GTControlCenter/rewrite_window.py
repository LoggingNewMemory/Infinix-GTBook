import re

def rewrite_window():
    with open('/home/yamada/CODE/device_infinix_gtbook/GTControlCenter/controlcenter/window.py', 'r') as f:
        old_content = f.read()
        
    match = re.search(r'(    def set_performance_mode\(self, mode: int, save=True\):.*)', old_content, re.DOTALL)
    if not match:
        print("Could not find logic methods")
        return
        
    logic_content = match.group(1)
    
    match_ui = re.search(r'(    def _update_color_button_ui\(self, button, rgba\):.*?)(    def set_performance_mode)', old_content, re.DOTALL)
    if match_ui:
        logic_content = match_ui.group(1) + logic_content
        
    new_ui = """import gi
gi.require_version('Gtk', '4.0')
gi.require_version('Adw', '1')
from gi.repository import Gtk, Adw, GLib, Gdk, Pango

import os
import sys

from controlcenter.models.tx_buf import KeyboardLightMode, KeyboardLight12Mode, BackLightCmd, FanCtrlMode
from controlcenter.services.acpi_wmi import ACPIWmi
from controlcenter.services.usb_service import USBService
from controlcenter.services.serial_service import SerialService
from controlcenter.services.lighting import LightingService
from controlcenter.services.fan_service import FanService
from controlcenter.services.monitor import MonitorService
from controlcenter.services.config import ConfigManager

class MainWindow(Adw.ApplicationWindow):
    def __init__(self, app):
        super().__init__(application=app, title="INFINIX - GT BOOK")
        self.set_default_size(1100, 700)
        
        if hasattr(sys, '_MEIPASS'):
            self.assets_dir = os.path.join(sys._MEIPASS, 'assets')
        else:
            base_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
            self.assets_dir = os.path.join(base_dir, 'assets')
            
        theme = Gtk.IconTheme.get_for_display(Gdk.Display.get_default())
        theme.add_search_path(self.assets_dir)
        self.set_icon_name("icon")
        
        self.setup_css()
        
        self.wmi = ACPIWmi()
        self.usb = USBService()
        self.usb.connect()
        self.serial = SerialService()
        self.lighting = LightingService(self.usb, self.serial)
        self.fan = FanService(self.usb, self.wmi)
        self.monitor = MonitorService(self.wmi)
        self.config_mgr = ConfigManager()
        
        self.setup_ui()
        self.load_settings()
        self.apply_all_settings()
        
        GLib.timeout_add(2000, self.update_monitors)

    def setup_css(self):
        css_provider = Gtk.CssProvider()
        css = '''
        window {
            background-color: #272727;
            color: #ffffff;
            font-family: "Infinix Display", sans-serif;
        }
        headerbar {
            background-color: #272727;
            background-image: none;
            border: none;
            box-shadow: none;
        }
        headerbar windowhandle {
            background-color: transparent;
        }
        .header-title {
            font-size: 24px;
            font-weight: bold;
            color: white;
            letter-spacing: 2px;
        }
        .nav-link {
            font-size: 22px;
            color: #ffffff;
            background: transparent;
            border: none;
            box-shadow: none;
            padding: 0 10px;
        }
        .nav-link:hover {
            color: #d1d1d1;
            background: transparent;
        }
        .nav-link.active {
            color: #29b6f6;
        }
        .nav-dot {
            font-size: 22px;
            color: #ffffff;
            margin: 0 5px;
        }
        
        .mode-btn {
            background-color: #3b3b3b;
            color: #ffffff;
            font-size: 26px;
            border-radius: 4px;
            padding: 12px 20px;
            margin-bottom: 15px;
            border: none;
        }
        .mode-btn:hover {
            background-color: #4a4a4a;
        }
        .mode-btn.active {
            background-color: #e53935;
        }
        .mode-btn.active:hover {
            background-color: #ff474c;
        }
        
        .bar-container {
            background-color: transparent;
            margin-top: 5px;
            margin-bottom: 5px;
        }
        .bar-cyan {
            background-color: #29b6f6;
            min-height: 15px;
        }
        .bar-green {
            background-color: #66bb6a;
            min-height: 15px;
        }
        .stat-label {
            font-size: 28px;
            font-weight: bold;
        }
        .stat-sub {
            font-size: 14px;
            color: #a0a0a0;
        }
        .stat-val {
            font-size: 20px;
        }
        .vert-sep {
            background-color: #ffffff;
            min-width: 2px;
            margin-left: 30px;
            margin-right: 30px;
        }
        
        .control-label {
            font-size: 22px;
        }
        .custom-dropdown > button {
            background-color: #444444;
            color: white;
            font-size: 20px;
            border-radius: 8px;
            min-height: 40px;
            border: none;
        }
        .custom-scale contents trough {
            background-color: #555555;
            min-height: 4px;
        }
        .custom-scale contents trough slider {
            background-color: #ffffff;
            min-width: 24px;
            min-height: 24px;
            border-radius: 12px;
        }
        .color-btn {
            border-radius: 8px;
            border: none;
        }
        .action-btn {
            background-color: #e53935;
            color: white;
            font-size: 22px;
            border-radius: 8px;
            padding: 10px;
            border: none;
        }
        '''
        css_provider.load_from_data(css.encode())
        Gtk.StyleContext.add_provider_for_display(Gdk.Display.get_default(), css_provider, Gtk.STYLE_PROVIDER_PRIORITY_APPLICATION)

    def setup_ui(self):
        self.header = Adw.HeaderBar()
        self.header.set_show_title(False)
        
        title_box = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL, spacing=10)
        img_icon = Gtk.Image.new_from_file(os.path.join(self.assets_dir, "icon.png"))
        img_icon.set_pixel_size(32)
        lbl_title = Gtk.Label(label="INFINIX - GT BOOK")
        lbl_title.add_css_class("header-title")
        title_box.append(img_icon)
        title_box.append(lbl_title)
        
        self.header.set_title_widget(title_box)
        
        self.main_box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL)
        self.main_box.append(self.header)
        
        self.stack = Gtk.Stack()
        self.stack.set_transition_type(Gtk.StackTransitionType.CROSSFADE)
        self.stack.set_vexpand(True)
        self.stack.set_margin_start(40)
        self.stack.set_margin_end(40)
        self.stack.set_margin_top(20)
        self.stack.set_margin_bottom(20)
        
        self.setup_overview_page()
        self.setup_keyboard_page()
        self.setup_back_zone_page()
        self.setup_misc_page()
        
        self.main_box.append(self.stack)
        
        nav_box = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL, spacing=10)
        nav_box.set_halign(Gtk.Align.CENTER)
        nav_box.set_margin_bottom(20)
        
        self.nav_btns = {}
        pages = [("overview", "Overview"), ("keyboard", "Keyboard Light"), ("backzone", "Back Zone Light"), ("misc", "Misc")]
        
        for i, (page_id, label) in enumerate(pages):
            btn = Gtk.Button(label=label)
            btn.add_css_class("nav-link")
            if i == 0:
                btn.add_css_class("active")
            btn.connect("clicked", self.on_nav_clicked, page_id)
            self.nav_btns[page_id] = btn
            nav_box.append(btn)
            
            if i < len(pages) - 1:
                dot = Gtk.Label(label="•")
                dot.add_css_class("nav-dot")
                nav_box.append(dot)
                
        self.main_box.append(nav_box)
        self.set_content(self.main_box)

    def on_nav_clicked(self, btn, page_id):
        for b in self.nav_btns.values():
            b.remove_css_class("active")
        btn.add_css_class("active")
        self.stack.set_visible_child_name(page_id)

    def setup_overview_page(self):
        page = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL)
        
        left_box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=30)
        left_box.set_hexpand(True)
        left_box.set_valign(Gtk.Align.CENTER)
        
        def create_stat_row(title, sub, val1_id, val2_id, has_double_bar=True):
            row = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL, spacing=20)
            
            title_box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL)
            lbl_title = Gtk.Label(label=title)
            lbl_title.add_css_class("stat-label")
            lbl_title.set_halign(Gtk.Align.START)
            title_box.append(lbl_title)
            
            if sub:
                lbl_sub = Gtk.Label(label=sub)
                lbl_sub.add_css_class("stat-sub")
                lbl_sub.set_halign(Gtk.Align.START)
                title_box.append(lbl_sub)
                
            title_box.set_size_request(200, -1)
            row.append(title_box)
            
            bar_box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=0)
            bar_box.set_hexpand(True)
            bar_box.set_valign(Gtk.Align.CENTER)
            
            bar1 = Gtk.Box()
            bar1.add_css_class("bar-cyan")
            bar_box.append(bar1)
            
            if has_double_bar:
                bar2 = Gtk.Box()
                bar2.add_css_class("bar-green")
                bar2.set_margin_end(60) # Simulate shorter green bar
                bar_box.append(bar2)
            else:
                bar2 = None
                
            row.append(bar_box)
            
            val_box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL)
            lbl_v1 = Gtk.Label(label="--")
            lbl_v1.add_css_class("stat-val")
            lbl_v1.set_halign(Gtk.Align.END)
            val_box.append(lbl_v1)
            setattr(self, val1_id, lbl_v1)
            
            if val2_id:
                lbl_v2 = Gtk.Label(label="--")
                lbl_v2.add_css_class("stat-val")
                lbl_v2.set_halign(Gtk.Align.END)
                val_box.append(lbl_v2)
                setattr(self, val2_id, lbl_v2)
                
            val_box.set_size_request(80, -1)
            row.append(val_box)
            
            return row, bar1, bar2
            
        cpu_row, self.cpu_bar1, self.cpu_bar2 = create_stat_row("CPU", "13th Gen Intel(R) Core(TM) i9-13900H", "lbl_cpu_freq", "lbl_cpu_temp", True)
        gpu_row, self.gpu_bar1, self.gpu_bar2 = create_stat_row("GPU", "NVIDIA GeForce RTX 4060", "lbl_gpu_freq", "lbl_gpu_temp", True)
        bat_row, self.bat_bar1, _ = create_stat_row("Battery", "[Battery Name]", "lbl_bat_pct", None, False)
        disk_row, self.disk_bar1, _ = create_stat_row("Disk", "[Disk Name]", "lbl_disk_pct", None, False)
        
        self.bat_bar1.set_margin_end(20)
        self.disk_bar1.set_margin_end(150)
        
        left_box.append(cpu_row)
        left_box.append(gpu_row)
        left_box.append(bat_row)
        left_box.append(disk_row)
        
        page.append(left_box)
        
        sep = Gtk.Box()
        sep.add_css_class("vert-sep")
        page.append(sep)
        
        right_box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL)
        right_box.set_valign(Gtk.Align.START)
        right_box.set_size_request(300, -1)
        right_box.set_margin_top(40)
        
        self.btn_office = Gtk.Button(label="Office")
        self.btn_office.add_css_class("mode-btn")
        self.btn_office.connect("clicked", lambda x: self.set_performance_mode(0))
        
        self.btn_balance = Gtk.Button(label="Balanced")
        self.btn_balance.add_css_class("mode-btn")
        self.btn_balance.connect("clicked", lambda x: self.set_performance_mode(1))
        
        self.btn_gaming = Gtk.Button(label="Gaming")
        self.btn_gaming.add_css_class("mode-btn")
        self.btn_gaming.connect("clicked", lambda x: self.set_performance_mode(2))
        
        right_box.append(self.btn_office)
        right_box.append(self.btn_balance)
        right_box.append(self.btn_gaming)
        
        fan_box = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL)
        fan_box.set_margin_top(40)
        lbl_fan = Gtk.Label(label="Max Fan")
        lbl_fan.add_css_class("control-label")
        lbl_fan.set_hexpand(True)
        lbl_fan.set_halign(Gtk.Align.START)
        
        self.max_fan_switch = Gtk.Switch()
        self.max_fan_switch.set_valign(Gtk.Align.CENTER)
        self.max_fan_switch.connect("state-set", self.on_max_fan_toggled)
        
        fan_box.append(lbl_fan)
        fan_box.append(self.max_fan_switch)
        right_box.append(fan_box)
        
        page.append(right_box)
        self.stack.add_named(page, "overview")

    def _create_control_row(self, label_text, widget):
        row = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL, spacing=30)
        row.set_margin_bottom(30)
        lbl = Gtk.Label(label=label_text)
        lbl.add_css_class("control-label")
        lbl.set_size_request(140, -1)
        lbl.set_halign(Gtk.Align.START)
        row.append(lbl)
        widget.set_hexpand(True)
        widget.set_valign(Gtk.Align.CENTER)
        row.append(widget)
        return row

    def setup_keyboard_page(self):
        page = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL, spacing=50)
        
        img_box = Gtk.Box()
        img_box.set_hexpand(True)
        img_box.set_valign(Gtk.Align.CENTER)
        img = Gtk.Image.new_from_file(os.path.join(self.assets_dir, "keyboard_all.png"))
        img_box.append(img)
        page.append(img_box)
        
        sep = Gtk.Box()
        sep.add_css_class("vert-sep")
        page.append(sep)
        
        ctrl_box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL)
        ctrl_box.set_valign(Gtk.Align.CENTER)
        ctrl_box.set_size_request(450, -1)
        
        self.kb_zone_dropdown = Gtk.DropDown.new_from_strings([
            "Main Keyboard (All)", "Keyboard Zone 1 (Left)", "Keyboard Zone 2 (Mid-Left)", 
            "Keyboard Zone 3 (Mid-Right)", "Keyboard Zone 4 (Right)"
        ])
        self.kb_zone_dropdown.add_css_class("custom-dropdown")
        self.kb_zone_dropdown.connect("notify::selected", self.on_kb_zone_changed)
        ctrl_box.append(self._create_control_row("Zone", self.kb_zone_dropdown))
        
        self.kb_color_button = Gtk.Button()
        self.kb_current_rgba = Gdk.RGBA()
        self.kb_current_rgba.parse("#FF0000")
        self.kb_color_popover = Gtk.Popover()
        self.kb_color_widget = Gtk.ColorChooserWidget()
        self.kb_color_widget.set_size_request(400, 300)
        self.kb_color_widget.set_rgba(self.kb_current_rgba)
        self.kb_color_popover.set_child(self.kb_color_widget)
        self.kb_color_popover.set_parent(self.kb_color_button)
        
        def on_popover_closed(p):
            self.kb_current_rgba = self.kb_color_widget.get_rgba()
            self._update_color_button_ui(self.kb_color_button, self.kb_current_rgba)
            
        self.kb_color_popover.connect("closed", on_popover_closed)
        
        def on_button_clicked(btn):
            self.kb_color_widget.set_rgba(self.kb_current_rgba)
            self.kb_color_popover.popup()
            
        self.kb_color_button.connect("clicked", on_button_clicked)
        self.kb_color_button.add_css_class("color-btn")
        self.kb_color_button.set_size_request(-1, 40)
        self._update_color_button_ui(self.kb_color_button, self.kb_current_rgba)
        ctrl_box.append(self._create_control_row("Color", self.kb_color_button))
        
        self.kb_mode_dropdown = Gtk.DropDown.new_from_strings([
            "Off", "Static Color", "Breathing", "Neon Cycle", "Rainbow", "Flow", "Wave", "Rhythm Dance"
        ])
        self.kb_mode_dropdown.add_css_class("custom-dropdown")
        ctrl_box.append(self._create_control_row("Effects", self.kb_mode_dropdown))
        
        self.kb_brightness_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.kb_brightness_scale.set_value(100)
        self.kb_brightness_scale.add_css_class("custom-scale")
        ctrl_box.append(self._create_control_row("Brightness", self.kb_brightness_scale))
        
        # Audio controls for rhythm mode
        self.kb_audio_names = ["Auto (Speaker Output)"]
        self.kb_audio_device_ids = ["auto_speaker"]
        try:
            import sounddevice as sd
            devices = sd.query_devices()
            for i, dev in enumerate(devices):
                if dev['max_input_channels'] > 0:
                    self.kb_audio_device_ids.append(i)
                    self.kb_audio_names.append(dev['name'])
        except Exception:
            pass
            
        self.kb_device_dropdown = Gtk.DropDown.new_from_strings(self.kb_audio_names)
        self.kb_sens_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.kb_sens_scale.set_value(35)
        self.kb_smooth_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.kb_smooth_scale.set_value(0)
        
        apply_btn = Gtk.Button(label="Apply Keyboard Lighting")
        apply_btn.add_css_class("action-btn")
        apply_btn.connect("clicked", self.apply_keyboard_lighting)
        ctrl_box.append(apply_btn)
        
        page.append(ctrl_box)
        self.stack.add_named(page, "keyboard")

    def setup_back_zone_page(self):
        page = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=50)
        page.set_valign(Gtk.Align.CENTER)
        
        img = Gtk.Image.new_from_file(os.path.join(self.assets_dir, "backzone.png"))
        img.set_halign(Gtk.Align.CENTER)
        page.append(img)
        
        ctrl_box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL)
        ctrl_box.set_halign(Gtk.Align.CENTER)
        ctrl_box.set_size_request(700, -1)
        
        row1 = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL, spacing=30)
        row1.set_margin_bottom(30)
        
        lbl_c = Gtk.Label(label="Color")
        lbl_c.add_css_class("control-label")
        row1.append(lbl_c)
        
        self.bz_color_button = Gtk.Button()
        self.bz_current_rgba = Gdk.RGBA()
        self.bz_current_rgba.parse("#FF0000")
        self.bz_color_popover = Gtk.Popover()
        self.bz_color_widget = Gtk.ColorChooserWidget()
        self.bz_color_widget.set_size_request(400, 300)
        self.bz_color_widget.set_rgba(self.bz_current_rgba)
        self.bz_color_popover.set_child(self.bz_color_widget)
        self.bz_color_popover.set_parent(self.bz_color_button)
        
        def on_popover_closed(p):
            self.bz_current_rgba = self.bz_color_widget.get_rgba()
            self._update_color_button_ui(self.bz_color_button, self.bz_current_rgba)
            
        self.bz_color_popover.connect("closed", on_popover_closed)
        
        def on_button_clicked(btn):
            self.bz_color_widget.set_rgba(self.bz_current_rgba)
            self.bz_color_popover.popup()
            
        self.bz_color_button.connect("clicked", on_button_clicked)
        self.bz_color_button.add_css_class("color-btn")
        self.bz_color_button.set_size_request(150, 40)
        self._update_color_button_ui(self.bz_color_button, self.bz_current_rgba)
        row1.append(self.bz_color_button)
        
        lbl_e = Gtk.Label(label="Effects")
        lbl_e.add_css_class("control-label")
        lbl_e.set_margin_start(40)
        row1.append(lbl_e)
        
        self.bz_mode_dropdown = Gtk.DropDown.new_from_strings([
            "Off", "Static Color", "Breathing", "Rhythm", "Rainbow Rhythm", "Jump", "Rainbow Jump", "Round", "Cover"
        ])
        self.bz_mode_dropdown.add_css_class("custom-dropdown")
        self.bz_mode_dropdown.set_hexpand(True)
        row1.append(self.bz_mode_dropdown)
        ctrl_box.append(row1)
        
        self.bz_brightness_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.bz_brightness_scale.set_value(100)
        self.bz_brightness_scale.add_css_class("custom-scale")
        ctrl_box.append(self._create_control_row("Brightness", self.bz_brightness_scale))
        
        self.bz_audio_names = ["Auto (Speaker Output)"]
        self.bz_audio_device_ids = ["auto_speaker"]
        try:
            import sounddevice as sd
            devices = sd.query_devices()
            for i, dev in enumerate(devices):
                if dev['max_input_channels'] > 0:
                    self.bz_audio_device_ids.append(i)
                    self.bz_audio_names.append(dev['name'])
        except Exception:
            pass
            
        self.bz_device_dropdown = Gtk.DropDown.new_from_strings(self.bz_audio_names)
        self.bz_sens_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.bz_sens_scale.set_value(35)
        self.bz_smooth_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.bz_smooth_scale.set_value(0)
        
        apply_btn = Gtk.Button(label="Apply Back Zone Lighting")
        apply_btn.add_css_class("action-btn")
        apply_btn.connect("clicked", self.apply_back_zone_lighting)
        ctrl_box.append(apply_btn)
        
        page.append(ctrl_box)
        self.stack.add_named(page, "backzone")

    def setup_misc_page(self):
        page = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL, spacing=50)
        
        img_box = Gtk.Box()
        img_box.set_hexpand(True)
        img_box.set_valign(Gtk.Align.CENTER)
        img_box.set_halign(Gtk.Align.CENTER)
        img = Gtk.Picture.new_for_filename(os.path.join(self.assets_dir, "GTBook.png"))
        img.set_can_shrink(True)
        img_box.append(img)
        page.append(img_box)
        
        sep = Gtk.Box()
        sep.add_css_class("vert-sep")
        page.append(sep)
        
        ctrl_box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL)
        ctrl_box.set_valign(Gtk.Align.CENTER)
        ctrl_box.set_size_request(450, -1)
        
        lbl_title = Gtk.Label(label="Audio Monitoring Devices")
        lbl_title.add_css_class("header-title")
        lbl_title.set_margin_bottom(20)
        ctrl_box.append(lbl_title)
        
        self.audio_dropdown = Gtk.DropDown.new_from_strings(self.kb_audio_names)
        self.audio_dropdown.add_css_class("custom-dropdown")
        self.audio_dropdown.set_margin_bottom(30)
        self.audio_dropdown.connect("notify::selected", self.on_global_audio_changed)
        ctrl_box.append(self.audio_dropdown)
        
        self.global_sens_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.global_sens_scale.set_value(35)
        self.global_sens_scale.add_css_class("custom-scale")
        self.global_sens_scale.connect("value-changed", self.on_global_sens_changed)
        ctrl_box.append(self._create_control_row("Sensitivity", self.global_sens_scale))
        
        self.global_smooth_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.global_smooth_scale.set_value(0)
        self.global_smooth_scale.add_css_class("custom-scale")
        self.global_smooth_scale.connect("value-changed", self.on_global_smooth_changed)
        ctrl_box.append(self._create_control_row("Smoothness", self.global_smooth_scale))
        
        reset_btn = Gtk.Button(label="Reset Configurations")
        reset_btn.add_css_class("action-btn")
        reset_btn.set_margin_top(20)
        reset_btn.connect("clicked", self.on_reset_settings)
        ctrl_box.append(reset_btn)
        
        page.append(ctrl_box)
        self.stack.add_named(page, "misc")

    def on_global_audio_changed(self, dropdown, pspec):
        val = dropdown.get_selected()
        self.kb_device_dropdown.set_selected(val)
        self.bz_device_dropdown.set_selected(val)

    def on_global_sens_changed(self, scale):
        val = scale.get_value()
        self.kb_sens_scale.set_value(val)
        self.bz_sens_scale.set_value(val)

    def on_global_smooth_changed(self, scale):
        val = scale.get_value()
        self.kb_smooth_scale.set_value(val)
        self.bz_smooth_scale.set_value(val)

"""
    
    with open('/home/yamada/CODE/device_infinix_gtbook/GTControlCenter/controlcenter/window.py', 'w') as f:
        f.write(new_ui + "\n" + logic_content)

rewrite_window()
