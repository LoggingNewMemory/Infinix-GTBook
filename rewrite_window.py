import re

with open("NewControlCenter/controlcenter/window.py", "r") as f:
    content = f.read()

# We will replace `self.setup_lighting_page()` with `self.setup_keyboard_page(); self.setup_back_zone_page()`
content = content.replace("self.setup_lighting_page()", "self.setup_keyboard_page()\n        self.setup_back_zone_page()")

# Find the start of setup_lighting_page
start_idx = content.find("    def setup_lighting_page(self):")

# Find the end of apply_lighting
end_idx = content.find("    def update_monitors(self):")

replacement = """
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
            "Off", "Static Color", "Breathing", "Neon Cycle", "Rainbow", "Flow", "Wave", "Rythm Dance"
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
        page.add(self._create_rhythm_group("kb"))
        
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
        page.add(self._create_rhythm_group("bz"))
        
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
        # Always just set the mode to the standard keyboard modes, or you can switch modes
        # based on individual zones if needed. Currently they share the same list.
        pass

""" + """
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
            self.fan.set_fan_mode(FanCtrlMode.FullSpeedOff)
            if self.btn_office.has_css_class("suggested-action"):
                self.fan.set_fan_mode(FanCtrlMode.OfficeMode)
            elif self.btn_balance.has_css_class("suggested-action"):
                self.fan.set_fan_mode(FanCtrlMode.PerformanceMode)
            elif self.btn_gaming.has_css_class("suggested-action"):
                self.fan.set_fan_mode(FanCtrlMode.GamingMode)
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
                7: KeyboardLightMode.RythmDance
            }
            mapped_mode = mode_map.get(idx, KeyboardLightMode.Always)
            if idx == 0:
                hex_color = "#000000"
            self.lighting.set_keyboard_mode(mapped_mode, hex_color, brightness=brightness, sens=sens, smooth=smooth, audio_device=audio_device)
        elif 1 <= zone <= 4:
            cmd_map = {1: 6, 2: 6, 3: 7, 4: 7}
            offset_map = {1: 0, 2: 4, 3: 0, 4: 4}
            cmd = cmd_map[zone]
            offset = offset_map[zone]
            
            param = offset | idx
            if idx == 0:
                hex_color = "#000000"
            self.lighting.set_zone_mode(cmd, param, hex_color, brightness=brightness)

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

"""

new_content = content[:start_idx] + replacement + content[end_idx:]

with open("NewControlCenter/controlcenter/window.py", "w") as f:
    f.write(new_content)

