using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BydCentral.Core.Models;
using BydCentral.Core.ViewModels;
using BydCentral.Services;
using BydContral;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using Microsoft.Win32;

namespace BydCentral;

public class MainWindow : System.Windows.Window, IDisposable, IComponentConnector
{
	public delegate int DeviceNotifyCallbackRoutine(IntPtr context, int type, IntPtr setting);

	public struct DEVICE_NOTIFY_SUBSCRIBE_PARAMETERS
	{
		public DeviceNotifyCallbackRoutine Callback;

		public IntPtr Context;
	}

	private static IntPtr registrationHandle;

	private static DEVICE_NOTIFY_SUBSCRIBE_PARAMETERS recipient;

	private NotifyIconViewModel notifyIconViewModel;

	private int cur = 1;

	private Page1 page1;

	private Page2 page2;

	private Page3 page3;

	private bool isMini;

	private NotifyIcon notifyIcon;

	private const int F13EventId = 9000;

	private const int F14EventId = 9001;

	private const int WM_POWERBROADCAST = 536;

	private const int PBT_APMPOWERSTATUSCHANGE = 10;

	private const int PBT_APMRESUMEAUTOMATIC = 18;

	private const int PBT_APMRESUMESUSPEND = 7;

	private const int PBT_APMSUSPEND = 4;

	private const int PBT_POWERSETTINGCHANGE = 32787;

	private const int DEVICE_NOTIFY_CALLBACK = 2;

	internal MainWindow MainWindowDark;

	internal Grid TitleBar;

	internal Button minilize;

	internal Grid MainGrid;

	internal Grid GirdSide;

	internal RowDefinition LightGrid;

	internal RowDefinition UpdateGrid;

	internal Button button1;

	internal ImageBrush image1;

	internal Button button2;

	internal ImageBrush image2;

	internal Button button3;

	internal ImageBrush image3;

	internal Button button4;

	internal ImageBrush image4;

	internal Frame contentFrame;

	private bool _contentLoaded;

	public MainWindow()
	{
		InitializeComponent();
		try
		{
			registrationHandle = default(IntPtr);
			recipient = default(DEVICE_NOTIFY_SUBSCRIBE_PARAMETERS);
			recipient.Callback = DeviceNotifyCallback;
			recipient.Context = IntPtr.Zero;
			if (PowerRegisterSuspendResumeNotification(2u, ref recipient, ref registrationHandle) != 0)
			{
				Console.WriteLine("fail to register power notification");
			}
		}
		catch (Exception)
		{
		}
		base.StateChanged += MainWindow_StateChanged;
		base.Left = 20.0;
		base.Top = 20.0;
		page1 = new Page1();
		page3 = new Page3();
		UpdateGrid.Height = new GridLength(0.0);
		GirdSide.Margin = new Thickness(0.0, 115.0, 0.0, 127.0);
		GlobalVars.LightCOM = GlobalVars.Win32.getCom();
		page2 = new Page2();
		AppControl appControl = new AppControl();
		appControl.CreateIniFile();
		if (appControl.INIRead("HotKey", "Windows") == "Y")
		{
			Page3ViewModel.WinkeylockCommand.Execute(true);
		}
		else
		{
			Page3ViewModel.WinkeylockCommand.Execute(false);
		}
		base.Unloaded += MainWindow_Unloaded;
		base.Closing += Window_Closing;
		WeakReferenceMessenger.Default.Register<string, string>(this, "Resume", SuspendAndResume);
		WeakReferenceMessenger.Default.Register<string, string>(this, "SaveKeyboard", SuspendAndResume);
		SystemEvents.DisplaySettingsChanged += delegate
		{
			Application.Current.Dispatcher.Invoke(delegate
			{
				double width = SystemParameters.WorkArea.Width;
				double height = SystemParameters.WorkArea.Height;
				if (width > 1350.0 && height > 760.0)
				{
					base.Width = 1350.0;
					base.Height = 760.0;
				}
				else
				{
					double width2 = width - 50.0;
					double num = (width - 50.0) * 760.0 / 1350.0;
					if (num < height)
					{
						base.Width = width2;
						base.Height = num;
					}
					else
					{
						base.Width = (height - 50.0) * 1350.0 / 760.0;
						base.Height = height - 50.0;
					}
				}
			});
		};
	}

	private async Task GCCollect()
	{
		int i = 0;
		while (isMini && i < 2)
		{
			await Task.Delay(TimeSpan.FromSeconds(20.0));
			await Task.Run(delegate
			{
				GC.Collect();
			});
			i++;
		}
	}

	private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
	{
	}

	private async void SuspendAndResume(object obj, string msg)
	{
		try
		{
			if (msg == "resume")
			{
				await Task.Delay(2000);
				page2.GetVersion();
				GlobalVars.Wmi.inMS(intoMS: false);
			}
		}
		catch (Exception ex)
		{
			LoggerHelper._.Error("cath error" + ex.ToString());
		}
	}

	private void Page1Button_Click(object sender, RoutedEventArgs e)
	{
		cur = 1;
		contentFrame.Navigate(page1);
		image1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/lig_b.png", UriKind.RelativeOrAbsolute));
		image2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/set.png", UriKind.RelativeOrAbsolute));
		image3.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/mod.png", UriKind.RelativeOrAbsolute));
		button1.Foreground = new SolidColorBrush(Color.FromRgb(48, 179, 235));
		button2.Foreground = new SolidColorBrush(Color.FromRgb(191, 191, 191));
		button3.Foreground = new SolidColorBrush(Color.FromRgb(191, 191, 191));
	}

	private void Page2Button_Click(object sender, RoutedEventArgs e)
	{
		cur = 2;
		contentFrame.Navigate(page2);
		image1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/lig.png", UriKind.RelativeOrAbsolute));
		image2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/set_b.png", UriKind.RelativeOrAbsolute));
		image3.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/mod.png", UriKind.RelativeOrAbsolute));
		button1.Foreground = new SolidColorBrush(Color.FromRgb(191, 191, 191));
		button2.Foreground = new SolidColorBrush(Color.FromRgb(48, 179, 235));
		button3.Foreground = new SolidColorBrush(Color.FromRgb(191, 191, 191));
	}

	private void Page3Button_Click(object sender, RoutedEventArgs e)
	{
		cur = 3;
		contentFrame.Navigate(page3);
		image1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/lig.png", UriKind.RelativeOrAbsolute));
		image2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/set.png", UriKind.RelativeOrAbsolute));
		image3.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/mod_b.png", UriKind.RelativeOrAbsolute));
		button1.Foreground = new SolidColorBrush(Color.FromRgb(191, 191, 191));
		button2.Foreground = new SolidColorBrush(Color.FromRgb(191, 191, 191));
		button3.Foreground = new SolidColorBrush(Color.FromRgb(48, 179, 235));
	}

	private void Page4Button_Click(object sender, RoutedEventArgs e)
	{
		cur = 4;
		image1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/lig.png", UriKind.RelativeOrAbsolute));
		image2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/set.png", UriKind.RelativeOrAbsolute));
		image3.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/mod.png", UriKind.RelativeOrAbsolute));
		button1.Foreground = new SolidColorBrush(Color.FromRgb(191, 191, 191));
		button2.Foreground = new SolidColorBrush(Color.FromRgb(191, 191, 191));
		button3.Foreground = new SolidColorBrush(Color.FromRgb(191, 191, 191));
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		Task.Run(delegate
		{
			Page1ViewModel.commands[0].Execute(null);
			Page2ViewModel.GetCOMCommand.Execute(null);
			Page3ViewModel.GetInfoCommand.Execute(null);
			Page3ViewModel.GpuModeCommand.Execute(null);
		});
		Application.Current.Dispatcher.Invoke(delegate
		{
			double width = SystemParameters.WorkArea.Width;
			double height = SystemParameters.WorkArea.Height;
			if (width > 1350.0 && height > 760.0)
			{
				base.Width = 1350.0;
				base.Height = 760.0;
			}
			else
			{
				double width2 = width - 50.0;
				double num = (width - 50.0) * 760.0 / 1350.0;
				if (num < height)
				{
					base.Width = width2;
					base.Height = num;
				}
				else
				{
					base.Width = (height - 50.0) * 1350.0 / 760.0;
					base.Height = height - 50.0;
				}
			}
		});
		contentFrame.Navigate(page1);
	}

	private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
		{
			DragMove();
		}
	}

	private void Window_Closing(object sender, CancelEventArgs e)
	{
		e.Cancel = true;
		base.WindowState = WindowState.Minimized;
		Hide();
	}

	private void HandleShortcut()
	{
		if (base.WindowState == WindowState.Minimized)
		{
			base.WindowState = WindowState.Normal;
			Activate();
		}
	}

	private void Grid_PreviewTouchDown(object sender, TouchEventArgs e)
	{
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		base.WindowState = WindowState.Minimized;
	}

	private void Button_Click_1(object sender, RoutedEventArgs e)
	{
		base.WindowState = WindowState.Minimized;
		Hide();
	}

	private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
	{
	}

	private void MainWindow_StateChanged(object sender, EventArgs e)
	{
		if (base.WindowState == WindowState.Minimized)
		{
			switch (cur)
			{
			case 1:
				page1.ExecUnload();
				break;
			case 2:
				page2.ExecUnLoad();
				break;
			case 3:
				page3.ExecUnLoad();
				break;
			}
			isMini = true;
			GCCollect();
		}
		else if (base.WindowState == WindowState.Normal)
		{
			switch (cur)
			{
			case 1:
				page1.ExecLoad();
				break;
			case 2:
				page2.ExecLoad();
				break;
			case 3:
				page3.ExecLoad();
				break;
			}
			isMini = false;
		}
	}

	private void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
	{
		Show();
		base.WindowState = WindowState.Normal;
	}

	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeys fsModifuers, int vk);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

	protected override void OnSourceInitialized(EventArgs e)
	{
		base.OnSourceInitialized(e);
		registerKey();
	}

	private void registerKey()
	{
		IntPtr handle = new WindowInteropHelper(this).Handle;
		HwndSource.FromHwnd(handle)?.AddHook(HwndHook);
		RegisterHotKey(handle, 9000, ModifierKeys.None, KeyInterop.VirtualKeyFromKey(Key.F13));
		RegisterHotKey(handle, 9001, ModifierKeys.None, KeyInterop.VirtualKeyFromKey(Key.F14));
	}

	private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	{
		if (msg == 786)
		{
			switch (wParam.ToInt32())
			{
			case 9000:
				GlobalVars.IsKeyCon = true;
				page2.Custom_Board.IsChecked = !page2.Custom_Board.IsChecked;
				page2.Custom_Board_Checked(page2.Custom_Board, null);
				if (page2.Custom_Board.IsChecked == true)
				{
					WeakReferenceMessenger.Default.Send("KbLightOn", "Popup");
				}
				else
				{
					WeakReferenceMessenger.Default.Send("KbLightOff", "Popup");
				}
				break;
			case 9001:
				GlobalVars.IsKeyCon = true;
				page2.Custom_Back.IsChecked = !page2.Custom_Back.IsChecked;
				page2.Custom_Back_Checked(page2.Custom_Back, null);
				if (page2.Custom_Back.IsChecked == true)
				{
					WeakReferenceMessenger.Default.Send("BackLightOn", "Popup");
				}
				else
				{
					WeakReferenceMessenger.Default.Send("BackLightOff", "Popup");
				}
				break;
			}
		}
		return IntPtr.Zero;
	}

	protected override void OnClosed(EventArgs e)
	{
		base.OnClosed(e);
		IntPtr handle = new WindowInteropHelper(this).Handle;
		UnregisterHotKey(handle, 9000);
		UnregisterHotKey(handle, 9001);
	}

	public void Dispose()
	{
		page2.Dispose();
	}

	private static int DeviceNotifyCallback(IntPtr context, int type, IntPtr setting)
	{
		switch (type)
		{
		case 7:
			WeakReferenceMessenger.Default.Send("resume", "Resume");
			break;
		case 4:
			WeakReferenceMessenger.Default.Send("suspend", "SaveKeyboard");
			break;
		}
		return 0;
	}

	[DllImport("Powrprof.dll", SetLastError = true)]
	private static extern uint PowerRegisterSuspendResumeNotification(uint flags, ref DEVICE_NOTIFY_SUBSCRIBE_PARAMETERS receipient, ref IntPtr registrationHandle);

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/views/mainwindow.xaml", UriKind.Relative);
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
			MainWindowDark = (MainWindow)target;
			MainWindowDark.Loaded += Window_Loaded;
			MainWindowDark.SizeChanged += Window_SizeChanged;
			break;
		case 2:
			TitleBar = (Grid)target;
			TitleBar.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
			break;
		case 3:
			((Grid)target).PreviewTouchDown += Grid_PreviewTouchDown;
			break;
		case 4:
			minilize = (Button)target;
			minilize.Click += Button_Click;
			break;
		case 5:
			((Button)target).Click += Button_Click_1;
			break;
		case 6:
			MainGrid = (Grid)target;
			break;
		case 7:
			GirdSide = (Grid)target;
			break;
		case 8:
			LightGrid = (RowDefinition)target;
			break;
		case 9:
			UpdateGrid = (RowDefinition)target;
			break;
		case 10:
			button1 = (Button)target;
			button1.Click += Page1Button_Click;
			break;
		case 11:
			image1 = (ImageBrush)target;
			break;
		case 12:
			button2 = (Button)target;
			button2.Click += Page2Button_Click;
			break;
		case 13:
			image2 = (ImageBrush)target;
			break;
		case 14:
			button3 = (Button)target;
			button3.Click += Page3Button_Click;
			break;
		case 15:
			image3 = (ImageBrush)target;
			break;
		case 16:
			button4 = (Button)target;
			button4.Click += Page4Button_Click;
			break;
		case 17:
			image4 = (ImageBrush)target;
			break;
		case 18:
			contentFrame = (Frame)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
