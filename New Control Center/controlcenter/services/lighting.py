import logging
from typing import Tuple

from controlcenter.models.tx_buf import (
    Command, KeyboardLightMode, KeyboardLight12Mode, get_keyboard_led_packet, 
    get_keyboard_save_packet, get_device_info_packet, DeviceInfoMode,
    build_packet
)
from controlcenter.services.usb_service import USBService
from controlcenter.services.serial_service import SerialService

logger = logging.getLogger(__name__)

class LightingService:
    def __init__(self, usb_service: USBService, serial_service: SerialService):
        self.usb = usb_service
        self.serial = serial_service

    def _hex_to_rgb(self, hex_color: str) -> Tuple[int, int, int]:
        hex_color = hex_color.lstrip('#')
        if len(hex_color) == 6:
            return tuple(int(hex_color[i:i+2], 16) for i in (0, 2, 4))
        return (0, 0, 0)

    def set_keyboard_brightness(self, level: int):
        """
        Set brightness (0-4). The exact packet for this might be `Light_Auto_Close` or `Light_Open`.
        """
        # C# app sends Command.KeyBoard_Light with param 8 for Open, 7 for Auto_Close
        # but brightness level itself is 'L' (0-4) in the RGB packet.
        # We'll just update the brightness state and re-apply current mode.
        pass

    def set_keyboard_mode(self, mode: KeyboardLightMode, color_hex: str = "#FFFFFF", brightness: int = 255):
        """
        Sets the keyboard lighting mode and color.
        """
        r, g, b = self._hex_to_rgb(color_hex)
        packet = get_keyboard_led_packet(mode.value, r, g, b, brightness)
        
        if self.usb.send_data(packet):
            logger.info(f"Set keyboard mode {mode.name} with color {color_hex} brightness {brightness}")
            return True
        else:
            logger.error(f"Failed to set keyboard mode {mode.name}")
            return False

    def set_zone_mode(self, command_id: int, mode_enum_val: int, color_hex: str = "#FFFFFF", brightness: int = 255):
        r, g, b = self._hex_to_rgb(color_hex)
        packet = build_packet(command_id, mode_enum_val, size=4, r=r, g=g, b=b, l=brightness)
        if self.usb.send_data(packet):
            logger.info(f"Set zone {command_id} mode {mode_enum_val} color {color_hex}")
            return True
        return False
        
    def set_serial_back_zone_mode(self, mode_enum_val: int, color_hex: str = "#FFFFFF", brightness: int = 255):
        r, g, b = self._hex_to_rgb(color_hex)
        from controlcenter.models.tx_buf import get_back_zone_packet
        packet = get_back_zone_packet(mode_enum_val, r, g, b, brightness, speed=1)
        if self.serial.send_data(packet):
            logger.info(f"Set serial back zone mode {mode_enum_val} color {color_hex}")
            return True
        return False

    def save_keyboard_settings(self):
        """
        Save the current keyboard settings to the device memory.
        """
        packet = get_keyboard_save_packet()
        if self.usb.send_data(packet):
            logger.info("Saved keyboard settings to device.")
            return True
        return False
        
    def turn_off(self):
        """
        Turn off keyboard backlight.
        """
        return self.set_keyboard_mode(KeyboardLightMode.LightOFF)
