using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using BydCentral.Core.Models;
using BydCentral.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BydCentral.Core.ViewModels;

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

	private async Task GetStatus()
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

	private async Task GetCOM()
	{
		GlobalVars.LightCOM = GlobalVars.Win32.getCom();
	}
}
