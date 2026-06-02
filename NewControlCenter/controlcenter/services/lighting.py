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
        
        self.kb_anim = None
        self.bz_anim = None
        self.audio_device = None

    def update_animations(self):
        needs_thread = self.kb_anim is not None or self.bz_anim is not None
        if needs_thread:
            self.stop_animation()
            self._stop_event.clear()
            self._anim_thread = threading.Thread(target=self._anim_loop, args=(self._stop_event,), daemon=True)
            self._anim_thread.start()
        else:
            self.stop_animation()

    def stop_animation(self):
        self._stop_event.set()
        if self._anim_thread:
            self._anim_thread.join(timeout=1.0)
            self._anim_thread = None

    def _anim_loop(self, stop_event):
        from controlcenter.models.tx_buf import get_back_zone_packet
        import time
        import colorsys
        import os
        import subprocess
        
        audio_device = self.audio_device
        
        if audio_device == "auto_speaker":
            try:
                out = subprocess.check_output(["pactl", "info"], text=True)
                default_sink = None
                for line in out.splitlines():
                    if line.startswith("Default Sink:"):
                        default_sink = line.split(":", 1)[1].strip()
                        break
                if default_sink:
                    os.environ["PULSE_SOURCE"] = default_sink + ".monitor"
            except Exception as e:
                logger.error(f"Failed to get pulseaudio sink: {e}")
            audio_device = None
        elif audio_device is not None:
            os.environ.pop("PULSE_SOURCE", None)
            
        try:
            import sounddevice as sd
            import numpy as np
        except ImportError:
            logger.error("sounddevice or numpy not installed. Cannot use real audio rhythm.")
            return

        current_b1 = 0.0
        current_b2 = 0.0
        current_b3 = 0.0
        current_b4 = 0.0

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
                
            active_anim = self.bz_anim or self.kb_anim
            if not active_anim:
                return
                
            sens = active_anim["sens"]
            smooth = active_anim["smooth"]
            
            mono_data = np.mean(indata, axis=1) if indata.shape[1] > 1 else indata[:, 0]
            window = np.hanning(len(mono_data))
            windowed = mono_data * window
            fft_result = np.fft.rfft(windowed)
            fft_mag = np.abs(fft_result) * 2.0 / len(mono_data)
            
            b1_raw = np.mean(fft_mag[1:6])
            b2_raw = np.mean(fft_mag[6:23])
            b3_raw = np.mean(fft_mag[23:93])
            b4_raw = np.mean(fft_mag[93:372])
            
            peak_b1 = max(peak_b1 * 0.99, b1_raw, 0.002)
            peak_b2 = max(peak_b2 * 0.99, b2_raw, 0.002)
            peak_b3 = max(peak_b3 * 0.99, b3_raw, 0.002)
            peak_b4 = max(peak_b4 * 0.99, b4_raw, 0.002)
            
            smooth_val = max(0.0, min(100.0, smooth)) / 100.0
            attack = 1.0 - (smooth_val * 0.9)
            decay = 0.5 - (smooth_val * 0.45)
            
            sens_val = max(0.0, min(100.0, sens)) / 50.0
            gain = sens_val ** 2.0
            
            v1 = min(255.0, (((b1_raw / peak_b1) ** 1.5) * 255.0 * gain))
            v2 = min(255.0, (((b2_raw / peak_b2) ** 1.5) * 255.0 * gain))
            v3 = min(255.0, (((b3_raw / peak_b3) ** 1.5) * 255.0 * gain))
            v4 = min(255.0, (((b4_raw / peak_b4) ** 1.5) * 255.0 * gain))
            
            current_b1 += (attack if v1 > current_b1 else decay) * (v1 - current_b1)
            current_b2 += (attack if v2 > current_b2 else decay) * (v2 - current_b2)
            current_b3 += (attack if v3 > current_b3 else decay) * (v3 - current_b3)
            current_b4 += (attack if v4 > current_b4 else decay) * (v4 - current_b4)
            
            if (self.bz_anim and self.bz_anim["is_rainbow"]) or (self.kb_anim and self.kb_anim["is_rainbow"]):
                rainbow_hue = (rainbow_hue + 0.005) % 1.0
                cr, cg, cb = colorsys.hsv_to_rgb(rainbow_hue, 1.0, 1.0)
                
            if self.bz_anim:
                bz = self.bz_anim
                bz_mode = bz["mode"]
                if bz["is_rainbow"]:
                    bz_r, bz_g, bz_b = int(cr * 255), int(cg * 255), int(cb * 255)
                else:
                    bz_r, bz_g, bz_b = bz["r"], bz["g"], bz["b"]
                
                bz_fd1 = int(current_b1)
                bz_fd2 = int(current_b2)
                bz_fd3 = int(current_b3)
                bz_fd4 = int(current_b4)
                
                if bz_mode == 4: # BackLightCmd.Light_Jump
                    overall_vol = int((current_b1 + current_b2 + current_b3 + current_b4) / 4)
                    bz_fd1 = overall_vol
                    bz_fd2 = 0
                    bz_fd3 = 0
                    bz_fd4 = overall_vol
                    
                packet = get_back_zone_packet(bz_mode, bz_r, bz_g, bz_b, bz["brightness"], speed=1, fd1=bz_fd1, fd2=bz_fd2, fd3=bz_fd3, fd4=bz_fd4)
                self.serial.send_data(packet)
                
            if self.kb_anim:
                kb = self.kb_anim
                if kb["is_rainbow"]:
                    kb_r, kb_g, kb_b = int(cr * 255), int(cg * 255), int(cb * 255)
                else:
                    kb_r, kb_g, kb_b = kb["r"], kb["g"], kb["b"]
                
                overall_vol = max(current_b1, current_b2, current_b3, current_b4)
                vol_ratio = overall_vol / 255.0
                mod_r = int(kb_r * vol_ratio)
                mod_g = int(kb_g * vol_ratio)
                mod_b = int(kb_b * vol_ratio)
                
                packet = get_keyboard_led_packet(1, mod_r, mod_g, mod_b, kb["brightness"])
                self.usb.send_data(packet)

        try:
            with sd.InputStream(device=audio_device, callback=audio_callback, blocksize=1024):
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

    def set_keyboard_mode(self, mode: KeyboardLightMode, color_hex: str = "#FF0000", brightness: int = 255, sens: int = 50, smooth: int = 50, audio_device=None):
        """
        Sets the keyboard lighting mode and color.
        """
        r, g, b = self._hex_to_rgb(color_hex)
        
        if mode == KeyboardLightMode.RythmDance:
            self.kb_anim = {
                "mode": mode.value, "r": r, "g": g, "b": b,
                "brightness": brightness, "is_rainbow": True,
                "sens": sens, "smooth": smooth
            }
            if audio_device is not None:
                self.audio_device = audio_device
            self.update_animations()
            return True
            
        self.kb_anim = None
        self.update_animations()
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
        
    def set_serial_back_zone_mode(self, mode_enum_val: int, color_hex: str = "#FF0000", brightness: int = 100, sens: int = 50, smooth: int = 50, audio_device=None):
        r, g, b = self._hex_to_rgb(color_hex)
        from controlcenter.models.tx_buf import get_back_zone_packet, BackLightCmd
        
        is_rainbow = False
        if mode_enum_val == 99:
            mode_enum_val = BackLightCmd.Light_Rythm
            is_rainbow = True
        elif mode_enum_val == 98:
            mode_enum_val = BackLightCmd.Light_Jump
            is_rainbow = True
        
        if mode_enum_val in (BackLightCmd.Light_Rythm, BackLightCmd.Light_Jump):
            self.bz_anim = {
                "mode": mode_enum_val, "r": r, "g": g, "b": b,
                "brightness": brightness, "is_rainbow": is_rainbow,
                "sens": sens, "smooth": smooth
            }
            if audio_device is not None:
                self.audio_device = audio_device
            self.update_animations()
            return True
            
        self.bz_anim = None
        self.update_animations()
        
        speed = 1
        fd1, fd2, fd3, fd4 = 0, 0, 0, 0
        if mode_enum_val == BackLightCmd.Light_Close:
            speed = 200
        elif mode_enum_val in (BackLightCmd.Light_Round, BackLightCmd.Light_Cover):
            speed = 4
            fd1, fd2, fd3, fd4 = 48, 235, 231, 100
            
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
