using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using BydCentral.Core.Services;
using NvAPIWrapper.GPU;
using NvAPIWrapper.Native;
using NvAPIWrapper.Native.Interfaces.GPU;

namespace BydCentral.Services;

public class Win32 : IWin32
{
	public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

	[StructLayout(LayoutKind.Sequential)]
	public class KeyBoardHookStruct
	{
		public int vkCode;

		public int scanCode;

		public int flags;

		public int time;

		public int dwExtraInfo;
	}

	private CounterSample cval;

	private static int hHook;

	public const int WH_KEYBOARD_LL = 13;

	private static HookProc KeyBoardHookProcedure;

	public string? GetBattery()
	{
		string result = "0";
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_Battery").GetInstances().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				result = managementObjectEnumerator.Current["EstimatedChargeRemaining"].ToString();
			}
		}
		catch
		{
			result = "0";
		}
		return result;
	}

	public string? GetBatteryStatus()
	{
		string result = "0";
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_Battery").GetInstances().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				result = managementObjectEnumerator.Current["BatteryStatus"].ToString();
			}
		}
		catch
		{
			result = "0";
		}
		return result;
	}

	public string GetCPUName()
	{
		string result = string.Empty;
		try
		{
			_ = string.Empty;
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_Processor").GetInstances().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				result = ((ManagementObject)managementObjectEnumerator.Current)["Name"].ToString();
			}
		}
		catch
		{
		}
		return result;
	}

	public int GetCPUOP(int Step)
	{
		string s = string.Empty;
		try
		{
			PerformanceCounter performanceCounter = new PerformanceCounter("Processor Information", "% Processor Utility", "_Total");
			if (Step == 0)
			{
				cval = performanceCounter.NextSample();
			}
			if (Step == 1)
			{
				s = CounterSample.Calculate(cval, performanceCounter.NextSample()).ToString("0.0");
			}
			performanceCounter.Dispose();
			return Convert.ToInt32(Math.Round(float.Parse(s)));
		}
		catch
		{
		}
		return 0;
	}

	public string? GetDiskName()
	{
		string result = string.Empty;
		try
		{
			_ = string.Empty;
			foreach (ManagementObject instance in new ManagementClass("Win32_DiskDrive").GetInstances())
			{
				if (instance["InterfaceType"].ToString() == "SCSI")
				{
					result = instance["Model"].ToString();
				}
			}
		}
		catch
		{
		}
		return result;
	}

	public int GetDiskOP()
	{
		double[] array = new double[2];
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo driveInfo in drives)
		{
			if (driveInfo.IsReady && driveInfo.DriveType.ToString() == "Fixed")
			{
				array[0] += driveInfo.TotalSize;
				array[1] += driveInfo.TotalFreeSpace;
			}
		}
		array[1] = array[0] - array[1];
		return Convert.ToInt32(100.0 * array[1] / array[0]);
	}

	public int GetDiskUesd()
	{
		double[] array = new double[2];
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo driveInfo in drives)
		{
			if (driveInfo.IsReady && driveInfo.DriveType.ToString() == "Fixed")
			{
				array[0] += driveInfo.TotalSize;
				array[1] += driveInfo.TotalFreeSpace;
			}
		}
		array[1] = array[0] - array[1];
		return Convert.ToInt32(array[1] / 1024.0 / 1024.0 / 1024.0);
	}

	public string GetDiskSize()
	{
		string result = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			foreach (ManagementObject instance in new ManagementClass("Win32_DiskDrive").GetInstances())
			{
				if (instance["InterfaceType"].ToString() == "SCSI")
				{
					long num = Convert.ToInt64(instance["Size"].ToString());
					stringBuilder.Append(ToGB(num, 1024.0) + "+");
				}
			}
			result = stringBuilder.ToString().TrimEnd('+');
		}
		catch
		{
		}
		return result;
	}

	public string? GetGPUName()
	{
		Tuple<string, string> tuple = null;
		try
		{
			ManagementObjectCollection instances = new ManagementClass("Win32_VideoController").GetInstances();
			if (instances.Count >= 2)
			{
				tuple = new Tuple<string, string>("null", "null");
				foreach (ManagementObject item in instances)
				{
					if (!item["Caption"].ToString().Contains("NVIDIA"))
					{
						continue;
					}
					if (item["Status"].ToString() == "OK")
					{
						tuple = new Tuple<string, string>(item["VideoProcessor"].ToString().Replace("Family", ""), ToGB(Convert.ToInt64(item["AdapterRAM"].ToString()), 1024.0));
						break;
					}
					foreach (ManagementObject item2 in instances)
					{
						if (item2["Caption"].ToString().Contains("Graphic"))
						{
							tuple = new Tuple<string, string>(item2["VideoProcessor"].ToString().Replace("Family", ""), ToGB(Convert.ToInt64(item2["AdapterRAM"].ToString()), 1024.0));
							break;
						}
					}
					break;
				}
			}
			else
			{
				using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = instances.GetEnumerator();
				if (managementObjectEnumerator.MoveNext())
				{
					ManagementObject managementObject3 = (ManagementObject)managementObjectEnumerator.Current;
					tuple = new Tuple<string, string>(managementObject3["VideoProcessor"].ToString().Replace("Family", ""), ToGB(Convert.ToInt64(managementObject3["AdapterRAM"].ToString()), 1024.0));
				}
			}
		}
		catch
		{
			tuple = new Tuple<string, string>("null", "null");
		}
		return tuple.Item1 + tuple.Item2;
	}

	public float GetGPUOP()
	{
		return 8.8f;
	}

	public string? GetMemoryName()
	{
		ManagementObjectCollection instances = new ManagementClass("Win32_PhysicalMemory").GetInstances();
		string result = string.Empty;
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = instances.GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				ManagementBaseObject current = managementObjectEnumerator.Current;
				result = current["Manufacturer"].ToString() + ": " + current["PartNumber"].ToString();
			}
		}
		catch
		{
		}
		return result;
	}

	public string GetMemoryAvailable()
	{
		string result = string.Empty;
		double num = 0.0;
		try
		{
			using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_PerfFormattedData_PerfOS_Memory").GetInstances().GetEnumerator())
			{
				if (managementObjectEnumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
					num += Math.Round((double)long.Parse(managementObject.Properties["AvailableMBytes"].Value.ToString()) / 1024.0, 1);
				}
			}
			result = num.ToString();
		}
		catch
		{
		}
		return result;
	}

	public string GetMemorySize()
	{
		string result = string.Empty;
		double num = 0.0;
		try
		{
			foreach (ManagementObject instance in new ManagementClass("Win32_PhysicalMemory").GetInstances())
			{
				num += Math.Round((double)(long.Parse(instance.Properties["Capacity"].Value.ToString()) / 1024 / 1024) / 1024.0, 1);
			}
			result = num.ToString();
		}
		catch
		{
		}
		return result;
	}

	public string GetCpuFrequency()
	{
		uint num = 0u;
		try
		{
			PerformanceCounter performanceCounter = new PerformanceCounter("Processor Information", "% Processor Performance", "_Total");
			double num2 = performanceCounter.NextValue();
			Thread.Sleep(1000);
			num2 = performanceCounter.NextValue();
			foreach (ManagementObject item in new ManagementObjectSearcher("SELECT *, Name FROM Win32_Processor").Get())
			{
				num = (uint)(Convert.ToDouble(item["MaxClockSpeed"]) * num2 / 100.0);
			}
		}
		catch
		{
		}
		return num.ToString();
	}

	public string GetGpuFrequency()
	{
		string result = "";
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("select * from Win32_VideoController").Get().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				result = ((ManagementObject)managementObjectEnumerator.Current)["CurrentRefreshRate"].ToString() ?? "";
			}
		}
		catch
		{
		}
		return result;
	}

	public string GetCpuMaxFrequency()
	{
		string result = "";
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("select * from Win32_Processor").Get().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				result = ((ManagementObject)managementObjectEnumerator.Current)["MaxClockSpeed"].ToString() ?? "";
			}
		}
		catch
		{
		}
		return result;
	}

	public string GetGpuMaxFrequency()
	{
		string result = "";
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("select * from Win32_VideoController").Get().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				result = ((ManagementObject)managementObjectEnumerator.Current)["MaxClockSpeed"].ToString() ?? "";
			}
		}
		catch
		{
		}
		return result;
	}

	public string ToGB(double size, double mod)
	{
		string[] array = new string[6] { "B", "K", "M", "G", "T", "P" };
		int num = 0;
		while (size >= mod)
		{
			size /= mod;
			num++;
		}
		return Math.Round(size) + array[num];
	}

	public uint getNVGPUclk()
	{
		uint num = 0u;
		try
		{
			PhysicalGPU[] physicalGPUs = PhysicalGPU.GetPhysicalGPUs();
			PhysicalGPU[] array = physicalGPUs;
			foreach (PhysicalGPU physicalGPU in array)
			{
				num += physicalGPU.CurrentClockFrequencies.GraphicsClock.Frequency / 1000;
			}
			return (uint)(num / physicalGPUs.Length);
		}
		catch
		{
		}
		return 0u;
	}

	public uint getNVGPUMaxFrequency()
	{
		uint num = 0u;
		try
		{
			PhysicalGPU physicalGPU = new PhysicalGPU(GPUApi.EnumPhysicalGPUs()[0]);
			IClockFrequencies baseClockFrequencies = physicalGPU.BaseClockFrequencies;
			IClockFrequencies boostClockFrequencies = physicalGPU.BoostClockFrequencies;
			return baseClockFrequencies.GraphicsClock.Frequency / 1000 + boostClockFrequencies.GraphicsClock.Frequency / 1000;
		}
		catch
		{
			return 1u;
		}
	}

	public int getNVTem()
	{
		int num = 0;
		try
		{
			PhysicalGPU[] physicalGPUs = PhysicalGPU.GetPhysicalGPUs();
			PhysicalGPU[] array = physicalGPUs;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (GPUThermalSensor thermalSensor in array[i].ThermalInformation.ThermalSensors)
				{
					num += thermalSensor.CurrentTemperature;
				}
			}
			return num / physicalGPUs.Length;
		}
		catch (Exception)
		{
		}
		return 0;
	}

	public string getCom()
	{
		string text = "";
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("select * from Win32_PNPEntity where PNPDeviceID ='USB\\\\VID_1A86&PID_7523\\\\5&157263AF&0&4'").Get().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				text = ((ManagementObject)managementObjectEnumerator.Current)["Name"].ToString() ?? "";
				text = text.Split('(', ')')[1];
				return text;
			}
			return text;
		}
		catch (Exception ex)
		{
			Console.WriteLine("WMI查询错误: " + ex.Message);
			return text;
		}
	}

	[DllImport("user32.dll")]
	public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
	public static extern bool UnhookWindowsHookEx(int idHook);

	[DllImport("user32.dll")]
	public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

	[DllImport("kernel32.dll")]
	public static extern IntPtr GetModuleHandle(string name);

	private static int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
	{
		if (nCode >= 0)
		{
			KeyBoardHookStruct keyBoardHookStruct = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
			if (keyBoardHookStruct.vkCode == 91 || keyBoardHookStruct.vkCode == 92)
			{
				return 1;
			}
		}
		return CallNextHookEx(hHook, nCode, wParam, lParam);
	}

	public void Hook_Start()
	{
		if (hHook == 0)
		{
			KeyBoardHookProcedure = KeyBoardHookProc;
			hHook = SetWindowsHookEx(13, KeyBoardHookProcedure, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
			if (hHook == 0)
			{
				Hook_Clear();
			}
		}
	}

	public void Hook_Clear()
	{
		bool flag = true;
		if (hHook != 0)
		{
			flag = UnhookWindowsHookEx(hHook);
			hHook = 0;
		}
		if (!flag)
		{
			MessageBox.Show("hHook error");
		}
	}
}
