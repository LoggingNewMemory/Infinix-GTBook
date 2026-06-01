using BydCentral.Core.Services;
using BYDWmi;
using CommunityToolkit.Mvvm.Messaging;

namespace BydCentral.Core.Models;

public static class GlobalVars
{
	public static Win32 Win32 = new Win32();

	public static Wmi Wmi = new Wmi();

	public static readonly object locker = new object();

	public static bool FanLoopFlag = true;

	public static bool IsKeyCon = false;

	private static int _displayMode = 2;

	private static string systemMode = "";

	private static string gboxLight = "";

	private static string gboxChargeStatus = "";

	public static int SSID = 0;

	public static string ssid = "";

	public static string Phase = "";

	public static uint skuid = 0u;

	public static uint FanMaxStatus = 0u;

	public static uint UsbChargeStatus = 0u;

	public static bool AllServiceStop = false;

	public static bool GetSystemMode = false;

	public static bool IsMusicMode = false;

	public static string LightCOM = "";

	public static Usb usb = new Usb();

	public static TxBuf txBufClass = new TxBuf();

	public static TxBuf.txBuF txBufStruct = default(TxBuf.txBuF);

	public static bool MiniStratup = false;

	public static bool GetSystemModeRun = true;

	public static bool GetInfoServiceRun = false;

	public static bool GetBatteryServiceRun = false;

	public static bool GetSystemTaskStop = false;

	public static bool Service1TaskStop = false;

	public static bool Service3TaskStop = true;

	public static bool isInMS = false;

	public static int DisplayMode
	{
		get
		{
			return _displayMode;
		}
		set
		{
			if (value == 1 || value == 2 || value == 3)
			{
				_displayMode = value;
			}
		}
	}

	public static string SystemMode
	{
		get
		{
			return systemMode;
		}
		set
		{
			if (systemMode != value)
			{
				switch (value)
				{
				case "0":
				case "1":
				case "2":
					systemMode = value;
					WeakReferenceMessenger.Default.Send(systemMode, "SystemMode");
					break;
				}
			}
		}
	}

	public static string GboxLight
	{
		get
		{
			return gboxLight;
		}
		set
		{
			if (gboxLight != value && (value == "0" || value == "1"))
			{
				gboxLight = value;
				WeakReferenceMessenger.Default.Send(gboxLight, "GboxLight");
			}
		}
	}

	public static string GboxChargeStatus
	{
		get
		{
			return gboxChargeStatus;
		}
		set
		{
			if (gboxChargeStatus != value)
			{
				switch (value)
				{
				case "1":
				case "2":
				case "3":
					gboxChargeStatus = value;
					WeakReferenceMessenger.Default.Send(gboxChargeStatus, "GboxChargeStatus");
					break;
				}
			}
		}
	}
}
