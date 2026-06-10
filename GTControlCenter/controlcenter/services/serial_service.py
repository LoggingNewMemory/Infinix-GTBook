import serial
import serial.tools.list_ports
import time


class SerialService:
    def __init__(self, port: str = None, baudrate: int = 115200):
        if port is None:
            # Auto-detect CH340 device (VID=0x1a86, PID=0x7523) used by BYD Back Zone
            for p in serial.tools.list_ports.comports():
                if p.vid == 0x1a86 and p.pid == 0x7523:
                    port = p.device
                    break
            
            if port is None:
                port = "/dev/ttyS4"
                
        self.port = port
        self.baudrate = baudrate
        self.ser = None
        
        try:
            self.ser = serial.Serial(self.port, self.baudrate, timeout=1)
            time.sleep(2) # Wait for MCU bootloader to finish after DTR reset
        except Exception as e:
            pass
        
    def send_data(self, data: bytearray) -> bool:
        if not self.ser:
            return False
            
        try:
            self.ser.write(data)
            self.ser.flush()
            return True
        except Exception as e:
            return False

