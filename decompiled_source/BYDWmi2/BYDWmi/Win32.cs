using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using NAudio.CoreAudioApi;
using NvAPIWrapper.GPU;
using NvAPIWrapper.Native;
using NvAPIWrapper.Native.Interfaces.GPU;

namespace BYDWmi;

public class Win32 : Win32Control
{
	public uint cpuMaxSpeed;

	public uint GetBatteryRemaining()
	{
		uint result = 0u;
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_Battery").GetInstances().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				result = Convert.ToUInt32(managementObjectEnumerator.Current["EstimatedChargeRemaining"].ToString());
			}
		}
		catch
		{
		}
		return result;
	}

	public uint GetBatteryChargingStatus()
	{
		uint result = 0u;
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_Battery").GetInstances().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				result = Convert.ToUInt32(managementObjectEnumerator.Current["BatteryStatus"].ToString());
			}
		}
		catch
		{
		}
		return result;
	}

	public string GetCPUName()
	{
		string result = string.Empty;
		try
		{
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

	public uint GetCPUOP()
	{
		uint result = 0u;
		_ = string.Empty;
		try
		{
			PerformanceCounter performanceCounter = new PerformanceCounter("Processor Information", "% Processor Utility", "_Total");
			CounterSample counterSample = performanceCounter.NextSample();
			Thread.Sleep(1000);
			result = Convert.ToUInt32(Math.Round(float.Parse(CounterSample.Calculate(counterSample, performanceCounter.NextSample()).ToString("0.0"))));
			performanceCounter.Dispose();
		}
		catch
		{
		}
		return result;
	}

	public string GetGPUName()
	{
		string result = string.Empty;
		try
		{
			ManagementObjectCollection instances = new ManagementClass("Win32_VideoController").GetInstances();
			string text = null;
			string text2 = null;
			foreach (ManagementObject item in instances)
			{
				string text3 = item["Name"].ToString();
				if (!string.IsNullOrEmpty(text3))
				{
					if (!text3.ToLower().Contains("graphics"))
					{
						text2 = text3;
					}
					else if (text == null)
					{
						text = text3;
					}
				}
			}
			result = ((!string.IsNullOrEmpty(text2)) ? text2 : (string.IsNullOrEmpty(text) ? "No GPU Found" : text));
		}
		catch
		{
		}
		return result;
	}

	public uint GetGPUOP()
	{
		uint result = 0u;
		try
		{
			List<List<PerformanceCounter>> list = new List<List<PerformanceCounter>>();
			List<string> obj = new List<string> { "engtype_3D", "engtype_Copy", "engtype_VideoDecode", "engtype_VideoProcessing", "engtype_Compute", "engtype_GDI Render" };
			PerformanceCounterCategory category = new PerformanceCounterCategory("GPU Engine");
			string[] instanceNames = category.GetInstanceNames();
			foreach (string name in obj)
			{
				List<PerformanceCounter> item = (from counter in instanceNames.Where((string counterName) => counterName.EndsWith(name)).SelectMany((string counterName) => category.GetCounters(counterName))
					where counter.CounterName.Equals("Utilization Percentage")
					select counter).ToList();
				list.Add(item);
			}
			foreach (List<PerformanceCounter> item2 in list)
			{
				item2.ForEach(delegate(PerformanceCounter x)
				{
					x.NextValue();
				});
			}
			Thread.Sleep(800);
			float[] array = new float[6];
			int num = 0;
			foreach (List<PerformanceCounter> item3 in list)
			{
				array[num] = item3.Sum((PerformanceCounter x) => x.NextValue());
				num++;
			}
			list.Clear();
			result = Convert.ToUInt32(Math.Round(array.Max()));
		}
		catch
		{
		}
		return result;
	}

	public string GetDiskName()
	{
		string result = string.Empty;
		try
		{
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

	public uint GetDiskOP()
	{
		uint result = 0u;
		try
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
			result = Convert.ToUInt32(100.0 * array[1] / array[0]);
		}
		catch
		{
		}
		return result;
	}

	public string GetDiskUsed()
	{
		string result = string.Empty;
		try
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
			result = ToGB(array[1], 1024.0);
		}
		catch
		{
		}
		return result;
	}

	public string GetDiskTotalSize()
	{
		string result = string.Empty;
		try
		{
			double num = 0.0;
			DriveInfo[] drives = DriveInfo.GetDrives();
			foreach (DriveInfo driveInfo in drives)
			{
				if (driveInfo.IsReady && driveInfo.DriveType.ToString() == "Fixed")
				{
					num += (double)driveInfo.TotalSize;
				}
			}
			result = ToGB(num, 1024.0);
		}
		catch
		{
		}
		return result;
	}

	public string GetDiskTotalSizeWmi()
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

	public string GetMemoryName()
	{
		string result = string.Empty;
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_PhysicalMemory").GetInstances().GetEnumerator();
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

	public uint GetMemoryOP()
	{
		uint result = 0u;
		try
		{
			result = 100 - (uint)Math.Round(100.0 * GetMemoryAvailable() / GetMemoryTotalSize());
		}
		catch
		{
		}
		return result;
	}

	public double GetMemoryUsed()
	{
		double result = 0.0;
		try
		{
			result = Math.Round(GetMemoryTotalSize() - GetMemoryAvailable(), 1);
		}
		catch
		{
		}
		return result;
	}

	public double GetMemoryAvailable()
	{
		double result = 0.0;
		try
		{
			double num = 0.0;
			using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_PerfFormattedData_PerfOS_Memory").GetInstances().GetEnumerator())
			{
				if (managementObjectEnumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
					num += Math.Round((double)long.Parse(managementObject.Properties["AvailableMBytes"].Value.ToString()) / 1024.0, 1);
				}
			}
			result = num;
		}
		catch
		{
		}
		return result;
	}

	public double GetMemoryTotalSize()
	{
		double result = 0.0;
		try
		{
			double num = 0.0;
			foreach (ManagementObject instance in new ManagementClass("Win32_PhysicalMemory").GetInstances())
			{
				num += Math.Round((double)(long.Parse(instance.Properties["Capacity"].Value.ToString()) / 1024 / 1024) / 1024.0, 1);
			}
			result = num - 0.3;
		}
		catch
		{
		}
		return result;
	}

	public string GetBIOSVersion()
	{
		string result = string.Empty;
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS").Get().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				result = ((ManagementObject)managementObjectEnumerator.Current)["SMBIOSBIOSVersion"].ToString();
			}
		}
		catch
		{
		}
		return result;
	}

	public string GetSoundVersion()
	{
		string result = string.Empty;
		try
		{
			foreach (ManagementObject item in new ManagementObjectSearcher("SELECT * FROM Win32_PnPSignedDriver").Get())
			{
				if (item["description"].ToString().Contains("Realtek High"))
				{
					result = item["DriverVersion"].ToString();
					break;
				}
			}
		}
		catch
		{
		}
		return result;
	}

	public uint GetCpuFrequency()
	{
		uint result = 0u;
		try
		{
			if (cpuMaxSpeed == 0)
			{
				cpuMaxSpeed = GetCpuMaxFrequency();
			}
			PerformanceCounter performanceCounter = new PerformanceCounter("Processor Information", "% Processor Performance", "_Total");
			double num = performanceCounter.NextValue();
			Thread.Sleep(1000);
			num = performanceCounter.NextValue();
			if (num == 0.0)
			{
				num = performanceCounter.NextValue();
			}
			result = (uint)((double)cpuMaxSpeed * num / 100.0);
			performanceCounter.Dispose();
		}
		catch
		{
		}
		return result;
	}

	public uint GetCpuMaxFrequency()
	{
		uint result = 0u;
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("select * from Win32_Processor").Get().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				result = Convert.ToUInt16(((ManagementObject)managementObjectEnumerator.Current)["MaxClockSpeed"]);
			}
		}
		catch
		{
		}
		return result;
	}

	public string GetNVVbiosVersion()
	{
		string result = string.Empty;
		try
		{
			result = GPUApi.GetVBIOSVersionString(GPUApi.EnumPhysicalGPUs()[0]);
		}
		catch
		{
		}
		return result;
	}

	public uint GetNVGpuFrequency()
	{
		uint result = 0u;
		try
		{
			IClockFrequencies allClockFrequencies = GPUApi.GetAllClockFrequencies(GPUApi.EnumPhysicalGPUs()[0]);
			_ = allClockFrequencies.ProcessorClock.Frequency / 1000;
			uint num = allClockFrequencies.GraphicsClock.Frequency / 1000;
			_ = allClockFrequencies.VideoDecodingClock.Frequency / 1000;
			result = num;
		}
		catch
		{
		}
		return result;
	}

	public uint GetNVGpuMaxFrequency()
	{
		uint result = 0u;
		try
		{
			PhysicalGPU physicalGPU = new PhysicalGPU(GPUApi.EnumPhysicalGPUs()[0]);
			IClockFrequencies baseClockFrequencies = physicalGPU.BaseClockFrequencies;
			IClockFrequencies boostClockFrequencies = physicalGPU.BoostClockFrequencies;
			result = baseClockFrequencies.GraphicsClock.Frequency / 1000 + boostClockFrequencies.GraphicsClock.Frequency / 1000;
		}
		catch
		{
		}
		return result;
	}

	public int GetNVTem()
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

	public string ToGB(double size, double mod)
	{
		string[] array = new string[6] { "B", "KB", "MB", "GB", "TB", "PB" };
		int num = 0;
		while (size >= mod && num < 3)
		{
			size /= mod;
			num++;
		}
		return Math.Round(size) + array[num];
	}

	public void RestartComputer()
	{
		Process process = new Process();
		try
		{
			process.StartInfo.FileName = "shutdown.exe";
			process.StartInfo.Arguments = "/r /t 0";
			process.Start();
		}
		catch (Exception ex)
		{
			Console.WriteLine("发生错误：" + ex.Message);
		}
		finally
		{
			process.Dispose();
		}
	}

	public void SetWinkeyLock(bool flag)
	{
		try
		{
			if (flag)
			{
				Hook_Start();
			}
			else
			{
				Hook_Clear();
			}
		}
		catch
		{
		}
	}

	public void SetDMicMute(bool flag)
	{
		try
		{
			new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications).AudioEndpointVolume.Mute = flag;
		}
		catch
		{
		}
	}

	public string getCom()
	{
		string text = "";
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("select * from Win32_PNPEntity where PNPDeviceID ='ACPI\\\\PNP0501\\\\SERIALIOUART1'").Get().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				text = ((ManagementObject)managementObjectEnumerator.Current)["Name"].ToString() ?? "";
				text = text.Split('(', ')')[1];
			}
		}
		catch
		{
		}
		return text;
	}
}
