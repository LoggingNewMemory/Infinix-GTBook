#define TRACE
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BydCentral.Core.Models;
using BydCentral.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BydCentral.Core.ViewModels;

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

	private Version _version;

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

	private async Task setVersion()
	{
		await Task.Run(delegate
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			version = executingAssembly.GetName().Version;
		});
	}

	private async Task CheckUpdate()
	{
		await Task.Run(delegate
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

	private async Task Update(string fileType)
	{
		await Task.Run(delegate
		{
			IclientRequest.downFileVersionInfo gerverVersionMap = clientRequest.getGerverVersionMap(fileType);
			clientRequest.DownloadFile(fileType, gerverVersionMap.latestVersion, Pvalue);
		});
	}

	private async Task CheckNowVersion()
	{
		await Task.Run(delegate
		{
		});
	}

	private async Task CheckBackup()
	{
		await Task.Run(delegate
		{
			clientRequest.OpenLocalFile();
		});
	}

	private async Task Driver(string name)
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
				await Task.Run(delegate
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
