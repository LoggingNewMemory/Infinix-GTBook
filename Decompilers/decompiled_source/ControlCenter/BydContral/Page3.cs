using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using BydCentral;
using BydCentral.Core.Models;
using BydCentral.Core.ViewModels;
using BydCentral.CustomControl.CustomUserControl;
using BydCentral.Services;
using CustomControlLibrary.Controls;
using HandyControl.Tools.Extension;

namespace BydContral;

public class Page3 : Page, IComponentConnector
{
	public int origin;

	private CancellationTokenSource cancellationTokenSource;

	private AppControl control = new AppControl();

	private Settings_Dark settings;

	public bool flag = true;

	internal Grid grid;

	internal CustomControlLibrary.Controls.Switch Switch1;

	internal CustomControlLibrary.Controls.Switch Switch2;

	internal Label Label3;

	internal CustomControlLibrary.Controls.Switch Switch3;

	private bool _contentLoaded;

	public Page3()
	{
		InitializeComponent();
		base.DataContext = App.Current.Services.GetService(typeof(Page3ViewModel));
		base.Loaded += page3Loaded;
		base.Unloaded += page3UnLoaded;
	}

	private void initSettings()
	{
		if (control.INIRead("HotKey", "Windows") == "Y")
		{
			Switch1.IsChecked = true;
		}
		else
		{
			Switch1.IsChecked = false;
		}
	}

	public void CancelToken()
	{
		cancellationTokenSource.Cancel();
	}

	public async void ExecLoad()
	{
		if (settings == null)
		{
			((Page3ViewModel)base.DataContext).SelectedDisplayOptionIndex = GlobalVars.DisplayMode - 1;
			origin = GlobalVars.DisplayMode - 1;
			Page3ViewModel source = base.DataContext as Page3ViewModel;
			settings = new Settings_Dark();
			Binding binding = new Binding("BatteryStatus");
			binding.Source = source;
			settings.SetBinding(Settings_Dark.ChargeModePorpetry, binding);
			Binding binding2 = new Binding("Battery");
			binding2.Source = source;
			settings.SetBinding(Settings_Dark.BatModePorpetry, binding2);
			settings.OutputMode = GlobalVars.DisplayMode;
			Grid.SetRow(settings, 0);
			grid.Children.Add(settings);
		}
		GlobalVars.Wmi.inMS(intoMS: false);
		initSettings();
		GlobalVars.GetBatteryServiceRun = true;
		cancellationTokenSource = new CancellationTokenSource();
		if (Page3ViewModel.BatteryCommand.CanExecute(cancellationTokenSource.Token))
		{
			Page3ViewModel.BatteryCommand.Execute(cancellationTokenSource.Token);
		}
		if (flag)
		{
			switch (GlobalVars.FanMaxStatus)
			{
			case 0u:
				Switch2.IsChecked = false;
				break;
			case 1u:
				Switch2.IsChecked = true;
				break;
			}
			switch (GlobalVars.UsbChargeStatus)
			{
			case 0u:
				Switch3.IsChecked = false;
				break;
			case 1u:
				Switch3.IsChecked = true;
				break;
			}
			flag = false;
		}
	}

	private void page3Loaded(object sender, RoutedEventArgs e)
	{
		ExecLoad();
	}

	public void ExecUnLoad()
	{
		GlobalVars.GetBatteryServiceRun = false;
		cancellationTokenSource.Cancel();
		cancellationTokenSource?.Dispose();
		if (settings != null)
		{
			grid.Children.Remove(settings);
			settings = null;
		}
	}

	private void page3UnLoaded(object sender, RoutedEventArgs e)
	{
		ExecUnLoad();
	}

	private void Switch1_Checked(object sender, RoutedEventArgs e)
	{
		if (((CustomControlLibrary.Controls.Switch)sender).IsChecked == true)
		{
			control.INIWrite("HotKey", "Windows", "Y");
		}
		else
		{
			control.INIWrite("HotKey", "Windows", "N");
		}
	}

	private void ComboBox_DropDownClosed(object sender, EventArgs e)
	{
		ComboBox comboBox = (ComboBox)sender;
		if (origin != comboBox.SelectedIndex)
		{
			comboBox.Show();
			string messageBoxText = (string)Application.Current.FindResource("ResetTips");
			string caption = (string)Application.Current.FindResource("Tips");
			_ = new string[3] { "dGpu only", "Dynamic", "iGpu only" };
			switch (MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
			{
			case MessageBoxResult.Yes:
				((Page3ViewModel)base.DataContext).DisplayCommand.Execute((comboBox.SelectedIndex + 1).ToString());
				Process.Start("shutdown", "/r /t 2");
				break;
			case MessageBoxResult.No:
				comboBox.SelectedIndex = origin;
				break;
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/views/pages/page3.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			grid = (Grid)target;
			break;
		case 2:
			Switch1 = (CustomControlLibrary.Controls.Switch)target;
			Switch1.Checked += Switch1_Checked;
			Switch1.Unchecked += Switch1_Checked;
			break;
		case 3:
			Switch2 = (CustomControlLibrary.Controls.Switch)target;
			break;
		case 4:
			Label3 = (Label)target;
			break;
		case 5:
			Switch3 = (CustomControlLibrary.Controls.Switch)target;
			break;
		case 6:
			((ComboBox)target).DropDownClosed += ComboBox_DropDownClosed;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
