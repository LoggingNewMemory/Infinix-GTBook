namespace BydCentral.Core.Services;

public interface IWmi
{
	uint WmiMethod(string mScope, string className, string methodName, string inParams, object InData, string outParams);

	string getSSID();

	uint GetCPUTem();

	uint GetGPUTem();

	uint getCPUFan();

	uint getGPUFan();

	string setQMode();

	string setBMode();

	string setGMode();

	string setBEMode();

	string queryMode();

	string setUSBChg(bool isEnable);

	uint getUSBChg();

	string setGPUMode(int whichMode);

	uint GetGPUMode();

	int getNVGPU();

	int getIGPU();

	uint getGPUFanMax();

	uint getCPUFanMax();

	string getBIOSver();

	string getECVer();

	string getSoundVer();

	uint GetFanFullMode();

	string FanFullMode();

	string disFanFullMode();

	uint getSkuId();

	string getPhase();
}
