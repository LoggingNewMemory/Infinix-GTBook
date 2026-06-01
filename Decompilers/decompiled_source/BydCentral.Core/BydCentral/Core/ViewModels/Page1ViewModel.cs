using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BydCentral.Core.Models;
using BydCentral.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BydCentral.Core.ViewModels;

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

	private async Task SetInitInfo()
	{
		Task.Run(delegate
		{
			CpuFanMax = 100 * GlobalVars.Wmi.GetCPUFanMax();
		});
		Task.Run(delegate
		{
			GpuFanMax = 100 * GlobalVars.Wmi.GetGPUFanMax();
		});
		Task.Run(delegate
		{
			CpuName = GlobalVars.Win32.GetCPUName();
		});
		Task.Run(delegate
		{
			GpuName = GlobalVars.Win32.GetGPUName();
		});
		Task.Run(delegate
		{
			SSDName = GlobalVars.Win32.GetDiskName();
		});
		Task.Run(delegate
		{
			MemName = GlobalVars.Win32.GetMemoryName();
		});
	}

	private async Task GetFanMax()
	{
		Task.Run(delegate
		{
			CpuFanMax = 100 * GlobalVars.Wmi.GetCPUFanMax();
			GpuFanMax = 100 * GlobalVars.Wmi.GetGPUFanMax();
		});
	}

	private async Task SetCpuMaxFrequency()
	{
		await Task.Run(delegate
		{
			CpuMaxFrequency = 2.2 * Convert.ToDouble(GlobalVars.Win32.GetCpuMaxFrequency());
		});
	}

	private async Task SetGpuMaxFrequency()
	{
		await Task.Run(delegate
		{
			GpuMaxFrequency = Convert.ToDouble(GlobalVars.Win32.GetNVGpuMaxFrequency());
		});
	}

	private async Task SetQMode()
	{
		await Task.Run(delegate
		{
			GlobalVars.Wmi.SetPerformanceMode(0);
			GlobalVars.usb.writeUsbFan(TxBuf.PARAMS.Fan_Ctrl.OfficeMode, GlobalVars.txBufClass, GlobalVars.txBufStruct);
		});
	}

	private async Task SetBMode()
	{
		await Task.Run(delegate
		{
			GlobalVars.Wmi.SetPerformanceMode(1);
			GlobalVars.usb.writeUsbFan(TxBuf.PARAMS.Fan_Ctrl.PerformanceMode, GlobalVars.txBufClass, GlobalVars.txBufStruct);
		});
	}

	private async Task SetGMode()
	{
		await Task.Run(delegate
		{
			GlobalVars.Wmi.SetPerformanceMode(2);
			GlobalVars.usb.writeUsbFan(TxBuf.PARAMS.Fan_Ctrl.GamingMode, GlobalVars.txBufClass, GlobalVars.txBufStruct);
		});
	}

	private async Task GetCpuUsageAsync()
	{
		await Task.Run(delegate
		{
			CpuOP = (int)GlobalVars.Win32.GetCPUOP();
		});
	}

	private async Task GetSystemMode(CancellationToken cancellationToken)
	{
		_ = 1;
		try
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				await Task.Run(delegate
				{
					string systemMode = resData[27].ToString();
					Application.Current.Dispatcher.Invoke(delegate
					{
						GlobalVars.SystemMode = systemMode;
					});
				});
				await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
			}
		}
		catch (Exception ex)
		{
			LoggerHelper._.Debug(ex.Message.ToString());
		}
	}

	private async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		_ = 1;
		try
		{
			while (!cancellationToken.IsCancellationRequested && GlobalVars.GetInfoServiceRun)
			{
				resData = GetInfoUsb.GetData(data);
				await Task.WhenAll(Task.Run(delegate
				{
					SSDOPL = GlobalVars.Win32.GetDiskUsed() + "/" + GlobalVars.Win32.GetDiskTotalSize();
				}), Task.Run(delegate
				{
					SSDOP = Convert.ToInt32(GlobalVars.Win32.GetDiskOP());
				}), Task.Run(delegate
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
				}), Task.Run(delegate
				{
					CpuOP = (int)GlobalVars.Win32.GetCPUOP();
				}), Task.Run(delegate
				{
					GpuOP = (int)GlobalVars.Win32.GetGPUOP();
				}), Task.Run(delegate
				{
					MemOP = 100 - (int)Math.Round(100.0 * Convert.ToDouble(GlobalVars.Win32.GetMemoryAvailable()) / Convert.ToDouble(GlobalVars.Win32.GetMemoryTotalSize()));
				}), Task.Run(delegate
				{
					MemOPL = (GlobalVars.Win32.GetMemoryTotalSize() - GlobalVars.Win32.GetMemoryAvailable()).ToString("0.0") + "GB/" + GlobalVars.Win32.GetMemoryTotalSize().ToString("0.0") + "GB";
				}));
				await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
			}
		}
		catch (Exception ex)
		{
			LoggerHelper._.Debug(ex.Message.ToString());
		}
	}
}
