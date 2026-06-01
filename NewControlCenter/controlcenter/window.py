import gi
gi.require_version('Gtk', '4.0')
gi.require_version('Adw', '1')
from gi.repository import Gtk, Adw, GLib

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
        self.set_default_size(900, 600)
        
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
        # Main layout
        self.box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL)
        self.set_content(self.box)
        
        # HeaderBar
        self.header = Adw.HeaderBar()
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
        mode_group = Adw.PreferencesGroup(title="System Mode")
        mode_box = Gtk.Box(orientation=Gtk.Orientation.HORIZONTAL, spacing=12)
        mode_box.set_halign(Gtk.Align.CENTER)
        mode_box.set_margin_top(12)
        mode_box.set_margin_bottom(12)
        
        self.btn_office = Gtk.Button(label="Office")
        self.btn_office.add_css_class("pill")
        self.btn_office.connect("clicked", lambda x: self.set_performance_mode(1))
        
        self.btn_balance = Gtk.Button(label="Balance")
        self.btn_balance.add_css_class("pill")
        self.btn_balance.connect("clicked", lambda x: self.set_performance_mode(2))
        
        self.btn_gaming = Gtk.Button(label="Gaming")
        self.btn_gaming.add_css_class("pill")
        self.btn_gaming.add_css_class("suggested-action") # Default highlight
        self.btn_gaming.connect("clicked", lambda x: self.set_performance_mode(3))
        
        mode_box.append(self.btn_office)
        mode_box.append(self.btn_balance)
        mode_box.append(self.btn_gaming)
        
        mode_group.add(mode_box)
        page.add(mode_group)
        
        # Monitoring Group
        mon_group = Adw.PreferencesGroup(title="System Status")
        
        self.lbl_cpu_temp = Gtk.Label(label="CPU Temp: -- °C")
        self.lbl_cpu_temp.set_halign(Gtk.Align.START)
        self.lbl_gpu_temp = Gtk.Label(label="GPU Temp: -- °C")
        self.lbl_gpu_temp.set_halign(Gtk.Align.START)
        
        mon_box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=6)
        mon_box.set_margin_top(12)
        mon_box.set_margin_bottom(12)
        mon_box.set_margin_start(12)
        mon_box.append(self.lbl_cpu_temp)
        mon_box.append(self.lbl_gpu_temp)
        
        mon_group.add(mon_box)
        page.add(mon_group)
        
        self.stack.add_titled(page, "performance", "Performance")

    def setup_lighting_page(self):
        page = Adw.PreferencesPage()
        
        group = Adw.PreferencesGroup(title="Lighting Settings")
        
        # Zone selector
        zone_row = Adw.ActionRow(title="Zone")
        self.zone_dropdown = Gtk.DropDown.new_from_strings([
            "Keyboard", 
            "Back Zone"
        ])
        zone_row.add_suffix(self.zone_dropdown)
        group.add(zone_row)
        
        # Color picker
        color_row = Adw.ActionRow(title="Color")
        self.color_dialog = Gtk.ColorDialog()
        self.color_button = Gtk.ColorDialogButton(dialog=self.color_dialog)
        color_row.add_suffix(self.color_button)
        group.add(color_row)
        
        # Mode selector
        mode_row = Adw.ActionRow(title="Effect")
        self.mode_dropdown = Gtk.DropDown.new_from_strings([
            "Off", "Static", "Breathing", "Gradual Change", "Rainbow", "Flow", "Wave"
        ])
        mode_row.add_suffix(self.mode_dropdown)
        group.add(mode_row)
        
        # Apply Button
        apply_btn = Gtk.Button(label="Apply Lighting")
        apply_btn.set_margin_top(12)
        apply_btn.add_css_class("suggested-action")
        apply_btn.connect("clicked", self.apply_lighting)
        group.add(apply_btn)
        
        page.add(group)
        self.stack.add_titled(page, "lighting", "Lighting")

    def set_performance_mode(self, mode: int):
        self.btn_office.remove_css_class("suggested-action")
        self.btn_balance.remove_css_class("suggested-action")
        self.btn_gaming.remove_css_class("suggested-action")
        
        if mode == 1:
            self.btn_office.add_css_class("suggested-action")
            self.fan.set_fan_mode(FanCtrlMode.OfficeMode)
        elif mode == 2:
            self.btn_balance.add_css_class("suggested-action")
            self.fan.set_fan_mode(FanCtrlMode.PerformanceMode)
        else:
            self.btn_gaming.add_css_class("suggested-action")
            self.fan.set_fan_mode(FanCtrlMode.GamingMode)
            
        self.fan.set_performance_mode(mode)

    def apply_lighting(self, btn):
        color = self.color_button.get_rgba()
        hex_color = f"#{int(color.red*255):02x}{int(color.green*255):02x}{int(color.blue*255):02x}"
        
        idx = self.mode_dropdown.get_selected()
        zone = self.zone_dropdown.get_selected()
        
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
            from controlcenter.models.tx_buf import Command
            self.lighting.set_zone_mode(Command.KeyBoard_Light, mapped_mode, hex_color, brightness=255)
            
        elif zone == 1:
            # Back Zone (Serial ttyS4)
            mode_map_back = {
                0: BackLightCmd.Light_Close,
                1: BackLightCmd.Light_AlwaysOn,
                2: BackLightCmd.Light_Breath,
                3: BackLightCmd.SliceMode, # Equivalent to Gradual
                4: BackLightCmd.Light_Round, # Equivalent to Rainbow
                5: BackLightCmd.Light_Cover, # Flow/Cover
                6: BackLightCmd.Light_Jump
            }
            mapped_mode = mode_map_back.get(idx, BackLightCmd.Light_AlwaysOn)
            if idx == 0:
                hex_color = "#000000"
            self.lighting.set_serial_back_zone_mode(mapped_mode, hex_color, brightness=100)

    def update_monitors(self):
        cpu_temp = self.monitor.get_cpu_temp()
        gpu_temp = self.monitor.get_gpu_temp()
        
        self.lbl_cpu_temp.set_label(f"CPU Temp: {cpu_temp} °C" if cpu_temp > 0 else "CPU Temp: N/A")
        self.lbl_gpu_temp.set_label(f"GPU Temp: {gpu_temp} °C" if gpu_temp > 0 else "GPU Temp: N/A")
        
        return True # Continue timer
