using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using BydCentral.Core.Models;
using BydCentral.Core.Services;
using BYDWmi;
using NvAPIWrapper.Native;

namespace BydCentral.Services;

public class Wmi : IWmi
{
	private BYDWmi.Wmi wmi = new BYDWmi.Wmi();

	public uint WmiMethod(string mScope, string className, string methodName, string inParams, object InData, string outParams)
	{
		uint result = 2147483649u;
		try
		{
			ManagementScope managementScope = new ManagementScope(mScope);
			managementScope.Options.EnablePrivileges = true;
			managementScope.Connect();
			ManagementPath path = new ManagementPath(className);
			foreach (ManagementObject instance in new ManagementClass(managementScope, path, null).GetInstances())
			{
				ManagementBaseObject methodParameters = instance.GetMethodParameters(methodName);
				methodParameters[inParams] = InData;
				ManagementBaseObject managementBaseObject = instance.InvokeMethod(methodName, methodParameters, null);
				if (outParams != "")
				{
					result = (uint)managementBaseObject[outParams];
				}
			}
		}
		catch (Exception)
		{
		}
		return result;
	}

	public string getSSID()
	{
		string result = string.Empty;
		byte[] inData = new byte[24]
		{
			66, 89, 68, 76, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		uint num = WmiMethod("\\root\\wmi", "ACPIMethod", "DoMethod", "IData", inData, "OData");
		if (num != 2147483649u)
		{
			result = num.ToString("X4");
		}
		return result;
	}

	public uint GetCPUTem()
	{
		uint result = 0u;
		byte[] inData = new byte[24]
		{
			66, 89, 68, 76, 3, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		try
		{
			uint num = WmiMethod("\\root\\wmi", "ACPIMethod", "DoMethod", "IData", inData, "OData");
			if (num < 2147483648u)
			{
				result = num;
			}
		}
		catch
		{
		}
		return result;
	}

	public uint GetGPUTem()
	{
		uint result = 0u;
		byte[] inData = new byte[24]
		{
			66, 89, 68, 76, 4, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		try
		{
			uint num = WmiMethod("\\root\\wmi", "ACPIMethod", "DoMethod", "IData", inData, "OData");
			if (num < 2147483648u)
			{
				result = num;
			}
		}
		catch
		{
		}
		return result;
	}

	public string setQMode()
	{
		string result = string.Empty;
		lock (GlobalVars.locker)
		{
			try
			{
				wmi.IO(768u, 1, 144, 0);
				wmi.IO(768u, 1, 145, 64);
				wmi.IO(768u, 1, 146, 1);
				wmi.IO(768u, 1, 160, 0);
				wmi.IO(768u, 1, 147, 161);
				result = wmi.IO(768u, 0, 148, 0).ToString();
			}
			catch
			{
			}
		}
		return result;
	}

	public string setBMode()
	{
		string result = string.Empty;
		lock (GlobalVars.locker)
		{
			try
			{
				wmi.IO(768u, 1, 144, 0);
				wmi.IO(768u, 1, 145, 64);
				wmi.IO(768u, 1, 146, 1);
				wmi.IO(768u, 1, 160, 1);
				wmi.IO(768u, 1, 147, 161);
				result = wmi.IO(768u, 0, 148, 0).ToString();
			}
			catch
			{
			}
		}
		return result;
	}

	public string setGMode()
	{
		string result = string.Empty;
		lock (GlobalVars.locker)
		{
			try
			{
				wmi.IO(768u, 1, 144, 0);
				wmi.IO(768u, 1, 145, 64);
				wmi.IO(768u, 1, 146, 1);
				wmi.IO(768u, 1, 160, 2);
				wmi.IO(768u, 1, 147, 161);
				result = wmi.IO(768u, 0, 148, 0).ToString();
			}
			catch
			{
			}
		}
		return result;
	}

	public string setBEMode()
	{
		string result = string.Empty;
		lock (GlobalVars.locker)
		{
			try
			{
				wmi.IO(768u, 1, 144, 0);
				wmi.IO(768u, 1, 145, 64);
				wmi.IO(768u, 1, 146, 1);
				wmi.IO(768u, 1, 160, 3);
				wmi.IO(768u, 1, 147, 161);
				result = wmi.IO(768u, 0, 148, 0).ToString();
			}
			catch
			{
			}
		}
		return result;
	}

	public string queryMode()
	{
		string result = string.Empty;
		try
		{
			if (wmi.IO(768u, 0, 148, 0) == 131)
			{
				wmi.IO(768u, 1, 148, 128);
			}
			wmi.IO(768u, 1, 144, 0);
			wmi.IO(768u, 1, 145, 64);
			wmi.IO(768u, 1, 146, 1);
			wmi.IO(768u, 1, 147, 160);
			result = wmi.IO(768u, 0, 160, 0).ToString();
		}
		catch
		{
		}
		return result;
	}

	public string setUSBChg(bool isEnable)
	{
		string result = string.Empty;
		lock (GlobalVars.locker)
		{
			try
			{
				wmi.IO(768u, 1, 144, 0);
				wmi.IO(768u, 1, 145, 78);
				wmi.IO(768u, 1, 146, 1);
				if (isEnable)
				{
					wmi.IO(768u, 1, 160, 2);
				}
				else
				{
					wmi.IO(768u, 1, 160, 0);
				}
				wmi.IO(768u, 1, 147, 161);
				result = wmi.IO(768u, 0, 148, 0).ToString();
			}
			catch
			{
			}
		}
		return result;
	}

	public uint getUSBChg()
	{
		uint result = 0u;
		try
		{
			wmi.IO(768u, 1, 144, 0);
			wmi.IO(768u, 1, 145, 78);
			wmi.IO(768u, 1, 146, 1);
			wmi.IO(768u, 1, 147, 160);
			result = wmi.IO(768u, 0, 160, 0);
		}
		catch
		{
		}
		return result;
	}

	public string setGPUMode(int whichMode)
	{
		string result = string.Empty;
		lock (GlobalVars.locker)
		{
			try
			{
				wmi.IO(768u, 1, 144, 0);
				wmi.IO(768u, 1, 145, 192);
				wmi.IO(768u, 1, 146, 1);
				switch (whichMode)
				{
				case 1:
					wmi.IO(768u, 1, 160, 1);
					break;
				case 2:
					wmi.IO(768u, 1, 160, 2);
					break;
				case 3:
					wmi.IO(768u, 1, 160, 3);
					break;
				default:
					wmi.IO(768u, 1, 160, 2);
					break;
				}
				wmi.IO(768u, 1, 147, 161);
				result = wmi.IO(768u, 0, 148, 0).ToString();
			}
			catch
			{
			}
		}
		return result;
	}

	public uint GetGPUMode()
	{
		uint result = 0u;
		try
		{
			wmi.IO(768u, 1, 144, 0);
			wmi.IO(768u, 1, 145, 192);
			wmi.IO(768u, 1, 146, 1);
			wmi.IO(768u, 1, 147, 160);
			result = wmi.IO(768u, 0, 160, 0);
		}
		catch
		{
		}
		return result;
	}

	public int getNVGPU()
	{
		int result = 0;
		try
		{
			result = (int)GPUApi.GetUsages(GPUApi.EnumPhysicalGPUs()[0]).GPU.Percentage;
		}
		catch
		{
		}
		return result;
	}

	public int getIGPU()
	{
		PerformanceCounterCategory category = new PerformanceCounterCategory("GPU Engine");
		string[] instanceNames = category.GetInstanceNames();
		List<string> list = new List<string> { "engtype_3D", "engtype_Copy", "engtype_VideoDecode", "engtype_VideoProcessing", "engtype_Compute", "engtype_GDI Render" };
		List<List<PerformanceCounter>> list2 = new List<List<PerformanceCounter>>();
		float[] array = new float[6];
		try
		{
			foreach (string name in list)
			{
				List<PerformanceCounter> item = (from instance in instanceNames.Where((string instanceName) => instanceName.EndsWith(name)).SelectMany((string instanceName) => category.GetCounters(instanceName))
					where instance.CounterName.Equals("Utilization Percentage")
					select instance).ToList();
				list2.Add(item);
			}
			foreach (List<PerformanceCounter> item2 in list2)
			{
				item2.ForEach(delegate(PerformanceCounter x)
				{
					x.NextValue();
				});
			}
			int num = 0;
			foreach (List<PerformanceCounter> item3 in list2)
			{
				array[num] = item3.Sum((PerformanceCounter x) => x.NextValue());
				num++;
			}
		}
		catch
		{
		}
		return Convert.ToInt16(array.Max());
	}

	public uint getCPUFanMax()
	{
		uint num = 0u;
		try
		{
			if (wmi.IO(768u, 0, 148, 0) == 131)
			{
				wmi.IO(768u, 1, 148, 128);
			}
			wmi.IO(768u, 1, 144, 0);
			wmi.IO(768u, 1, 145, 6);
			wmi.IO(768u, 1, 146, 1);
			wmi.IO(768u, 1, 147, 160);
			num = wmi.IO(768u, 0, 160, 0);
			if (num < 2147483648u)
			{
				return num * 100;
			}
		}
		catch
		{
		}
		return 4000u;
	}

	public uint getGPUFanMax()
	{
		uint num = 0u;
		try
		{
			if (wmi.IO(768u, 0, 148, 0) == 131)
			{
				wmi.IO(768u, 1, 148, 128);
			}
			wmi.IO(768u, 1, 144, 0);
			wmi.IO(768u, 1, 145, 7);
			wmi.IO(768u, 1, 146, 1);
			wmi.IO(768u, 1, 147, 160);
			num = wmi.IO(768u, 0, 160, 0);
			if (num < 2147483648u)
			{
				return num * 100;
			}
		}
		catch
		{
		}
		return 4000u;
	}

	public uint getCPUFan()
	{
		uint num = 0u;
		lock (GlobalVars.locker)
		{
			if (GlobalVars.FanLoopFlag)
			{
				try
				{
					if (wmi.IO(768u, 0, 148, 0) == 131)
					{
						wmi.IO(768u, 1, 148, 128);
					}
					wmi.IO(768u, 1, 144, 0);
					wmi.IO(768u, 1, 145, 8);
					wmi.IO(768u, 1, 146, 1);
					wmi.IO(768u, 1, 147, 160);
					num = wmi.IO(768u, 0, 160, 0);
					if (num < 2147483648u)
					{
						return num * 100;
					}
				}
				catch
				{
				}
			}
		}
		return 0u;
	}

	public uint getGPUFan()
	{
		uint num = 0u;
		lock (GlobalVars.locker)
		{
			if (GlobalVars.FanLoopFlag)
			{
				try
				{
					if (wmi.IO(768u, 0, 148, 0) == 131)
					{
						wmi.IO(768u, 1, 148, 128);
					}
					wmi.IO(768u, 1, 144, 0);
					wmi.IO(768u, 1, 145, 9);
					wmi.IO(768u, 1, 146, 1);
					wmi.IO(768u, 1, 147, 160);
					num = wmi.IO(768u, 0, 160, 0);
					if (num < 2147483648u)
					{
						return num * 100;
					}
				}
				catch
				{
				}
			}
		}
		return 0u;
	}

	public void restartComputer()
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

	public uint GetFanFullMode()
	{
		uint result = 0u;
		try
		{
			wmi.IO(768u, 1, 144, 0);
			wmi.IO(768u, 1, 145, 65);
			wmi.IO(768u, 1, 146, 1);
			wmi.IO(768u, 1, 147, 160);
			result = wmi.IO(768u, 0, 160, 0);
		}
		catch
		{
		}
		return result;
	}

	public string FanFullMode()
	{
		string result = string.Empty;
		try
		{
			wmi.IO(768u, 1, 144, 0);
			wmi.IO(768u, 1, 145, 65);
			wmi.IO(768u, 1, 146, 1);
			wmi.IO(768u, 1, 160, 1);
			wmi.IO(768u, 1, 147, 161);
			result = wmi.IO(768u, 0, 148, 0).ToString();
		}
		catch
		{
		}
		return result;
	}

	public string disFanFullMode()
	{
		string result = string.Empty;
		try
		{
			wmi.IO(768u, 1, 144, 0);
			wmi.IO(768u, 1, 145, 65);
			wmi.IO(768u, 1, 146, 1);
			wmi.IO(768u, 1, 160, 0);
			wmi.IO(768u, 1, 147, 161);
			result = wmi.IO(768u, 0, 148, 0).ToString();
		}
		catch
		{
		}
		return result;
	}

	public uint getSkuId()
	{
		uint num = 0u;
		try
		{
			wmi.IO(768u, 1, 148, 0);
			wmi.IO(768u, 1, 144, 0);
			wmi.IO(768u, 1, 145, 5);
			wmi.IO(768u, 1, 146, 1);
			wmi.IO(768u, 1, 147, 160);
			return wmi.IO(768u, 0, 160, 0);
		}
		catch
		{
			return 2147483651u;
		}
	}

	public string getPhase()
	{
		string text = string.Empty;
		try
		{
			wmi.IO(768u, 1, 148, 0);
			wmi.IO(768u, 1, 144, 0);
			wmi.IO(768u, 1, 145, 51);
			wmi.IO(768u, 1, 146, 1);
			wmi.IO(768u, 1, 147, 160);
			string text2 = wmi.IO(768u, 0, 160, 0).ToString();
			wmi.IO(768u, 1, 148, 0);
			wmi.IO(768u, 1, 145, 50);
			wmi.IO(768u, 1, 147, 160);
			string text3 = wmi.IO(768u, 0, 160, 0).ToString();
			text = text2 + "." + text3;
			return text switch
			{
				"1.0" => "EVT", 
				"1.1" => "EVT1", 
				"2.0" => "DVT", 
				"2.1" => "DVT2", 
				"2.2" => "DVT3", 
				"3.0" => "PVT", 
				"3.1" => "PVT2", 
				"3.2" => "DVT3", 
				"4.0" => "MP", 
				"4.1" => "MP2", 
				"4.2" => "RSV2", 
				_ => text, 
			};
		}
		catch
		{
			return text;
		}
	}

	public string getECVer()
	{
		string empty = string.Empty;
		try
		{
			wmi.IO(768u, 1, 144, 0);
			wmi.IO(768u, 1, 145, 0);
			wmi.IO(768u, 1, 146, 1);
			wmi.IO(768u, 1, 147, 160);
			char c = (char)byte.Parse(wmi.IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
			wmi.IO(768u, 1, 145, 1);
			wmi.IO(768u, 1, 147, 160);
			char c2 = (char)byte.Parse(wmi.IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
			wmi.IO(768u, 1, 145, 2);
			wmi.IO(768u, 1, 147, 160);
			char c3 = (char)byte.Parse(wmi.IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
			wmi.IO(768u, 1, 145, 3);
			wmi.IO(768u, 1, 147, 160);
			char c4 = (char)byte.Parse(wmi.IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
			return string.Concat(c, c2, ".", c3, c4);
		}
		catch (Exception)
		{
			return "not found";
		}
	}

	public string getSoundVer()
	{
		string text = "{4d36e96c-e325-11ce-bfc1-08002be10318}";
		string result = string.Empty;
		try
		{
			foreach (ManagementObject item in new ManagementObjectSearcher("SELECT * FROM Win32_PnPSignedDriver WHERE classguid = \"" + text + "\"").Get())
			{
				if (item["description"].ToString().Contains("Realtek"))
				{
					result = item["DriverVersion"] as string;
					break;
				}
			}
		}
		catch (Exception)
		{
			result = "not found";
		}
		return result;
	}

	public string getBIOSver()
	{
		string result = string.Empty;
		try
		{
			foreach (ManagementObject item in new ManagementObjectSearcher("SELECT * FROM Win32_BIOS").Get())
			{
				result = item["SMBIOSBIOSVersion"] as string;
			}
		}
		catch (Exception)
		{
			result = "not found";
		}
		return result;
	}
}
