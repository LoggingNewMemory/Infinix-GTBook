import gi
gi.require_version('Gtk', '4.0')
gi.require_version('Adw', '1')
from gi.repository import Gtk, Adw, GLib, Gdk

import logging
from typing import Optional

from controlcenter.models.tx_buf import KeyboardLightMode, KeyboardLight12Mode, BackLightCmd, FanCtrlMode
from controlcenter.services.acpi_wmi import ACPIWmi
from controlcenter.services.usb_service import USBService
from controlcenter.services.serial_service import SerialService
from controlcenter.services.lighting import LightingService
from controlcenter.services.fan_service import FanService
from controlcenter.services.monitor import MonitorService

logger = logging.getLogger(__name__)

class MainWindow(Adw.ApplicationWindow):
    def __init__(self, app):
        super().__init__(application=app, title="BYD Control Center")
        self.set_default_size(1100, 750)
        
        style_manager = Adw.StyleManager.get_default()
        style_manager.set_color_scheme(Adw.ColorScheme.FORCE_DARK)
        
        # Initialize services
        self.wmi = ACPIWmi()
        self.usb = USBService()
        self.usb.connect()
        self.serial = SerialService()
        self.lighting = LightingService(self.usb, self.serial)
        self.fan = FanService(self.usb, self.wmi)
        self.monitor = MonitorService(self.wmi)
        
        self.setup_ui()
        
        # Start monitor update loop
        GLib.timeout_add(2000, self.update_monitors)

    def setup_ui(self):
        css_provider = Gtk.CssProvider()
        css = """
        .stat-card { background-color: alpha(currentColor, 0.05); border-radius: 12px; padding: 24px; border: 1px solid alpha(currentColor, 0.1); }
        .temp-value { font-size: 36pt; font-weight: 900; color: #30b3eb; }
        .temp-label { font-size: 16pt; font-weight: bold; color: alpha(currentColor, 0.5); }
        """
        css_provider.load_from_data(css.encode())
        Gtk.StyleContext.add_provider_for_display(Gdk.Display.get_default(), css_provider, Gtk.STYLE_PROVIDER_PRIORITY_APPLICATION)
        
        # Main layout
        self.box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL)
        self.set_content(self.box)
        
        # HeaderBar
        self.header = Adw.HeaderBar()
        self.header.set_title_widget(Adw.WindowTitle(title="BYD Control Center"))
        self.box.append(self.header)
        
        # ViewStack for tabs
        self.stack = Adw.ViewStack()
        self.stack.set_vexpand(True)
        
        # ViewSwitcher (Tab bar)
        self.switcher_bar = Adw.ViewSwitcherBar()
        self.switcher_bar.set_stack(self.stack)
        self.switcher_bar.set_reveal(True)
        self.box.append(self.switcher_bar)
        
        # Add Pages
        self.setup_performance_page()
        self.setup_lighting_page()
        
        self.box.append(self.stack)

    def setup_performance_page(self):
        page = Adw.PreferencesPage()
        
        # System Mode Group
        mode_group = Adw.PreferencesGroup(title="System Mode", description="Choose the performance profile")
        mode_box = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL)
        mode_box.add_css_class("linked")
        mode_box.set_halign(Gtk.Align.CENTER)
        mode_box.set_margin_top(12)
        mode_box.set_margin_bottom(12)
        
        self.btn_office = Gtk.Button(label="Office")
        self.btn_office.connect("clicked", lambda x: self.set_performance_mode(1))
        
        self.btn_balance = Gtk.Button(label="Balance")
        self.btn_balance.connect("clicked", lambda x: self.set_performance_mode(2))
        
        self.btn_gaming = Gtk.Button(label="Gaming")
        self.btn_gaming.add_css_class("suggested-action") # Default highlight
        self.btn_gaming.connect("clicked", lambda x: self.set_performance_mode(3))
        
        mode_box.append(self.btn_office)
        mode_box.append(self.btn_balance)
        mode_box.append(self.btn_gaming)
        
        mode_group.add(mode_box)
        
        # Max Fan
        max_fan_row = Adw.ActionRow(title="Max Fan")
        max_fan_row.set_subtitle("Forces fans to run at maximum speed unconditionally")
        self.max_fan_switch = Gtk.Switch()
        self.max_fan_switch.set_valign(Gtk.Align.CENTER)
        self.max_fan_switch.connect("state-set", self.on_max_fan_toggled)
        max_fan_row.add_suffix(self.max_fan_switch)
        
        mode_group.add(max_fan_row)
        
        page.add(mode_group)
        
        # Monitoring Group
        mon_group = Adw.PreferencesGroup(title="System Dashboard", description="Real-time component temperatures")
        
        dashboard_box = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL, spacing=24)
        dashboard_box.set_halign(Gtk.Align.CENTER)
        dashboard_box.set_margin_top(24)
        dashboard_box.set_margin_bottom(24)
        
        # CPU Card
        cpu_card = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=8)
        cpu_card.add_css_class("stat-card")
        cpu_card.set_size_request(200, -1)
        lbl_cpu_title = Gtk.Label(label="CPU Temp")
        lbl_cpu_title.add_css_class("temp-label")
        self.lbl_cpu_temp = Gtk.Label(label="-- °C")
        self.lbl_cpu_temp.add_css_class("temp-value")
        cpu_card.append(lbl_cpu_title)
        cpu_card.append(self.lbl_cpu_temp)
        
        # GPU Card
        gpu_card = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=8)
        gpu_card.add_css_class("stat-card")
        gpu_card.set_size_request(200, -1)
        lbl_gpu_title = Gtk.Label(label="GPU Temp")
        lbl_gpu_title.add_css_class("temp-label")
        self.lbl_gpu_temp = Gtk.Label(label="-- °C")
        self.lbl_gpu_temp.add_css_class("temp-value")
        gpu_card.append(lbl_gpu_title)
        gpu_card.append(self.lbl_gpu_temp)
        
        dashboard_box.append(cpu_card)
        dashboard_box.append(gpu_card)
        mon_group.add(dashboard_box)
        page.add(mon_group)
        
        perf_page = self.stack.add_titled(page, "performance", "Performance")
        perf_page.set_icon_name("utilities-system-monitor-symbolic")

    def setup_lighting_page(self):
        page = Adw.PreferencesPage()
        
        group = Adw.PreferencesGroup(title="Lighting Settings")
        
        # Zone selector
        zone_row = Adw.ActionRow(title="Zone")
        self.zone_dropdown = Gtk.DropDown.new_from_strings([
            "Main Keyboard (All)",
            "Keyboard Zone 1 (Left)",
            "Keyboard Zone 2 (Mid-Left)",
            "Keyboard Zone 3 (Mid-Right)",
            "Keyboard Zone 4 (Right)",
            "Back Zone (Laptop Rear)"
        ])
        self.zone_dropdown.connect("notify::selected", self.on_zone_changed)
        zone_row.add_suffix(self.zone_dropdown)
        group.add(zone_row)
        
        # Color picker
        color_row = Adw.ActionRow(title="Color")
        
        self.color_button = Gtk.Button()
        self.color_button.set_size_request(80, 45)
        self.color_button.set_valign(Gtk.Align.CENTER)
        
        self.current_rgba = Gdk.RGBA()
        self.current_rgba.parse("#FF0000")
        
        self.color_popover = Gtk.Popover()
        self.color_widget = Gtk.ColorChooserWidget()
        self.color_widget.set_size_request(600, 450)
        self.color_widget.set_rgba(self.current_rgba)
        
        self.color_popover.set_child(self.color_widget)
        self.color_popover.set_parent(self.color_button)
        
        def on_popover_closed(p):
            self.current_rgba = self.color_widget.get_rgba()
            self._update_color_button_ui(self.current_rgba)
            
        self.color_popover.connect("closed", on_popover_closed)
        
        def on_button_clicked(btn):
            self.color_widget.set_rgba(self.current_rgba)
            self.color_popover.popup()
            
        self.color_button.connect("clicked", on_button_clicked)
        self._update_color_button_ui(self.current_rgba)
        
        color_row.add_suffix(self.color_button)
        group.add(color_row)
        
        # Mode selector
        mode_row = Adw.ActionRow(title="Effect")
        self.mode_dropdown = Gtk.DropDown.new_from_strings([
            "Off", "Static Color", "Breathing", "Neon Cycle", "Rainbow", "Flow", "Wave"
        ])
        self.mode_dropdown.set_selected(1)
        mode_row.add_suffix(self.mode_dropdown)
        group.add(mode_row)
        
        # Brightness slider
        bright_row = Adw.ActionRow(title="Brightness")
        self.brightness_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.brightness_scale.set_value(100)
        self.brightness_scale.set_valign(Gtk.Align.CENTER)
        self.brightness_scale.set_size_request(200, -1)
        bright_row.add_suffix(self.brightness_scale)
        group.add(bright_row)
        
        page.add(group)
        
        # Rhythm Settings Group
        self.rhythm_group = Adw.PreferencesGroup(title="Rhythm & Spectrum Settings")
        
        sens_row = Adw.ActionRow(title="Sensitivity")
        sens_row.set_subtitle("Adjust how strongly LEDs react to sound")
        self.sens_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.sens_scale.set_value(35)
        self.sens_scale.set_valign(Gtk.Align.CENTER)
        self.sens_scale.set_size_request(200, -1)
        sens_row.add_suffix(self.sens_scale)
        self.rhythm_group.add(sens_row)
        
        smooth_row = Adw.ActionRow(title="Smoothness")
        smooth_row.set_subtitle("Adjust the attack/decay speed of the lights")
        self.smooth_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.smooth_scale.set_value(0)
        self.smooth_scale.set_valign(Gtk.Align.CENTER)
        self.smooth_scale.set_size_request(200, -1)
        smooth_row.add_suffix(self.smooth_scale)
        self.rhythm_group.add(smooth_row)
        
        page.add(self.rhythm_group)
        
        # Apply Group
        apply_group = Adw.PreferencesGroup()
        apply_btn = Gtk.Button(label="Apply Lighting")
        apply_btn.set_margin_top(24)
        apply_btn.set_margin_bottom(12)
        apply_btn.add_css_class("suggested-action")
        apply_btn.add_css_class("pill")
        apply_btn.set_halign(Gtk.Align.CENTER)
        apply_btn.set_size_request(200, 40)
        apply_btn.connect("clicked", self.apply_lighting)
        apply_group.add(apply_btn)
        
        page.add(apply_group)
        light_page = self.stack.add_titled(page, "lighting", "Lighting")
        light_page.set_icon_name("keyboard-symbolic")

    def _update_color_button_ui(self, rgba):
        css_provider = Gtk.CssProvider()
        css = f"* {{ background-color: {rgba.to_string()}; background-image: none; border-radius: 6px; }}"
        css_provider.load_from_data(css.encode())
        context = self.color_button.get_style_context()
        context.add_provider(css_provider, Gtk.STYLE_PROVIDER_PRIORITY_USER)

    def on_zone_changed(self, dropdown, pspec):
        zone = dropdown.get_selected()
        if zone < 5:
            self.mode_dropdown.set_model(Gtk.StringList.new(["Off", "Static Color", "Breathing", "Neon Cycle", "Rainbow", "Flow", "Wave"]))
            self.mode_dropdown.set_selected(1)
        elif zone == 5:
            self.mode_dropdown.set_model(Gtk.StringList.new(["Off", "Static Color", "Breathing", "Rhythm", "Rainbow Rhythm", "Jump", "Rainbow Jump", "Round", "Cover"]))
            self.mode_dropdown.set_selected(1)

    def set_performance_mode(self, mode: int):
        self.btn_office.remove_css_class("suggested-action")
        self.btn_balance.remove_css_class("suggested-action")
        self.btn_gaming.remove_css_class("suggested-action")
        
        if mode == 1:
            self.btn_office.add_css_class("suggested-action")
            target_fan_mode = FanCtrlMode.OfficeMode
        elif mode == 2:
            self.btn_balance.add_css_class("suggested-action")
            target_fan_mode = FanCtrlMode.PerformanceMode
        else:
            self.btn_gaming.add_css_class("suggested-action")
            target_fan_mode = FanCtrlMode.GamingMode
            
        self.fan.set_performance_mode(mode)
        
        if hasattr(self, 'max_fan_switch') and self.max_fan_switch.get_active():
            self.fan.set_fan_mode(FanCtrlMode.FullSpeed)
        else:
            self.fan.set_fan_mode(target_fan_mode)

    def on_max_fan_toggled(self, switch, state):
        if state:
            self.fan.set_fan_mode(FanCtrlMode.FullSpeed)
        else:
            # We must explicitly send the FullSpeedOff packet to disengage the hardware override
            self.fan.set_fan_mode(FanCtrlMode.FullSpeedOff)
            
            # Then restore the proper mode
            if self.btn_office.has_css_class("suggested-action"):
                self.fan.set_fan_mode(FanCtrlMode.OfficeMode)
            elif self.btn_balance.has_css_class("suggested-action"):
                self.fan.set_fan_mode(FanCtrlMode.PerformanceMode)
            elif self.btn_gaming.has_css_class("suggested-action"):
                self.fan.set_fan_mode(FanCtrlMode.GamingMode)
        return False

    def apply_lighting(self, btn):
        color = self.current_rgba
        hex_color = f"#{int(color.red*255):02x}{int(color.green*255):02x}{int(color.blue*255):02x}"
        
        idx = self.mode_dropdown.get_selected()
        zone = self.zone_dropdown.get_selected()
        
        # Scale 0-100 percentage to 0-255 hardware range for maximum brightness
        brightness_pct = self.brightness_scale.get_value()
        brightness = int((brightness_pct / 100.0) * 255)
        
        sens = int(self.sens_scale.get_value())
        smooth = int(self.smooth_scale.get_value())
        
        if zone == 0:
            # Main Keyboard
            mode_map = {
                0: KeyboardLightMode.LightOFF,
                1: KeyboardLightMode.Always,
                2: KeyboardLightMode.Breath,
                3: KeyboardLightMode.GradualChange,
                4: KeyboardLightMode.RainBow,
                5: KeyboardLightMode.Flow,
                6: KeyboardLightMode.Wave
            }
            mapped_mode = mode_map.get(idx, KeyboardLightMode.Always)
            if idx == 0:
                hex_color = "#000000"
            self.lighting.set_keyboard_mode(mapped_mode, hex_color, brightness=brightness)
        elif 1 <= zone <= 4:
            # Individual Keyboard Zones
            cmd_map = {1: 6, 2: 6, 3: 7, 4: 7}
            offset_map = {1: 0, 2: 4, 3: 0, 4: 4}
            cmd = cmd_map[zone]
            offset = offset_map[zone]
            
            param = offset | idx
            if idx == 0:
                hex_color = "#000000"
                
            self.lighting.set_zone_mode(cmd, param, hex_color, brightness=brightness)
        elif zone == 5:
            # Back Zone (Serial ttyS4)
            mode_map_back = {
                0: BackLightCmd.Light_Close,
                1: BackLightCmd.Light_AlwaysOn,
                2: BackLightCmd.Light_Breath,
                3: BackLightCmd.Light_Rythm,
                4: 99, # 99 is our custom Rainbow Rhythm
                5: BackLightCmd.Light_Jump,
                6: 98, # 98 is our custom Rainbow Jump
                7: BackLightCmd.Light_Round,
                8: BackLightCmd.Light_Cover
            }
            mapped_mode = mode_map_back.get(idx, BackLightCmd.Light_AlwaysOn)
            if idx == 0:
                hex_color = "#000000"
            self.lighting.set_serial_back_zone_mode(mapped_mode, hex_color, brightness=brightness, sens=sens, smooth=smooth)

    def update_monitors(self):
        cpu_temp = self.monitor.get_cpu_temp()
        gpu_temp = self.monitor.get_gpu_temp()
        
        self.lbl_cpu_temp.set_label(f"{cpu_temp} °C" if cpu_temp > 0 else "N/A")
        self.lbl_gpu_temp.set_label(f"{gpu_temp} °C" if gpu_temp > 0 else "N/A")
        
        return True # Continue timer
