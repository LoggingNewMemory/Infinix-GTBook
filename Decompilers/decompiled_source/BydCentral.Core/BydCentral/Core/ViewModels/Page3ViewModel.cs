using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using BydCentral.Core.Models;
using BydCentral.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BydCentral.Core.ViewModels;

public class Page3ViewModel : ObservableObject
{
	private IList<string> _dataList;

	private int _selectedDisplayOptionIndex = GlobalVars.DisplayMode - 1;

	private int _GPUMode;

	private Version _version;

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

	public Version version
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

	private async Task setBattery(CancellationToken cancellationToken)
	{
		GlobalVars.Service3TaskStop = false;
		try
		{
			while (!cancellationToken.IsCancellationRequested && GlobalVars.GetBatteryServiceRun)
			{
				await Task.WhenAll(Task.Run(delegate
				{
					BatteryStatus = Convert.ToInt32(GlobalVars.Win32.GetBatteryChargingStatus());
				}), Task.Run(delegate
				{
					Battery = Convert.ToInt32(GlobalVars.Win32.GetBatteryRemaining());
				}));
				await Task.Delay(TimeSpan.FromSeconds(1.0), cancellationToken);
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

	private async Task setVersion()
	{
		await Task.Run(delegate
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			version = executingAssembly.GetName().Version;
		});
	}

	private async Task GetGpuMode()
	{
		await Task.Run(delegate
		{
			GlobalVars.DisplayMode = (int)GlobalVars.Wmi.GetGPUMode();
		});
	}

	private async Task GetInfo()
	{
		await Task.Run(delegate
		{
			GlobalVars.UsbChargeStatus = GlobalVars.Wmi.SMI(3u, 0, 0, 0);
		});
		await Task.Run(delegate
		{
			GlobalVars.FanMaxStatus = GlobalVars.Wmi.GetFanFullMode();
		});
	}
}
