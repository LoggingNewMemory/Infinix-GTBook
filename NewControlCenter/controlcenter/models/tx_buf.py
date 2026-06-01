import enum
import struct

class Command(enum.IntEnum):
    Device_Info = 0
    KeyBoard_Light = 1
    Side_Light = 2
    Logo_Light = 3
    Fan_Ctrl = 4
    Periph_Ctrl = 5
    KeyBoard_Light12 = 6
    KeyBoard_Light34 = 7

class KeyboardLightMode(enum.IntEnum):
    LightOFF = 0
    Always = 1
    Breath = 2
    GradualChange = 3
    RainBow = 4
    Flow = 5
    Wave = 6
    Light_Auto_Close = 7
    Light_Open = 8

class KeyboardLight12Mode(enum.IntEnum):
    Always1 = 0
    Breath1 = 1
    GradualChange1 = 2
    RainBow1 = 3
    Always2 = 4
    Breath2 = 5
    GradualChange2 = 6
    RainBow2 = 7

class FanCtrlMode(enum.IntEnum):
    OfficeMode = 0
    PerformanceMode = 1
    GamingMode = 2
    FullSpeed = 3
    FullSpeedOff = 4

class DeviceInfoMode(enum.IntEnum):
    Reset = 0
    SaveKeyboard = 1
    Version = 2
    Battery = 3
    Update = 4

class SideLightMode(enum.IntEnum):
    LightOFF = 0
    Always = 1
    Breath = 2
    GradualChange = 3
    RainBow = 4
    TemperatureControl = 5

class LogoLightMode(enum.IntEnum):
    LightOFF = 0
    Always = 1
    Breath = 2
    GradualChange = 3
    RainBow = 4
    PowerLight = 5
class BackLightCmd(enum.IntEnum):
    Light_Close = 0
    Light_AlwaysOn = 1
    Light_Breath = 2
    Light_Rythm = 3
    Light_Jump = 4
    Light_Round = 5
    Light_Cover = 6
    SliceMode = 7
    BalanceMode = 8
    GameMode = 9

def get_back_zone_packet(mode: int, r: int, g: int, b: int, brightness: int = 100, speed: int = 1) -> bytearray:
    buf = bytearray(17)
    buf[0] = 52  # h1
    buf[1] = 14  # h2
    buf[2] = mode
    buf[3] = r
    buf[4] = 0 # r2
    buf[5] = g
    buf[6] = 0 # g2
    buf[7] = b
    buf[8] = 0 # b2
    buf[9] = speed
    buf[10] = int(brightness * 0.44) # l1
    buf[11] = 0 # l2
    # Bytes 12-16 remain 0
    return buf
class RxField(enum.IntEnum):
    CPU_FAN1_Speed = 9
    DGPU_FAN2_Speed = 10
    CPU_Temperature = 11
    DGPU_Temperature = 12
    System_Power_State = 13
    Adapter_Current = 41
    Adapter_Voltage = 43
    Thermal_Flag1_State = 23
    Thermal_Control_Mode = 24
    Color_R = 51
    Color_G = 52
    Color_B = 53
    Fan_Mode = 50

def build_packet(cmd: int, param: int, size: int = 0, r: int = 0, g: int = 0, b: int = 0, l: int = 0) -> bytearray:
    """
    Builds a 64-byte USB HID report packet for the BYD controller.
    """
    buf = bytearray(64)
    buf[0] = 0x06  # ID
    
    # unionCMD (1 byte): upper 4 bits = COMMAND, lower 4 bits = PARAMS
    buf[1] = ((cmd & 0x0F) << 4) | (param & 0x0F)
    
    # unionSize (1 byte)
    buf[2] = size & 0xFF
    
    # unionReserved is bytes 3-6 (leave as 0)
    
    # unionData starts at byte 7
    if size > 0:
        buf[7] = r & 0xFF
        buf[8] = g & 0xFF
        buf[9] = b & 0xFF
        buf[10] = l & 0xFF
        
    # CheckSum at byte 63: sum of bytes 1 to 62
    checksum = sum(buf[1:63]) & 0xFF
    buf[63] = checksum
    
    return buf

# Helper functions for specific commands
def get_keyboard_led_packet(mode: int, r: int = 0, g: int = 0, b: int = 0, l: int = 0) -> bytearray:
    return build_packet(Command.KeyBoard_Light, mode, size=4, r=r, g=g, b=b, l=l)

def get_fan_control_packet(mode: int) -> bytearray:
    return build_packet(Command.Fan_Ctrl, mode, size=0)

def get_device_info_packet(mode: int) -> bytearray:
    return build_packet(Command.Device_Info, mode, size=0)

def get_keyboard_save_packet() -> bytearray:
    return build_packet(Command.Device_Info, DeviceInfoMode.SaveKeyboard, size=0)
