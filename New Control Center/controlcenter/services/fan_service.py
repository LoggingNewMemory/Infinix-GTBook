import logging
from typing import Tuple

from controlcenter.models.tx_buf import FanCtrlMode, get_fan_control_packet
from controlcenter.services.usb_service import USBService
from controlcenter.services.acpi_wmi import ACPIWmi

logger = logging.getLogger(__name__)

class FanService:
    def __init__(self, usb_service: USBService, acpi_wmi: ACPIWmi):
        self.usb = usb_service
        self.wmi = acpi_wmi

    def set_fan_mode(self, mode: FanCtrlMode) -> bool:
        """
        Set fan mode via USB command (equivalent to Page1/Page2 behavior).
        """
        packet = get_fan_control_packet(mode.value)
        if self.usb.send_data(packet):
            logger.info(f"Set fan mode to {mode.name} via USB")
            return True
        else:
            logger.error(f"Failed to set fan mode to {mode.name}")
            return False

    def set_performance_mode(self, mode: int) -> bool:
        """
        Set performance mode via EC RAM (WMI).
        mode: 1 = Office, 2 = Balance, 3 = Gaming
        """
        try:
            # Address 64 (0x40) is used for Performance mode
            self.wmi.ec_write_ram_cmd(64, mode)
            logger.info(f"Set performance mode to {mode} via WMI EC")
            return True
        except Exception as e:
            logger.error(f"Failed to set performance mode: {e}")
            return False

    def get_performance_mode(self) -> int:
        """
        Get current performance mode.
        """
        try:
            return self.wmi.ec_read_ram_cmd(64)
        except Exception as e:
            logger.error(f"Failed to get performance mode: {e}")
            return 0
