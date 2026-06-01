#define TRACE
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BydCentral.Core.Models;
using BydCentral.Core.Services;
using BydCentral.Services;
using BYDWmi;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using IWshRuntimeLibrary;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
using Microsoft.CodeAnalysis;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using NLog;
using OpenLibSys;
using ProtoBuf;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName = ".NET 6.0")]
[assembly: AssemblyCompany("BydCentral.Core")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0")]
[assembly: AssemblyProduct("BydCentral.Core")]
[assembly: AssemblyTitle("BydCentral.Core")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: AssemblyVersion("1.0.0.0")]
[module: UnverifiableCode]
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
namespace OpenLibSys
{
	public class Ols : IDisposable
	{
		public enum Status
		{
			NO_ERROR,
			DLL_NOT_FOUND,
			DLL_INCORRECT_VERSION,
			DLL_INITIALIZE_ERROR
		}

		public enum OlsDllStatus
		{
			OLS_DLL_NO_ERROR = 0,
			OLS_DLL_UNSUPPORTED_PLATFORM = 1,
			OLS_DLL_DRIVER_NOT_LOADED = 2,
			OLS_DLL_DRIVER_NOT_FOUND = 3,
			OLS_DLL_DRIVER_UNLOADED = 4,
			OLS_DLL_DRIVER_NOT_LOADED_ON_NETWORK = 5,
			OLS_DLL_UNKNOWN_ERROR = 9
		}

		public enum OlsDriverType
		{
			OLS_DRIVER_TYPE_UNKNOWN,
			OLS_DRIVER_TYPE_WIN_9X,
			OLS_DRIVER_TYPE_WIN_NT,
			OLS_DRIVER_TYPE_WIN_NT4,
			OLS_DRIVER_TYPE_WIN_NT_X64,
			OLS_DRIVER_TYPE_WIN_NT_IA64
		}

		public enum OlsErrorPci : uint
		{
			OLS_ERROR_PCI_BUS_NOT_EXIST = 3758096385u,
			OLS_ERROR_PCI_NO_DEVICE,
			OLS_ERROR_PCI_WRITE_CONFIG,
			OLS_ERROR_PCI_READ_CONFIG
		}

		public delegate uint _GetDllStatus();

		public delegate uint _GetDllVersion(ref byte major, ref byte minor, ref byte revision, ref byte release);

		public delegate uint _GetDriverVersion(ref byte major, ref byte minor, ref byte revision, ref byte release);

		public delegate uint _GetDriverType();

		public delegate int _InitializeOls();

		public delegate void _DeinitializeOls();

		public delegate int _IsCpuid();

		public delegate int _IsMsr();

		public delegate int _IsTsc();

		public delegate int _Hlt();

		public delegate int _HltTx(UIntPtr threadAffinityMask);

		public delegate int _HltPx(UIntPtr processAffinityMask);

		public delegate int _Rdmsr(uint index, ref uint eax, ref uint edx);

		public delegate int _RdmsrTx(uint index, ref uint eax, ref uint edx, UIntPtr threadAffinityMask);

		public delegate int _RdmsrPx(uint index, ref uint eax, ref uint edx, UIntPtr processAffinityMask);

		public delegate int _Wrmsr(uint index, uint eax, uint edx);

		public delegate int _WrmsrTx(uint index, uint eax, uint edx, UIntPtr threadAffinityMask);

		public delegate int _WrmsrPx(uint index, uint eax, uint edx, UIntPtr processAffinityMask);

		public delegate int _Rdpmc(uint index, ref uint eax, ref uint edx);

		public delegate int _RdpmcTx(uint index, ref uint eax, ref uint edx, UIntPtr threadAffinityMask);

		public delegate int _RdpmcPx(uint index, ref uint eax, ref uint edx, UIntPtr processAffinityMask);

		public delegate int _Cpuid(uint index, ref uint eax, ref uint ebx, ref uint ecx, ref uint edx);

		public delegate int _CpuidTx(uint index, ref uint eax, ref uint ebx, ref uint ecx, ref uint edx, UIntPtr threadAffinityMask);

		public delegate int _CpuidPx(uint index, ref uint eax, ref uint ebx, ref uint ecx, ref uint edx, UIntPtr processAffinityMask);

		public delegate int _Rdtsc(ref uint eax, ref uint edx);

		public delegate int _RdtscTx(ref uint eax, ref uint edx, UIntPtr threadAffinityMask);

		public delegate int _RdtscPx(ref uint eax, ref uint edx, UIntPtr processAffinityMask);

		public delegate byte _ReadIoPortByte(ushort port);

		public delegate ushort _ReadIoPortWord(ushort port);

		public delegate uint _ReadIoPortDword(ushort port);

		public delegate int _ReadIoPortByteEx(ushort port, ref byte value);

		public delegate int _ReadIoPortWordEx(ushort port, ref ushort value);

		public delegate int _ReadIoPortDwordEx(ushort port, ref uint value);

		public delegate void _WriteIoPortByte(ushort port, byte value);

		public delegate void _WriteIoPortWord(ushort port, ushort value);

		public delegate void _WriteIoPortDword(ushort port, uint value);

		public delegate int _WriteIoPortByteEx(ushort port, byte value);

		public delegate int _WriteIoPortWordEx(ushort port, ushort value);

		public delegate int _WriteIoPortDwordEx(ushort port, uint value);

		public delegate void _SetPciMaxBusIndex(byte max);

		public delegate byte _ReadPciConfigByte(uint pciAddress, byte regAddress);

		public delegate ushort _ReadPciConfigWord(uint pciAddress, byte regAddress);

		public delegate uint _ReadPciConfigDword(uint pciAddress, byte regAddress);

		public delegate int _ReadPciConfigByteEx(uint pciAddress, uint regAddress, ref byte value);

		public delegate int _ReadPciConfigWordEx(uint pciAddress, uint regAddress, ref ushort value);

		public delegate int _ReadPciConfigDwordEx(uint pciAddress, uint regAddress, ref uint value);

		public delegate void _WritePciConfigByte(uint pciAddress, byte regAddress, byte value);

		public delegate void _WritePciConfigWord(uint pciAddress, byte regAddress, ushort value);

		public delegate void _WritePciConfigDword(uint pciAddress, byte regAddress, uint value);

		public delegate int _WritePciConfigByteEx(uint pciAddress, uint regAddress, byte value);

		public delegate int _WritePciConfigWordEx(uint pciAddress, uint regAddress, ushort value);

		public delegate int _WritePciConfigDwordEx(uint pciAddress, uint regAddress, uint value);

		public delegate uint _FindPciDeviceById(ushort vendorId, ushort deviceId, byte index);

		public delegate uint _FindPciDeviceByClass(byte baseClass, byte subClass, byte programIf, byte index);

		private const string dllNameX64 = "WinRing0x64.dll";

		private const string dllName = "WinRing0.dll";

		private IntPtr module = IntPtr.Zero;

		private uint status;

		public _GetDllStatus GetDllStatus;

		public _GetDriverType GetDriverType;

		public _GetDllVersion GetDllVersion;

		public _GetDriverVersion GetDriverVersion;

		public _InitializeOls InitializeOls;

		public _DeinitializeOls DeinitializeOls;

		public _IsCpuid IsCpuid;

		public _IsMsr IsMsr;

		public _IsTsc IsTsc;

		public _Hlt Hlt;

		public _HltTx HltTx;

		public _HltPx HltPx;

		public _Rdmsr Rdmsr;

		public _RdmsrTx RdmsrTx;

		public _RdmsrPx RdmsrPx;

		public _Wrmsr Wrmsr;

		public _WrmsrTx WrmsrTx;

		public _WrmsrPx WrmsrPx;

		public _Rdpmc Rdpmc;

		public _RdpmcTx RdpmcTx;

		public _RdpmcPx RdpmcPx;

		public _Cpuid Cpuid;

		public _CpuidTx CpuidTx;

		public _CpuidPx CpuidPx;

		public _Rdtsc Rdtsc;

		public _RdtscTx RdtscTx;

		public _RdtscPx RdtscPx;

		public _ReadIoPortByte ReadIoPortByte;

		public _ReadIoPortWord ReadIoPortWord;

		public _ReadIoPortDword ReadIoPortDword;

		public _ReadIoPortByteEx ReadIoPortByteEx;

		public _ReadIoPortWordEx ReadIoPortWordEx;

		public _ReadIoPortDwordEx ReadIoPortDwordEx;

		public _WriteIoPortByte WriteIoPortByte;

		public _WriteIoPortWord WriteIoPortWord;

		public _WriteIoPortDword WriteIoPortDword;

		public _WriteIoPortByteEx WriteIoPortByteEx;

		public _WriteIoPortWordEx WriteIoPortWordEx;

		public _WriteIoPortDwordEx WriteIoPortDwordEx;

		public _SetPciMaxBusIndex SetPciMaxBusIndex;

		public _ReadPciConfigByte ReadPciConfigByte;

		public _ReadPciConfigWord ReadPciConfigWord;

		public _ReadPciConfigDword ReadPciConfigDword;

		public _ReadPciConfigByteEx ReadPciConfigByteEx;

		public _ReadPciConfigWordEx ReadPciConfigWordEx;

		public _ReadPciConfigDwordEx ReadPciConfigDwordEx;

		public _WritePciConfigByte WritePciConfigByte;

		public _WritePciConfigWord WritePciConfigWord;

		public _WritePciConfigDword WritePciConfigDword;

		public _WritePciConfigByteEx WritePciConfigByteEx;

		public _WritePciConfigWordEx WritePciConfigWordEx;

		public _WritePciConfigDwordEx WritePciConfigDwordEx;

		public _FindPciDeviceById FindPciDeviceById;

		public _FindPciDeviceByClass FindPciDeviceByClass;

		public uint PciBusDevFunc(uint bus, uint dev, uint func)
		{
			return ((bus & 0xFF) << 8) | ((dev & 0x1F) << 3) | (func & 7);
		}

		public uint PciGetBus(uint address)
		{
			return (address >> 8) & 0xFF;
		}

		public uint PciGetDev(uint address)
		{
			return (address >> 3) & 0x1F;
		}

		public uint PciGetFunc(uint address)
		{
			return address & 7;
		}

		[DllImport("kernel32")]
		public static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32", SetLastError = true)]
		private static extern bool FreeLibrary(IntPtr hModule);

		[DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

		public Ols()
		{
			module = LoadLibrary((IntPtr.Size != 8) ? "WinRing0.dll" : "WinRing0x64.dll");
			if (module == IntPtr.Zero)
			{
				status = 1u;
				return;
			}
			GetDllStatus = (_GetDllStatus)GetDelegate("GetDllStatus", typeof(_GetDllStatus));
			GetDllVersion = (_GetDllVersion)GetDelegate("GetDllVersion", typeof(_GetDllVersion));
			GetDriverVersion = (_GetDriverVersion)GetDelegate("GetDriverVersion", typeof(_GetDriverVersion));
			GetDriverType = (_GetDriverType)GetDelegate("GetDriverType", typeof(_GetDriverType));
			InitializeOls = (_InitializeOls)GetDelegate("InitializeOls", typeof(_InitializeOls));
			DeinitializeOls = (_DeinitializeOls)GetDelegate("DeinitializeOls", typeof(_DeinitializeOls));
			IsCpuid = (_IsCpuid)GetDelegate("IsCpuid", typeof(_IsCpuid));
			IsMsr = (_IsMsr)GetDelegate("IsMsr", typeof(_IsMsr));
			IsTsc = (_IsTsc)GetDelegate("IsTsc", typeof(_IsTsc));
			Hlt = (_Hlt)GetDelegate("Hlt", typeof(_Hlt));
			HltTx = (_HltTx)GetDelegate("HltTx", typeof(_HltTx));
			HltPx = (_HltPx)GetDelegate("HltPx", typeof(_HltPx));
			Rdmsr = (_Rdmsr)GetDelegate("Rdmsr", typeof(_Rdmsr));
			RdmsrTx = (_RdmsrTx)GetDelegate("RdmsrTx", typeof(_RdmsrTx));
			RdmsrPx = (_RdmsrPx)GetDelegate("RdmsrPx", typeof(_RdmsrPx));
			Wrmsr = (_Wrmsr)GetDelegate("Wrmsr", typeof(_Wrmsr));
			WrmsrTx = (_WrmsrTx)GetDelegate("WrmsrTx", typeof(_WrmsrTx));
			WrmsrPx = (_WrmsrPx)GetDelegate("WrmsrPx", typeof(_WrmsrPx));
			Rdpmc = (_Rdpmc)GetDelegate("Rdpmc", typeof(_Rdpmc));
			RdpmcTx = (_RdpmcTx)GetDelegate("RdpmcTx", typeof(_RdpmcTx));
			RdpmcPx = (_RdpmcPx)GetDelegate("RdpmcPx", typeof(_RdpmcPx));
			Cpuid = (_Cpuid)GetDelegate("Cpuid", typeof(_Cpuid));
			CpuidTx = (_CpuidTx)GetDelegate("CpuidTx", typeof(_CpuidTx));
			CpuidPx = (_CpuidPx)GetDelegate("CpuidPx", typeof(_CpuidPx));
			Rdtsc = (_Rdtsc)GetDelegate("Rdtsc", typeof(_Rdtsc));
			RdtscTx = (_RdtscTx)GetDelegate("RdtscTx", typeof(_RdtscTx));
			RdtscPx = (_RdtscPx)GetDelegate("RdtscPx", typeof(_RdtscPx));
			ReadIoPortByte = (_ReadIoPortByte)GetDelegate("ReadIoPortByte", typeof(_ReadIoPortByte));
			ReadIoPortWord = (_ReadIoPortWord)GetDelegate("ReadIoPortWord", typeof(_ReadIoPortWord));
			ReadIoPortDword = (_ReadIoPortDword)GetDelegate("ReadIoPortDword", typeof(_ReadIoPortDword));
			ReadIoPortByteEx = (_ReadIoPortByteEx)GetDelegate("ReadIoPortByteEx", typeof(_ReadIoPortByteEx));
			ReadIoPortWordEx = (_ReadIoPortWordEx)GetDelegate("ReadIoPortWordEx", typeof(_ReadIoPortWordEx));
			ReadIoPortDwordEx = (_ReadIoPortDwordEx)GetDelegate("ReadIoPortDwordEx", typeof(_ReadIoPortDwordEx));
			WriteIoPortByte = (_WriteIoPortByte)GetDelegate("WriteIoPortByte", typeof(_WriteIoPortByte));
			WriteIoPortWord = (_WriteIoPortWord)GetDelegate("WriteIoPortWord", typeof(_WriteIoPortWord));
			WriteIoPortDword = (_WriteIoPortDword)GetDelegate("WriteIoPortDword", typeof(_WriteIoPortDword));
			WriteIoPortByteEx = (_WriteIoPortByteEx)GetDelegate("WriteIoPortByteEx", typeof(_WriteIoPortByteEx));
			WriteIoPortWordEx = (_WriteIoPortWordEx)GetDelegate("WriteIoPortWordEx", typeof(_WriteIoPortWordEx));
			WriteIoPortDwordEx = (_WriteIoPortDwordEx)GetDelegate("WriteIoPortDwordEx", typeof(_WriteIoPortDwordEx));
			SetPciMaxBusIndex = (_SetPciMaxBusIndex)GetDelegate("SetPciMaxBusIndex", typeof(_SetPciMaxBusIndex));
			ReadPciConfigByte = (_ReadPciConfigByte)GetDelegate("ReadPciConfigByte", typeof(_ReadPciConfigByte));
			ReadPciConfigWord = (_ReadPciConfigWord)GetDelegate("ReadPciConfigWord", typeof(_ReadPciConfigWord));
			ReadPciConfigDword = (_ReadPciConfigDword)GetDelegate("ReadPciConfigDword", typeof(_ReadPciConfigDword));
			ReadPciConfigByteEx = (_ReadPciConfigByteEx)GetDelegate("ReadPciConfigByteEx", typeof(_ReadPciConfigByteEx));
			ReadPciConfigWordEx = (_ReadPciConfigWordEx)GetDelegate("ReadPciConfigWordEx", typeof(_ReadPciConfigWordEx));
			ReadPciConfigDwordEx = (_ReadPciConfigDwordEx)GetDelegate("ReadPciConfigDwordEx", typeof(_ReadPciConfigDwordEx));
			WritePciConfigByte = (_WritePciConfigByte)GetDelegate("WritePciConfigByte", typeof(_WritePciConfigByte));
			WritePciConfigWord = (_WritePciConfigWord)GetDelegate("WritePciConfigWord", typeof(_WritePciConfigWord));
			WritePciConfigDword = (_WritePciConfigDword)GetDelegate("WritePciConfigDword", typeof(_WritePciConfigDword));
			WritePciConfigByteEx = (_WritePciConfigByteEx)GetDelegate("WritePciConfigByteEx", typeof(_WritePciConfigByteEx));
			WritePciConfigWordEx = (_WritePciConfigWordEx)GetDelegate("WritePciConfigWordEx", typeof(_WritePciConfigWordEx));
			WritePciConfigDwordEx = (_WritePciConfigDwordEx)GetDelegate("WritePciConfigDwordEx", typeof(_WritePciConfigDwordEx));
			FindPciDeviceById = (_FindPciDeviceById)GetDelegate("FindPciDeviceById", typeof(_FindPciDeviceById));
			FindPciDeviceByClass = (_FindPciDeviceByClass)GetDelegate("FindPciDeviceByClass", typeof(_FindPciDeviceByClass));
			if (GetDllStatus == null || GetDllVersion == null || GetDriverVersion == null || GetDriverType == null || InitializeOls == null || DeinitializeOls == null || IsCpuid == null || IsMsr == null || IsTsc == null || Hlt == null || HltTx == null || HltPx == null || Rdmsr == null || RdmsrTx == null || RdmsrPx == null || Wrmsr == null || WrmsrTx == null || WrmsrPx == null || Rdpmc == null || RdpmcTx == null || RdpmcPx == null || Cpuid == null || CpuidTx == null || CpuidPx == null || Rdtsc == null || RdtscTx == null || RdtscPx == null || ReadIoPortByte == null || ReadIoPortWord == null || ReadIoPortDword == null || ReadIoPortByteEx == null || ReadIoPortWordEx == null || ReadIoPortDwordEx == null || WriteIoPortByte == null || WriteIoPortWord == null || WriteIoPortDword == null || WriteIoPortByteEx == null || WriteIoPortWordEx == null || WriteIoPortDwordEx == null || SetPciMaxBusIndex == null || ReadPciConfigByte == null || ReadPciConfigWord == null || ReadPciConfigDword == null || ReadPciConfigByteEx == null || ReadPciConfigWordEx == null || ReadPciConfigDwordEx == null || WritePciConfigByte == null || WritePciConfigWord == null || WritePciConfigDword == null || WritePciConfigByteEx == null || WritePciConfigWordEx == null || WritePciConfigDwordEx == null || FindPciDeviceById == null || FindPciDeviceByClass == null)
			{
				status = 2u;
			}
			if (InitializeOls() == 0)
			{
				status = 3u;
			}
		}

		public uint GetStatus()
		{
			return status;
		}

		public void Dispose()
		{
			if (module != IntPtr.Zero)
			{
				DeinitializeOls();
				FreeLibrary(module);
				module = IntPtr.Zero;
			}
		}

		public Delegate GetDelegate(string procName, Type delegateType)
		{
			IntPtr procAddress = GetProcAddress(module, procName);
			if (procAddress != IntPtr.Zero)
			{
				return Marshal.GetDelegateForFunctionPointer(procAddress, delegateType);
			}
			throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
		}
	}
}
namespace BydCentral.Services
{
	public class AppControl
	{
		private const string QuickName = "ControlCenter";

		private string appAllPath => Process.GetCurrentProcess().MainModule.FileName;

		private string iniPath => Directory.GetCurrentDirectory() + "/config.ini";

		private string systemStartPath => Environment.GetFolderPath(Environment.SpecialFolder.Startup);

		private string desktopPath => Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		public void CreateIniFile()
		{
			if (!File.Exists(iniPath))
			{
				File.Create(iniPath);
			}
		}

		public string GetPath()
		{
			return iniPath;
		}

		public void INIWrite(string section, string key, string value)
		{
			WritePrivateProfileString(section, key, value, iniPath);
		}

		public string INIRead(string section, string key)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			GetPrivateProfileString(section, key, "", stringBuilder, 255, iniPath);
			if (stringBuilder.Length == 0)
			{
				return "no config item";
			}
			return stringBuilder.ToString();
		}

		public void INIDelete(string FilePath)
		{
			File.Delete(FilePath);
		}

		public void SetMeAutoStart(bool onOff = true)
		{
			if (onOff)
			{
				List<string> quickFromFolder = GetQuickFromFolder(systemStartPath, appAllPath);
				if (quickFromFolder.Count >= 2)
				{
					for (int i = 1; i < quickFromFolder.Count; i++)
					{
						DeleteFile(quickFromFolder[i]);
					}
				}
				else if (quickFromFolder.Count < 1)
				{
					CreateShortcut(systemStartPath, "ControlCenter", appAllPath, "ControlCenter");
				}
			}
			else
			{
				List<string> quickFromFolder2 = GetQuickFromFolder(systemStartPath, appAllPath);
				if (quickFromFolder2.Count > 0)
				{
					for (int j = 0; j < quickFromFolder2.Count; j++)
					{
						DeleteFile(quickFromFolder2[j]);
					}
				}
			}
			_ = Process.GetCurrentProcess().MainModule.FileName;
		}

		private bool CreateShortcut(string directory, string shortcutName, string targetPath, string description = null, string iconLocation = null)
		{
			try
			{
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
				string pathLink = Path.Combine(directory, $"{shortcutName}.lnk");
				WshShell wshShell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
				IWshShortcut obj = (IWshShortcut)(dynamic)wshShell.CreateShortcut(pathLink);
				obj.TargetPath = targetPath;
				obj.WorkingDirectory = Path.GetDirectoryName(targetPath);
				obj.WindowStyle = 1;
				obj.Description = description;
				obj.IconLocation = (string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation);
				obj.Save();
				return true;
			}
			catch (Exception ex)
			{
				_ = ex.Message;
			}
			return false;
		}

		private List<string> GetQuickFromFolder(string directory, string targetPath)
		{
			List<string> list = new List<string>();
			list.Clear();
			string[] files = Directory.GetFiles(directory, "*.lnk");
			if (files == null || files.Length < 1)
			{
				return list;
			}
			for (int i = 0; i < files.Length; i++)
			{
				if (GetAppPathFromQuick(files[i]) == targetPath)
				{
					list.Add(files[i]);
				}
			}
			return list;
		}

		private string GetAppPathFromQuick(string shortcutPath)
		{
			if (File.Exists(shortcutPath))
			{
				WshShell wshShell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
				return ((IWshShortcut)(dynamic)wshShell.CreateShortcut(shortcutPath)).TargetPath;
			}
			return "";
		}

		private void DeleteFile(string path)
		{
			if (File.GetAttributes(path) == FileAttributes.Directory)
			{
				Directory.Delete(path, recursive: true);
			}
			else
			{
				File.Delete(path);
			}
		}

		public void CreateDesktopQuick(string desktopPath = "", string quickName = "", string appPath = "")
		{
			if (GetQuickFromFolder(desktopPath, appPath).Count < 1)
			{
				CreateShortcut(desktopPath, quickName, appPath, "软件描述");
			}
		}

		public bool SetMeAutoStartRegistry(bool onOff)
		{
			string appName = "ControlCenter";
			string fileName = Process.GetCurrentProcess().MainModule.FileName;
			return SetAutoStart(onOff, appName, fileName);
		}

		public bool SetAutoStart(bool onOff, string appName, string appPath)
		{
			bool result = true;
			if (!IsExistKey(appName) && onOff)
			{
				result = SelfRunning(onOff, appName, appPath);
			}
			else if (IsExistKey(appName) && !onOff)
			{
				result = SelfRunning(onOff, appName, appPath);
			}
			return result;
		}

		private bool IsExistKey(string keyName)
		{
			try
			{
				bool result = false;
				RegistryKey localMachine = Registry.LocalMachine;
				RegistryKey registryKey = localMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
				if (registryKey == null)
				{
					registryKey = localMachine.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows")
						.CreateSubKey("CurrentVersion")
						.CreateSubKey("Run");
				}
				string[] valueNames = registryKey.GetValueNames();
				for (int i = 0; i < valueNames.Length; i++)
				{
					if (valueNames[i].ToUpper() == keyName.ToUpper())
					{
						return true;
					}
				}
				return result;
			}
			catch
			{
				return false;
			}
		}

		private bool SelfRunning(bool isStart, string exeName, string path)
		{
			try
			{
				RegistryKey localMachine = Registry.LocalMachine;
				RegistryKey registryKey = localMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
				if (registryKey == null)
				{
					localMachine.CreateSubKey("SOFTWARE//Microsoft//Windows//CurrentVersion//Run");
				}
				if (isStart)
				{
					registryKey.SetValue(exeName, path);
					registryKey.Close();
				}
				else
				{
					string[] valueNames = registryKey.GetValueNames();
					for (int i = 0; i < valueNames.Length; i++)
					{
						if (valueNames[i].ToUpper() == exeName.ToUpper())
						{
							registryKey.DeleteValue(exeName);
							registryKey.Close();
						}
					}
				}
			}
			catch (Exception ex)
			{
				_ = ex.Message;
				return false;
			}
			return true;
		}

		public void setAutoTask(bool autoTask)
		{
			if (autoTask)
			{
				CreateTask();
			}
			else
			{
				delTask();
			}
		}

		private void CreateTask()
		{
			TaskService taskService = new TaskService();
			try
			{
				Microsoft.Win32.TaskScheduler.Task task = taskService.GetTask("ControlCenter");
				if (task != null)
				{
					TaskDefinition definition = task.Definition;
					definition.Actions.Clear();
					definition.Actions.Add(new ExecAction(appAllPath, "-minimized", Directory.GetCurrentDirectory()));
					taskService.RootFolder.RegisterTaskDefinition("ControlCenter", definition);
					return;
				}
				TaskDefinition taskDefinition = taskService.NewTask();
				taskDefinition.RegistrationInfo.Description = "ControlCenter";
				taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;
				taskDefinition.Settings.DisallowStartIfOnBatteries = false;
				taskDefinition.Settings.StopIfGoingOnBatteries = false;
				taskDefinition.Triggers.Add(new LogonTrigger());
				taskDefinition.Actions.Add(new ExecAction(appAllPath, "-minimized", Directory.GetCurrentDirectory()));
				taskService.RootFolder.RegisterTaskDefinition("ControlCenter", taskDefinition);
			}
			finally
			{
				((IDisposable)(object)taskService)?.Dispose();
			}
		}

		private void delTask()
		{
			TaskService taskService = new TaskService();
			try
			{
				if (taskService.GetTask("ControlCenter") != null)
				{
					taskService.RootFolder.DeleteTask("ControlCenter");
				}
			}
			finally
			{
				((IDisposable)(object)taskService)?.Dispose();
			}
		}
	}
}
namespace BydCentral.Core.ViewModels
{
	public class DelegateCommand : ICommand
	{
		public System.Action CommandAction { get; set; }

		public Func<bool> CanExecuteFunc { get; set; }

		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}

		public void Execute(object parameter)
		{
			CommandAction();
		}

		public bool CanExecute(object parameter)
		{
			if (CanExecuteFunc != null)
			{
				return CanExecuteFunc();
			}
			return true;
		}
	}
	public class MainViewModel : ObservableObject
	{
		private int _value;

		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				SetProperty(ref _value, value, "Value");
			}
		}

		public ICommand BtnCommand { get; set; }

		public MainViewModel()
		{
			BtnCommand = new RelayCommand<object>(RunTest);
		}

		private void Message(object obj, string message)
		{
		}

		private async void RunTest(object obj)
		{
			int i = 0;
			while (i < 10)
			{
				await System.Threading.Tasks.Task.Delay(1000);
				Value = i++;
			}
		}
	}
	public class NotifyIconViewModel
	{
		public ICommand ShowWindowCommand => new DelegateCommand
		{
			CanExecuteFunc = () => Application.Current.MainWindow != null,
			CommandAction = delegate
			{
				if (Application.Current.MainWindow.FindName("CentralIcon") is NotifyIcon notifyIcon)
				{
					notifyIcon.ContextMenu.IsOpen = false;
				}
				System.Windows.Window mainWindow = Application.Current.MainWindow;
				if (mainWindow != null)
				{
					if (mainWindow.WindowState == WindowState.Minimized)
					{
						mainWindow.WindowState = WindowState.Normal;
						mainWindow.ShowInTaskbar = true;
					}
					mainWindow.Show();
					mainWindow.Activate();
				}
			}
		};

		public ICommand HideWindowCommand => new DelegateCommand
		{
			CommandAction = delegate
			{
				Application.Current.MainWindow.Hide();
			},
			CanExecuteFunc = () => Application.Current.MainWindow != null
		};

		public ICommand ExitApplicationCommand => new DelegateCommand
		{
			CommandAction = delegate
			{
				Application.Current.Shutdown();
			}
		};
	}
	public class Page1ViewModel : ObservableObject
	{
		public static List<ICommand> commands = new List<ICommand>();

		public static List<ICommand> commandsRoute = new List<ICommand>();

		private byte[] resData = new byte[64];

		private Usb GetInfoUsb = new Usb();

		private byte[] data = new byte[64]
		{
			6, 2, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 2
		};

		private int _CpuOP;

		private string? _CpuName = "CPU";

		private uint _CpuTem;

		private int _GpuOP;

		private string? _GpuName = "GPU";

		private uint _GpuTem;

		private int _SSDOP;

		private string? _SSDName;

		private string? _SSDOPL;

		private int _MemOP;

		private string? _MemName;

		private string? _MemOPL;

		private double _CpuFan;

		private double _CpuFanMax = 4000.0;

		private double _GpuFan;

		private double _GpuFanMax = 4000.0;

		private double _CpuFrequency;

		private double _CpuMaxFrequency = 3600.0;

		private double _GpuFrequency;

		private double _GpuMaxFrequency = 3600.0;

		public int CpuOP
		{
			get
			{
				return _CpuOP;
			}
			set
			{
				if (value > 100)
				{
					value = 100;
				}
				SetProperty(ref _CpuOP, value, "CpuOP");
			}
		}

		public string? CpuName
		{
			get
			{
				return _CpuName;
			}
			set
			{
				SetProperty(ref _CpuName, value, "CpuName");
			}
		}

		public uint CpuTem
		{
			get
			{
				return _CpuTem;
			}
			set
			{
				if (value != 0)
				{
					SetProperty(ref _CpuTem, value, "CpuTem");
				}
			}
		}

		public int GpuOP
		{
			get
			{
				return _GpuOP;
			}
			set
			{
				if (value > 100)
				{
					value = 100;
				}
				SetProperty(ref _GpuOP, value, "GpuOP");
			}
		}

		public string? GpuName
		{
			get
			{
				return _GpuName;
			}
			set
			{
				SetProperty(ref _GpuName, value, "GpuName");
			}
		}

		public uint GpuTem
		{
			get
			{
				return _GpuTem;
			}
			set
			{
				SetProperty(ref _GpuTem, value, "GpuTem");
			}
		}

		public int SSDOP
		{
			get
			{
				return _SSDOP;
			}
			set
			{
				SetProperty(ref _SSDOP, value, "SSDOP");
			}
		}

		public string? SSDName
		{
			get
			{
				return _SSDName;
			}
			set
			{
				SetProperty(ref _SSDName, value, "SSDName");
			}
		}

		public string? SSDOPL
		{
			get
			{
				return _SSDOPL;
			}
			set
			{
				SetProperty(ref _SSDOPL, value, "SSDOPL");
			}
		}

		public int MemOP
		{
			get
			{
				return _MemOP;
			}
			set
			{
				SetProperty(ref _MemOP, value, "MemOP");
			}
		}

		public string? MemName
		{
			get
			{
				return _MemName;
			}
			set
			{
				SetProperty(ref _MemName, value, "MemName");
			}
		}

		public string? MemOPL
		{
			get
			{
				return _MemOPL;
			}
			set
			{
				SetProperty(ref _MemOPL, value, "MemOPL");
			}
		}

		public double CpuFan
		{
			get
			{
				return _CpuFan;
			}
			set
			{
				SetProperty(ref _CpuFan, value, "CpuFan");
			}
		}

		public double CpuFanMax
		{
			get
			{
				return _CpuFanMax;
			}
			set
			{
				SetProperty(ref _CpuFanMax, value, "CpuFanMax");
			}
		}

		public double GpuFan
		{
			get
			{
				return _GpuFan;
			}
			set
			{
				SetProperty(ref _GpuFan, value, "GpuFan");
			}
		}

		public double GpuFanMax
		{
			get
			{
				return _GpuFanMax;
			}
			set
			{
				SetProperty(ref _GpuFanMax, value, "GpuFanMax");
			}
		}

		public double CpuFrequency
		{
			get
			{
				return _CpuFrequency;
			}
			set
			{
				if (value != 2022.0)
				{
					SetProperty(ref _CpuFrequency, value, "CpuFrequency");
				}
			}
		}

		public double CpuMaxFrequency
		{
			get
			{
				return _CpuMaxFrequency;
			}
			set
			{
				SetProperty(ref _CpuMaxFrequency, value, "CpuMaxFrequency");
			}
		}

		public double GpuFrequency
		{
			get
			{
				return _GpuFrequency;
			}
			set
			{
				SetProperty(ref _GpuFrequency, value, "GpuFrequency");
			}
		}

		public double GpuMaxFrequency
		{
			get
			{
				return _GpuMaxFrequency;
			}
			set
			{
				SetProperty(ref _GpuMaxFrequency, value, "GpuMaxFrequency");
			}
		}

		public ICommand SetQModeCommand { get; }

		public ICommand SetBModeCommand { get; }

		public ICommand SetGModeCommand { get; }

		public static ICommand? SetCpuFrequencyCommand { get; set; }

		public static ICommand? SetGpuFrequencyCommand { get; set; }

		public static ICommand? SetCpuMaxFrequencyCommand { get; set; }

		public static ICommand? SetGpuMaxFrequencyCommand { get; set; }

		public static ICommand? SetFanCommand { get; set; }

		public static ICommand? GetSystemModeCommand { get; set; }

		public static ICommand? GetFanMaxCommand { get; set; }

		public Page1ViewModel()
		{
			SetInitInfo();
			commands.Add(new AsyncRelayCommand(SetInitInfo));
			SetQModeCommand = new AsyncRelayCommand(SetQMode);
			SetBModeCommand = new AsyncRelayCommand(SetBMode);
			SetGModeCommand = new AsyncRelayCommand(SetGMode);
			commandsRoute.Add(new AsyncRelayCommand<CancellationToken>(ExecuteAsync));
			SetCpuMaxFrequencyCommand = new AsyncRelayCommand(SetCpuMaxFrequency);
			SetGpuMaxFrequencyCommand = new AsyncRelayCommand(SetGpuMaxFrequency);
			GetSystemModeCommand = new AsyncRelayCommand<CancellationToken>(GetSystemMode);
			GetFanMaxCommand = new AsyncRelayCommand(GetFanMax);
		}

		private async System.Threading.Tasks.Task SetInitInfo()
		{
			System.Threading.Tasks.Task.Run(delegate
			{
				CpuFanMax = 100 * GlobalVars.Wmi.GetCPUFanMax();
			});
			System.Threading.Tasks.Task.Run(delegate
			{
				GpuFanMax = 100 * GlobalVars.Wmi.GetGPUFanMax();
			});
			System.Threading.Tasks.Task.Run(delegate
			{
				CpuName = GlobalVars.Win32.GetCPUName();
			});
			System.Threading.Tasks.Task.Run(delegate
			{
				GpuName = GlobalVars.Win32.GetGPUName();
			});
			System.Threading.Tasks.Task.Run(delegate
			{
				SSDName = GlobalVars.Win32.GetDiskName();
			});
			System.Threading.Tasks.Task.Run(delegate
			{
				MemName = GlobalVars.Win32.GetMemoryName();
			});
		}

		private async System.Threading.Tasks.Task GetFanMax()
		{
			System.Threading.Tasks.Task.Run(delegate
			{
				CpuFanMax = 100 * GlobalVars.Wmi.GetCPUFanMax();
				GpuFanMax = 100 * GlobalVars.Wmi.GetGPUFanMax();
			});
		}

		private async System.Threading.Tasks.Task SetCpuMaxFrequency()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				CpuMaxFrequency = 2.2 * Convert.ToDouble(GlobalVars.Win32.GetCpuMaxFrequency());
			});
		}

		private async System.Threading.Tasks.Task SetGpuMaxFrequency()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				GpuMaxFrequency = Convert.ToDouble(GlobalVars.Win32.GetNVGpuMaxFrequency());
			});
		}

		private async System.Threading.Tasks.Task SetQMode()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				GlobalVars.Wmi.SetPerformanceMode(0);
				GlobalVars.usb.writeUsbFan(TxBuf.PARAMS.Fan_Ctrl.OfficeMode, GlobalVars.txBufClass, GlobalVars.txBufStruct);
			});
		}

		private async System.Threading.Tasks.Task SetBMode()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				GlobalVars.Wmi.SetPerformanceMode(1);
				GlobalVars.usb.writeUsbFan(TxBuf.PARAMS.Fan_Ctrl.PerformanceMode, GlobalVars.txBufClass, GlobalVars.txBufStruct);
			});
		}

		private async System.Threading.Tasks.Task SetGMode()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				GlobalVars.Wmi.SetPerformanceMode(2);
				GlobalVars.usb.writeUsbFan(TxBuf.PARAMS.Fan_Ctrl.GamingMode, GlobalVars.txBufClass, GlobalVars.txBufStruct);
			});
		}

		private async System.Threading.Tasks.Task GetCpuUsageAsync()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				CpuOP = (int)GlobalVars.Win32.GetCPUOP();
			});
		}

		private async System.Threading.Tasks.Task GetSystemMode(CancellationToken cancellationToken)
		{
			_ = 1;
			try
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					await System.Threading.Tasks.Task.Run(delegate
					{
						string systemMode = resData[27].ToString();
						Application.Current.Dispatcher.Invoke(delegate
						{
							GlobalVars.SystemMode = systemMode;
						});
					});
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
				}
			}
			catch (Exception ex)
			{
				LoggerHelper._.Debug(ex.Message.ToString());
			}
		}

		private async System.Threading.Tasks.Task ExecuteAsync(CancellationToken cancellationToken)
		{
			_ = 1;
			try
			{
				while (!cancellationToken.IsCancellationRequested && GlobalVars.GetInfoServiceRun)
				{
					resData = GetInfoUsb.GetData(data);
					await System.Threading.Tasks.Task.WhenAll(System.Threading.Tasks.Task.Run(delegate
					{
						SSDOPL = GlobalVars.Win32.GetDiskUsed() + "/" + GlobalVars.Win32.GetDiskTotalSize();
					}), System.Threading.Tasks.Task.Run(delegate
					{
						SSDOP = Convert.ToInt32(GlobalVars.Win32.GetDiskOP());
					}), System.Threading.Tasks.Task.Run(delegate
					{
						if (GpuName.Contains("NV"))
						{
							CpuTem = resData[11];
							GpuTem = resData[12];
						}
						else
						{
							CpuTem = resData[13];
							GpuTem = GlobalVars.Wmi.getIgpuTem();
						}
						GpuFan = 100 * resData[10];
						CpuFan = 100 * resData[9];
						string systemMode = resData[26].ToString();
						Application.Current.Dispatcher.Invoke(delegate
						{
							GlobalVars.SystemMode = systemMode;
						});
					}), System.Threading.Tasks.Task.Run(delegate
					{
						CpuOP = (int)GlobalVars.Win32.GetCPUOP();
					}), System.Threading.Tasks.Task.Run(delegate
					{
						GpuOP = (int)GlobalVars.Win32.GetGPUOP();
					}), System.Threading.Tasks.Task.Run(delegate
					{
						MemOP = 100 - (int)Math.Round(100.0 * Convert.ToDouble(GlobalVars.Win32.GetMemoryAvailable()) / Convert.ToDouble(GlobalVars.Win32.GetMemoryTotalSize()));
					}), System.Threading.Tasks.Task.Run(delegate
					{
						MemOPL = (GlobalVars.Win32.GetMemoryTotalSize() - GlobalVars.Win32.GetMemoryAvailable()).ToString("0.0") + "GB/" + GlobalVars.Win32.GetMemoryTotalSize().ToString("0.0") + "GB";
					}));
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
				}
			}
			catch (Exception ex)
			{
				LoggerHelper._.Debug(ex.Message.ToString());
			}
		}
	}
	public class Page2ViewModel : ObservableObject, INotifyPropertyChanged
	{
		private AppControl app;

		private bool isIni;

		private string keyboardLightStatus = "";

		private string backLightStatus = "";

		private bool _isModeCustom;

		public string KeyboardLightStatus
		{
			get
			{
				return keyboardLightStatus;
			}
			set
			{
				if (keyboardLightStatus != value)
				{
					keyboardLightStatus = value;
					WeakReferenceMessenger.Default.Send(keyboardLightStatus, "KeyboardLightStatus");
				}
			}
		}

		public string BackLightStatus
		{
			get
			{
				return backLightStatus;
			}
			set
			{
				if (backLightStatus != value)
				{
					backLightStatus = value;
					WeakReferenceMessenger.Default.Send(backLightStatus, "BackLightStatus");
				}
			}
		}

		public bool IsModeCustom
		{
			get
			{
				return _isModeCustom;
			}
			set
			{
				SetProperty(ref _isModeCustom, value, "IsModeCustom");
				SetModeDefaultOrNot();
			}
		}

		public ICommand DefaultButtonCommand { get; set; }

		public static ICommand? GetStatusCommand { get; set; }

		public static ICommand? GetCOMCommand { get; set; }

		public Page2ViewModel()
		{
			GetStatusCommand = new AsyncRelayCommand(GetStatus);
			GetCOMCommand = new AsyncRelayCommand(GetCOM);
			InitPage2();
		}

		private async System.Threading.Tasks.Task GetStatus()
		{
			BackLightStatus = GlobalVars.Wmi.getLight2().ToString();
			KeyboardLightStatus = GlobalVars.Wmi.getLight1().ToString();
		}

		private void InitPage2()
		{
			app = new AppControl();
			string text = app.INIRead("system", "IsCustom");
			IsModeCustom = text == "Y";
		}

		private void SetModeDefaultOrNot()
		{
			string value = (IsModeCustom ? "Y" : "N");
			app.INIWrite("system", "IsCustom", value);
		}

		private async System.Threading.Tasks.Task GetCOM()
		{
			GlobalVars.LightCOM = GlobalVars.Win32.getCom();
		}
	}
	public class Page3ViewModel : ObservableObject
	{
		private IList<string> _dataList;

		private int _selectedDisplayOptionIndex = GlobalVars.DisplayMode - 1;

		private int _GPUMode;

		private System.Version _version;

		private int _BatteryStatus;

		private int _Battery;

		public IList<string> DataList
		{
			get
			{
				return _dataList;
			}
			set
			{
				SetProperty(ref _dataList, value, "DataList");
			}
		}

		public int SelectedDisplayOptionIndex
		{
			get
			{
				return _selectedDisplayOptionIndex;
			}
			set
			{
				if (_selectedDisplayOptionIndex != value)
				{
					_selectedDisplayOptionIndex = value;
					OnPropertyChanged("SelectedDisplayOptionIndex");
				}
			}
		}

		public int GPUMode
		{
			get
			{
				return _GPUMode;
			}
			set
			{
				SetProperty(ref _GPUMode, value, "GPUMode");
			}
		}

		public System.Version version
		{
			get
			{
				return _version;
			}
			set
			{
				SetProperty(ref _version, value, "version");
			}
		}

		public int BatteryStatus
		{
			get
			{
				return _BatteryStatus;
			}
			set
			{
				SetProperty(ref _BatteryStatus, value, "BatteryStatus");
			}
		}

		public int Battery
		{
			get
			{
				return _Battery;
			}
			set
			{
				SetProperty(ref _Battery, value, "Battery");
			}
		}

		public static ICommand? BatteryCommand { get; set; }

		public static ICommand? WinkeylockCommand { get; set; }

		public static ICommand UsbChargeCommand { get; set; }

		public ICommand FullFanCommand { get; }

		public ICommand FullFanOffCommand { get; }

		public ICommand DisplayCommand { get; }

		public static ICommand? GpuModeCommand { get; set; }

		public static ICommand? GetInfoCommand { get; set; }

		public Page3ViewModel()
		{
			UsbChargeCommand = new RelayCommand<bool>(setUsbCharge);
			BatteryCommand = new AsyncRelayCommand<CancellationToken>(setBattery);
			WinkeylockCommand = new RelayCommand<bool>(setWinkeyLock);
			FullFanCommand = new RelayCommand<bool>(setFullFan);
			DisplayCommand = new RelayCommand<string>(setDisplay);
			GpuModeCommand = new AsyncRelayCommand(GetGpuMode);
			GetInfoCommand = new AsyncRelayCommand(GetInfo);
			GetComboBoxDemoDataList();
		}

		private List<string> GetComboBoxDemoDataList()
		{
			List<string> list = new List<string>();
			list.Add("dGpu only");
			list.Add("Dynamic");
			list.Add("iGpu only");
			DataList = list;
			return list;
		}

		private async System.Threading.Tasks.Task setBattery(CancellationToken cancellationToken)
		{
			GlobalVars.Service3TaskStop = false;
			try
			{
				while (!cancellationToken.IsCancellationRequested && GlobalVars.GetBatteryServiceRun)
				{
					await System.Threading.Tasks.Task.WhenAll(System.Threading.Tasks.Task.Run(delegate
					{
						BatteryStatus = Convert.ToInt32(GlobalVars.Win32.GetBatteryChargingStatus());
					}), System.Threading.Tasks.Task.Run(delegate
					{
						Battery = Convert.ToInt32(GlobalVars.Win32.GetBatteryRemaining());
					}));
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
				}
			}
			catch (Exception ex)
			{
				LoggerHelper._.Debug(ex.Message.ToString());
			}
			GlobalVars.Service3TaskStop = true;
		}

		private void setWinkeyLock(bool flag)
		{
			if (flag)
			{
				GlobalVars.Win32.Hook_Start();
			}
			else
			{
				GlobalVars.Win32.Hook_Clear();
			}
		}

		private void setFullFan(bool flag)
		{
			if (flag)
			{
				GlobalVars.Wmi.SetFanFullMode(flag: true);
				GlobalVars.usb.writeUsbFan(TxBuf.PARAMS.Fan_Ctrl.FullSpeed, GlobalVars.txBufClass, GlobalVars.txBufStruct);
			}
			else
			{
				GlobalVars.Wmi.SetFanFullMode(flag: false);
				GlobalVars.usb.writeUsbFan(TxBuf.PARAMS.Fan_Ctrl.FullSpeedOff, GlobalVars.txBufClass, GlobalVars.txBufStruct);
			}
		}

		private void setUsbCharge(bool flag)
		{
			GlobalVars.Wmi.SetUSBCharging(flag);
			if (flag)
			{
				GlobalVars.Wmi.SMI(3u, 1, 0, 1);
			}
			else
			{
				GlobalVars.Wmi.SMI(3u, 1, 0, 0);
			}
		}

		private void setDisplay(string flag)
		{
			int gPUMode = Convert.ToInt32(flag);
			GlobalVars.Wmi.SetGPUMode(gPUMode);
		}

		private async System.Threading.Tasks.Task setVersion()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				version = executingAssembly.GetName().Version;
			});
		}

		private async System.Threading.Tasks.Task GetGpuMode()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				GlobalVars.DisplayMode = (int)GlobalVars.Wmi.GetGPUMode();
			});
		}

		private async System.Threading.Tasks.Task GetInfo()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				GlobalVars.UsbChargeStatus = GlobalVars.Wmi.SMI(3u, 0, 0, 0);
			});
			await System.Threading.Tasks.Task.Run(delegate
			{
				GlobalVars.FanMaxStatus = GlobalVars.Wmi.GetFanFullMode();
			});
		}
	}
	public class Page4ViewModel : ObservableObject
	{
		public readonly IclientRequest clientRequest;

		private string[] _nowVersion = new string[1] { "" };

		private string[] _newVersion;

		private Visibility _visibilitybios = Visibility.Hidden;

		private Visibility[] _visibility = new Visibility[3]
		{
			Visibility.Hidden,
			Visibility.Hidden,
			Visibility.Hidden
		};

		private string[] _CanUpdate;

		private int _Pvalue;

		private System.Version _version;

		public int processIDValue = 1;

		public ObservableCollection<Item> items { get; } = new ObservableCollection<Item>();

		public ObservableCollection<Item> FwItems { get; } = new ObservableCollection<Item>();

		public ICommand CheckUpdateCommand { get; }

		public ICommand UpdateCommand { get; }

		public ICommand DriverCommand { get; }

		public static ICommand? VersionCommand { get; set; }

		public ICommand CheckBackupCommand { get; set; }

		public string[] nowVersion
		{
			get
			{
				return _nowVersion;
			}
			set
			{
				SetProperty(ref _nowVersion, value, "nowVersion");
			}
		}

		public string[] newVersion
		{
			get
			{
				return _newVersion;
			}
			set
			{
				SetProperty(ref _newVersion, value, "newVersion");
			}
		}

		public Visibility visibilitybios
		{
			get
			{
				return _visibilitybios;
			}
			set
			{
				SetProperty(ref _visibilitybios, value, "visibilitybios");
			}
		}

		public Visibility[] visibility
		{
			get
			{
				return _visibility;
			}
			set
			{
				SetProperty(ref _visibility, value, "visibility");
			}
		}

		public string[] CanUpdate
		{
			get
			{
				return _CanUpdate;
			}
			set
			{
				SetProperty(ref _CanUpdate, value, "CanUpdate");
			}
		}

		public int Pvalue
		{
			get
			{
				return _Pvalue;
			}
			set
			{
				SetProperty(ref _Pvalue, value, "Pvalue");
			}
		}

		public System.Version version
		{
			get
			{
				return _version;
			}
			set
			{
				SetProperty(ref _version, value, "version");
			}
		}

		public void AddItem(Item item)
		{
			items.Add(item);
		}

		public void AddFwItems(Item item)
		{
			FwItems.Add(item);
		}

		public Page4ViewModel(IclientRequest clientRequest)
		{
			this.clientRequest = clientRequest;
			CheckUpdateCommand = new AsyncRelayCommand(CheckUpdate);
			UpdateCommand = new AsyncRelayCommand<string>(Update);
			DriverCommand = new AsyncRelayCommand<string>(Driver);
			VersionCommand = new AsyncRelayCommand(setVersion);
			CheckBackupCommand = new AsyncRelayCommand(CheckBackup);
		}

		private async System.Threading.Tasks.Task setVersion()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				version = executingAssembly.GetName().Version;
			});
		}

		private async System.Threading.Tasks.Task CheckUpdate()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				(string[], int) tuple = clientRequest.checkUpdateServer();
				(CanUpdate, _) = tuple;
				if (tuple.Item2 == 1)
				{
					int num = CanUpdate.Length;
					if (num > 0)
					{
						for (int i = 0; i < num && !("NULL" == CanUpdate[i]); i++)
						{
							if ("BIOS" == CanUpdate[i])
							{
								visibility[0] = Visibility.Visible;
							}
							if ("EC" == CanUpdate[i])
							{
								visibility[1] = Visibility.Visible;
							}
							if ("DP" == CanUpdate[i])
							{
								visibility[2] = Visibility.Visible;
							}
						}
					}
				}
			});
		}

		private async System.Threading.Tasks.Task Update(string fileType)
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				IclientRequest.downFileVersionInfo gerverVersionMap = clientRequest.getGerverVersionMap(fileType);
				clientRequest.DownloadFile(fileType, gerverVersionMap.latestVersion, Pvalue);
			});
		}

		private async System.Threading.Tasks.Task CheckNowVersion()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
			});
		}

		private async System.Threading.Tasks.Task CheckBackup()
		{
			await System.Threading.Tasks.Task.Run(delegate
			{
				clientRequest.OpenLocalFile();
			});
		}

		private async System.Threading.Tasks.Task Driver(string name)
		{
			if (string.IsNullOrEmpty(name) || name.Length == 0)
			{
				return;
			}
			Item item1;
			int processID;
			try
			{
				Trace.WriteLine("zqp_item.Name:" + name);
				item1 = null;
				processID = 0;
				foreach (Item fwItem in FwItems)
				{
					if (fwItem.Id == name)
					{
						item1 = fwItem;
						break;
					}
				}
				if (item1 == null)
				{
					foreach (Item item2 in items)
					{
						if (item2.Id == name)
						{
							item1 = item2;
							break;
						}
					}
				}
				if (item1 != null)
				{
					item1.Progress = 0;
					await System.Threading.Tasks.Task.Run(delegate
					{
						Trace.WriteLine("zqp_test download driver name: " + name);
						IclientRequest.downFileVersionInfo gerverVersionMap = clientRequest.getGerverVersionMap(name);
						clientRequest.DownloadProgressBar += DownloadCompletedHandler;
						processIDValue++;
						processID = processIDValue;
						clientRequest.DownloadFile(name, gerverVersionMap.latestVersion, processIDValue);
						item1.Progress = 0;
					});
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine("未知错误：" + ex);
			}
			void DownloadCompletedHandler(int processId, int downloadedValue)
			{
				Trace.WriteLine("zqp_test:processId:" + processId + ";downloadedValue:" + downloadedValue);
				if (processId == processID)
				{
					item1.Progress = downloadedValue;
				}
			}
		}
	}
}
namespace BydCentral.Core.Services
{
	public interface IclientRequest
	{
		[Serializable]
		public struct downFileVersionInfo
		{
			public string type;

			public string latestVersion;

			public string[] allVersion;

			public string[] releaseNote;

			public string[] updateTime;
		}

		event Action<int, int> DownloadProgressBar;

		(string[], string[]) checkLoadUpdate();

		(string[], int) checkUpdateServer();

		downFileVersionInfo getGerverVersionMap(string fileType);

		string getLocalVersionMap(string fileType);

		Dictionary<string, string> getAllLoacalVersionMap();

		Dictionary<string, string> getAllLoacalNameMap();

		bool DownloadFile(string fileType, string version, int value);

		(downFileVersionInfo, int) getAppVersion();

		bool UpdateAPP();

		(int, Exception) Compare(string version1, string version2);

		void DeleteAllFile();

		void OpenLocalFile();
	}
	public interface IPopupWindowService
	{
		void ShowPopup();

		void ClosePopup();

		void HidePopup();
	}
	public interface IUSBServices
	{
		IUsbDevice InitUsb(int pid, int vid);

		bool SendData(IUsbDevice device, byte[] data, out int bytes, out Error error);

		bool ReceiveData(IUsbDevice device, byte[] indata, out byte[] data, out int bytes, out Error error);

		bool SendData2(byte[] data, out int bytes, out Error error);

		bool ReceiveData2(byte[] indata, out byte[] data, out int bytes, out Error error);
	}
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
	public class LoggerHelper
	{
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

		private static LoggerHelper _obj;

		public static LoggerHelper _
		{
			get
			{
				return _obj ?? new LoggerHelper();
			}
			set
			{
				_obj = value;
			}
		}

		public void Debug(string msg)
		{
			_logger.Debug(msg);
		}

		public void Debug(string msg, Exception err)
		{
			_logger.Debug(err, msg);
		}

		public void Info(string msg)
		{
			_logger.Info(msg);
		}

		public void Info(string msg, Exception err)
		{
			_logger.Info(err, msg);
		}

		public void Warn(string msg)
		{
			_logger.Warn(msg);
		}

		public void Warn(string msg, Exception err)
		{
			_logger.Warn(err, msg);
		}

		public void Trace(string msg)
		{
			_logger.Trace(msg);
		}

		public void Trace(string msg, Exception err)
		{
			_logger.Trace(err, msg);
		}

		public void Error(string msg)
		{
			_logger.Error(msg);
		}

		public void Error(string msg, Exception err)
		{
			_logger.Error(err, msg);
		}

		public void Fatal(string msg)
		{
			_logger.Fatal(msg);
		}

		public void Fatal(string msg, Exception err)
		{
			_logger.Fatal(err, msg);
		}
	}
	public class Usb : IUSBServices
	{
		public static UsbDeviceFinder MyUsbFinder = new UsbDeviceFinder(13326, 32770);

		public IUsbDevice CurUsb;

		public IUsbDevice InitUsb(int pid, int vid)
		{
			using UsbContext usbContext = new UsbContext();
			try
			{
				IUsbDevice? usbDevice = usbContext.List().FirstOrDefault((IUsbDevice d) => d.ProductId == pid && d.VendorId == vid);
				usbDevice.Open();
				usbDevice.ClaimInterface(usbDevice.Configs[0].Interfaces[1].Number);
				return usbDevice;
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("发生异常：" + ex.ToString());
				return null;
			}
		}

		public void InitUsb()
		{
			using UsbContext usbContext = new UsbContext();
			try
			{
				CurUsb = (UsbDevice)usbContext.Find(MyUsbFinder);
				CurUsb.ClaimInterface(1);
			}
			catch (Exception ex)
			{
				LoggerHelper._.Debug("Init error：" + ex.ToString());
			}
		}

		public bool OpenUsb()
		{
			using (new UsbContext())
			{
				try
				{
					if (CurUsb.TryOpen())
					{
						return true;
					}
					return false;
				}
				catch (Exception ex)
				{
					LoggerHelper._.Debug("Open fail" + ex.Message.ToString());
					return false;
				}
			}
		}

		public bool CloseUsb()
		{
			using (new UsbContext())
			{
				try
				{
					CurUsb.Close();
					return true;
				}
				catch (Exception ex)
				{
					LoggerHelper._.Debug("Close fail" + ex.Message.ToString());
					return false;
				}
			}
		}

		public byte[] GetData(byte[] data)
		{
			using UsbContext usbContext = new UsbContext();
			try
			{
				IUsbDevice usbDevice = (UsbDevice)usbContext.Find(MyUsbFinder);
				usbDevice.TryOpen();
				usbDevice.ClaimInterface(1);
				usbDevice.OpenEndpointWriter(WriteEndpointID.Ep02).Write(data, 3000, out var _);
				UsbEndpointReader usbEndpointReader = usbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
				byte[] array = new byte[64];
				int transferLength2;
				Error num = usbEndpointReader.Read(array, 3000, out transferLength2);
				usbDevice.Close();
				usbDevice.Dispose();
				if (num == Error.Success)
				{
					return array;
				}
				throw new Exception("read error");
			}
			catch (Exception ex)
			{
				LoggerHelper._.Debug("Get fail" + ex.Message.ToString());
				return null;
			}
		}

		public bool SendData2(byte[] data, out int bytes, out Error error)
		{
			using UsbContext usbContext = new UsbContext();
			try
			{
				UsbDevice obj = (UsbDevice)usbContext.Find(MyUsbFinder);
				((IUsbDevice)obj).TryOpen();
				((IUsbDevice)obj).ClaimInterface(1);
				int transferLength;
				Error error2 = ((IUsbDevice)obj).OpenEndpointWriter(WriteEndpointID.Ep02).Write(data, 3000, out transferLength);
				bytes = transferLength;
				((IUsbDevice)obj).Close();
				((IDisposable)obj).Dispose();
				if (error2 == Error.Success)
				{
					error = Error.Success;
					return true;
				}
				error = error2;
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine("发生异常" + ex.ToString());
				bytes = 0;
				error = Error.Success;
				return false;
			}
		}

		public bool ReceiveData2(byte[] indata, out byte[] data, out int bytes, out Error error)
		{
			using UsbContext usbContext = new UsbContext();
			try
			{
				IUsbDevice usbDevice = usbContext.List().FirstOrDefault((IUsbDevice d) => d.ProductId == 32770 && d.VendorId == 13326);
				usbDevice.Open();
				usbDevice.ClaimInterface(usbDevice.Configs[0].Interfaces[1].Number);
				SendData(usbDevice, indata, out var _, out var _);
				UsbEndpointReader usbEndpointReader = usbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
				data = new byte[64];
				int transferLength;
				Error error3 = usbEndpointReader.Read(data, 3000, out transferLength);
				bytes = transferLength;
				if (error3 == Error.Success)
				{
					error = Error.Success;
					usbDevice.Close();
					return true;
				}
				error = error3;
				usbDevice.Close();
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine("发生异常" + ex.ToString());
				bytes = 0;
				error = Error.Success;
				data = null;
				return false;
			}
		}

		public bool SendData(IUsbDevice device, byte[] data, out int bytes, out Error error)
		{
			using UsbContext usbContext = new UsbContext();
			try
			{
				IUsbDevice usbDevice = usbContext.List().FirstOrDefault((IUsbDevice d) => d.ProductId == 32770 && d.VendorId == 13326);
				usbDevice.Open();
				usbDevice.ClaimInterface(usbDevice.Configs[0].Interfaces[1].Number);
				int transferLength;
				Error error2 = usbDevice.OpenEndpointWriter(WriteEndpointID.Ep02).Write(data, 3000, out transferLength);
				bytes = transferLength;
				if (error2 == Error.Success)
				{
					error = Error.Success;
					usbDevice.Close();
					System.Windows.MessageBox.Show("end send data");
					return true;
				}
				error = error2;
				usbDevice.Close();
				System.Windows.MessageBox.Show("end send data");
				return false;
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("发生异常" + ex.ToString());
				bytes = 0;
				error = Error.Success;
				return false;
			}
		}

		public bool ReceiveData(IUsbDevice device, byte[] indata, out byte[] data, out int bytes, out Error error)
		{
			using UsbContext usbContext = new UsbContext();
			try
			{
				IUsbDevice usbDevice = usbContext.List().FirstOrDefault((IUsbDevice d) => d.ProductId == 32770 && d.VendorId == 13326);
				usbDevice.Open();
				usbDevice.ClaimInterface(usbDevice.Configs[0].Interfaces[1].Number);
				SendData(device, indata, out var _, out var _);
				UsbEndpointReader usbEndpointReader = device.OpenEndpointReader(ReadEndpointID.Ep02);
				data = new byte[64];
				int transferLength;
				Error error3 = usbEndpointReader.Read(data, 3000, out transferLength);
				bytes = transferLength;
				if (error3 == Error.Success)
				{
					error = Error.Success;
					usbDevice.Close();
					return true;
				}
				error = error3;
				usbDevice.Close();
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine("发生异常" + ex.ToString());
				bytes = 0;
				error = Error.Success;
				data = null;
				return false;
			}
		}

		public void writeUsbFan(object mode, TxBuf txBufClass, TxBuf.txBuF txBufStruct)
		{
			txBufStruct = txBufClass.on_setFanControl((TxBuf.PARAMS.Fan_Ctrl)mode);
			byte[] array = new byte[65];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(txBufStruct));
			try
			{
				Marshal.StructureToPtr(txBufStruct, intPtr, fDeleteOld: false);
				Marshal.Copy(intPtr, array, 0, Marshal.SizeOf(txBufStruct));
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (!SendData2(array, out var bytes, out var error))
			{
				Console.WriteLine("writeBytes" + bytes);
				Console.WriteLine(error.ToString());
			}
		}
	}
	public class WinRing0_IO
	{
		private static Ols MyOls;

		public ushort BRAM_Port_0x300 = 768;

		public ushort BRAM_Port_0x302 = 770;

		public WinRing0_IO()
		{
			Initialize();
		}

		public bool Initialize()
		{
			MyOls = new Ols();
			return MyOls.GetStatus() == 0;
		}

		public byte IndexIO_GetRegValue(ushort address, byte offset)
		{
			MyOls.WriteIoPortByte(address, offset);
			return MyOls.ReadIoPortByte((ushort)(address + 1));
		}

		public void IndexIO_SetRegValue(ushort address, byte offset, byte value)
		{
			MyOls.WriteIoPortByte(address, offset);
			MyOls.WriteIoPortByte((ushort)(address + 1), value);
		}

		public void Write_EC(byte command, byte value)
		{
			IndexIO_SetRegValue(BRAM_Port_0x300, 148, 0);
			IndexIO_SetRegValue(BRAM_Port_0x300, 145, 0);
			IndexIO_SetRegValue(BRAM_Port_0x300, 146, 0);
			IndexIO_SetRegValue(BRAM_Port_0x300, 146, 1);
			IndexIO_SetRegValue(BRAM_Port_0x300, 144, 0);
			IndexIO_SetRegValue(BRAM_Port_0x300, 145, command);
			IndexIO_SetRegValue(BRAM_Port_0x300, 160, value);
			IndexIO_SetRegValue(BRAM_Port_0x300, 147, 161);
		}

		public uint Read_EC(byte command)
		{
			uint result = 0u;
			IndexIO_SetRegValue(BRAM_Port_0x300, 148, 0);
			IndexIO_SetRegValue(BRAM_Port_0x300, 146, 1);
			IndexIO_SetRegValue(BRAM_Port_0x300, 144, 0);
			IndexIO_SetRegValue(BRAM_Port_0x300, 145, command);
			IndexIO_SetRegValue(BRAM_Port_0x300, 147, 160);
			int num = 0;
			while (num < 100 && IndexIO_GetRegValue(768, 147) != 0)
			{
				num++;
				Thread.Sleep(2);
			}
			uint num2 = IndexIO_GetRegValue(768, 160);
			if (num2 != 2147483649u)
			{
				result = num2;
			}
			return result;
		}
	}
}
namespace BydCentral.Core.Models
{
	public class Back_light_buf
	{
		public enum COMMAND : byte
		{
			Light_Close = 0,
			Light_AlwaysOn = 1,
			Light_Breath = 2,
			Light_Rythm = 3,
			Light_Jump = 4,
			Light_Round = 5,
			Light_Cover = 6,
			SliceMode = 7,
			BalanceMode = 8,
			GameMode = 9,
			Save = 124,
			Back_Light = 15,
			ReadBuf = 144
		}

		[StructLayout(LayoutKind.Explicit, Size = 17)]
		public struct PARAMS
		{
			[FieldOffset(0)]
			public byte h1;

			[FieldOffset(1)]
			public byte h2;

			[FieldOffset(2)]
			public COMMAND EffectCMD;

			[FieldOffset(3)]
			public byte r1;

			[FieldOffset(4)]
			public byte r2;

			[FieldOffset(5)]
			public byte g1;

			[FieldOffset(6)]
			public byte g2;

			[FieldOffset(7)]
			public byte b1;

			[FieldOffset(8)]
			public byte b2;

			[FieldOffset(9)]
			public byte Spd;

			[FieldOffset(10)]
			public byte l1;

			[FieldOffset(11)]
			public byte l2;

			[FieldOffset(12)]
			public byte fd1;

			[FieldOffset(13)]
			public byte fd2;

			[FieldOffset(14)]
			public byte fd3;

			[FieldOffset(15)]
			public byte fd4;

			[FieldOffset(16)]
			public byte checkSum;
		}

		public PARAMS setBackLight(COMMAND mode, byte r, byte g, byte b, byte speed, byte l, byte[] AudioData)
		{
			PARAMS pARAMS = new PARAMS
			{
				h1 = 52,
				h2 = 14,
				EffectCMD = mode,
				Spd = speed
			};
			switch (mode)
			{
			case COMMAND.Light_AlwaysOn:
			case COMMAND.Light_Breath:
			case COMMAND.Back_Light:
				return setRGBL(pARAMS, r, g, b, l);
			case COMMAND.Light_Rythm:
			case COMMAND.Light_Jump:
			case COMMAND.Light_Round:
			case COMMAND.Light_Cover:
				return setBackLightWithAudio(pARAMS, r, g, b, l, AudioData);
			default:
				return pARAMS;
			}
		}

		public PARAMS setRGBL(PARAMS p, byte r, byte g, byte b, byte l)
		{
			p.r1 = r;
			p.g1 = g;
			p.b1 = b;
			p.l1 = (byte)((double)(int)l * 0.44);
			return p;
		}

		public PARAMS setBackLightWithAudio(PARAMS p, byte r, byte g, byte b, byte l, byte[] AudioData)
		{
			p.r1 = r;
			p.g1 = g;
			p.b1 = b;
			p.l1 = (byte)((double)(int)l * 0.44);
			p.fd1 = AudioData[0];
			p.fd2 = AudioData[1];
			p.fd3 = AudioData[2];
			p.fd4 = AudioData[3];
			return p;
		}
	}
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
	public class Item : INotifyPropertyChanged
	{
		private string _NowVersion;

		private string _NewVersion;

		private int _Progress;

		private string _btntext;

		private string _tips = "用于显示更新提示的内容...";

		public string? Id { get; set; }

		public string? Name { get; set; }

		public string? Description { get; set; }

		public string NowVersion
		{
			get
			{
				return _NowVersion;
			}
			set
			{
				if (_NowVersion != value)
				{
					_NowVersion = value;
					OnPropertyChanged("NowVersion");
				}
			}
		}

		public string NewVersion
		{
			get
			{
				return _NewVersion;
			}
			set
			{
				if (_NewVersion != value)
				{
					_NewVersion = value;
					OnPropertyChanged("NewVersion");
				}
			}
		}

		public int Progress
		{
			get
			{
				return _Progress;
			}
			set
			{
				if (_Progress != value)
				{
					_Progress = value;
					OnPropertyChanged("Progress");
				}
			}
		}

		public string btntext
		{
			get
			{
				return _btntext;
			}
			set
			{
				if (_btntext != value)
				{
					_btntext = value;
					OnPropertyChanged("btntext");
				}
			}
		}

		public string Tips
		{
			get
			{
				return _tips;
			}
			set
			{
				if (_tips != value)
				{
					_tips = value;
					OnPropertyChanged("Tips");
				}
			}
		}

		public ICommand? Command { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
	[ProtoContract]
	[ProtoInclude(8, typeof(Light_Infinix))]
	public class Light : INotifyPropertyChanged
	{
		private byte _class;

		private byte _switch;

		private byte _mode;

		private byte _r;

		private byte _g;

		private byte _b;

		private byte _l;

		[ProtoMember(1)]
		public byte Class
		{
			get
			{
				return _class;
			}
			set
			{
				if (_class != value)
				{
					_class = value;
					OnPropertyChanged("Class");
				}
			}
		}

		[ProtoMember(2)]
		public byte Switch
		{
			get
			{
				return _switch;
			}
			set
			{
				if (_switch != value)
				{
					_switch = value;
					OnPropertyChanged("Switch");
				}
			}
		}

		[ProtoMember(3)]
		public byte Mode
		{
			get
			{
				return _mode;
			}
			set
			{
				if (_mode != value)
				{
					_mode = value;
					OnPropertyChanged("Mode");
				}
			}
		}

		[ProtoMember(4)]
		public byte R
		{
			get
			{
				return _r;
			}
			set
			{
				if (_r != value)
				{
					_r = value;
					OnPropertyChanged("R");
				}
			}
		}

		[ProtoMember(5)]
		public byte G
		{
			get
			{
				return _g;
			}
			set
			{
				if (_g != value)
				{
					_g = value;
					OnPropertyChanged("G");
				}
			}
		}

		[ProtoMember(6)]
		public byte B
		{
			get
			{
				return _b;
			}
			set
			{
				if (_b != value)
				{
					_b = value;
					OnPropertyChanged("B");
				}
			}
		}

		[ProtoMember(7)]
		public byte L
		{
			get
			{
				return _l;
			}
			set
			{
				if (_l != value)
				{
					_l = value;
					OnPropertyChanged("L");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
	[ProtoContract]
	public class Light_Infinix : Light, INotifyPropertyChanged
	{
		private byte _backr;

		private byte _backg;

		private byte _backb;

		private byte _backl;

		[ProtoMember(1)]
		public byte backR
		{
			get
			{
				return _backr;
			}
			set
			{
				if (_backr != value)
				{
					_backr = value;
					OnPropertyChanged("backR");
				}
			}
		}

		[ProtoMember(2)]
		public byte backG
		{
			get
			{
				return _backg;
			}
			set
			{
				if (_backg != value)
				{
					_backg = value;
					OnPropertyChanged("backG");
				}
			}
		}

		[ProtoMember(3)]
		public byte backB
		{
			get
			{
				return _backb;
			}
			set
			{
				if (_backb != value)
				{
					_backb = value;
					OnPropertyChanged("backB");
				}
			}
		}

		[ProtoMember(4)]
		public byte backL
		{
			get
			{
				return _backl;
			}
			set
			{
				if (_backl != value)
				{
					_backl = value;
					OnPropertyChanged("backL");
				}
			}
		}
	}
	public class TxBuf
	{
		public struct outerstruct
		{
			public int outervar;

			public int othervar;

			public union union;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct union
		{
			[FieldOffset(0)]
			public byte Address;

			[FieldOffset(0)]
			public Address1 Address1;

			[FieldOffset(0)]
			public Address2 Address2;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct Address1
		{
			[FieldOffset(0)]
			public byte Address;

			[FieldOffset(0)]
			public byte Byte1;

			[FieldOffset(1)]
			public byte Byte2;

			[FieldOffset(2)]
			public byte Byte3;

			[FieldOffset(3)]
			public byte Byte4;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct Address2
		{
			[FieldOffset(0)]
			public byte Address;

			[FieldOffset(0)]
			public byte Byte1;

			[FieldOffset(1)]
			public byte Byte2;

			[FieldOffset(2)]
			public byte Byte3;

			[FieldOffset(3)]
			public byte Byte4;
		}

		public enum COMMAND : byte
		{
			Device_Info,
			KeyBoard_Light,
			Side_Light,
			Logo_Light,
			Fan_Ctrl,
			Periph_Ctrl,
			KeyBoard_Light12,
			KeyBoard_Light34
		}

		public enum ACTION
		{
			Set,
			Get
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct PARAMS
		{
			public enum Device_Info
			{
				Reset,
				SaveKeyboard,
				Version,
				Battery,
				Update
			}

			public enum Keyboard_Light
			{
				LightOFF,
				Always,
				Breath,
				GradualChange,
				RainBow,
				Flow,
				Wave,
				Light_Auto_Close,
				Light_Open
			}

			public enum Side_Light
			{
				LightOFF,
				Always,
				Breath,
				GradualChange,
				RainBow,
				TemperatureControl
			}

			public enum Logo_Light
			{
				LightOFF,
				Always,
				Breath,
				GradualChange,
				RainBow,
				PowerLight
			}

			public enum Fan_Ctrl
			{
				OfficeMode,
				PerformanceMode,
				GamingMode,
				FullSpeed,
				FullSpeedOff
			}

			public enum Periph_Ctrl
			{
				AGPU,
				GPUOnly,
				DDS,
				APUOnly,
				CharingON,
				CharingOFF,
				WinKeyOFF,
				WinKeyON
			}

			public enum Keyboard_Light12
			{
				Always1,
				Breath1,
				GradualChange1,
				RainBow1,
				Always2,
				Breath2,
				GradualChange2,
				RainBow2
			}

			public enum Keyboard_Light34
			{
				Always1,
				Breath1,
				GradualChange1,
				RainBow1,
				Always2,
				Breath2,
				GradualChange2,
				RainBow2
			}
		}

		public enum RXField : byte
		{
			CPU_FAN1_Speed = 9,
			DGPU_FAN2_Speed = 10,
			CPU_Temperature = 11,
			DGPU_Temperature = 12,
			System_Power_State = 13,
			Adapter_Current = 41,
			Adapter_Voltage = 43,
			Thermal_Flag1_State = 23,
			Thermal_Control_Mode = 24,
			Color_R = 51,
			Color_G = 52,
			Color_B = 53,
			Fan_Mode = 50
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct unionCMD
		{
			[FieldOffset(0)]
			public PositionCMD PositionCMD;

			[FieldOffset(0)]
			public byte Value;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct unionSize
		{
			[FieldOffset(0)]
			public PositionSize PositionSize;

			[FieldOffset(0)]
			public byte Value;
		}

		[StructLayout(LayoutKind.Explicit, Size = 4)]
		public struct unionReserved
		{
			[FieldOffset(0)]
			public byte C1;

			[FieldOffset(1)]
			public byte C2;

			[FieldOffset(2)]
			public byte C3;

			[FieldOffset(3)]
			public byte C4;
		}

		[StructLayout(LayoutKind.Explicit, Size = 56)]
		public struct unionData
		{
			[FieldOffset(0)]
			public Color Color;

			[FieldOffset(0)]
			public Other Other;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct CheckSum
		{
			[FieldOffset(0)]
			public byte CheckSum_num1;

			[FieldOffset(0)]
			public byte CheckSum_num2;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct PositionCMD
		{
			[FieldOffset(0)]
			public byte PARAMS;

			[FieldOffset(0)]
			public byte COMMAND;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct PositionSize
		{
			[FieldOffset(0)]
			public byte size;

			[FieldOffset(0)]
			public byte isEmpty;
		}

		[StructLayout(LayoutKind.Explicit, Size = 56)]
		public struct Color
		{
			[FieldOffset(0)]
			public byte R;

			[FieldOffset(1)]
			public byte G;

			[FieldOffset(2)]
			public byte B;

			[FieldOffset(3)]
			public byte L;

			[FieldOffset(4)]
			public unsafe fixed byte Name[52];
		}

		[StructLayout(LayoutKind.Explicit, Size = 56)]
		public struct Other
		{
			[FieldOffset(0)]
			public byte index;

			[FieldOffset(1)]
			public unsafe fixed byte Name[55];
		}

		public struct txBuF
		{
			public byte ID;

			public unionCMD unionCMD;

			public unionSize unionSize;

			public unionReserved unionReserved;

			public unionData unionData;

			public CheckSum CheckSum;
		}

		public byte[] rxBuf = new byte[64];

		public int tx_actual_length;

		public int rx_actual_length;

		public int hid_timeout = 50;

		public static byte SetbitValue(byte data, int index, bool flag)
		{
			int num;
			switch (index)
			{
			default:
				throw new ArgumentOutOfRangeException();
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				num = 2 << index - 2;
				break;
			case 1:
				num = index;
				break;
			}
			int num2 = num;
			if (!flag)
			{
				return (byte)(data & ~num2);
			}
			return (byte)(data | num2);
		}

		public byte GetByte(byte data, int index, byte count)
		{
			return (byte)((data >> 8 - index - count) % (1 << (int)count));
		}

		public byte SetByte(int value, byte data, byte index, byte count)
		{
			int num = data % (1 << 8 - index - count);
			return (byte)(((data >> 8 - index << (int)count) + value << (int)index) + num);
		}

		public bool TestBit(byte data, int index)
		{
			return GetByte(data, index, 1) == 1;
		}

		public byte SetBit(bool value, byte data, byte index, byte count)
		{
			return SetByte(value ? 1 : 0, data, index, count);
		}

		public bool TypeFlagsAudio(byte flags, int index)
		{
			return TestBit(flags, index);
		}

		public void TenToTwo(int value)
		{
			int num = 0;
			int[] array = new int[20];
			bool[] array2 = new bool[4];
			while (value / 2 == 1)
			{
				array[num++] = value % 2;
				array2[num++] = true;
				value /= 2;
			}
			array[num++] = value % 2;
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
			}
		}

		public txBuF setID(txBuF setID)
		{
			byte iD = 6;
			setID.ID = iD;
			return setID;
		}

		public txBuF setCommand(COMMAND arg, txBuF setCMD)
		{
			Convert.ToString((int)arg, 2);
			bool[] array = new bool[4];
			byte[] array2 = new byte[4];
			string text = Convert.ToString((byte)arg, 2);
			for (int i = 0; i < text.Length; i++)
			{
				array2[i] = Convert.ToByte(text[i].ToString());
				array[i] = Convert.ToBoolean(array2[i]);
				setCMD.unionCMD.Value = SetbitValue(setCMD.unionCMD.Value, 4 + text.Length - i, array[i]);
				TypeFlagsAudio(setCMD.unionCMD.Value, 3 + text.Length - i);
			}
			return setCMD;
		}

		public txBuF setAction(ACTION arg, txBuF setAction)
		{
			Convert.ToString((int)arg, 2);
			bool[] array = new bool[1];
			byte[] array2 = new byte[1];
			string text = Convert.ToString((byte)arg, 2);
			for (int i = 0; i < text.Length; i++)
			{
				array2[i] = Convert.ToByte(text[i].ToString());
				array[i] = Convert.ToBoolean(array2[i]);
				setAction.unionCMD.Value = SetbitValue(setAction.unionCMD.Value, 3 + text.Length - i, array[i]);
				TypeFlagsAudio(setAction.unionCMD.Value, 3 + text.Length - 1 - i);
			}
			return setAction;
		}

		public txBuF setParams(int arg, txBuF setParams)
		{
			Convert.ToString(arg, 2);
			bool[] array = new bool[4];
			byte[] array2 = new byte[4];
			string text = Convert.ToString((byte)arg, 2);
			for (int i = 0; i < text.Length; i++)
			{
				array2[i] = Convert.ToByte(text[i].ToString());
				array[i] = Convert.ToBoolean(array2[i]);
				setParams.unionCMD.Value = SetbitValue(setParams.unionCMD.Value, text.Length - i, array[i]);
				TypeFlagsAudio(setParams.unionCMD.Value, text.Length - 1 - i);
			}
			return setParams;
		}

		public txBuF setDataRGBL(byte R, byte G, byte B, byte L, txBuF setRGBL)
		{
			setRGBL.unionData.Color.R = R;
			setRGBL.unionData.Color.G = G;
			setRGBL.unionData.Color.B = B;
			setRGBL.unionData.Color.L = L;
			return setRGBL;
		}

		public txBuF setDataSize(byte size, txBuF setDataSize)
		{
			if (size == 0)
			{
				return setDataSize;
			}
			if (size > 56)
			{
				size = 56;
			}
			setDataSize.unionSize.PositionSize.isEmpty = 0;
			setDataSize.unionSize.PositionSize.size = size;
			return setDataSize;
		}

		public txBuF clearData(txBuF setDataSize)
		{
			setDataSize.unionData.Other.index = 0;
			return setDataSize;
		}

		public txBuF setCheckSum(txBuF setCheckSum)
		{
			byte[] array = new byte[64];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(setCheckSum));
			try
			{
				Marshal.StructureToPtr(setCheckSum, intPtr, fDeleteOld: false);
				Marshal.Copy(intPtr, array, 0, Marshal.SizeOf(setCheckSum));
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			uint num = 0u;
			for (int i = 1; i < 63; i++)
			{
				num += array[i];
			}
			setCheckSum.CheckSum.CheckSum_num1 = (byte)num;
			return setCheckSum;
		}

		public byte getUSBFiled(RXField field)
		{
			return rxBuf[(uint)field];
		}

		private void send()
		{
		}

		public txBuF on_setKeyboardSave()
		{
			txBuF setCMD = setID(default(txBuF));
			setCMD = setCommand(COMMAND.Device_Info, setCMD);
			setCMD = setParams(1, setCMD);
			return setCheckSum(setCMD);
		}

		public txBuF on_getVersion()
		{
			txBuF setCMD = setID(default(txBuF));
			setCMD = setCommand(COMMAND.Device_Info, setCMD);
			setCMD = setParams(2, setCMD);
			return setCheckSum(setCMD);
		}

		public txBuF On_ChatEC_setKeyboardLED(txBuF setKeyboardLED, byte R, byte G, byte B, byte L)
		{
			setKeyboardLED = setID(setKeyboardLED);
			setKeyboardLED = setCommand(COMMAND.KeyBoard_Light, setKeyboardLED);
			setKeyboardLED = setParams(1, setKeyboardLED);
			setKeyboardLED = setDataSize(4, setKeyboardLED);
			setKeyboardLED = clearData(setKeyboardLED);
			setKeyboardLED = setDataRGBL(R, G, B, L, setKeyboardLED);
			setKeyboardLED = setCheckSum(setKeyboardLED);
			return setKeyboardLED;
		}

		public txBuF on_setKeyboardLED(PARAMS.Keyboard_Light state, byte R, byte G, byte B, byte L)
		{
			txBuF setCMD = setID(default(txBuF));
			setCMD = setCommand(COMMAND.KeyBoard_Light, setCMD);
			setCMD = setParams((int)state, setCMD);
			setCMD = setDataSize(4, setCMD);
			setCMD = clearData(setCMD);
			setCMD = setDataRGBL(R, G, B, L, setCMD);
			return setCheckSum(setCMD);
		}

		public txBuF on_setKeyboard12LED(PARAMS.Keyboard_Light12 state, byte R, byte G, byte B, byte L)
		{
			txBuF setCMD = setID(default(txBuF));
			setCMD = setCommand(COMMAND.KeyBoard_Light12, setCMD);
			setCMD = setParams((int)state, setCMD);
			setCMD = setDataSize(4, setCMD);
			setCMD = clearData(setCMD);
			setCMD = setDataRGBL(R, G, B, L, setCMD);
			return setCheckSum(setCMD);
		}

		public txBuF on_setKeyboard34LED(PARAMS.Keyboard_Light34 state, byte R, byte G, byte B, byte L)
		{
			txBuF setCMD = setID(default(txBuF));
			setCMD = setCommand(COMMAND.KeyBoard_Light34, setCMD);
			setCMD = setParams((int)state, setCMD);
			setCMD = setDataSize(4, setCMD);
			setCMD = clearData(setCMD);
			setCMD = setDataRGBL(R, G, B, L, setCMD);
			return setCheckSum(setCMD);
		}

		public txBuF on_setLogoLED(PARAMS.Logo_Light state, byte R, byte G, byte B, byte L)
		{
			txBuF setCMD = setID(default(txBuF));
			setCMD = setCommand(COMMAND.Logo_Light, setCMD);
			setCMD = setParams((int)state, setCMD);
			setCMD = setDataSize(4, setCMD);
			setCMD = clearData(setCMD);
			setCMD = setDataRGBL(R, G, B, L, setCMD);
			return setCheckSum(setCMD);
		}

		public txBuF on_setCircleLED(PARAMS.Side_Light state, byte R, byte G, byte B, byte L)
		{
			txBuF setCMD = setID(default(txBuF));
			setCMD = setCommand(COMMAND.Side_Light, setCMD);
			setCMD = setParams((int)state, setCMD);
			setCMD = setDataSize(4, setCMD);
			setCMD = clearData(setCMD);
			setCMD = setDataRGBL(R, G, B, L, setCMD);
			return setCheckSum(setCMD);
		}

		public txBuF on_setFanControl(PARAMS.Fan_Ctrl state)
		{
			txBuF setCMD = setID(default(txBuF));
			setCMD = setCommand(COMMAND.Fan_Ctrl, setCMD);
			setCMD = setParams((int)state, setCMD);
			setCMD = setDataSize(0, setCMD);
			setCMD = clearData(setCMD);
			return setCheckSum(setCMD);
		}

		public txBuF on_setPraphicsControl(PARAMS.Periph_Ctrl state, byte R, byte G, byte B, byte L)
		{
			txBuF setCMD = setID(default(txBuF));
			setCMD = setCommand(COMMAND.Periph_Ctrl, setCMD);
			setCMD = setParams((int)state, setCMD);
			setCMD = setDataSize(0, setCMD);
			setCMD = clearData(setCMD);
			return setCheckSum(setCMD);
		}

		public txBuF on_setDeviceInfo(PARAMS.Device_Info state, byte R, byte G, byte B, byte L)
		{
			txBuF setCMD = setID(default(txBuF));
			setCMD = setCommand(COMMAND.Device_Info, setCMD);
			setCMD = setParams((int)state, setCMD);
			setCMD = setDataSize(0, setCMD);
			setCMD = clearData(setCMD);
			return setCheckSum(setCMD);
		}

		public txBuF on_setKeyboardLightState(PARAMS.Keyboard_Light state)
		{
			txBuF setCMD = setID(default(txBuF));
			setCMD = setCommand(COMMAND.KeyBoard_Light, setCMD);
			setCMD = setParams((int)state, setCMD);
			setCMD = setDataSize(4, setCMD);
			setCMD = clearData(setCMD);
			setCMD = setDataRGBL(0, 0, 0, 0, setCMD);
			return setCheckSum(setCMD);
		}
	}
}
namespace IWshRuntimeLibrary
{
	[ComImport]
	[CompilerGenerated]
	[Guid("F935DC21-1CF0-11D0-ADB9-00C04FD58A0B")]
	[TypeIdentifier]
	public interface IWshShell
	{
	}
	[ComImport]
	[CompilerGenerated]
	[Guid("24BE5A30-EDFE-11D2-B933-00104B365C9F")]
	[TypeIdentifier]
	public interface IWshShell2 : IWshShell
	{
	}
	[ComImport]
	[CompilerGenerated]
	[Guid("41904400-BE18-11D3-A28B-00104BD35090")]
	[TypeIdentifier]
	public interface IWshShell3 : IWshShell2
	{
		void _VtblGap1_4();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1002)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		object CreateShortcut([In][MarshalAs(UnmanagedType.BStr)] string PathLink);
	}
	[ComImport]
	[CompilerGenerated]
	[DefaultMember("FullName")]
	[Guid("F935DC23-1CF0-11D0-ADB9-00C04FD58A0B")]
	[TypeIdentifier]
	public interface IWshShortcut
	{
		[DispId(0)]
		string FullName
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(0)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
		}

		void _VtblGap1_2();

		[DispId(1001)]
		string Description
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(1001)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(1001)]
			[param: In]
			[param: MarshalAs(UnmanagedType.BStr)]
			set;
		}

		void _VtblGap2_2();

		[DispId(1003)]
		string IconLocation
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(1003)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(1003)]
			[param: In]
			[param: MarshalAs(UnmanagedType.BStr)]
			set;
		}

		void _VtblGap3_1();

		[DispId(1005)]
		string TargetPath
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(1005)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(1005)]
			[param: In]
			[param: MarshalAs(UnmanagedType.BStr)]
			set;
		}

		[DispId(1006)]
		int WindowStyle
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(1006)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(1006)]
			[param: In]
			set;
		}

		[DispId(1007)]
		string WorkingDirectory
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(1007)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(1007)]
			[param: In]
			[param: MarshalAs(UnmanagedType.BStr)]
			set;
		}

		void _VtblGap4_1();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2001)]
		void Save();
	}
	[ComImport]
	[CompilerGenerated]
	[Guid("41904400-BE18-11D3-A28B-00104BD35090")]
	[CoClass(typeof(object))]
	[TypeIdentifier]
	public interface WshShell : IWshShell3
	{
	}
}
