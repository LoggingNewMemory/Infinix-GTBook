using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using Costura;
using Microsoft.CodeAnalysis;
using NAudio.CoreAudioApi;
using NvAPIWrapper.GPU;
using NvAPIWrapper.Native;
using NvAPIWrapper.Native.Interfaces.GPU;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName = ".NET 6.0")]
[assembly: AssemblyCompany("BYDWmi2")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0")]
[assembly: AssemblyProduct("BYDWmi2")]
[assembly: AssemblyTitle("BYDWmi2")]
[assembly: TargetPlatform("Windows7.0")]
[assembly: SupportedOSPlatform("Windows7.0")]
[assembly: AssemblyVersion("1.0.0.0")]
internal class <Module>
{
	static <Module>()
	{
		AssemblyLoader.Attach();
	}
}
namespace Microsoft.CodeAnalysis
{
	[CompilerGenerated]
	[Microsoft.CodeAnalysis.Embedded]
	internal sealed class EmbeddedAttribute : Attribute
	{
	}
}
namespace System.Runtime.CompilerServices
{
	[CompilerGenerated]
	[Microsoft.CodeAnalysis.Embedded]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableAttribute : Attribute
	{
		public readonly byte[] NullableFlags;

		public NullableAttribute(byte P_0)
		{
			NullableFlags = new byte[1] { P_0 };
		}

		public NullableAttribute(byte[] P_0)
		{
			NullableFlags = P_0;
		}
	}
	[CompilerGenerated]
	[Microsoft.CodeAnalysis.Embedded]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableContextAttribute : Attribute
	{
		public readonly byte Flag;

		public NullableContextAttribute(byte P_0)
		{
			Flag = P_0;
		}
	}
}
namespace BYDWmi
{
	public class Wmi : WmiServer
	{
		public void inMS(bool intoMS)
		{
			WmiServer.isMS = intoMS;
		}

		public uint SMI(uint cmd, int rw, int length = 0, object data = null)
		{
			uint result = 0u;
			byte[] array = new byte[24]
			{
				66, 89, 68, 76, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			array[4] = (byte)(cmd & 0xFF);
			array[5] = (byte)((cmd >> 8) & 0xFF);
			array[6] = (byte)((cmd >> 16) & 0xFF);
			array[7] = (byte)((cmd >> 24) & 0xFF);
			array[12] = (byte)rw;
			array[13] = (byte)length;
			if (rw == 1)
			{
				if (data == null)
				{
					return 2147483649u;
				}
				if (data is Array)
				{
					int num = ((byte[])data).Length;
					if (num > 8)
					{
						return 2147483649u;
					}
					for (int i = 0; i < num; i++)
					{
						array[16 + i] = ((byte[])data)[i];
					}
				}
				else
				{
					array[16] = Convert.ToByte(data);
				}
			}
			try
			{
				result = WmiMethod("SMI", array);
			}
			catch
			{
			}
			return result;
		}

		public uint IfNvReady()
		{
			uint num = 0u;
			byte[] inData = new byte[24]
			{
				66, 89, 68, 76, 7, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			try
			{
				uint num2 = WmiMethod("DoMethod", inData);
				if (num2 != 2147483649u)
				{
					num = num2;
				}
			}
			catch
			{
			}
			return num & 2;
		}

		public uint GetSSID()
		{
			uint result = 0u;
			byte[] inData = new byte[24]
			{
				66, 89, 68, 76, 1, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			try
			{
				uint num = WmiMethod("DoMethod", inData);
				if (num != 2147483649u)
				{
					result = num;
				}
			}
			catch
			{
			}
			return result;
		}

		public uint GetSkuId()
		{
			uint num = 0u;
			try
			{
				IO(768u, 1, 148, 0);
				IO(768u, 1, 144, 0);
				IO(768u, 1, 145, 5);
				IO(768u, 1, 146, 1);
				IO(768u, 1, 147, 160);
				return IO(768u, 0, 160, 0);
			}
			catch
			{
				return 2147483651u;
			}
		}

		public string GetPhase()
		{
			string text = string.Empty;
			try
			{
				IO(768u, 1, 148, 0);
				IO(768u, 1, 144, 0);
				IO(768u, 1, 145, 51);
				IO(768u, 1, 146, 1);
				IO(768u, 1, 147, 160);
				string text2 = IO(768u, 0, 160, 0).ToString();
				IO(768u, 1, 148, 0);
				IO(768u, 1, 145, 50);
				IO(768u, 1, 147, 160);
				string text3 = IO(768u, 0, 160, 0).ToString();
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

		public uint GetECStatus()
		{
			uint result = 0u;
			byte[] inData = new byte[24]
			{
				66, 89, 68, 76, 2, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			try
			{
				uint num = WmiMethod("DoMethod", inData);
				if (num != 2147483649u)
				{
					result = num;
				}
			}
			catch
			{
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
				uint num = WmiMethod("DoMethod", inData);
				if (num != 2147483649u)
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
				uint num = WmiMethod("DoMethod", inData);
				if (num != 2147483649u)
				{
					result = num;
				}
			}
			catch
			{
			}
			return result;
		}

		public uint SetAirplaneMode()
		{
			uint result = 0u;
			byte[] inData = new byte[24]
			{
				66, 89, 68, 76, 5, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			try
			{
				result = WmiMethod("DoMethod", inData);
			}
			catch
			{
			}
			return result;
		}

		public uint SetSoundMute()
		{
			uint result = 0u;
			byte[] inData = new byte[24]
			{
				66, 89, 68, 76, 6, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			try
			{
				result = WmiMethod("DoMethod", inData);
			}
			catch
			{
			}
			return result;
		}

		public string GetECVersion()
		{
			string result = string.Empty;
			try
			{
				uint num = 0u;
				IO(768u, 1, 144, 0);
				IO(768u, 1, 145, 0);
				IO(768u, 1, 146, 1);
				IO(768u, 1, 147, 160);
				char c = (char)byte.Parse(IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
				IO(768u, 1, 145, 1);
				IO(768u, 1, 147, 160);
				char c2 = (char)byte.Parse(IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
				IO(768u, 1, 145, 2);
				IO(768u, 1, 147, 160);
				char c3 = (char)byte.Parse(IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
				IO(768u, 1, 145, 3);
				IO(768u, 1, 147, 160);
				char c4 = (char)byte.Parse(IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
				result = string.Concat(c, c2, '.', c3, c4);
			}
			catch
			{
			}
			return result;
		}

		public uint SetPerformanceMode(int mode)
		{
			uint num = 0u;
			return mode switch
			{
				3 => ECWriteRamCMD(64, 3), 
				2 => ECWriteRamCMD(64, 2), 
				1 => ECWriteRamCMD(64, 1), 
				_ => ECWriteRamCMD(64, 0), 
			};
		}

		public uint GetPerformanceMode()
		{
			return ECReadRamCMD(64);
		}

		public uint getIgpuTem()
		{
			return 100 - Memory(4275853762u, 0, 8);
		}

		public uint KeyLight(bool isOpen)
		{
			uint light = getLight2();
			uint lightPageStatus = getLightPageStatus();
			try
			{
				if (isOpen)
				{
					ECWriteRamCMD(94, (byte)(0 | light | lightPageStatus));
				}
				else
				{
					ECWriteRamCMD(94, (byte)(1 | light | lightPageStatus));
				}
			}
			catch
			{
			}
			return 0u;
		}

		public uint BackLight(bool isOpen)
		{
			uint light = getLight1();
			uint lightPageStatus = getLightPageStatus();
			try
			{
				if (isOpen)
				{
					ECWriteRamCMD(94, (byte)(0 | light | lightPageStatus));
				}
				else
				{
					ECWriteRamCMD(94, (byte)(2 | light | lightPageStatus));
				}
			}
			catch
			{
			}
			return 0u;
		}

		public uint getLightPageStatus()
		{
			uint result = 0u;
			try
			{
				result = ECReadRamCMD(94) & 0x80;
			}
			catch
			{
			}
			return result;
		}

		public uint setLightPageStatus(bool isDefault)
		{
			uint light = getLight1();
			uint light2 = getLight2();
			try
			{
				if (isDefault)
				{
					ECWriteRamCMD(94, (byte)(0 | light | light2));
				}
				else
				{
					ECWriteRamCMD(94, (byte)(0x80 | light | light2));
				}
			}
			catch
			{
			}
			return 0u;
		}

		public uint getLight1()
		{
			uint result = 0u;
			try
			{
				result = ECReadRamCMD(94) & 1;
			}
			catch
			{
			}
			return result;
		}

		public uint getLight2()
		{
			uint result = 0u;
			try
			{
				result = ECReadRamCMD(94) & 2;
			}
			catch
			{
			}
			return result;
		}

		public uint getLightVer()
		{
			return (ECReadRamCMD(81) << 8) | ECReadRamCMD(80);
		}

		public uint GetGBoxPower()
		{
			return ECReadRamCMD(104);
		}

		public uint GetGBoxFanTable()
		{
			return ECReadRamCMD(106);
		}

		public uint SetGBoxLight(bool flag)
		{
			uint num = 0u;
			if (flag)
			{
				return ECWriteRamCMD(100, 2);
			}
			return ECWriteRamCMD(100, 3);
		}

		public uint GetGBoxLight()
		{
			return ECReadRamCMD(100);
		}

		public uint SetGBoxTurbo(bool flag)
		{
			uint num = 0u;
			if (flag)
			{
				return ECWriteRamCMD(103, 3);
			}
			return ECWriteRamCMD(103, 2);
		}

		public uint GetGBoxTurbo()
		{
			return ECReadRamCMD(103);
		}

		public uint GetGBoxCustomMode()
		{
			return ECReadRamCMD(105);
		}

		public uint GetGBoxCustomLevel()
		{
			return ECReadRamCMD(106);
		}

		public uint SetGBoxCustomMode()
		{
			return ECWriteRamCMD(105, 1);
		}

		public uint SetGBoxCustomLevel(int flag)
		{
			uint num = 0u;
			return flag switch
			{
				4 => ECWriteRamCMD(106, 4), 
				3 => ECWriteRamCMD(106, 3), 
				2 => ECWriteRamCMD(106, 2), 
				1 => ECWriteRamCMD(106, 1), 
				_ => ECWriteRamCMD(106, 5), 
			};
		}

		public uint SetGBoxMode(int flag)
		{
			uint num = 0u;
			num = ECWriteRamCMD(105, 0);
			if (flag != 2 && flag == 3)
			{
				return ECWriteRamCMD(64, 3);
			}
			return ECWriteRamCMD(64, 2);
		}

		public uint GetGBoxChargeMode()
		{
			return ECReadRamCMD(101) * 2 + ECReadRamCMD(102);
		}

		public uint GetUSBCharging()
		{
			uint result = 0u;
			byte[] inData = new byte[24]
			{
				66, 89, 68, 76, 4, 0, 0, 0, 0, 0,
				0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			try
			{
				uint num = WmiMethod("DoMethod", inData);
				if (num != 2147483649u)
				{
					result = num;
				}
			}
			catch
			{
			}
			return result;
		}

		public uint SetUSBCharging(bool isEnable)
		{
			uint result = 0u;
			byte[] array = new byte[24]
			{
				66, 89, 68, 76, 4, 0, 0, 0, 0, 0,
				0, 0, 3, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			try
			{
				if (isEnable)
				{
					array[20] = 1;
				}
				else
				{
					array[20] = 0;
				}
				uint num = WmiMethod("DoMethod", array);
				if (num != 2147483649u)
				{
					result = num;
				}
			}
			catch
			{
			}
			return result;
		}

		public uint SetGPUMode(int mode)
		{
			uint num = 0u;
			return mode switch
			{
				1 => ECWriteRamCMD(192, 1), 
				3 => ECWriteRamCMD(192, 3), 
				_ => ECWriteRamCMD(192, 2), 
			};
		}

		public uint GetGPUMode()
		{
			return ECReadRamCMD(192);
		}

		public uint SetFanFullMode(bool flag)
		{
			uint num = 0u;
			if (flag)
			{
				return ECWriteRamCMD(65, 1);
			}
			return ECWriteRamCMD(65, 0);
		}

		public uint GetFanFullMode()
		{
			return ECReadRamCMD(65);
		}

		public uint GetCPUFanMax()
		{
			return ECReadRamCMD(6);
		}

		public uint GetGPUFanMax()
		{
			return ECReadRamCMD(7);
		}

		public uint GetCPUFan()
		{
			return ECReadRamCMD(8);
		}

		public uint GetGPUFan()
		{
			return ECReadRamCMD(9);
		}

		public uint ECWriteRamCMD(byte address, byte value)
		{
			lock (this)
			{
				mutex = true;
				uint result = 80u;
				byte data = address;
				byte data2 = value;
				try
				{
					IO(768u, 1, 148, 0);
					IO(768u, 1, 145, 0);
					IO(768u, 1, 146, 0);
					IO(768u, 1, 146, 1);
					IO(768u, 1, 144, 0);
					IO(768u, 1, 145, data);
					IO(768u, 1, 160, data2);
					IO(768u, 1, 147, 161);
				}
				catch
				{
				}
				mutex = false;
				return result;
			}
		}

		public uint ECReadRamCMD(byte address)
		{
			lock (this)
			{
				uint result = 0u;
				byte data = address;
				int num = 0;
				while (mutex && num < 200)
				{
					Thread.Sleep(1);
					num++;
				}
				try
				{
					IO(768u, 1, 148, 0);
					IO(768u, 1, 146, 1);
					IO(768u, 1, 144, 0);
					IO(768u, 1, 145, data);
					IO(768u, 1, 147, 160);
					int num2 = 0;
					while (num2 < 100 && IO(768u, 0, 147, 0) != 0)
					{
						num2++;
						Thread.Sleep(2);
					}
					uint num3 = IO(768u, 0, 160, 0);
					if (num3 != 2147483649u)
					{
						result = num3;
					}
				}
				catch
				{
				}
				return result;
			}
		}

		public uint Memory(uint address, int rw, int length, object data = null)
		{
			uint result = 0u;
			byte[] array = new byte[24]
			{
				66, 89, 68, 76, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			array[4] = (byte)(address & 0xFF);
			array[5] = (byte)((address >> 8) & 0xFF);
			array[6] = (byte)((address >> 16) & 0xFF);
			array[7] = (byte)((address >> 24) & 0xFF);
			array[12] = 0;
			array[13] = (byte)rw;
			array[14] = (byte)length;
			if (length != 8 && length != 16 && length != 32)
			{
				return 2147483649u;
			}
			if (rw == 1)
			{
				if (data == null)
				{
					return 2147483649u;
				}
				if (data is Array)
				{
					int num = ((byte[])data).Length;
					for (int i = 0; i < num; i++)
					{
						array[16 + i] = ((byte[])data)[i];
					}
				}
				else
				{
					array[16] = Convert.ToByte(data);
				}
			}
			try
			{
				result = WmiMethod("MemIO", array);
			}
			catch
			{
			}
			return result;
		}

		public uint IO(uint address, int rw, byte index, byte data = 0)
		{
			uint result = 0u;
			byte[] array = new byte[24]
			{
				66, 89, 68, 76, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			};
			array[4] = (byte)(address & 0xFF);
			array[5] = (byte)((address >> 8) & 0xFF);
			array[12] = 1;
			array[13] = (byte)rw;
			array[14] = 8;
			array[15] = index;
			array[16] = data;
			try
			{
				result = WmiMethod("MemIO", array);
			}
			catch
			{
			}
			return result;
		}

		public uint IOPort(uint address, int rw, object data = null)
		{
			uint result = 0u;
			byte[] array = new byte[10020];
			byte[] array2 = new byte[20]
			{
				66, 89, 68, 76, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0
			};
			array2[4] = (byte)(address & 0xFF);
			array2[5] = (byte)((address >> 8) & 0xFF);
			array2[6] = (byte)((address >> 16) & 0xFF);
			array2[7] = (byte)((address >> 24) & 0xFF);
			array2[12] = (byte)rw;
			int num = array2.Length;
			for (int i = 0; i < num; i++)
			{
				array[i] = array2[i];
			}
			if (rw == 1)
			{
				if (data == null)
				{
					return 2147483649u;
				}
				if (data is Array)
				{
					byte[] array3 = (byte[])data;
					int num2 = array3.Length;
					array[16] = (byte)(num2 & 0xFF);
					array[17] = (byte)((num2 >> 8) & 0xFF);
					array[18] = (byte)((num2 >> 16) & 0xFF);
					array[19] = (byte)((num2 >> 24) & 0xFF);
					for (int j = 0; j < num2; j++)
					{
						array[num + j] = array3[j];
					}
				}
				else
				{
					array[16] = 1;
					array[num] = Convert.ToByte(data);
				}
			}
			try
			{
				result = WmiMethod("IOPort", array);
			}
			catch
			{
			}
			return result;
		}
	}
	public class WmiServer
	{
		public bool mutex;

		private bool isLock;

		private ManagementEventWatcher eWatcher;

		public static bool isMS;

		public uint WmiMethod(string methodName, object inData)
		{
			if (isMS)
			{
				return 5u;
			}
			lock (this)
			{
				uint num = 0u;
				uint num2 = 0u;
				string path = "ACPIMethod";
				string propertyName = "IData";
				string text = "OData";
				ManagementScope managementScope = new ManagementScope("\\root\\wmi");
				managementScope.Options.EnablePrivileges = true;
				managementScope.Connect();
				ManagementPath path2 = new ManagementPath(path);
				ManagementObjectCollection instances = new ManagementClass(managementScope, path2, null).GetInstances();
				ManagementObject managementObject = null;
				ManagementBaseObject managementBaseObject = null;
				ManagementBaseObject managementBaseObject2 = null;
				using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = instances.GetEnumerator())
				{
					if (managementObjectEnumerator.MoveNext())
					{
						managementObject = (ManagementObject)managementObjectEnumerator.Current;
						managementBaseObject = managementObject.GetMethodParameters(methodName);
						managementBaseObject[propertyName] = inData;
					}
				}
				int num3 = 5;
				do
				{
					try
					{
						managementBaseObject2 = managementObject.InvokeMethod(methodName, managementBaseObject, null);
						num2 = 0u;
						num = 0u;
						if (text != "")
						{
							num = (uint)managementBaseObject2[text];
						}
					}
					catch
					{
						num2 = 2147483649u;
						num = 2147483649u;
						Thread.Sleep(new Random().Next(10, 20));
					}
					num3--;
				}
				while (num2 == 2147483649u && num3 > 0);
				return num;
			}
		}

		public uint WmiEvent_Start(string eScope, string eClassName)
		{
			uint result = 0u;
			bool flag = false;
			try
			{
				ManagementScope managementScope = new ManagementScope(eScope);
				managementScope.Options.EnablePrivileges = true;
				managementScope.Connect();
				WqlEventQuery query = new WqlEventQuery("select * from" + eClassName);
				eWatcher = new ManagementEventWatcher(managementScope, query);
				if (!flag)
				{
					if (!isLock)
					{
						isLock = true;
						eWatcher.EventArrived += WmiEvent_Handler;
						eWatcher.Start();
					}
				}
				else
				{
					eWatcher.Options.Timeout = new TimeSpan(0, 0, 20);
					eWatcher.WaitForNextEvent();
				}
			}
			catch
			{
				result = 2147483649u;
			}
			return result;
		}

		public void WmiEvent_Stop()
		{
			if (eWatcher != null)
			{
				isLock = false;
				eWatcher.Stop();
			}
		}

		private void WmiEvent_Handler(object sender, EventArrivedEventArgs e)
		{
			_ = e.NewEvent;
		}
	}
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
	public class Win32Control
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

		private static int hHook;

		public const int WH_KEYBOARD_LL = 13;

		private static HookProc KeyBoardHookProcedure;

		[DllImport("user32.dll")]
		public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern bool UnhookWindowsHookEx(int idHook);

		[DllImport("user32.dll")]
		public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetModuleHandle(string name);

		private int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
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
			if (hHook != 0)
			{
				UnhookWindowsHookEx(hHook);
				hHook = 0;
			}
		}
	}
}
namespace Costura
{
	[CompilerGenerated]
	internal static class AssemblyLoader
	{
		private static object nullCacheLock = new object();

		private static Dictionary<string, bool> nullCache = new Dictionary<string, bool>();

		private static Dictionary<string, string> assemblyNames = new Dictionary<string, string>();

		private static Dictionary<string, string> symbolNames = new Dictionary<string, string>();

		private static int isAttached;

		private static string CultureToString(CultureInfo culture)
		{
			if (culture == null)
			{
				return "";
			}
			return culture.Name;
		}

		private static Assembly ReadExistingAssembly(AssemblyName name)
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			Assembly[] assemblies = currentDomain.GetAssemblies();
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				AssemblyName name2 = assembly.GetName();
				if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(CultureToString(name2.CultureInfo), CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly;
				}
			}
			return null;
		}

		private static void CopyTo(Stream source, Stream destination)
		{
			byte[] array = new byte[81920];
			int count;
			while ((count = source.Read(array, 0, array.Length)) != 0)
			{
				destination.Write(array, 0, count);
			}
		}

		private static Stream LoadStream(string fullName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (fullName.EndsWith(".compressed"))
			{
				using (Stream stream = executingAssembly.GetManifestResourceStream(fullName))
				{
					using DeflateStream source = new DeflateStream(stream, CompressionMode.Decompress);
					MemoryStream memoryStream = new MemoryStream();
					CopyTo(source, memoryStream);
					memoryStream.Position = 0L;
					return memoryStream;
				}
			}
			return executingAssembly.GetManifestResourceStream(fullName);
		}

		private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
		{
			if (resourceNames.TryGetValue(name, out var value))
			{
				return LoadStream(value);
			}
			return null;
		}

		private static byte[] ReadStream(Stream stream)
		{
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			return array;
		}

		private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
		{
			string text = requestedAssemblyName.Name.ToLowerInvariant();
			if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
			{
				text = requestedAssemblyName.CultureInfo.Name + "." + text;
			}
			byte[] rawAssembly;
			using (Stream stream = LoadStream(assemblyNames, text))
			{
				if (stream == null)
				{
					return null;
				}
				rawAssembly = ReadStream(stream);
			}
			using (Stream stream2 = LoadStream(symbolNames, text))
			{
				if (stream2 != null)
				{
					byte[] rawSymbolStore = ReadStream(stream2);
					return Assembly.Load(rawAssembly, rawSymbolStore);
				}
			}
			return Assembly.Load(rawAssembly);
		}

		public static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			lock (nullCacheLock)
			{
				if (nullCache.ContainsKey(e.Name))
				{
					return null;
				}
			}
			AssemblyName assemblyName = new AssemblyName(e.Name);
			Assembly assembly = ReadExistingAssembly(assemblyName);
			if ((object)assembly != null)
			{
				return assembly;
			}
			assembly = ReadFromEmbeddedResources(assemblyNames, symbolNames, assemblyName);
			if ((object)assembly == null)
			{
				lock (nullCacheLock)
				{
					nullCache[e.Name] = true;
				}
				if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != AssemblyNameFlags.None)
				{
					assembly = Assembly.Load(assemblyName);
				}
			}
			return assembly;
		}

		static AssemblyLoader()
		{
			assemblyNames.Add("naudio.wasapi", "costura.naudio.wasapi.dll.compressed");
			assemblyNames.Add("nvapiwrapper", "costura.nvapiwrapper.dll.compressed");
		}

		public static void Attach()
		{
			if (Interlocked.Exchange(ref isAttached, 1) != 1)
			{
				AppDomain currentDomain = AppDomain.CurrentDomain;
				currentDomain.AssemblyResolve += ResolveAssembly;
			}
		}
	}
}
internal class BYDWmi2_ProcessedByFody
{
	internal const string FodyVersion = "6.5.5.0";

	internal const string Costura = "5.7.0";
}
