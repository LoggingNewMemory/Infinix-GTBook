import psutil
try:
    import pynvml
    HAS_NVML = True
except ImportError:
    HAS_NVML = False

from controlcenter.services.acpi_wmi import ACPIWmi


class MonitorService:
    def __init__(self, wmi_service: ACPIWmi):
        self.wmi = wmi_service
        self._nvml_initialized = False
        
        if HAS_NVML:
            try:
                pynvml.nvmlInit()
                self._nvml_initialized = True
                self.gpu_handle = pynvml.nvmlDeviceGetHandleByIndex(0)
            except Exception as e:
                pass

    def get_cpu_temp(self) -> int:
        """
        Try ACPI WMI first, fallback to psutil.
        """
        try:
            temp = self.wmi.get_cpu_temp()
            if temp > 0:
                return temp
        except Exception:
            pass
            
        # Fallback to psutil
        try:
            temps = psutil.sensors_temperatures()
            if 'coretemp' in temps and len(temps['coretemp']) > 0:
                return int(temps['coretemp'][0].current)
            elif 'k10temp' in temps and len(temps['k10temp']) > 0:
                return int(temps['k10temp'][0].current)
            elif 'acpitz' in temps and len(temps['acpitz']) > 0:
                return int(temps['acpitz'][0].current)
        except Exception as e:
            pass
            
        return 0

    def get_gpu_temp(self) -> int:
        """
        Try NVML first (more reliable for NVIDIA), fallback to WMI.
        """
        if self._nvml_initialized:
            try:
                temp = int(pynvml.nvmlDeviceGetTemperature(self.gpu_handle, pynvml.NVML_TEMPERATURE_GPU))
                if temp > 0:
                    return temp
            except Exception as e:
                # NVML usually throws an error if the GPU is in D3cold sleep state
                pass
                
        try:
            temp = self.wmi.get_gpu_temp()
            if temp > 0:
                return temp
        except Exception:
            pass
            
        return 0

    def get_cpu_usage(self) -> int:
        return int(psutil.cpu_percent(interval=0.1))

    def get_gpu_usage(self) -> int:
        if self._nvml_initialized:
            try:
                utilrates = pynvml.nvmlDeviceGetUtilizationRates(self.gpu_handle)
                return utilrates.gpu
            except Exception:
                pass
        return 0

    def __del__(self):
        if self._nvml_initialized:
            try:
                pynvml.nvmlShutdown()
            except Exception:
                pass
