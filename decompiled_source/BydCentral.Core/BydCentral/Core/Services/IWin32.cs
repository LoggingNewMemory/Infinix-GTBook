namespace BydCentral.Core.Services;

public interface IWin32
{
	string ToGB(double size, double mod);

	string? GetDiskName();

	string GetDiskSize();

	int GetDiskOP();

	int GetDiskUesd();

	string? GetMemoryName();

	string GetMemorySize();

	string GetMemoryAvailable();

	string GetCPUName();

	int GetCPUOP(int Step);

	string? GetGPUName();

	float GetGPUOP();

	string? GetBattery();

	string? GetBatteryStatus();

	void Hook_Start();

	void Hook_Clear();

	string GetCpuFrequency();

	string GetGpuFrequency();

	string GetCpuMaxFrequency();

	string GetGpuMaxFrequency();

	uint getNVGPUclk();

	uint getNVGPUMaxFrequency();

	int getNVTem();

	string getCom();
}
