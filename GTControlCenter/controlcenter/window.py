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
from controlcenter.services.config import ConfigManager

logger = logging.getLogger(__name__)


class MainWindow(Adw.ApplicationWindow):
    def __init__(self, app):
        super().__init__(application=app, title="GT Control Center")
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
        self.config_mgr = ConfigManager()
        
        self.setup_ui()
        self.load_settings()
        self.apply_all_settings()
        
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
        self.header.set_title_widget(Adw.WindowTitle(title="GT Control Center"))
        
        # Reset button in header bar
        reset_btn = Gtk.Button(label="Reset Settings")
        reset_btn.set_tooltip_text("Reset all settings to default")
        reset_btn.connect("clicked", self.on_reset_settings)
        self.header.pack_end(reset_btn)
        
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
        self.setup_keyboard_page()
        self.setup_back_zone_page()
        
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


    def _create_rhythm_group(self, prefix):
        group = Adw.PreferencesGroup(title="Rhythm & Spectrum Settings")
        
        device_row = Adw.ActionRow(title="Audio Device")
        device_row.set_subtitle("Select which device to monitor")
        
        audio_device_ids = ["auto_speaker"]
        device_names = ["Auto (Speaker Output)"]
        try:
            import sounddevice as sd
            devices = sd.query_devices()
            for i, dev in enumerate(devices):
                if dev['max_input_channels'] > 0:
                    audio_device_ids.append(i)
                    device_names.append(dev['name'])
        except Exception as e:
            pass
            
        dropdown = Gtk.DropDown.new_from_strings(device_names)
        dropdown.set_selected(0)
        device_row.add_suffix(dropdown)
        group.add(device_row)
        
        sens_row = Adw.ActionRow(title="Sensitivity")
        sens_row.set_subtitle("Adjust how strongly LEDs react to sound")
        sens_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        sens_scale.set_value(35)
        sens_scale.set_valign(Gtk.Align.CENTER)
        sens_scale.set_size_request(200, -1)
        sens_row.add_suffix(sens_scale)
        group.add(sens_row)
        
        smooth_row = Adw.ActionRow(title="Smoothness")
        smooth_row.set_subtitle("Adjust the attack/decay speed of the lights")
        smooth_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        smooth_scale.set_value(0)
        smooth_scale.set_valign(Gtk.Align.CENTER)
        smooth_scale.set_size_request(200, -1)
        smooth_row.add_suffix(smooth_scale)
        group.add(smooth_row)
        
        setattr(self, f"{prefix}_audio_device_ids", audio_device_ids)
        setattr(self, f"{prefix}_device_dropdown", dropdown)
        setattr(self, f"{prefix}_sens_scale", sens_scale)
        setattr(self, f"{prefix}_smooth_scale", smooth_scale)
        
        return group

    def setup_keyboard_page(self):
        page = Adw.PreferencesPage()
        
        group = Adw.PreferencesGroup(title="Keyboard Lighting")
        
        zone_row = Adw.ActionRow(title="Zone")
        self.kb_zone_dropdown = Gtk.DropDown.new_from_strings([
            "Main Keyboard (All)",
            "Keyboard Zone 1 (Left)",
            "Keyboard Zone 2 (Mid-Left)",
            "Keyboard Zone 3 (Mid-Right)",
            "Keyboard Zone 4 (Right)"
        ])
        self.kb_zone_dropdown.connect("notify::selected", self.on_kb_zone_changed)
        zone_row.add_suffix(self.kb_zone_dropdown)
        group.add(zone_row)
        
        color_row = Adw.ActionRow(title="Color")
        self.kb_color_button = Gtk.Button()
        self.kb_color_button.set_size_request(80, 45)
        self.kb_color_button.set_valign(Gtk.Align.CENTER)
        
        self.kb_current_rgba = Gdk.RGBA()
        self.kb_current_rgba.parse("#FF0000")
        
        self.kb_color_popover = Gtk.Popover()
        self.kb_color_widget = Gtk.ColorChooserWidget()
        self.kb_color_widget.set_size_request(600, 450)
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
        self._update_color_button_ui(self.kb_color_button, self.kb_current_rgba)
        color_row.add_suffix(self.kb_color_button)
        group.add(color_row)
        
        mode_row = Adw.ActionRow(title="Effect")
        self.kb_mode_dropdown = Gtk.DropDown.new_from_strings([
            "Off", "Static Color", "Breathing", "Neon Cycle", "Rainbow", "Flow", "Wave", "Rhythm Dance"
        ])
        self.kb_mode_dropdown.set_selected(1)
        mode_row.add_suffix(self.kb_mode_dropdown)
        group.add(mode_row)
        
        bright_row = Adw.ActionRow(title="Brightness")
        self.kb_brightness_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.kb_brightness_scale.set_value(100)
        self.kb_brightness_scale.set_valign(Gtk.Align.CENTER)
        self.kb_brightness_scale.set_size_request(200, -1)
        bright_row.add_suffix(self.kb_brightness_scale)
        group.add(bright_row)
        
        page.add(group)
        
        self.kb_rhythm_group = self._create_rhythm_group("kb")
        page.add(self.kb_rhythm_group)
        self.kb_rhythm_group.set_visible(False)
        
        def on_kb_mode_changed(dropdown, pspec):
            # 7 is Rhythm Dance
            is_rhythm = dropdown.get_selected() == 7
            self.kb_rhythm_group.set_visible(is_rhythm)
        self.kb_mode_dropdown.connect("notify::selected", on_kb_mode_changed)

        
        apply_group = Adw.PreferencesGroup()
        apply_btn = Gtk.Button(label="Apply Keyboard Lighting")
        apply_btn.set_margin_top(24)
        apply_btn.set_margin_bottom(12)
        apply_btn.add_css_class("suggested-action")
        apply_btn.add_css_class("pill")
        apply_btn.set_halign(Gtk.Align.CENTER)
        apply_btn.set_size_request(250, 40)
        apply_btn.connect("clicked", self.apply_keyboard_lighting)
        apply_group.add(apply_btn)
        
        page.add(apply_group)
        kb_page = self.stack.add_titled(page, "keyboard", "Keyboard")
        kb_page.set_icon_name("keyboard-symbolic")

    def setup_back_zone_page(self):
        page = Adw.PreferencesPage()
        
        group = Adw.PreferencesGroup(title="Back Zone Lighting")
        
        color_row = Adw.ActionRow(title="Color")
        self.bz_color_button = Gtk.Button()
        self.bz_color_button.set_size_request(80, 45)
        self.bz_color_button.set_valign(Gtk.Align.CENTER)
        
        self.bz_current_rgba = Gdk.RGBA()
        self.bz_current_rgba.parse("#FF0000")
        
        self.bz_color_popover = Gtk.Popover()
        self.bz_color_widget = Gtk.ColorChooserWidget()
        self.bz_color_widget.set_size_request(600, 450)
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
        self._update_color_button_ui(self.bz_color_button, self.bz_current_rgba)
        color_row.add_suffix(self.bz_color_button)
        group.add(color_row)
        
        mode_row = Adw.ActionRow(title="Effect")
        self.bz_mode_dropdown = Gtk.DropDown.new_from_strings([
            "Off", "Static Color", "Breathing", "Rhythm", "Rainbow Rhythm", "Jump", "Rainbow Jump", "Round", "Cover"
        ])
        self.bz_mode_dropdown.set_selected(1)
        mode_row.add_suffix(self.bz_mode_dropdown)
        group.add(mode_row)
        
        bright_row = Adw.ActionRow(title="Brightness")
        self.bz_brightness_scale = Gtk.Scale.new_with_range(Gtk.Orientation.HORIZONTAL, 0, 100, 1)
        self.bz_brightness_scale.set_value(100)
        self.bz_brightness_scale.set_valign(Gtk.Align.CENTER)
        self.bz_brightness_scale.set_size_request(200, -1)
        bright_row.add_suffix(self.bz_brightness_scale)
        group.add(bright_row)
        
        page.add(group)
        
        self.bz_rhythm_group = self._create_rhythm_group("bz")
        page.add(self.bz_rhythm_group)
        self.bz_rhythm_group.set_visible(False)
        
        def on_bz_mode_changed(dropdown, pspec):
            # 3: Rhythm, 4: Rainbow Rhythm, 5: Jump, 6: Rainbow Jump
            is_rhythm = dropdown.get_selected() in (3, 4, 5, 6)
            self.bz_rhythm_group.set_visible(is_rhythm)
        self.bz_mode_dropdown.connect("notify::selected", on_bz_mode_changed)

        
        apply_group = Adw.PreferencesGroup()
        apply_btn = Gtk.Button(label="Apply Back Zone Lighting")
        apply_btn.set_margin_top(24)
        apply_btn.set_margin_bottom(12)
        apply_btn.add_css_class("suggested-action")
        apply_btn.add_css_class("pill")
        apply_btn.set_halign(Gtk.Align.CENTER)
        apply_btn.set_size_request(250, 40)
        apply_btn.connect("clicked", self.apply_back_zone_lighting)
        apply_group.add(apply_btn)
        
        page.add(apply_group)
        bz_page = self.stack.add_titled(page, "backzone", "Back Zone")
        bz_page.set_icon_name("video-display-symbolic")

    def _update_color_button_ui(self, button, rgba):
        css_provider = Gtk.CssProvider()
        css = f"* {{ background-color: {rgba.to_string()}; background-image: none; border-radius: 6px; }}"
        css_provider.load_from_data(css.encode())
        context = button.get_style_context()
        context.add_provider(css_provider, Gtk.STYLE_PROVIDER_PRIORITY_USER)

    def on_kb_zone_changed(self, dropdown, pspec):
        zone = dropdown.get_selected()
        zone_str = str(zone)
        zones_config = self.config_mgr.config.get("keyboard_zones", {})
        
        if zone_str in zones_config:
            kb = zones_config[zone_str]
            self.kb_mode_dropdown.set_selected(kb.get("mode", 1))
            self.kb_current_rgba.parse(kb.get("color", "#FF0000"))
            self._update_color_button_ui(self.kb_color_button, self.kb_current_rgba)
            self.kb_brightness_scale.set_value(kb.get("brightness", 100))
            if hasattr(self, 'kb_device_dropdown') and hasattr(self, 'kb_audio_device_ids'):
                device_idx = kb.get("audio_device", 0)
                if device_idx < len(self.kb_audio_device_ids):
                    self.kb_device_dropdown.set_selected(device_idx)
            if hasattr(self, 'kb_sens_scale'):
                self.kb_sens_scale.set_value(kb.get("sens", 35))
            if hasattr(self, 'kb_smooth_scale'):
                self.kb_smooth_scale.set_value(kb.get("smooth", 0))


    def set_performance_mode(self, mode: int, save=True):
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
            
        if save:
            self.config_mgr.config["performance"]["mode"] = mode
            self.config_mgr.save()

    def on_max_fan_toggled(self, switch, state):
        if state:
            self.fan.set_fan_mode(FanCtrlMode.FullSpeed)
        else:
            self.fan.set_fan_mode(FanCtrlMode.FullSpeedOff)
            if self.btn_office.has_css_class("suggested-action"):
                self.fan.set_fan_mode(FanCtrlMode.OfficeMode)
            elif self.btn_balance.has_css_class("suggested-action"):
                self.fan.set_fan_mode(FanCtrlMode.PerformanceMode)
            elif self.btn_gaming.has_css_class("suggested-action"):
                self.fan.set_fan_mode(FanCtrlMode.GamingMode)
                
        self.config_mgr.config["performance"]["max_fan"] = state
        self.config_mgr.save()
        return False

    def apply_keyboard_lighting(self, btn):
        color = self.kb_current_rgba
        hex_color = f"#{int(color.red*255):02x}{int(color.green*255):02x}{int(color.blue*255):02x}"
        
        idx = self.kb_mode_dropdown.get_selected()
        zone = self.kb_zone_dropdown.get_selected()
        
        brightness_pct = self.kb_brightness_scale.get_value()
        brightness = int((brightness_pct / 100.0) * 255)
        
        sens = int(self.kb_sens_scale.get_value())
        smooth = int(self.kb_smooth_scale.get_value())
        
        device_idx = self.kb_device_dropdown.get_selected()
        audio_device = self.kb_audio_device_ids[device_idx] if hasattr(self, 'kb_audio_device_ids') and device_idx < len(self.kb_audio_device_ids) else None
        
        if zone == 0:
            mode_map = {
                0: KeyboardLightMode.LightOFF,
                1: KeyboardLightMode.Always,
                2: KeyboardLightMode.Breath,
                3: KeyboardLightMode.GradualChange,
                4: KeyboardLightMode.RainBow,
                5: KeyboardLightMode.Flow,
                6: KeyboardLightMode.Wave,
                7: KeyboardLightMode.RhythmDance
            }
            mapped_mode = mode_map.get(idx, KeyboardLightMode.Always)
            if idx == 0:
                hex_color = "#000000"
            self.lighting.set_keyboard_mode(mapped_mode, hex_color, brightness=brightness, sens=sens, smooth=smooth, audio_device=audio_device)
            
            # Sync the effect to all individual zones so they don't revert if a specific zone is later configured
            if idx <= 4:
                cmd_map = {1: 6, 2: 6, 3: 7, 4: 7}
                offset_map = {1: 0, 2: 4, 3: 0, 4: 4}
                zone_mode_map = {0: 0, 1: 0, 2: 1, 3: 2, 4: 3}
                zone_mode = zone_mode_map.get(idx, 0)
                sync_color = "#00FF00" if idx in (3, 4) else hex_color
                for z in range(1, 5):
                    self.lighting.set_zone_mode(cmd_map[z], offset_map[z] | zone_mode, sync_color, brightness=brightness)
        elif 1 <= zone <= 4:
            cmd_map = {1: 6, 2: 6, 3: 7, 4: 7}
            offset_map = {1: 0, 2: 4, 3: 0, 4: 4}
            cmd = cmd_map[zone]
            offset = offset_map[zone]
            
            zone_mode_map = {
                0: 0, # Off -> Always (black)
                1: 0, # Static Color -> Always
                2: 1, # Breathing -> Breath
                3: 2, # Neon Cycle -> GradualChange
                4: 3  # Rainbow -> RainBow
            }
            zone_mode = zone_mode_map.get(idx, 0)
            
            param = offset | zone_mode
            if idx == 0:
                hex_color = "#000000"
            elif idx in (3, 4):
                hex_color = "#00FF00"  # Hardware might require this specific color for animations
            self.lighting.set_zone_mode(cmd, param, hex_color, brightness=brightness)
            
        if "keyboard_zones" not in self.config_mgr.config:
            self.config_mgr.config["keyboard_zones"] = {}
            
        zone_str = str(zone)
        zone_settings = {
            "mode": idx,
            "color": hex_color if idx != 0 else f"#{int(color.red*255):02x}{int(color.green*255):02x}{int(color.blue*255):02x}",
            "brightness": brightness_pct,
            "audio_device": device_idx,
            "sens": sens,
            "smooth": smooth
        }
        self.config_mgr.config["keyboard_zones"][zone_str] = zone_settings
        
        if zone == 0:
            for z in range(1, 5):
                self.config_mgr.config["keyboard_zones"][str(z)] = zone_settings.copy()

        self.config_mgr.config["keyboard"].update(zone_settings)
        self.config_mgr.config["keyboard"]["zone"] = zone
        self.config_mgr.save()

    def apply_back_zone_lighting(self, btn):
        color = self.bz_current_rgba
        hex_color = f"#{int(color.red*255):02x}{int(color.green*255):02x}{int(color.blue*255):02x}"
        
        idx = self.bz_mode_dropdown.get_selected()
        
        brightness_pct = self.bz_brightness_scale.get_value()
        brightness = int((brightness_pct / 100.0) * 255)
        
        sens = int(self.bz_sens_scale.get_value())
        smooth = int(self.bz_smooth_scale.get_value())
        
        device_idx = self.bz_device_dropdown.get_selected()
        audio_device = self.bz_audio_device_ids[device_idx] if hasattr(self, 'bz_audio_device_ids') and device_idx < len(self.bz_audio_device_ids) else None
        
        mode_map_back = {
            0: BackLightCmd.Light_Close,
            1: BackLightCmd.Light_AlwaysOn,
            2: BackLightCmd.Light_Breath,
            3: BackLightCmd.Light_Rythm,
            4: 99,
            5: BackLightCmd.Light_Jump,
            6: 98,
            7: BackLightCmd.Light_Round,
            8: BackLightCmd.Light_Cover
        }
        mapped_mode = mode_map_back.get(idx, BackLightCmd.Light_AlwaysOn)
        if idx == 0:
            hex_color = "#000000"
        self.lighting.set_serial_back_zone_mode(mapped_mode, hex_color, brightness=brightness, sens=sens, smooth=smooth, audio_device=audio_device)

        self.config_mgr.config["backzone"].update({
            "mode": idx,
            "color": hex_color if idx != 0 else f"#{int(color.red*255):02x}{int(color.green*255):02x}{int(color.blue*255):02x}",
            "brightness": brightness_pct,
            "audio_device": device_idx,
            "sens": sens,
            "smooth": smooth
        })
        self.config_mgr.save()

    def update_monitors(self):
        cpu_temp = self.monitor.get_cpu_temp()
        gpu_temp = self.monitor.get_gpu_temp()
        
        self.lbl_cpu_temp.set_label(f"{cpu_temp} °C" if cpu_temp > 0 else "N/A")
        self.lbl_gpu_temp.set_label(f"{gpu_temp} °C" if gpu_temp > 0 else "N/A")
        
        return True # Continue timer

    def load_settings(self):
        conf = self.config_mgr.config
        
        # Performance
        perf = conf.get("performance", {})
        self.max_fan_switch.set_active(perf.get("max_fan", False))

        # Keyboard
        kb = conf.get("keyboard", {})
        if hasattr(self, 'kb_zone_dropdown'):
            self.kb_zone_dropdown.set_selected(kb.get("zone", 0))
            self.kb_mode_dropdown.set_selected(kb.get("mode", 1))
            self.kb_current_rgba.parse(kb.get("color", "#FF0000"))
            self._update_color_button_ui(self.kb_color_button, self.kb_current_rgba)
            self.kb_brightness_scale.set_value(kb.get("brightness", 100))
            if hasattr(self, 'kb_device_dropdown') and hasattr(self, 'kb_audio_device_ids'):
                device_idx = kb.get("audio_device", 0)
                if device_idx < len(self.kb_audio_device_ids):
                    self.kb_device_dropdown.set_selected(device_idx)
            if hasattr(self, 'kb_sens_scale'):
                self.kb_sens_scale.set_value(kb.get("sens", 35))
            if hasattr(self, 'kb_smooth_scale'):
                self.kb_smooth_scale.set_value(kb.get("smooth", 0))

        # Back Zone
        bz = conf.get("backzone", {})
        if hasattr(self, 'bz_mode_dropdown'):
            self.bz_mode_dropdown.set_selected(bz.get("mode", 1))
            self.bz_current_rgba.parse(bz.get("color", "#FF0000"))
            self._update_color_button_ui(self.bz_color_button, self.bz_current_rgba)
            self.bz_brightness_scale.set_value(bz.get("brightness", 100))
            if hasattr(self, 'bz_device_dropdown') and hasattr(self, 'bz_audio_device_ids'):
                device_idx = bz.get("audio_device", 0)
                if device_idx < len(self.bz_audio_device_ids):
                    self.bz_device_dropdown.set_selected(device_idx)
            if hasattr(self, 'bz_sens_scale'):
                self.bz_sens_scale.set_value(bz.get("sens", 35))
            if hasattr(self, 'bz_smooth_scale'):
                self.bz_smooth_scale.set_value(bz.get("smooth", 0))

    def apply_all_settings(self):
        conf = self.config_mgr.config
        perf = conf.get("performance", {})
        self.set_performance_mode(perf.get("mode", 2), save=False)
        self.apply_keyboard_lighting(None)
        self.apply_back_zone_lighting(None)

    def on_reset_settings(self, btn):
        dialog = Adw.MessageDialog(
            transient_for=self,
            heading="Reset Settings",
            body="Are you sure you want to reset all settings to their default values? This cannot be undone."
        )
        dialog.add_response("cancel", "Cancel")
        dialog.add_response("reset", "Reset")
        dialog.set_response_appearance("reset", Adw.ResponseAppearance.DESTRUCTIVE)
        
        def on_response(dlg, response):
            if response == "reset":
                self.config_mgr.reset()
                self.load_settings()
                self.apply_all_settings()
                
        dialog.connect("response", on_response)
        dialog.present()

