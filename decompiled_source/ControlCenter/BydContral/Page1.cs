using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BydCentral;
using BydCentral.Core.Models;
using BydCentral.Core.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace BydContral;

public class Page1 : Page, IComponentConnector
{
	public CancellationTokenSource cancellationGetSystemModeTokenSource;

	private CancellationTokenSource cancellationTokenSource;

	internal Button ButtonOffice;

	internal Button ButtonBalance;

	internal Button ButtonGaming;

	internal Image imageMode;

	internal Grid grid;

	internal RowDefinition Row1;

	private bool _contentLoaded;

	public Page1()
	{
		InitializeComponent();
		base.DataContext = App.Current.Services.GetService(typeof(Page1ViewModel));
		WeakReferenceMessenger.Default.Register<string, string>(this, "SystemMode", Message);
		cancellationGetSystemModeTokenSource = new CancellationTokenSource();
	}

	private void Message(object obj, string message)
	{
		if (message == "0")
		{
			ButtonOffice.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			ButtonBalance.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			ButtonGaming.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			imageMode.Source = new BitmapImage(new Uri("/image/办公模式.png", UriKind.RelativeOrAbsolute));
		}
		if (message == "1")
		{
			ButtonOffice.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			ButtonBalance.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			ButtonGaming.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			imageMode.Source = new BitmapImage(new Uri("/image/性能模式.png", UriKind.RelativeOrAbsolute));
		}
		if (message == "2")
		{
			ButtonOffice.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			ButtonBalance.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			ButtonGaming.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			imageMode.Source = new BitmapImage(new Uri("/image/游戏模式.png", UriKind.RelativeOrAbsolute));
		}
		Page1ViewModel.GetFanMaxCommand.Execute(null);
	}

	public async void ExecLoad()
	{
		if (GlobalVars.MiniStratup)
		{
			GlobalVars.MiniStratup = false;
			return;
		}
		GlobalVars.Wmi.inMS(intoMS: false);
		GlobalVars.GetInfoServiceRun = true;
		cancellationTokenSource = new CancellationTokenSource();
		if (Page1ViewModel.commandsRoute[0].CanExecute(cancellationTokenSource.Token))
		{
			Page1ViewModel.commandsRoute[0].Execute(cancellationTokenSource.Token);
		}
	}

	public void GetSystemMode()
	{
		GlobalVars.GetSystemModeRun = true;
		cancellationGetSystemModeTokenSource = new CancellationTokenSource();
		if (Page1ViewModel.GetSystemModeCommand.CanExecute(cancellationGetSystemModeTokenSource.Token))
		{
			Page1ViewModel.GetSystemModeCommand.Execute(cancellationGetSystemModeTokenSource.Token);
		}
	}

	public void CancelToken()
	{
		CancelGetInfo();
	}

	private void CancelGetSystem()
	{
		GlobalVars.GetSystemModeRun = false;
		cancellationGetSystemModeTokenSource.Cancel();
		cancellationGetSystemModeTokenSource.Dispose();
	}

	private void CancelGetInfo()
	{
		GlobalVars.GetInfoServiceRun = false;
		cancellationTokenSource.Cancel();
		cancellationTokenSource?.Dispose();
	}

	private void Page_Loaded(object sender, RoutedEventArgs e)
	{
		ExecLoad();
	}

	public void ExecUnload()
	{
		CancelGetInfo();
	}

	private void Page_UnLoaded(object sender, RoutedEventArgs e)
	{
		ExecUnload();
	}

	private void ButtonOffice_Click(object sender, RoutedEventArgs e)
	{
		ButtonOffice.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
		ButtonBalance.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
		ButtonGaming.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
		imageMode.Source = new BitmapImage(new Uri("/image/办公模式.png", UriKind.RelativeOrAbsolute));
		Page1ViewModel.GetFanMaxCommand.Execute(null);
	}

	private void ButtonBalance_Click(object sender, RoutedEventArgs e)
	{
		ButtonOffice.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
		ButtonBalance.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
		ButtonGaming.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
		imageMode.Source = new BitmapImage(new Uri("/image/性能模式.png", UriKind.RelativeOrAbsolute));
		Page1ViewModel.GetFanMaxCommand.Execute(null);
	}

	private void ButtonGaming_Click(object sender, RoutedEventArgs e)
	{
		ButtonOffice.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
		ButtonBalance.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
		ButtonGaming.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
		imageMode.Source = new BitmapImage(new Uri("/image/游戏模式.png", UriKind.RelativeOrAbsolute));
		Page1ViewModel.GetFanMaxCommand.Execute(null);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/views/pages/page1.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			((Page1)target).Loaded += Page_Loaded;
			((Page1)target).Unloaded += Page_UnLoaded;
			break;
		case 2:
			ButtonOffice = (Button)target;
			ButtonOffice.Click += ButtonOffice_Click;
			break;
		case 3:
			ButtonBalance = (Button)target;
			ButtonBalance.Click += ButtonBalance_Click;
			break;
		case 4:
			ButtonGaming = (Button)target;
			ButtonGaming.Click += ButtonGaming_Click;
			break;
		case 5:
			imageMode = (Image)target;
			break;
		case 6:
			grid = (Grid)target;
			break;
		case 7:
			Row1 = (RowDefinition)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
