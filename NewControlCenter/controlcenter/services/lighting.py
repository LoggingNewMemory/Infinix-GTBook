import logging
from typing import Tuple
import threading
import time
import random

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
        self._anim_thread = None
        self._stop_event = threading.Event()

    def start_animation(self, mode, r, g, b, brightness, is_rainbow=False, sens=50, smooth=50):
        self.stop_animation()
        self._stop_event.clear()
        self._anim_thread = threading.Thread(target=self._anim_loop, args=(mode, r, g, b, brightness, is_rainbow, sens, smooth, self._stop_event), daemon=True)
        self._anim_thread.start()

    def stop_animation(self):
        self._stop_event.set()
        if self._anim_thread:
            self._anim_thread.join(timeout=1.0)
            self._anim_thread = None

    def _anim_loop(self, mode, r, g, b, brightness, is_rainbow, sens, smooth, stop_event):
        from controlcenter.models.tx_buf import get_back_zone_packet
        import time
        import colorsys
        try:
            import sounddevice as sd
            import numpy as np
        except ImportError:
            logger.error("sounddevice or numpy not installed. Cannot use real audio rhythm.")
            return

        # 4 Frequency Bands for the Spectrum Analyzer
        current_b1 = 0.0
        current_b2 = 0.0
        current_b3 = 0.0
        current_b4 = 0.0

        # AGC (Automatic Gain Control) Peak Tracking
        peak_b1 = 0.002
        peak_b2 = 0.002
        peak_b3 = 0.002
        peak_b4 = 0.002
        
        rainbow_hue = 0.0

        def audio_callback(indata, frames, time_info, status):
            nonlocal current_b1, current_b2, current_b3, current_b4
            nonlocal peak_b1, peak_b2, peak_b3, peak_b4, rainbow_hue
            if stop_event.is_set():
                raise sd.CallbackStop
            
            # Mix down to mono for frequency analysis
            mono_data = np.mean(indata, axis=1) if indata.shape[1] > 1 else indata[:, 0]
            
            # Apply a Hanning window to reduce spectral leakage
            window = np.hanning(len(mono_data))
            windowed = mono_data * window
            
            # Compute FFT (Fast Fourier Transform)
            fft_result = np.fft.rfft(windowed)
            # Normalize magnitude
            fft_mag = np.abs(fft_result) * 2.0 / len(mono_data)
            
            # 1. Bass:       43Hz - 250Hz   (Bins 1 to 6)
            # 2. Low-Mid:    250Hz - 1000Hz (Bins 6 to 23)
            # 3. High-Mid:   1000Hz - 4000Hz(Bins 23 to 93)
            # 4. Treble:     4000Hz - 16kHz (Bins 93 to 372)
            
            b1_raw = np.mean(fft_mag[1:6])
            b2_raw = np.mean(fft_mag[6:23])
            b3_raw = np.mean(fft_mag[23:93])
            b4_raw = np.mean(fft_mag[93:372])
            
            # Automatic Gain Control (AGC): 
            # Slowly decay the peak tracking, but instantly rise to new loud peaks.
            # We enforce a hard minimum (0.002) so pure background silence doesn't get amplified.
            peak_b1 = max(peak_b1 * 0.99, b1_raw, 0.002)
            peak_b2 = max(peak_b2 * 0.99, b2_raw, 0.002)
            peak_b3 = max(peak_b3 * 0.99, b3_raw, 0.002)
            peak_b4 = max(peak_b4 * 0.99, b4_raw, 0.002)
            
            # Smooth Attack and Decay based on UI (0 to 100)
            # Higher smooth -> lower attack/decay values (slower reaction)
            smooth_val = max(0.0, min(100.0, smooth)) / 100.0
            
            # Map smooth to attack: 1.0 (fast) down to 0.1 (slow)
            attack = 1.0 - (smooth_val * 0.9)
            # Map smooth to decay: 0.5 (fast) down to 0.05 (slow)
            decay = 0.5 - (smooth_val * 0.45)
            
            # Sensitivity (Gain multiplier) based on UI (0 to 100)
            sens_val = max(0.0, min(100.0, sens)) / 50.0
            # Square the sensitivity to give a steep exponential range (0.0004 to 4.0)
            gain = sens_val ** 2.0
            
            v1 = min(255.0, (((b1_raw / peak_b1) ** 1.5) * 255.0 * gain))
            v2 = min(255.0, (((b2_raw / peak_b2) ** 1.5) * 255.0 * gain))
            v3 = min(255.0, (((b3_raw / peak_b3) ** 1.5) * 255.0 * gain))
            v4 = min(255.0, (((b4_raw / peak_b4) ** 1.5) * 255.0 * gain))
            
            current_b1 += (attack if v1 > current_b1 else decay) * (v1 - current_b1)
            current_b2 += (attack if v2 > current_b2 else decay) * (v2 - current_b2)
            current_b3 += (attack if v3 > current_b3 else decay) * (v3 - current_b3)
            current_b4 += (attack if v4 > current_b4 else decay) * (v4 - current_b4)
            
            fd1 = int(current_b1)
            fd2 = int(current_b2)
            fd3 = int(current_b3)
            fd4 = int(current_b4)
            
            if mode == 4: # BackLightCmd.Light_Jump
                overall_vol = int((current_b1 + current_b2 + current_b3 + current_b4) / 4)
                fd1 = overall_vol
                fd2 = 0
                fd3 = 0
                fd4 = overall_vol

            
            if is_rainbow:
                # Cycle hue over time (approx 5 seconds for a full cycle at ~43 fps)
                rainbow_hue = (rainbow_hue + 0.005) % 1.0
                cr, cg, cb = colorsys.hsv_to_rgb(rainbow_hue, 1.0, 1.0)
                cur_r, cur_g, cur_b = int(cr * 255), int(cg * 255), int(cb * 255)
            else:
                cur_r, cur_g, cur_b = r, g, b
            
            packet = get_back_zone_packet(mode, cur_r, cur_g, cur_b, brightness, speed=1, fd1=fd1, fd2=fd2, fd3=fd3, fd4=fd4)
            self.serial.send_data(packet)

        try:
            # Blocksize 1024 = ~23ms updates (super fast ~43 fps for fluid lighting)
            with sd.InputStream(callback=audio_callback, blocksize=1024):
                while not stop_event.is_set():
                    stop_event.wait(0.5)
        except Exception as e:
            logger.error(f"Audio capture failed: {e}")

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

    def set_keyboard_mode(self, mode: KeyboardLightMode, color_hex: str = "#FF0000", brightness: int = 255):
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

    def set_zone_mode(self, command_id: int, mode_enum_val: int, color_hex: str = "#FF0000", brightness: int = 255):
        r, g, b = self._hex_to_rgb(color_hex)
        packet = build_packet(command_id, mode_enum_val, size=4, r=r, g=g, b=b, l=brightness)
        if self.usb.send_data(packet):
            logger.info(f"Set zone {command_id} mode {mode_enum_val} color {color_hex}")
            return True
        return False
        
    def set_serial_back_zone_mode(self, mode_enum_val: int, color_hex: str = "#FF0000", brightness: int = 100, sens: int = 50, smooth: int = 50):
        r, g, b = self._hex_to_rgb(color_hex)
        from controlcenter.models.tx_buf import get_back_zone_packet, BackLightCmd
        
        is_rainbow = False
        if mode_enum_val == 99:
            mode_enum_val = BackLightCmd.Light_Rythm
            is_rainbow = True
        elif mode_enum_val == 98:
            mode_enum_val = BackLightCmd.Light_Jump
            is_rainbow = True
        
        self.stop_animation()
        
        if mode_enum_val in (BackLightCmd.Light_Rythm, BackLightCmd.Light_Jump):
            self.start_animation(mode_enum_val, r, g, b, brightness, is_rainbow, sens, smooth)
            return True
        
        speed = 1
        fd1, fd2, fd3, fd4 = 0, 0, 0, 0
        if mode_enum_val == BackLightCmd.Light_Close:
            speed = 200
        elif mode_enum_val in (BackLightCmd.Light_Round, BackLightCmd.Light_Cover):
            speed = 4
            fd1, fd2, fd3, fd4 = 48, 235, 231, 100
        elif mode_enum_val in (BackLightCmd.Light_Rythm, BackLightCmd.Light_Jump):
            self.start_animation(mode_enum_val, r, g, b, brightness)
            return True
            
        packet = get_back_zone_packet(mode_enum_val, r, g, b, brightness, speed=speed, fd1=fd1, fd2=fd2, fd3=fd3, fd4=fd4)
        if self.serial.send_data(packet):
            logger.info(f"Set serial back zone mode {mode_enum_val} color {color_hex} speed {speed}")
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
