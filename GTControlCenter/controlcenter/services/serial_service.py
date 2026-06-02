import serial
import serial.tools.list_ports
import logging
import time

logger = logging.getLogger(__name__)

class SerialService:
    def __init__(self, port: str = None, baudrate: int = 115200):
        if port is None:
            # Auto-detect CH340 device (VID=0x1a86, PID=0x7523) used by BYD Back Zone
            for p in serial.tools.list_ports.comports():
                if p.vid == 0x1a86 and p.pid == 0x7523:
                    port = p.device
                    logger.info(f"Auto-detected Back Zone serial port at {port}")
                    break
            
            if port is None:
                logger.warning("Could not auto-detect Back Zone serial port, defaulting to /dev/ttyS4")
                port = "/dev/ttyS4"
                
        self.port = port
        self.baudrate = baudrate
        self.ser = None
        
        try:
            self.ser = serial.Serial(self.port, self.baudrate, timeout=1)
            time.sleep(2) # Wait for MCU bootloader to finish after DTR reset
        except Exception as e:
            logger.error(f"Failed to open serial port {self.port}: {e}")
        
    def send_data(self, data: bytearray) -> bool:
        if not self.ser:
            return False
            
        try:
            self.ser.write(data)
            self.ser.flush()
            logger.info(f"Sent {len(data)} bytes to {self.port}: {[hex(x) for x in data]}")
            return True
        except Exception as e:
            logger.error(f"Failed to write to {self.port}: {e}")
            return False

