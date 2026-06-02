import struct
import logging

logger = logging.getLogger(__name__)

class ACPIWmi:
    """
    ACPI-WMI Bridge for GT Control Center.
    Maps to the Windows \root\wmi ACPIMethod class.
    GUID: 3161C7C3-489F-4074-82AE-538101CCE1C2
    """
    def __init__(self):
        self._is_mock = True
        logger.info("Initializing ACPI WMI service (Mock Mode)")
        
    def _call_wmi_method(self, method_id: int, in_data: bytearray) -> int:
        # Format bytearray to {0xXX, 0xXX, ...} string
        buf_str = "{" + ", ".join([f"0x{b:02X}" for b in in_data]) + "}"
        cmd = f"\\_SB.AMW0.WMBA 0 {method_id} {buf_str}"
        
        try:
            with open("/proc/acpi/call", "w") as f:
                f.write(cmd)
            with open("/proc/acpi/call", "r") as f:
                result = f.read().strip('\x00').strip()
                
            if result == "Error: AE_NOT_FOUND" or "Error" in result:
                logger.error(f"acpi_call failed: {result}")
                return 0
                
            if result.startswith("0x"):
                return int(result, 16)
            return int(result)
        except PermissionError:
            logger.error("Permission denied to /proc/acpi/call. Run with sudo or set up udev rules.")
            return 0
        except FileNotFoundError:
            logger.error("acpi_call module not loaded. 'sudo modprobe acpi_call'")
            return 0

    def do_method(self, cmd: int) -> int:
        """
        Equivalent to Wmi.DoMethod
        """
        buf = bytearray(24)
        buf[0:4] = b"BYDL"
        buf[4] = cmd & 0xFF
        buf[5] = (cmd >> 8) & 0xFF
        buf[6] = (cmd >> 16) & 0xFF
        buf[7] = (cmd >> 24) & 0xFF
        return self._call_wmi_method(1, buf)

    def smi(self, cmd: int, rw: int, length: int = 0, data: int = 0) -> int:
        """
        Equivalent to Wmi.SMI
        """
        buf = bytearray(24)
        buf[0:4] = b"BYDL"
        buf[4] = cmd & 0xFF
        buf[5] = (cmd >> 8) & 0xFF
        buf[6] = (cmd >> 16) & 0xFF
        buf[7] = (cmd >> 24) & 0xFF
        buf[12] = rw
        buf[13] = length
        if rw == 1:
            buf[16] = data & 0xFF
            
        return self._call_wmi_method(3, buf)
        
    def mem_io(self, address: int, rw: int, index: int, data: int = 0) -> int:
        """
        Equivalent to Wmi.IO (MemIO)
        """
        buf = bytearray(24)
        buf[0:4] = b"BYDL"
        buf[4] = address & 0xFF
        buf[5] = (address >> 8) & 0xFF
        buf[12] = 1 # ?
        buf[13] = rw
        buf[14] = 8 # length?
        buf[15] = index
        buf[16] = data & 0xFF
        return self._call_wmi_method(2, buf)

    # Higher-level helpers
    def get_cpu_temp(self) -> int:
        return self.do_method(3)
        
    def get_gpu_temp(self) -> int:
        return self.do_method(4)

    def get_ec_status(self) -> int:
        return self.do_method(2)
        
    def ec_write_ram_cmd(self, address: int, data: int) -> int:
        return self.smi(128, 1, 1, data) # Derived from C# SMI(0x80, 1, 1, data)
        
    def ec_read_ram_cmd(self, address: int) -> int:
        return self.smi(128, 0, 1) # Needs proper mapping

