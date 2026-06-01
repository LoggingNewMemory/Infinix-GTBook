#define TRACE
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using AngleSharp.Text;
using BydCentral;
using BydCentral.Core.Models;
using BydCentral.Core.Services;
using BydCentral.Core.ViewModels;
using BydCentral.CustomControl.CustomUserControl;
using BydCentral.Services;
using BydCentral.Views;
using BydContral;
using BYDWmi;
using CommunityToolkit.Mvvm.Messaging;
using CustomControl;
using CustomControlLibrary.Controls;
using FftSharp;
using FftSharp.Windows;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using IWshRuntimeLibrary;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
using Microsoft.CodeAnalysis;
using Microsoft.Expression.Shapes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;
using Newtonsoft.Json;
using NLog;
using NvAPIWrapper.GPU;
using NvAPIWrapper.Native;
using NvAPIWrapper.Native.Interfaces.GPU;
using Panuon.WPF.UI;
using ProtoBuf;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: ComVisible(false)]
[assembly: Guid("5980fb7d-c8ac-4ff8-b2b6-2098e2933e3a")]
[assembly: TargetFramework(".NETCoreApp,Version=v6.0", FrameworkDisplayName = ".NET 6.0")]
[assembly: AssemblyCompany("ControlCenter")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.2.2")]
[assembly: AssemblyInformationalVersion("1.0.0")]
[assembly: AssemblyProduct("ControlCenter")]
[assembly: AssemblyTitle("ControlCenter")]
[assembly: TargetPlatform("Windows7.0")]
[assembly: SupportedOSPlatform("Windows7.0")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: AssemblyVersion("1.2.2.0")]
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
namespace BydContral
{
	public class Page1 : Page, IComponentConnector
	{
		public CancellationTokenSource cancellationGetSystemModeTokenSource;

		private CancellationTokenSource cancellationTokenSource;

		internal System.Windows.Controls.Button ButtonOffice;

		internal System.Windows.Controls.Button ButtonBalance;

		internal System.Windows.Controls.Button ButtonGaming;

		internal System.Windows.Controls.Image imageMode;

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
				ButtonOffice.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				ButtonBalance.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				ButtonGaming.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				imageMode.Source = new BitmapImage(new Uri("/image/办公模式.png", UriKind.RelativeOrAbsolute));
			}
			if (message == "1")
			{
				ButtonOffice.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				ButtonBalance.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				ButtonGaming.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				imageMode.Source = new BitmapImage(new Uri("/image/性能模式.png", UriKind.RelativeOrAbsolute));
			}
			if (message == "2")
			{
				ButtonOffice.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				ButtonBalance.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				ButtonGaming.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
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
			ButtonOffice.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
			ButtonBalance.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
			ButtonGaming.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
			imageMode.Source = new BitmapImage(new Uri("/image/办公模式.png", UriKind.RelativeOrAbsolute));
			Page1ViewModel.GetFanMaxCommand.Execute(null);
		}

		private void ButtonBalance_Click(object sender, RoutedEventArgs e)
		{
			ButtonOffice.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
			ButtonBalance.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
			ButtonGaming.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
			imageMode.Source = new BitmapImage(new Uri("/image/性能模式.png", UriKind.RelativeOrAbsolute));
			Page1ViewModel.GetFanMaxCommand.Execute(null);
		}

		private void ButtonGaming_Click(object sender, RoutedEventArgs e)
		{
			ButtonOffice.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
			ButtonBalance.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
			ButtonGaming.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
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
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				ButtonOffice = (System.Windows.Controls.Button)target;
				ButtonOffice.Click += ButtonOffice_Click;
				break;
			case 3:
				ButtonBalance = (System.Windows.Controls.Button)target;
				ButtonBalance.Click += ButtonBalance_Click;
				break;
			case 4:
				ButtonGaming = (System.Windows.Controls.Button)target;
				ButtonGaming.Click += ButtonGaming_Click;
				break;
			case 5:
				imageMode = (System.Windows.Controls.Image)target;
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
	public class Page2 : Page, IDisposable, IComponentConnector
	{
		private enum LightMode : byte
		{
			Always,
			Breath,
			Clock,
			Rainbow,
			Wave,
			Flow
		}

		private enum BackLightMode : byte
		{
			Always,
			Breath,
			MusicRhythm,
			MusicJump,
			GameRound,
			GameCovering,
			Default
		}

		public struct HsvColor
		{
			public double H { get; set; }

			public double S { get; set; }

			public double V { get; set; }
		}

		private bool isIni;

		private bool brushChange;

		private bool isWrite;

		private bool isSlideDrag;

		private bool isTextKeyDown;

		private bool isSendingData;

		private bool isCustom;

		private WriteableBitmap bitmap;

		private int titleFlag;

		private int keyboardFlag = 1;

		private Thread comThread;

		private readonly ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);

		private readonly AutoResetEvent threadResetEvent = new AutoResetEvent(initialState: false);

		private DispatcherTimer dispatcherTimer = new DispatcherTimer();

		private volatile WriteableBitmap[] colorZoneBitmap = new WriteableBitmap[6];

		private int musicRythm = 24;

		private int musicJump = 25;

		private int gameRound = 26;

		private int gameCover = 27;

		private SemaphoreSlim semaphore = new SemaphoreSlim(1);

		private Dictionary<string, int> CurMap = new Dictionary<string, int>();

		private BydCentral.Services.Usb? usb;

		private TxBuf txBufClass;

		private TxBuf.txBuF txBufStruct;

		private Back_light_buf back_light_buf;

		private Back_light_buf.PARAMS back_light_bufStruct;

		private AudioCapturer capture;

		private SerPort serPort;

		private AppControl app;

		private Light_Infinix[] light;

		private int cur = -1;

		private int width = 388;

		private int height = 388;

		private int backWidth = 600;

		private int backHeight = 40;

		private int back2Width = 501;

		private int back2Height = 33;

		private int x1 = 65;

		private int y1 = 170;

		private int x2 = 328;

		private int y2 = 269;

		private int keyBoardWidth = 267;

		private int keyBoardHeight = 99;

		private int backX1 = 15;

		private int backY1 = 11;

		private int backX2 = 584;

		private int backY2 = 15;

		private int back2X1 = 12;

		private int back2Y1 = 9;

		private int back2X2 = 487;

		private int back2Y2 = 14;

		private static CancellationTokenSource breathTokenSource;

		private CancellationToken breathToken;

		private static CancellationTokenSource waveTokenSource;

		private CancellationToken waveToken;

		private static CancellationTokenSource clockTokenSource;

		private CancellationToken clockToken;

		private static CancellationTokenSource rainbowTokenSource;

		private CancellationToken rainbowToken;

		private static CancellationTokenSource flowTokenSource;

		private CancellationToken flowToken;

		private static CancellationTokenSource musicRythmTokenSource;

		private CancellationToken rythmToken;

		private static CancellationTokenSource musicJumpTokenSource;

		private CancellationToken jumpToken;

		private static CancellationTokenSource gameRoundTokenSource;

		private CancellationToken roundToken;

		private static CancellationTokenSource gameCoverTokenSource;

		private CancellationToken coverToken;

		private System.Windows.Media.Color keyboardColor = Colors.Green;

		private System.Windows.Media.Color backColor = Colors.Green;

		private System.Windows.Media.Color sideColor = Colors.Green;

		private System.Windows.Media.Color keyboardColorRecord = Colors.Green;

		private System.Windows.Media.Color backColorRecord = Colors.Green;

		private System.Windows.Media.Color sideColorRecord = Colors.Green;

		private DateTime startTime = DateTime.Now;

		private int waveIndex;

		private int clockIndex;

		private int RainbowIndex;

		private volatile System.Windows.Media.Color[] rainbowColors = new System.Windows.Media.Color[7]
		{
			Colors.Red,
			Colors.Orange,
			Colors.Yellow,
			Colors.Green,
			Colors.Blue,
			Colors.Indigo,
			Colors.Violet
		};

		private List<System.Windows.Media.Color> flowColors;

		private int FlowIndex;

		internal System.Windows.Controls.Button keyboard;

		internal System.Windows.Controls.Button back;

		internal Viewbox keyBoardView;

		internal Grid KeyboardGrid;

		internal System.Windows.Controls.Image ColorZone_back;

		internal System.Windows.Controls.Image ColorZone;

		internal System.Windows.Controls.Image Zone;

		internal System.Windows.Shapes.Path KeyboardPath1;

		internal System.Windows.Shapes.Path KeyboardPath2;

		internal System.Windows.Shapes.Path KeyboardPath3;

		internal System.Windows.Shapes.Path KeyboardPath4;

		internal Viewbox backView_2;

		internal Border border1;

		internal Grid back1_grid;

		internal TextBlock back1_text;

		internal System.Windows.Controls.Image ColorZone_back_1;

		internal System.Windows.Controls.Image ColorZone_1;

		internal System.Windows.Controls.Image Zone_1;

		internal Grid music1;

		internal TextBlock back1_color1_text;

		internal System.Windows.Controls.RadioButton back1_color1;

		internal Border back1_color1_border;

		internal System.Windows.Shapes.Rectangle back1_color1_rec;

		internal Grid game1;

		internal TextBlock back1_color2_text;

		internal System.Windows.Controls.RadioButton back1_color2;

		internal Border back1_color2_border;

		internal System.Windows.Shapes.Rectangle back1_color2_rec;

		internal TextBlock back1_color3_text;

		internal System.Windows.Controls.RadioButton back1_color3;

		internal Border back1_color3_border;

		internal System.Windows.Shapes.Rectangle back1_color3_rec;

		internal Border border2;

		internal TextBlock back2_text;

		internal System.Windows.Controls.Image ColorZone_back_2;

		internal System.Windows.Controls.Image ColorZone_2;

		internal System.Windows.Controls.Image Zone_2;

		internal Grid music2;

		internal TextBlock back2_color1_text;

		internal System.Windows.Controls.RadioButton back2_color1;

		internal Border back2_color1_border;

		internal System.Windows.Shapes.Rectangle back2_color1_rec;

		internal Grid game2;

		internal TextBlock back2_color2_text;

		internal System.Windows.Controls.RadioButton back2_color2;

		internal Border back2_color2_border;

		internal System.Windows.Shapes.Rectangle back2_color2_rec;

		internal TextBlock back2_color3_text;

		internal System.Windows.Controls.RadioButton back2_color3;

		internal Border back2_color3_border;

		internal System.Windows.Shapes.Rectangle back2_color3_rec;

		internal Grid KeyBoardGrid;

		internal System.Windows.Controls.RadioButton KeyBoardAlways;

		internal System.Windows.Controls.Image KeyBoardAlwaysImg;

		internal System.Windows.Controls.RadioButton KeyBoardBreath;

		internal System.Windows.Controls.Image KeyBoardBreathImg;

		internal System.Windows.Controls.RadioButton KeyBoardWave;

		internal System.Windows.Controls.Image KeyBoardWaveImg;

		internal System.Windows.Controls.RadioButton KeyBoardClock;

		internal System.Windows.Controls.Image KeyBoardClockImg;

		internal System.Windows.Controls.RadioButton KeyBoardRainbow;

		internal System.Windows.Controls.Image KeyBoardRainbowImg;

		internal System.Windows.Controls.RadioButton KeyBoardFlow;

		internal System.Windows.Controls.Image KeyBoardFlowImg;

		internal Grid ZoneGrid;

		internal System.Windows.Controls.RadioButton ZoneAlways;

		internal System.Windows.Controls.Image ZoneAlwaysImg;

		internal System.Windows.Controls.RadioButton ZoneBreath;

		internal System.Windows.Controls.Image ZoneBreathImg;

		internal System.Windows.Controls.RadioButton ZoneClock;

		internal System.Windows.Controls.Image ZoneClockImg;

		internal System.Windows.Controls.RadioButton ZoneRainbow;

		internal System.Windows.Controls.Image ZoneRainbowImg;

		internal Grid BackGrid;

		internal System.Windows.Controls.RadioButton BackAlways;

		internal System.Windows.Controls.Image BackAlwaysImg;

		internal System.Windows.Controls.RadioButton BackBreath;

		internal System.Windows.Controls.Image BackBreathImg;

		internal System.Windows.Controls.RadioButton BackMusic;

		internal System.Windows.Controls.Image BackMusicImg;

		internal System.Windows.Controls.RadioButton BackGaming;

		internal System.Windows.Controls.Image BackGameImg;

		internal Grid grid;

		internal Viewbox viewbox;

		internal TextBlock Custom_Text;

		internal CustomControlLibrary.Controls.Switch Custom_Board;

		internal CustomControlLibrary.Controls.Switch Custom_Back;

		internal CustomControlLibrary.Controls.Switch Custom_Button;

		internal Grid PanelGrid;

		internal ColorControlPanel ColorPanel;

		internal Grid ColorTextGrid;

		internal StackPanel ColorStackPanel;

		internal System.Windows.Controls.TextBox R;

		internal System.Windows.Controls.TextBox G;

		internal System.Windows.Controls.TextBox B;

		internal FlatSilder Brightness;

		private bool _contentLoaded;

		public Page2()
		{
			base.DataContext = App.Current.Services.GetService(typeof(Page2ViewModel));
			InitializeComponent();
			InitCfg();
			StartPage2();
			base.Unloaded += page2UnLoaded;
			base.Loaded += page2Loaded;
			InitSwitch();
			isIni = true;
		}

		private void InitCfg()
		{
			capture = new AudioCapturer();
			serPort = new SerPort();
			back_light_buf = new Back_light_buf();
			app = new AppControl();
			txBufStruct = default(TxBuf.txBuF);
			txBufClass = new TxBuf();
			usb = new BydCentral.Services.Usb();
			light = new Light_Infinix[28];
			colorZoneBitmap[0] = new WriteableBitmap(width, height, 96.0, 96.0, PixelFormats.Pbgra32, null);
			colorZoneBitmap[1] = new WriteableBitmap(backWidth, backHeight, 96.0, 96.0, PixelFormats.Pbgra32, null);
			colorZoneBitmap[2] = new WriteableBitmap(back2Width, back2Height, 96.0, 96.0, PixelFormats.Pbgra32, null);
			colorZoneBitmap[3] = new WriteableBitmap(back2Width, back2Height, 96.0, 96.0, PixelFormats.Pbgra32, null);
			colorZoneBitmap[4] = new WriteableBitmap(back2Width, back2Height, 96.0, 96.0, PixelFormats.Pbgra32, null);
			colorZoneBitmap[5] = new WriteableBitmap(back2Width, back2Height, 96.0, 96.0, PixelFormats.Pbgra32, null);
			CurMap["keyboard"] = 0;
			CurMap["keyboard1"] = 6;
			CurMap["keyboard2"] = 10;
			CurMap["keyboard3"] = 14;
			CurMap["keyboard4"] = 18;
			CurMap["backlight"] = 22;
			flowColors = LoadFlowColorsFromFile("/resource/FlowColors.xml");
			string filename = System.Windows.Forms.Application.StartupPath + "\\LightConfig.dat";
			CreateOrLoadCfg(filename);
			ChangeKeyBoardColor(light[CurMap["keyboard"]].R, light[CurMap["keyboard"]].G, light[CurMap["keyboard"]].B, (int)light[CurMap["keyboard"]].L);
			ChangeBackColor(light[CurMap["backlight"]].R, light[CurMap["backlight"]].G, light[CurMap["backlight"]].B, (int)light[CurMap["backlight"]].L);
			ColorZone.Source = colorZoneBitmap[0];
			ColorZone_1.Source = colorZoneBitmap[2];
			ColorZone_2.Source = colorZoneBitmap[3];
			ColorZone_back_1.Source = colorZoneBitmap[4];
			ColorZone_back_2.Source = colorZoneBitmap[5];
			if (app.INIRead("system", "keyboard") == "no config item")
			{
				app.INIWrite("system", "keyboard", "keyboard");
			}
			if (app.INIRead("system", "IsCustom") == "Y")
			{
				GlobalVars.Wmi.setLightPageStatus(isDefault: false);
			}
			keyboard.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
			stopAllTimer();
			WeakReferenceMessenger.Default.Register<string, string>(this, "SystemMode", Message);
			WeakReferenceMessenger.Default.Register<string, string>(this, "KeyboardLightStatus", ChangeKeyboardSwitch);
			WeakReferenceMessenger.Default.Register<string, string>(this, "BackLightStatus", ChangeBackSwitch);
			WeakReferenceMessenger.Default.Register<string, string>(this, "SaveKeyboard", SaveKeyboard);
			ColorControlPanel colorPanel = ColorPanel;
			DependencyPropertyDescriptor.FromProperty(ColorControlPanel.SelectBrushProperty, typeof(ColorControlPanel))?.AddValueChanged(colorPanel, OnSelectBrushChanged);
		}

		private void StartPage2()
		{
			comThread = new Thread(ComThreadFunction);
			comThread.IsBackground = true;
			comThread.Start();
			dispatcherTimer.Tick += DispatcherTimer_Tick;
			dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
			startComTick();
			if (light[CurMap["backlight"]].Switch == 1 && (light[CurMap["backlight"]].Mode == 2 || light[CurMap["backlight"]].Mode == 3))
			{
				StartSendMusicData();
			}
		}

		private void OnSelectBrushChanged(object sender, EventArgs e)
		{
			try
			{
				if (isIni)
				{
					brushChange = true;
					System.Windows.Media.Color color = ColorPanel.SelectBrush.Color;
					ConvertRgbToHsv(color);
					int num = cur;
					int mode = light[num].Mode;
					light[num + mode].PropertyChanged -= Light_PropertyChanged;
					R.Text = color.R.ToString();
					G.Text = color.G.ToString();
					B.Text = color.B.ToString();
					RGBLChanged(light[num + mode]);
					light[num + mode].PropertyChanged += Light_PropertyChanged;
					brushChange = false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		private void ColorPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			SaveLightData();
		}

		private void DispatcherTimer_Tick(object sender, EventArgs e)
		{
			if (!isSendingData)
			{
				serPort.CloseCom();
				dispatcherTimer.Stop();
			}
		}

		private async System.Threading.Tasks.Task startComTick()
		{
			if (!serPort.IsComOpen())
			{
				await System.Threading.Tasks.Task.Delay(50);
				serPort.OpenCom();
				dispatcherTimer.Stop();
				dispatcherTimer.Start();
				await System.Threading.Tasks.Task.Delay(50);
			}
		}

		private void InitSwitch()
		{
			Page2ViewModel.GetStatusCommand?.Execute(null);
		}

		~Page2()
		{
			txBufClass = null;
			usb = null;
			capture = null;
			serPort = null;
			back_light_buf = null;
		}

		private void Message(object obj, string msg)
		{
			if (light[CurMap["backlight"]].Mode == 6)
			{
				StartAutoEvent();
			}
		}

		private void ChangeKeyboardSwitch(object obj, string msg)
		{
			if (isWrite)
			{
				return;
			}
			if (msg == "0")
			{
				base.Dispatcher.Invoke(delegate
				{
					Custom_Board.IsChecked = true;
				});
			}
			else if (msg == "1")
			{
				base.Dispatcher.Invoke(delegate
				{
					Custom_Board.IsChecked = false;
				});
			}
			Custom_Board_Checked(Custom_Board, null);
		}

		private void ChangeBackSwitch(object obj, string msg)
		{
			if (isWrite)
			{
				return;
			}
			if (msg == "0")
			{
				base.Dispatcher.Invoke(delegate
				{
					Custom_Back.IsChecked = true;
				});
			}
			else if (msg == "2")
			{
				base.Dispatcher.Invoke(delegate
				{
					Custom_Back.IsChecked = false;
				});
			}
			Custom_Back_Checked(Custom_Back, null);
		}

		private void SaveKeyboard(object obj, string msg)
		{
			txBufStruct = txBufClass.on_setKeyboardSave();
			WriteUsb();
		}

		public void GetVersion()
		{
			txBufStruct = txBufClass.on_getVersion();
			WriteUsb();
		}

		private void InitLight()
		{
			InitBoardLight();
			InitBackLight();
		}

		private void InitBoardLight()
		{
			for (int i = 0; i < CurMap["backlight"]; i++)
			{
				if (light[i] == null)
				{
					light[i] = new Light_Infinix();
				}
				light[i].PropertyChanged -= Light_PropertyChanged;
				light[i].Class = (byte)i;
				light[i].Switch = 1;
				light[i].Mode = 0;
				light[i].R = 0;
				light[i].G = byte.MaxValue;
				light[i].B = 0;
				light[i].L = 100;
				light[i].backL = 100;
				light[i].PropertyChanged += Light_PropertyChanged;
			}
		}

		private void InitBackLight()
		{
			for (int i = CurMap["backlight"]; i < light.Length; i++)
			{
				if (light[i] == null)
				{
					light[i] = new Light_Infinix();
				}
				light[i].PropertyChanged -= Light_PropertyChanged;
				light[i].Class = (byte)i;
				if (i == CurMap["backlight"])
				{
					light[i].Mode = 6;
					light[i].Switch = 1;
				}
				else
				{
					light[i].Mode = 0;
				}
				light[i].R = byte.MaxValue;
				light[i].G = byte.MaxValue;
				light[i].B = byte.MaxValue;
				light[i].L = 100;
				light[i].backR = 48;
				light[i].backG = 235;
				light[i].backB = 231;
				light[i].backL = 100;
				light[i].PropertyChanged += Light_PropertyChanged;
			}
		}

		private void CreateOrLoadCfg(string filename)
		{
			if (!File.Exists(filename))
			{
				new FileStream(filename, FileMode.Create, FileAccess.ReadWrite).Close();
				InitLight();
				SaveLightData();
				return;
			}
			try
			{
				LoadLight(filename);
			}
			catch
			{
				light = new Light_Infinix[28];
				new FileStream(filename, FileMode.Create, FileAccess.ReadWrite).Close();
				InitLight();
				SaveLightData();
				LoadLight(filename);
			}
		}

		private void LoadLight(string fp)
		{
			using (FileStream source = File.OpenRead(fp))
			{
				light = Serializer.Deserialize<Light_Infinix[]>((Stream)source);
			}
			if (light.Length < 28)
			{
				throw new Exception("灯光配置数量出错，重置");
			}
			for (int i = 0; i < light.Length; i++)
			{
				light[i].PropertyChanged += Light_PropertyChanged;
			}
		}

		public async System.Threading.Tasks.Task SaveLightData()
		{
			using (FileStream destination = new FileStream(System.Windows.Forms.Application.StartupPath + "\\LightConfig.dat", FileMode.Create))
			{
				Serializer.Serialize((Stream)destination, light);
			}
			await System.Threading.Tasks.Task.CompletedTask;
		}

		private List<System.Windows.Media.Color> LoadFlowColorsFromFile(string relativeResourcePath)
		{
			List<System.Windows.Media.Color> list = new List<System.Windows.Media.Color>();
			try
			{
				StreamResourceInfo resourceStream = System.Windows.Application.GetResourceStream(new Uri(relativeResourcePath, UriKind.RelativeOrAbsolute));
				if (resourceStream != null)
				{
					using Stream stream = resourceStream.Stream;
					foreach (XElement item2 in XDocument.Load(stream).Descendants("Color"))
					{
						byte r = byte.Parse(item2.Element("Red").Value.Substring(2), NumberStyles.HexNumber);
						byte g = byte.Parse(item2.Element("Green").Value.Substring(2), NumberStyles.HexNumber);
						byte b = byte.Parse(item2.Element("Blue").Value.Substring(2), NumberStyles.HexNumber);
						System.Windows.Media.Color item = System.Windows.Media.Color.FromRgb(r, g, b);
						list.Add(item);
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		public void ExecLoad()
		{
			WriteLightNotAutoClose();
			if (isIni)
			{
				if (cur < CurMap["backlight"] && light[CurMap["keyboard"]].Switch == 1)
				{
					AnimationSelect(light[cur]);
				}
				else if (cur == CurMap["backlight"] && light[CurMap["backlight"]].Switch == 1)
				{
					AnimationSelect(light[cur]);
				}
			}
			ColorPanel.SelectValue = CombineColorByText();
		}

		private void page2Loaded(object sender, RoutedEventArgs e)
		{
			ExecLoad();
		}

		public void ExecUnLoad()
		{
			stopAllTimer();
			WriteLightAutoClose();
		}

		private void page2UnLoaded(object sender, RoutedEventArgs e)
		{
			ExecUnLoad();
		}

		private async void ComThreadFunction()
		{
			while (true)
			{
				WaitHandle.WaitAny(new WaitHandle[2] { manualResetEvent, threadResetEvent });
				await SendComData();
			}
		}

		private void StartAutoEvent()
		{
			threadResetEvent.Set();
		}

		private void StopAutoEvent()
		{
			threadResetEvent.Reset();
		}

		private void StartManualEvent()
		{
			manualResetEvent.Set();
		}

		private void StopManualEvent()
		{
			manualResetEvent.Reset();
		}

		private void Keyboard_Click(object sender, RoutedEventArgs e)
		{
			int num = cur;
			cur = CurMap[app.INIRead("system", "keyboard")];
			if (num != cur)
			{
				keyBoardView.Visibility = Visibility.Visible;
				backView_2.Visibility = Visibility.Hidden;
				width = 388;
				height = 388;
				Custom_Button.Visibility = Visibility.Hidden;
				Custom_Text.Visibility = Visibility.Hidden;
				if (cur == 0)
				{
					KeyBoardGrid.Visibility = Visibility.Visible;
					ZoneGrid.Visibility = Visibility.Hidden;
					BackGrid.Visibility = Visibility.Hidden;
				}
				else
				{
					KeyBoardGrid.Visibility = Visibility.Hidden;
					ZoneGrid.Visibility = Visibility.Visible;
					BackGrid.Visibility = Visibility.Hidden;
				}
				KeyboardPath1.Visibility = Visibility.Visible;
				KeyboardPath2.Visibility = Visibility.Visible;
				KeyboardPath3.Visibility = Visibility.Visible;
				KeyboardPath4.Visibility = Visibility.Visible;
				Custom_Board.Visibility = Visibility.Visible;
				Custom_Back.Visibility = Visibility.Hidden;
				keyboardFlag = cur / 4 + 1;
				titleFlag = 0;
				keyboard.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				back.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				Zone.Source = new BitmapImage(new Uri($"/image/{keyboardFlag}.png", UriKind.RelativeOrAbsolute));
				ColorZone.Source = colorZoneBitmap[0];
				Zone.Margin = new Thickness(-18.0, 9.0, 0.0, 0.0);
				ColorZone.Margin = new Thickness(-18.0, 9.0, 0.0, 0.0);
				Zone.Height = 410.0;
				ColorZone.Height = 410.0;
				Zone.Width = 410.0;
				ColorZone.Width = 410.0;
				SetUI(light[cur]);
			}
		}

		private void back_Click(object sender, RoutedEventArgs e)
		{
			int num = cur;
			cur = CurMap["backlight"];
			if (num != cur)
			{
				keyBoardView.Visibility = Visibility.Visible;
				backView_2.Visibility = Visibility.Hidden;
				width = 600;
				height = 40;
				titleFlag = 1;
				KeyboardPath1.Visibility = Visibility.Hidden;
				KeyboardPath2.Visibility = Visibility.Hidden;
				KeyboardPath3.Visibility = Visibility.Hidden;
				KeyboardPath4.Visibility = Visibility.Hidden;
				KeyBoardGrid.Visibility = Visibility.Hidden;
				ZoneGrid.Visibility = Visibility.Hidden;
				BackGrid.Visibility = Visibility.Visible;
				Custom_Board.Visibility = Visibility.Hidden;
				Custom_Back.Visibility = Visibility.Visible;
				Custom_Button.Visibility = Visibility.Visible;
				Custom_Text.Visibility = Visibility.Visible;
				keyboard.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				back.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				Zone.Source = new BitmapImage(new Uri("/image/back_light.png", UriKind.RelativeOrAbsolute));
				ColorZone.Source = colorZoneBitmap[1];
				Zone.Margin = new Thickness(5.0, 0.0, 0.0, 10.0);
				ColorZone.Margin = new Thickness(5.0, 0.0, 0.0, 10.0);
				Zone.Width = 600.0;
				ColorZone.Width = 600.0;
				Zone.Height = 35.0;
				ColorZone.Height = 35.0;
				SetUI(light[cur]);
			}
		}

		public static ColorHSV ConvertRgbToHsv(System.Windows.Media.Color color)
		{
			ColorHSV colorHSV = new ColorHSV();
			double num = (double)(int)color.R / 255.0;
			double num2 = (double)(int)color.G / 255.0;
			double num3 = (double)(int)color.B / 255.0;
			double num4 = num;
			double num5 = num;
			if (num2 > num5)
			{
				num5 = num2;
			}
			else if (num2 < num4)
			{
				num4 = num2;
			}
			if (num3 > num5)
			{
				num5 = num3;
			}
			else if (num3 < num4)
			{
				num4 = num3;
			}
			colorHSV.V = num5;
			double num6 = num5 - num4;
			if (colorHSV.V == 0.0)
			{
				colorHSV.S = 0.0;
			}
			else
			{
				colorHSV.S = num6 / num5;
			}
			if (colorHSV.S == 0.0)
			{
				colorHSV.H = 0.0;
			}
			else if (num5 == num)
			{
				colorHSV.H = (num2 - num3) / num6;
			}
			else if (num5 == num2)
			{
				colorHSV.H = 2.0 + (num3 - num) / num6;
			}
			else
			{
				colorHSV.H = 4.0 + (num - num2) / num6;
			}
			colorHSV.H *= 60.0;
			if (colorHSV.H < 0.0)
			{
				colorHSV.H += 360.0;
			}
			return colorHSV;
		}

		private int CombineRGBWithBrightness(int val, double brightness)
		{
			brightness /= 100.0;
			val = (int)((double)val / 255.0 * brightness * 255.0);
			return val;
		}

		private System.Windows.Media.Color lightColor(Light_Infinix light_cur)
		{
			return System.Windows.Media.Color.FromRgb((byte)CombineRGBWithBrightness(light_cur.R, (int)light_cur.L), (byte)CombineRGBWithBrightness(light_cur.G, (int)light_cur.L), (byte)CombineRGBWithBrightness(light_cur.B, (int)light_cur.L));
		}

		private System.Windows.Media.Color lightBackColor(Light_Infinix light_cur)
		{
			return System.Windows.Media.Color.FromRgb((byte)CombineRGBWithBrightness(light_cur.backR, (int)light_cur.backL), (byte)CombineRGBWithBrightness(light_cur.backG, (int)light_cur.backL), (byte)CombineRGBWithBrightness(light_cur.backB, (int)light_cur.backL));
		}

		private unsafe void ChangeKeyBoardColor(int ColorR, int ColorG, int ColorB, double brightness)
		{
			ColorR = CombineRGBWithBrightness(ColorR, brightness);
			ColorG = CombineRGBWithBrightness(ColorG, brightness);
			ColorB = CombineRGBWithBrightness(ColorB, brightness);
			colorZoneBitmap[0].Lock();
			byte* ptr = (byte*)(void*)colorZoneBitmap[0].BackBuffer;
			int backBufferStride = colorZoneBitmap[0].BackBufferStride;
			int num = (colorZoneBitmap[0].Format.BitsPerPixel + 7) / 8;
			for (int i = y1; i < y2; i++)
			{
				for (int j = x1; j < x2; j++)
				{
					int num2 = i * backBufferStride + j * num;
					ptr[num2 + 2] = (byte)ColorR;
					ptr[num2 + 1] = (byte)ColorG;
					ptr[num2] = (byte)ColorB;
					ptr[num2 + 3] = byte.MaxValue;
				}
			}
			colorZoneBitmap[0].AddDirtyRect(new Int32Rect(0, 0, width, height));
			colorZoneBitmap[0].Unlock();
		}

		private unsafe void ChangeBackColor(int ColorR, int ColorG, int ColorB, double brightness)
		{
			ColorR = CombineRGBWithBrightness(ColorR, brightness);
			ColorG = CombineRGBWithBrightness(ColorG, brightness);
			ColorB = CombineRGBWithBrightness(ColorB, brightness);
			colorZoneBitmap[1].Lock();
			byte* ptr = (byte*)(void*)colorZoneBitmap[1].BackBuffer;
			int backBufferStride = colorZoneBitmap[1].BackBufferStride;
			int num = (colorZoneBitmap[1].Format.BitsPerPixel + 7) / 8;
			for (int i = backY1; i < backY2; i++)
			{
				for (int j = backX1; j < backX2; j++)
				{
					int num2 = i * backBufferStride + j * num;
					ptr[num2 + 2] = (byte)ColorR;
					ptr[num2 + 1] = (byte)ColorG;
					ptr[num2] = (byte)ColorB;
					ptr[num2 + 3] = byte.MaxValue;
				}
			}
			colorZoneBitmap[1].AddDirtyRect(new Int32Rect(0, 0, 600, 40));
			colorZoneBitmap[1].Unlock();
		}

		private void ChangeColor(Light_Infinix light_cur)
		{
			if (cur == CurMap["backlight"])
			{
				ChangeBackColor(light_cur.R, light_cur.G, light_cur.B, (int)light_cur.L);
				ColorZone.Source = colorZoneBitmap[1];
			}
			else
			{
				ChangeKeyBoardColor(light_cur.R, light_cur.G, light_cur.B, (int)light_cur.L);
				ColorZone.Source = colorZoneBitmap[0];
			}
		}

		private void ChangeBack1RecColor(int ColorR, int ColorG, int ColorB, double brightness)
		{
			if (light[cur].Mode == 2)
			{
				back1_color1_rec.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)ColorR, (byte)ColorG, (byte)ColorB));
			}
			else if (light[cur].Mode == 4)
			{
				back1_color2_rec.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)ColorR, (byte)ColorG, (byte)ColorB));
			}
		}

		private void ChangeBack2RecColor(int ColorR, int ColorG, int ColorB, double brightness)
		{
			if (light[cur].Mode == 3)
			{
				back2_color1_rec.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)ColorR, (byte)ColorG, (byte)ColorB));
			}
			else if (light[cur].Mode == 5)
			{
				back2_color2_rec.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)ColorR, (byte)ColorG, (byte)ColorB));
			}
		}

		private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			System.Windows.Shapes.Path path = (System.Windows.Shapes.Path)sender;
			if (path.Name == "KeyboardPath1")
			{
				app.INIWrite("system", "keyboard", "keyboard1");
				keyboardFlag = 2;
				cur = 6;
				Zone.Source = new BitmapImage(new Uri($"/image/{keyboardFlag}.png", UriKind.RelativeOrAbsolute));
			}
			else if (path.Name == "KeyboardPath2")
			{
				app.INIWrite("system", "keyboard", "keyboard2");
				keyboardFlag = 3;
				cur = 10;
				Zone.Source = new BitmapImage(new Uri($"/image/{keyboardFlag}.png", UriKind.RelativeOrAbsolute));
			}
			else if (path.Name == "KeyboardPath3")
			{
				app.INIWrite("system", "keyboard", "keyboard3");
				keyboardFlag = 4;
				cur = 14;
				Zone.Source = new BitmapImage(new Uri($"/image/{keyboardFlag}.png", UriKind.RelativeOrAbsolute));
			}
			else if (path.Name == "KeyboardPath4")
			{
				app.INIWrite("system", "keyboard", "keyboard4");
				keyboardFlag = 5;
				cur = 18;
				Zone.Source = new BitmapImage(new Uri($"/image/{keyboardFlag}.png", UriKind.RelativeOrAbsolute));
			}
			KeyBoardGrid.Visibility = Visibility.Hidden;
			ZoneGrid.Visibility = Visibility.Visible;
			BackGrid.Visibility = Visibility.Hidden;
			SetUI(light[cur]);
		}

		private void Zone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed && cur < CurMap["backlight"] && cur != CurMap["keyboard"])
			{
				ShowMainKeyboard();
				SetUI(light[cur]);
			}
		}

		private void ShowMainKeyboard()
		{
			app.INIWrite("system", "keyboard", "keyboard");
			keyboardFlag = 1;
			cur = 0;
			Zone.Source = new BitmapImage(new Uri($"/image/{keyboardFlag}.png", UriKind.RelativeOrAbsolute));
			KeyBoardGrid.Visibility = Visibility.Visible;
			ZoneGrid.Visibility = Visibility.Hidden;
		}

		private void KeyBoardAlways_Checked(object sender, RoutedEventArgs e)
		{
			if (KeyBoardAlways.IsChecked == true)
			{
				light[cur].Mode = 0;
				KeyBoardAlways.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				KeyBoardAlwaysImg.Source = new BitmapImage(new Uri("/image/ao_on.png", UriKind.RelativeOrAbsolute));
				changeRGBText(light[cur]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[cur].L);
				EnableUserSelect();
			}
			else
			{
				KeyBoardAlways.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				KeyBoardAlwaysImg.Source = new BitmapImage(new Uri("/image/ao_off.png", UriKind.RelativeOrAbsolute));
			}
		}

		private void ZoneAlways_Checked(object sender, RoutedEventArgs e)
		{
			if (ZoneAlways.IsChecked == true)
			{
				light[cur].Mode = 0;
				ZoneAlways.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				ZoneAlwaysImg.Source = new BitmapImage(new Uri("/image/dark/ao_on.png", UriKind.RelativeOrAbsolute));
				changeRGBText(light[cur]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[cur].L);
				EnableUserSelect();
			}
			else
			{
				ZoneAlways.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				ZoneAlwaysImg.Source = new BitmapImage(new Uri("/image/dark/ao_off.png", UriKind.RelativeOrAbsolute));
			}
		}

		private void BackAlways_Checked(object sender, RoutedEventArgs e)
		{
			if (BackAlways.IsChecked == true)
			{
				keyBoardView.Visibility = Visibility.Visible;
				backView_2.Visibility = Visibility.Hidden;
				light[cur].Mode = 0;
				changeRGBText(light[cur]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[cur].L);
				ClearRadioButtonGroup("back1_group");
				ClearRadioButtonGroup("back2_group");
				BackAlways.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				BackAlwaysImg.Source = new BitmapImage(new Uri("/image/ao_on.png", UriKind.RelativeOrAbsolute));
				EnableUserSelect();
			}
			else
			{
				BackAlways.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				BackAlwaysImg.Source = new BitmapImage(new Uri("/image/ao_off.png", UriKind.RelativeOrAbsolute));
			}
		}

		private void KeyBoardBreath_Checked(object sender, RoutedEventArgs e)
		{
			if (KeyBoardBreath.IsChecked == true)
			{
				KeyBoardBreath.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				KeyBoardBreathImg.Source = new BitmapImage(new Uri("/image/br_on.png", UriKind.RelativeOrAbsolute));
				light[cur].Mode = 1;
				changeRGBText(light[cur + 1]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[cur + 1].L);
				EnableUserSelect();
			}
			else
			{
				KeyBoardBreath.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				KeyBoardBreathImg.Source = new BitmapImage(new Uri("/image/br_off.png", UriKind.RelativeOrAbsolute));
				StopAnimation(breathTokenSource);
			}
		}

		private void ZoneBreath_Checked(object sender, RoutedEventArgs e)
		{
			if (ZoneBreath.IsChecked == true)
			{
				light[cur].Mode = 1;
				ZoneBreath.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				ZoneBreathImg.Source = new BitmapImage(new Uri("/image/dark/br_on.png", UriKind.RelativeOrAbsolute));
				changeRGBText(light[cur + 1]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[cur + 1].L);
				EnableUserSelect();
				if (Custom_Board.IsChecked == true)
				{
					startTime = DateTime.Now;
					StartBreathAnimation();
				}
			}
			else
			{
				ZoneBreath.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				ZoneBreathImg.Source = new BitmapImage(new Uri("/image/dark/br_off.png", UriKind.RelativeOrAbsolute));
				StopAnimation(breathTokenSource);
			}
		}

		private void BackBreath_Checked(object sender, RoutedEventArgs e)
		{
			if (BackBreath.IsChecked == true)
			{
				keyBoardView.Visibility = Visibility.Visible;
				backView_2.Visibility = Visibility.Hidden;
				light[cur].Mode = 1;
				changeRGBText(light[cur + 1]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[cur + 1].L);
				EnableUserSelect();
				ClearRadioButtonGroup("back1_group");
				ClearRadioButtonGroup("back2_group");
				BackBreath.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				BackBreathImg.Source = new BitmapImage(new Uri("/image/br_on.png", UriKind.RelativeOrAbsolute));
				if (Custom_Back.IsChecked == true)
				{
					startTime = DateTime.Now;
					StartBreathAnimation();
				}
			}
			else
			{
				BackBreath.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				BackBreathImg.Source = new BitmapImage(new Uri("/image/br_off.png", UriKind.RelativeOrAbsolute));
				StopAnimation(breathTokenSource);
			}
		}

		private void KeyBoardWave_Checked(object sender, RoutedEventArgs e)
		{
			if (KeyBoardWave.IsChecked == true)
			{
				KeyBoardWave.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				KeyBoardWaveImg.Source = new BitmapImage(new Uri("/image/wa_on.png", UriKind.RelativeOrAbsolute));
				light[cur].Mode = 4;
				changeRGBText(light[cur + 4]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[cur + 4].L);
				EnableUserSelect();
			}
			else
			{
				KeyBoardWave.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				KeyBoardWaveImg.Source = new BitmapImage(new Uri("/image/wa_off.png", UriKind.RelativeOrAbsolute));
				StopAnimation(waveTokenSource);
			}
		}

		private void KeyBoardClock_Checked(object sender, RoutedEventArgs e)
		{
			if (KeyBoardClock.IsChecked == true)
			{
				KeyBoardClock.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				KeyBoardClockImg.Source = new BitmapImage(new Uri("/image/cl_on.png", UriKind.RelativeOrAbsolute));
				light[cur].Mode = 2;
				DisableUserSelect();
				changeLightBrightness(light[cur + 2].L);
			}
			else
			{
				KeyBoardClock.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				KeyBoardClockImg.Source = new BitmapImage(new Uri("/image/cl_off.png", UriKind.RelativeOrAbsolute));
				StopAnimation(clockTokenSource);
			}
		}

		private void ZoneClock_Checked(object sender, RoutedEventArgs e)
		{
			if (ZoneClock.IsChecked == true)
			{
				light[cur].Mode = 2;
				ZoneClock.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				ZoneClockImg.Source = new BitmapImage(new Uri("/image/dark/cl_on.png", UriKind.RelativeOrAbsolute));
				DisableUserSelect();
				changeLightBrightness(light[cur + 2].L);
				if (Custom_Board.IsChecked == true)
				{
					clockIndex = 0;
					StartClockAnimation();
				}
			}
			else
			{
				ZoneClock.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				ZoneClockImg.Source = new BitmapImage(new Uri("/image/dark/cl_off.png", UriKind.RelativeOrAbsolute));
				StopAnimation(clockTokenSource);
			}
		}

		private void KeyBoardRainbow_Checked(object sender, RoutedEventArgs e)
		{
			if (KeyBoardRainbow.IsChecked == true)
			{
				KeyBoardRainbow.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				KeyBoardRainbowImg.Source = new BitmapImage(new Uri("/image/rb_on.png", UriKind.RelativeOrAbsolute));
				light[cur].Mode = 3;
				DisableUserSelect();
				changeLightBrightness(light[cur + 3].L);
			}
			else
			{
				KeyBoardRainbow.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				KeyBoardRainbowImg.Source = new BitmapImage(new Uri("/image/rb_off.png", UriKind.RelativeOrAbsolute));
				StopAnimation(rainbowTokenSource);
			}
		}

		private void ZoneRainbow_Checked(object sender, RoutedEventArgs e)
		{
			if (ZoneRainbow.IsChecked == true)
			{
				light[cur].Mode = 3;
				ZoneRainbow.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				ZoneRainbowImg.Source = new BitmapImage(new Uri("/image/dark/rb_on.png", UriKind.RelativeOrAbsolute));
				DisableUserSelect();
				changeLightBrightness(light[cur + 3].L);
				if (Custom_Board.IsChecked == true)
				{
					RainbowIndex = 0;
					StartRainbowAnimation();
				}
			}
			else
			{
				ZoneRainbow.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				ZoneRainbowImg.Source = new BitmapImage(new Uri("/image/dark/rb_off.png", UriKind.RelativeOrAbsolute));
				StopAnimation(rainbowTokenSource);
			}
		}

		private void KeyBoardFlow_Checked(object sender, RoutedEventArgs e)
		{
			if (KeyBoardFlow.IsChecked == true)
			{
				KeyBoardFlow.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				KeyBoardFlowImg.Source = new BitmapImage(new Uri("/image/fl_on.png", UriKind.RelativeOrAbsolute));
				light[0].Mode = 5;
				DisableUserSelect();
				changeLightBrightness(light[cur + 5].L);
			}
			else
			{
				KeyBoardFlow.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				KeyBoardFlowImg.Source = new BitmapImage(new Uri("/image/fl_off.png", UriKind.RelativeOrAbsolute));
				StopAnimation(flowTokenSource);
			}
		}

		private void BackMusic_Checked(object sender, RoutedEventArgs e)
		{
			stopAllTimer();
			if (BackMusic.IsChecked == true)
			{
				BackMusic.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				BackMusicImg.Source = new BitmapImage(new Uri("/image/mu_on.png", UriKind.RelativeOrAbsolute));
				showMusic();
				SetSelectedBack();
				if (light[CurMap["backlight"]].Switch == 1 && light[CurMap["backlight"]].Mode == 2)
				{
					StartSendMusicData();
					StartRythmAnimation();
				}
				if (light[CurMap["backlight"]].Switch == 1 && light[CurMap["backlight"]].Mode == 3)
				{
					StartSendMusicData();
					StartJumpAnimation();
				}
			}
			else
			{
				BackMusic.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				BackMusicImg.Source = new BitmapImage(new Uri("/image/mu_off.png", UriKind.RelativeOrAbsolute));
			}
		}

		private void BackGaming_Checked(object sender, RoutedEventArgs e)
		{
			stopAllTimer();
			if (BackGaming.IsChecked == true)
			{
				BackGaming.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
				BackGameImg.Source = new BitmapImage(new Uri("/image/ga_on.png", UriKind.RelativeOrAbsolute));
				showGame();
				SetSelectedBack();
				if (light[CurMap["backlight"]].Switch == 1 && light[CurMap["backlight"]].Mode == 4)
				{
					ChangeBack1RecColor(light[gameRound].R, light[gameRound].G, light[gameRound].B, (int)light[gameRound].L);
					colorZoneBitmap[4].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, System.Windows.Media.Color.FromRgb(light[gameRound].backR, light[gameRound].backG, light[gameRound].backB));
					StartRoundAnimation();
				}
				if (light[CurMap["backlight"]].Switch == 1 && light[CurMap["backlight"]].Mode == 5)
				{
					ChangeBack2RecColor(light[gameCover].R, light[gameCover].G, light[gameCover].B, (int)light[gameCover].L);
					colorZoneBitmap[5].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, System.Windows.Media.Color.FromRgb(light[gameCover].backR, light[gameCover].backG, light[gameCover].backB));
					StartCoverAnimation();
				}
			}
			else
			{
				BackGaming.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFBFBFBF"));
				BackGameImg.Source = new BitmapImage(new Uri("/image/ga_off.png", UriKind.RelativeOrAbsolute));
			}
		}

		private void back1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (BackMusic.IsChecked == true)
			{
				if (light[cur].Mode != 2)
				{
					back1_color1.IsChecked = true;
					light[cur].Mode = 2;
				}
			}
			else if (BackGaming.IsChecked == true && light[cur].Mode != 4)
			{
				back1_color2.IsChecked = true;
				light[cur].Mode = 4;
			}
		}

		private void back2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (BackMusic.IsChecked == true)
			{
				if (light[cur].Mode != 3)
				{
					back2_color1.IsChecked = true;
					light[cur].Mode = 3;
				}
			}
			else if (BackGaming.IsChecked == true && light[cur].Mode != 5)
			{
				back2_color2.IsChecked = true;
				light[cur].Mode = 5;
			}
		}

		public void Custom_Board_Checked(object sender, RoutedEventArgs e)
		{
			isWrite = true;
			if (Custom_Board.IsChecked == true)
			{
				light[CurMap["keyboard"]].Switch = 1;
				System.Threading.Tasks.Task.Run(delegate
				{
					GlobalVars.Wmi.KeyLight(isOpen: true);
				});
			}
			else
			{
				light[CurMap["keyboard"]].Switch = 0;
				System.Threading.Tasks.Task.Run(delegate
				{
					GlobalVars.Wmi.KeyLight(isOpen: false);
				});
				if (cur < CurMap["backlight"])
				{
					stopAllTimer();
				}
			}
			isWrite = false;
		}

		public void Custom_Back_Checked(object sender, RoutedEventArgs e)
		{
			isWrite = true;
			if (Custom_Back.IsChecked == true)
			{
				light[CurMap["backlight"]].Switch = 1;
				System.Threading.Tasks.Task.Run(delegate
				{
					GlobalVars.Wmi.BackLight(isOpen: true);
				});
			}
			else
			{
				light[CurMap["backlight"]].Switch = 0;
				System.Threading.Tasks.Task.Run(delegate
				{
					GlobalVars.Wmi.BackLight(isOpen: false);
				});
				if (cur == CurMap["backlight"])
				{
					stopAllTimer();
				}
			}
			isWrite = false;
		}

		private void Custom_Button_Checked(object sender, RoutedEventArgs e)
		{
			SetUserSelectForDefaultMode();
		}

		private void SetUserSelectForDefaultMode()
		{
			if (Custom_Button.IsChecked == true)
			{
				ColorPanel.CanChoose = true;
				EnableUserSelect();
				Brightness.IsEnabled = true;
				BackGrid.IsHitTestVisible = true;
				light[CurMap["backlight"]].Mode = (byte)((!(app.INIRead("Light", "BackMode") == "no config item")) ? byte.Parse(app.INIRead("Light", "BackMode")) : 0);
				SetMode(light[CurMap["backlight"]]);
				System.Threading.Tasks.Task.Run(delegate
				{
					GlobalVars.Wmi.setLightPageStatus(isDefault: false);
				});
				return;
			}
			ColorPanel.CanChoose = false;
			DisableUserSelect();
			Brightness.IsEnabled = false;
			BackGrid.IsHitTestVisible = false;
			keyBoardView.Visibility = Visibility.Visible;
			backView_2.Visibility = Visibility.Hidden;
			ClearRadioButtonGroup("BackMode");
			ClearRadioButtonGroup("back1_group");
			ClearRadioButtonGroup("back2_group");
			ChangeBackColor(0, 0, 0, 0.0);
			if (light[CurMap["backlight"]].Mode != 6)
			{
				app.INIWrite("Light", "BackMode", ((int)light[CurMap["backlight"]].Mode).ToString());
			}
			light[CurMap["backlight"]].Mode = 6;
			System.Threading.Tasks.Task.Run(delegate
			{
				GlobalVars.Wmi.setLightPageStatus(isDefault: true);
			});
		}

		private void reset_Click(object sender, RoutedEventArgs e)
		{
			InitBackLight();
			InitBoardLight();
			SaveLightData();
			if (cur < CurMap["backlight"])
			{
				cur = CurMap["keyboard"];
				ShowMainKeyboard();
			}
			SetUI(light[cur]);
			if (cur == CurMap["backlight"])
			{
				SetBack1NoSelect();
				SetBack2NoSelect();
			}
		}

		private void ClearRadioButtonGroup(string groupName)
		{
			foreach (System.Windows.Controls.RadioButton item in from rb in FindVisualChildren<System.Windows.Controls.RadioButton>(this)
				where rb.GroupName == groupName
				select rb)
			{
				item.IsChecked = false;
			}
		}

		private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj == null)
			{
				yield break;
			}
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
				if (child != null && child is T)
				{
					yield return (T)child;
				}
				foreach (T item in FindVisualChildren<T>(child))
				{
					yield return item;
				}
			}
		}

		private void setTxbufInternal(object mode)
		{
			int mode2 = light[cur].Mode;
			byte r = light[cur + mode2].R;
			byte g = light[cur + mode2].G;
			byte b = light[cur + mode2].B;
			byte l = (byte)((double)(int)light[cur + mode2].L * 2.55);
			if (mode is TxBuf.PARAMS.Keyboard_Light)
			{
				txBufStruct = txBufClass.on_setKeyboardLED((TxBuf.PARAMS.Keyboard_Light)mode, r, g, b, l);
			}
			else if (mode is TxBuf.PARAMS.Keyboard_Light12)
			{
				txBufStruct = txBufClass.on_setKeyboard12LED((TxBuf.PARAMS.Keyboard_Light12)mode, r, g, b, l);
			}
			else if (mode is TxBuf.PARAMS.Keyboard_Light34)
			{
				txBufStruct = txBufClass.on_setKeyboard34LED((TxBuf.PARAMS.Keyboard_Light34)mode, r, g, b, l);
			}
		}

		private void WriteLightDataToUsb(object light)
		{
			setTxbufInternal(light);
			WriteUsb();
		}

		public void WriteLightNotAutoClose()
		{
			txBufStruct = txBufClass.on_setKeyboardLightState((TxBuf.PARAMS.Keyboard_Light)9);
			WriteUsb();
		}

		private void WriteLightAutoClose()
		{
			txBufStruct = txBufClass.on_setKeyboardLightState(TxBuf.PARAMS.Keyboard_Light.Light_Open);
			WriteUsb();
		}

		private void WriteUsb()
		{
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
			if (!usb.SendData2(array, out var bytes, out var error))
			{
				Console.WriteLine("writeBytes" + bytes);
				Console.WriteLine(error.ToString());
			}
		}

		private void setComData()
		{
			if (light[CurMap["backlight"]].Switch == 0)
			{
				back_light_bufStruct = back_light_buf.setBackLight(Back_light_buf.COMMAND.Light_Close, light[CurMap["backlight"]].R, light[CurMap["backlight"]].G, light[CurMap["backlight"]].B, 200, light[CurMap["backlight"]].L, null);
				return;
			}
			switch (light[CurMap["backlight"]].Mode)
			{
			case 6:
				switch (GlobalVars.SystemMode)
				{
				case "0":
					back_light_bufStruct = back_light_buf.setBackLight(Back_light_buf.COMMAND.SliceMode, light[CurMap["backlight"]].R, light[CurMap["backlight"]].G, light[CurMap["backlight"]].B, 1, light[CurMap["backlight"]].L, null);
					break;
				case "1":
					back_light_bufStruct = back_light_buf.setBackLight(Back_light_buf.COMMAND.BalanceMode, light[CurMap["backlight"]].R, light[CurMap["backlight"]].G, light[CurMap["backlight"]].B, 1, light[CurMap["backlight"]].L, null);
					break;
				case "2":
					back_light_bufStruct = back_light_buf.setBackLight(Back_light_buf.COMMAND.GameMode, light[CurMap["backlight"]].R, light[CurMap["backlight"]].G, light[CurMap["backlight"]].B, 1, light[CurMap["backlight"]].L, null);
					break;
				}
				break;
			case 0:
				back_light_bufStruct = back_light_buf.setBackLight(Back_light_buf.COMMAND.Light_AlwaysOn, light[CurMap["backlight"]].R, light[CurMap["backlight"]].G, light[CurMap["backlight"]].B, 1, light[CurMap["backlight"]].L, null);
				break;
			case 1:
				back_light_bufStruct = back_light_buf.setBackLight(Back_light_buf.COMMAND.Light_Breath, light[CurMap["backlight"] + 1].R, light[CurMap["backlight"] + 1].G, light[CurMap["backlight"] + 1].B, 1, light[CurMap["backlight"] + 1].L, null);
				break;
			case 4:
			{
				byte[] audioData2 = new byte[4]
				{
					light[gameRound].backR,
					light[gameRound].backG,
					light[gameRound].backB,
					light[gameRound].backL
				};
				back_light_bufStruct = back_light_buf.setBackLight(Back_light_buf.COMMAND.Light_Round, light[gameRound].R, light[gameRound].G, light[gameRound].B, 4, light[gameRound].L, audioData2);
				break;
			}
			case 5:
			{
				byte[] audioData = new byte[4]
				{
					light[gameCover].backR,
					light[gameCover].backG,
					light[gameCover].backB,
					light[gameCover].backL
				};
				back_light_bufStruct = back_light_buf.setBackLight(Back_light_buf.COMMAND.Light_Cover, light[gameCover].R, light[gameCover].G, light[gameCover].B, 4, light[gameCover].L, audioData);
				break;
			}
			case 2:
				back_light_bufStruct = back_light_buf.setBackLight(Back_light_buf.COMMAND.Light_Rythm, light[musicRythm].R, light[musicRythm].G, light[musicRythm].B, 1, light[musicRythm].L, capture.MaxVolumn);
				break;
			case 3:
				back_light_bufStruct = back_light_buf.setBackLight(Back_light_buf.COMMAND.Light_Jump, light[musicJump].R, light[musicJump].G, light[musicJump].B, 1, light[musicJump].L, capture.MaxVolumn);
				break;
			}
		}

		private async System.Threading.Tasks.Task SendComData()
		{
			_ = 1;
			try
			{
				isSendingData = true;
				await startComTick();
				setComData();
				serPort.sendCom(back_light_bufStruct);
				await System.Threading.Tasks.Task.Delay(50);
			}
			finally
			{
				isSendingData = false;
			}
		}

		private async System.Threading.Tasks.Task StartSendMusicData()
		{
			if (!semaphore.Wait(0))
			{
				return;
			}
			try
			{
				GlobalVars.IsMusicMode = true;
				capture.start();
				await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.2));
				StartManualEvent();
				while (light[CurMap["backlight"]].Switch == 1 && (light[CurMap["backlight"]].Mode == 2 || light[CurMap["backlight"]].Mode == 3))
				{
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.1));
				}
				capture.stop();
			}
			finally
			{
				semaphore.Release();
				GlobalVars.IsMusicMode = false;
				StopManualEvent();
			}
		}

		private void CloseLight()
		{
			if (cur == CurMap["backlight"])
			{
				StartAutoEvent();
				ChangeBackColor(0, 0, 0, 0.0);
				colorZoneBitmap[2].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
				colorZoneBitmap[3].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
				colorZoneBitmap[4].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
				colorZoneBitmap[5].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
				ColorZone.Source = colorZoneBitmap[1];
			}
			else
			{
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.LightOFF);
				ChangeKeyBoardColor(0, 0, 0, 0.0);
				ColorZone.Source = colorZoneBitmap[0];
			}
		}

		public void AnimationSelect(Light_Infinix light_cur)
		{
			stopAllTimer();
			if (light_cur.Class < CurMap["backlight"])
			{
				switch (light_cur.Mode)
				{
				case 0:
					ChangeColor(light_cur);
					writeAlwaysData(cur);
					break;
				case 1:
					startTime = DateTime.Now;
					StartBreathAnimation();
					writeBreathData(cur);
					break;
				case 4:
					waveIndex = 0;
					StartWaveAnimation();
					WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.Wave);
					break;
				case 2:
					clockIndex = 0;
					StartClockAnimation();
					writeClockData(cur);
					break;
				case 3:
					RainbowIndex = 0;
					StartRainbowAnimation();
					writeRainbowData(cur);
					break;
				case 5:
					FlowIndex = 0;
					WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.Flow);
					StartFlowAnimation();
					break;
				}
				return;
			}
			switch (light_cur.Mode)
			{
			case 0:
				ChangeColor(light_cur);
				writeAlwaysData(cur);
				break;
			case 1:
				startTime = DateTime.Now;
				StartBreathAnimation();
				writeBreathData(cur);
				break;
			case 6:
				StartAutoEvent();
				break;
			case 2:
				SetSelectedBack();
				if (light[CurMap["backlight"]].Switch == 1)
				{
					ChangeBack1RecColor(light[musicRythm].R, light[musicRythm].G, light[musicRythm].B, (int)light[musicRythm].L);
					StartSendMusicData();
					if (BackMusic.IsChecked == true)
					{
						StartRythmAnimation();
					}
				}
				break;
			case 3:
				SetSelectedBack();
				if (light[CurMap["backlight"]].Switch == 1)
				{
					ChangeBack2RecColor(light[musicJump].R, light[musicJump].G, light[musicJump].B, (int)light[musicJump].L);
					StartSendMusicData();
					if (BackMusic.IsChecked == true)
					{
						StartJumpAnimation();
					}
				}
				break;
			case 4:
				SetSelectedBack();
				if (light[CurMap["backlight"]].Switch == 1)
				{
					ChangeBack1RecColor(light[gameRound].R, light[gameRound].G, light[gameRound].B, (int)light[gameRound].L);
					colorZoneBitmap[4].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, System.Windows.Media.Color.FromRgb(light[gameRound].backR, light[gameRound].backG, light[gameRound].backB));
					StartAutoEvent();
					if (BackGaming.IsChecked == true)
					{
						StartRoundAnimation();
					}
				}
				break;
			case 5:
				SetSelectedBack();
				if (light[CurMap["backlight"]].Switch == 1)
				{
					ChangeBack2RecColor(light[gameCover].R, light[gameCover].G, light[gameCover].B, (int)light[gameCover].L);
					colorZoneBitmap[5].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, System.Windows.Media.Color.FromRgb(light[gameCover].backR, light[gameCover].backG, light[gameCover].backB));
					StartAutoEvent();
					if (BackGaming.IsChecked == true)
					{
						StartCoverAnimation();
					}
				}
				break;
			}
		}

		private void DisableUserSelect()
		{
			PanelGrid.IsHitTestVisible = false;
			PanelGrid.Opacity = 0.38;
			ColorStackPanel.IsEnabled = false;
			ColorTextGrid.IsHitTestVisible = false;
		}

		private void EnableUserSelect()
		{
			PanelGrid.IsHitTestVisible = true;
			PanelGrid.Opacity = 1.0;
			ColorStackPanel.IsEnabled = true;
			ColorTextGrid.IsHitTestVisible = true;
		}

		public async System.Threading.Tasks.Task StartBreathAnimation()
		{
			if (breathTokenSource != null)
			{
				breathTokenSource.Cancel();
				await System.Threading.Tasks.Task.Delay(100);
			}
			breathTokenSource = new CancellationTokenSource();
			breathToken = breathTokenSource.Token;
			try
			{
				while (!breathToken.IsCancellationRequested)
				{
					double brightness = (Math.Cos((DateTime.Now - startTime).TotalSeconds) + 1.0) / 2.0 * (double)(int)light[cur + 1].L;
					if (titleFlag == 0)
					{
						ChangeKeyBoardColor(light[cur + 1].R, light[cur + 1].G, light[cur + 1].B, brightness);
						ColorZone.Source = colorZoneBitmap[0];
					}
					else if (titleFlag == 1)
					{
						ChangeBackColor(light[cur + 1].R, light[cur + 1].G, light[cur + 1].B, brightness);
						ColorZone.Source = colorZoneBitmap[1];
					}
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromMilliseconds(50.0), breathToken);
				}
			}
			finally
			{
				breathTokenSource = null;
			}
		}

		private unsafe void WaveKeyBoardColor(int index)
		{
			int num = CombineRGBWithBrightness(light[cur + 4].R, (int)light[cur + 4].L);
			int num2 = CombineRGBWithBrightness(light[cur + 4].G, (int)light[cur + 4].L);
			int num3 = CombineRGBWithBrightness(light[cur + 4].B, (int)light[cur + 4].L);
			int num4 = keyBoardWidth / 7;
			colorZoneBitmap[0].Lock();
			byte* ptr = (byte*)(void*)colorZoneBitmap[0].BackBuffer;
			int backBufferStride = colorZoneBitmap[0].BackBufferStride;
			int num5 = (colorZoneBitmap[0].Format.BitsPerPixel + 7) / 8;
			_ = keyBoardHeight;
			if (waveIndex < 7)
			{
				for (int i = y1; i < y2; i++)
				{
					for (int j = x1 + waveIndex * num4; j < x1 + waveIndex * num4 + num4; j++)
					{
						int num6 = i * backBufferStride + j * num5;
						ptr[num6 + 2] = (byte)num;
						ptr[num6 + 1] = (byte)num2;
						ptr[num6] = (byte)num3;
						ptr[num6 + 3] = byte.MaxValue;
					}
				}
			}
			else
			{
				int num7 = waveIndex - 7;
				for (int k = y1; k < y2; k++)
				{
					for (int l = x1 + num7 * num4; l < x1 + num7 * num4 + num4; l++)
					{
						int num8 = k * backBufferStride + l * num5;
						ptr[num8 + 2] = 0;
						ptr[num8 + 1] = 0;
						ptr[num8] = 0;
						ptr[num8 + 3] = 0;
					}
				}
			}
			colorZoneBitmap[0].AddDirtyRect(new Int32Rect(0, 0, width, height));
			colorZoneBitmap[0].Unlock();
		}

		public async System.Threading.Tasks.Task StartWaveAnimation()
		{
			if (waveTokenSource != null)
			{
				waveTokenSource.Cancel();
				await System.Threading.Tasks.Task.Delay(150);
			}
			waveTokenSource = new CancellationTokenSource();
			waveToken = waveTokenSource.Token;
			try
			{
				while (!waveToken.IsCancellationRequested)
				{
					if (waveIndex == 14)
					{
						waveIndex = 0;
					}
					WaveKeyBoardColor(waveIndex);
					waveIndex++;
					ColorZone.Source = colorZoneBitmap[0];
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.1), waveToken);
				}
			}
			finally
			{
				waveTokenSource.Cancel();
			}
		}

		public async System.Threading.Tasks.Task StartClockAnimation()
		{
			if (clockTokenSource != null)
			{
				clockTokenSource.Cancel();
				await System.Threading.Tasks.Task.Delay(150);
			}
			clockTokenSource = new CancellationTokenSource();
			clockToken = clockTokenSource.Token;
			try
			{
				while (!clockToken.IsCancellationRequested)
				{
					int num = 20;
					int num2 = keyboardColor.R;
					int num3 = keyboardColor.G;
					int num4 = keyboardColor.B;
					switch (clockIndex)
					{
					case 0:
						num3 += num;
						if (num3 >= 255)
						{
							num3 = 255;
							clockIndex++;
						}
						break;
					case 1:
						num2 -= num;
						if (num2 <= 0)
						{
							num2 = 0;
							clockIndex++;
						}
						break;
					case 2:
						num4 += num;
						if (num4 >= 255)
						{
							num4 = 255;
							clockIndex++;
						}
						break;
					case 3:
						num3 -= num;
						if (num3 <= 0)
						{
							num3 = 0;
							clockIndex++;
						}
						break;
					case 4:
						num2 += num;
						num4 -= num;
						if (num2 >= 255)
						{
							num2 = 255;
						}
						if (num4 <= 0)
						{
							num4 = 0;
						}
						if (num2 == 255 && num4 == 0)
						{
							clockIndex = 0;
						}
						break;
					}
					keyboardColor = System.Windows.Media.Color.FromArgb(byte.MaxValue, (byte)num2, (byte)num3, (byte)num4);
					if (titleFlag == 0)
					{
						ChangeKeyBoardColor(num2, num3, num4, (int)light[cur + 2].L);
						ColorZone.Source = colorZoneBitmap[0];
					}
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.1), clockToken);
				}
			}
			finally
			{
				clockTokenSource.Cancel();
			}
		}

		public async System.Threading.Tasks.Task StartRainbowAnimation()
		{
			if (rainbowTokenSource != null)
			{
				rainbowTokenSource.Cancel();
				await System.Threading.Tasks.Task.Delay(1150);
			}
			rainbowTokenSource = new CancellationTokenSource();
			rainbowToken = rainbowTokenSource.Token;
			try
			{
				while (!rainbowToken.IsCancellationRequested)
				{
					System.Windows.Media.Color color = rainbowColors[RainbowIndex];
					RainbowIndex++;
					if (RainbowIndex >= rainbowColors.Length)
					{
						RainbowIndex = 0;
					}
					if (titleFlag == 0)
					{
						ChangeKeyBoardColor(color.R, color.G, color.B, (int)light[cur + 3].L);
						ColorZone.Source = colorZoneBitmap[0];
					}
					else if (titleFlag == 1)
					{
						ChangeBackColor(color.R, color.G, color.B, (int)light[cur + 3].L);
						ColorZone.Source = colorZoneBitmap[1];
					}
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.0), rainbowToken);
				}
			}
			finally
			{
				rainbowTokenSource.Cancel();
			}
		}

		private void DrawColorBlock(WriteableBitmap bitmap, int x, int y, int width, int height, System.Windows.Media.Color color)
		{
			int[] array = new int[width * height];
			int num = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = num;
			}
			Int32Rect sourceRect = new Int32Rect(x, y, width, height);
			bitmap.WritePixels(sourceRect, array, width * 4, 0);
		}

		private void FlowKeyBoardColor()
		{
			int num = keyBoardWidth / 4;
			int num2 = num;
			int num3 = keyBoardHeight;
			_ = new int[num2 * num3 * 4];
			for (int i = 0; i < 4; i++)
			{
				DrawColorBlock(colorZoneBitmap[0], x1 + i * num, y1, num2, num3, System.Windows.Media.Color.FromRgb((byte)CombineRGBWithBrightness(flowColors[(i + 21 - FlowIndex) % 21].R, (int)light[cur + 5].L), (byte)CombineRGBWithBrightness(flowColors[(i + 21 - FlowIndex) % 21].G, (int)light[cur + 5].L), (byte)CombineRGBWithBrightness(flowColors[(i + 21 - FlowIndex) % 21].B, (int)light[cur + 5].L)));
			}
		}

		private async System.Threading.Tasks.Task StartFlowAnimation()
		{
			if (flowTokenSource != null)
			{
				flowTokenSource.Cancel();
				await System.Threading.Tasks.Task.Delay(200);
			}
			flowTokenSource = new CancellationTokenSource();
			flowToken = flowTokenSource.Token;
			try
			{
				while (!flowToken.IsCancellationRequested)
				{
					FlowKeyBoardColor();
					ColorZone.Source = colorZoneBitmap[0];
					FlowIndex++;
					if (FlowIndex == 21)
					{
						FlowIndex = 0;
					}
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.15), flowToken);
				}
			}
			finally
			{
				flowTokenSource.Cancel();
			}
		}

		private void showMusic()
		{
			keyBoardView.Visibility = Visibility.Hidden;
			backView_2.Visibility = Visibility.Visible;
			back1_text.Text = (string)System.Windows.Application.Current.FindResource("Rhythm");
			back2_text.Text = (string)System.Windows.Application.Current.FindResource("Beat");
			music1.Visibility = Visibility.Visible;
			music2.Visibility = Visibility.Visible;
			game1.Visibility = Visibility.Hidden;
			game2.Visibility = Visibility.Hidden;
		}

		private void showGame()
		{
			back1_text.Text = (string)System.Windows.Application.Current.FindResource("Loop");
			back2_text.Text = (string)System.Windows.Application.Current.FindResource("Cover");
			keyBoardView.Visibility = Visibility.Hidden;
			backView_2.Visibility = Visibility.Visible;
			music1.Visibility = Visibility.Hidden;
			music2.Visibility = Visibility.Hidden;
			game1.Visibility = Visibility.Visible;
			game2.Visibility = Visibility.Visible;
		}

		private async System.Threading.Tasks.Task StartRythmAnimation()
		{
			if (musicRythmTokenSource != null)
			{
				musicRythmTokenSource.Cancel();
				await System.Threading.Tasks.Task.Delay(150);
			}
			colorZoneBitmap[2].Clear();
			colorZoneBitmap[4].Clear();
			musicRythmTokenSource = new CancellationTokenSource();
			rythmToken = musicRythmTokenSource.Token;
			double position = (back2X2 + back2X1) / 2;
			double len = (position - (double)backX1) / 16.0;
			double originLeft = position;
			double originRight = position;
			try
			{
				while (!rythmToken.IsCancellationRequested)
				{
					double num = (int)capture.MaxVolumn[0];
					double num2 = position - num * len;
					double num3 = position + num * len;
					double left = originLeft;
					double right = originRight;
					double leftStep = (num2 - originLeft) / 10.0;
					double rightStep = (num3 - originRight) / 10.0;
					originLeft = num2;
					originRight = num3;
					for (int i = 0; i < 10; i++)
					{
						if (rythmToken.IsCancellationRequested)
						{
							break;
						}
						left += leftStep;
						right += rightStep;
						left = ((left < (double)back2X1) ? ((double)back2X1) : left);
						left = ((left > position) ? position : left);
						right = ((right < position) ? position : right);
						right = ((right > (double)back2X2) ? ((double)back2X2) : right);
						colorZoneBitmap[2].Clear();
						colorZoneBitmap[2].FillRectangle((int)left, back2Y1, (int)right, back2Y2, lightColor(light[musicRythm]));
						await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.01), rythmToken);
					}
				}
			}
			finally
			{
				musicRythmTokenSource.Cancel();
			}
		}

		private async System.Threading.Tasks.Task StartJumpAnimation()
		{
			if (musicJumpTokenSource != null)
			{
				musicJumpTokenSource.Cancel();
				await System.Threading.Tasks.Task.Delay(150);
			}
			_ = DateTime.Now;
			Random random = new Random();
			HashSet<int> uniqueNumbers = new HashSet<int>();
			colorZoneBitmap[3].Clear();
			colorZoneBitmap[5].Clear();
			musicJumpTokenSource = new CancellationTokenSource();
			jumpToken = musicJumpTokenSource.Token;
			int num = 8;
			int len = (back2X2 - 15 * num - back2X1) / 16;
			int[] position = new int[16];
			for (int i = 0; i < position.Length; i++)
			{
				position[i] = (num + len) * i + 2;
			}
			try
			{
				while (!jumpToken.IsCancellationRequested)
				{
					colorZoneBitmap[3].Clear();
					int num2 = capture.MaxVolumn[3] / 2;
					if (num2 > 10)
					{
						num2 /= 8;
					}
					uniqueNumbers.Clear();
					while (uniqueNumbers.Count < num2)
					{
						int item = random.Next(0, 17);
						if (!uniqueNumbers.Contains(item))
						{
							uniqueNumbers.Add(item);
							Console.WriteLine("Random Number: " + item);
						}
					}
					for (int j = 0; j < position.Length; j++)
					{
						if (uniqueNumbers.Contains(j))
						{
							colorZoneBitmap[3].FillRectangle(back2X1 + position[j], back2Y1, back2X1 + position[j] + len, back2Y2, lightColor(light[musicJump]));
						}
						else
						{
							colorZoneBitmap[3].FillRectangle(back2X1 + position[j], back2Y1, back2X1 + position[j] + len, back2Y2, Colors.Transparent);
						}
					}
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.08), jumpToken);
				}
			}
			finally
			{
				musicJumpTokenSource.Cancel();
			}
		}

		private async System.Threading.Tasks.Task StartRoundAnimation()
		{
			if (gameRoundTokenSource != null)
			{
				gameRoundTokenSource.Cancel();
				await System.Threading.Tasks.Task.Delay(100);
			}
			colorZoneBitmap[2].Clear();
			colorZoneBitmap[4].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, lightBackColor(light[gameRound]));
			gameRoundTokenSource = new CancellationTokenSource();
			roundToken = gameRoundTokenSource.Token;
			bool right = true;
			int position = back2X1;
			int step = 5;
			int width = (back2X2 - back2X1) / 6;
			try
			{
				while (!roundToken.IsCancellationRequested)
				{
					if (right)
					{
						position += step;
						if (position + width > back2X2)
						{
							right = false;
						}
					}
					else
					{
						position -= step;
						if (position <= back2X1)
						{
							right = true;
						}
					}
					colorZoneBitmap[2].Clear();
					colorZoneBitmap[2].FillRectangle(position, back2Y1, position + width, back2Y2, lightColor(light[gameRound]));
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.005), roundToken);
				}
			}
			finally
			{
				gameRoundTokenSource.Cancel();
			}
		}

		private async System.Threading.Tasks.Task StartCoverAnimation()
		{
			if (gameCoverTokenSource != null)
			{
				gameCoverTokenSource.Cancel();
				await System.Threading.Tasks.Task.Delay(100);
			}
			colorZoneBitmap[3].Clear();
			colorZoneBitmap[5].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, lightBackColor(light[gameCover]));
			gameCoverTokenSource = new CancellationTokenSource();
			coverToken = gameCoverTokenSource.Token;
			bool right = true;
			int position = backX1;
			int step = 5;
			try
			{
				while (!coverToken.IsCancellationRequested)
				{
					if (right)
					{
						position += step;
						if (position > back2X2)
						{
							position = back2X2;
							right = false;
						}
					}
					else
					{
						position -= step;
						if (position <= back2X1)
						{
							position = back2X1;
							right = true;
						}
					}
					width = position - backX1;
					colorZoneBitmap[3].Clear();
					colorZoneBitmap[3].FillRectangle(back2X1, back2Y1, back2X1 + position, back2Y2, lightColor(light[gameCover]));
					await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.01), coverToken);
				}
			}
			finally
			{
				gameCoverTokenSource.Cancel();
			}
		}

		private void StopAnimation(CancellationTokenSource source)
		{
			source?.Cancel();
		}

		private void stopAllTimer()
		{
			StopAnimation(breathTokenSource);
			StopAnimation(waveTokenSource);
			StopAnimation(clockTokenSource);
			StopAnimation(rainbowTokenSource);
			StopAnimation(flowTokenSource);
			StopAnimation(musicRythmTokenSource);
			StopAnimation(musicJumpTokenSource);
			StopAnimation(gameRoundTokenSource);
			StopAnimation(gameCoverTokenSource);
		}

		private void SetSelectedBack()
		{
			if (BackMusic.IsChecked == true && light[cur].Mode == 2)
			{
				SetBack1Select();
				SetBack2NoSelect();
			}
			else if (BackMusic.IsChecked == true && light[cur].Mode == 3)
			{
				SetBack1NoSelect();
				SetBack2Select();
			}
			else if (BackGaming.IsChecked == true && light[cur].Mode == 4)
			{
				SetBack1Select();
				SetBack2NoSelect();
			}
			else if (BackGaming.IsChecked == true && light[cur].Mode == 5)
			{
				SetBack1NoSelect();
				SetBack2Select();
			}
			else
			{
				SetBack1NoSelect();
				SetBack2NoSelect();
			}
		}

		private void SetBack1NoSelect()
		{
			if (BackMusic.IsChecked == true)
			{
				SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Gray);
				border1.BorderBrush = solidColorBrush;
				back1_text.Foreground = solidColorBrush;
				colorZoneBitmap[2].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
				back1_color1_text.Foreground = solidColorBrush;
				back1_color1_border.BorderBrush = solidColorBrush;
				back1_color1_rec.Fill = solidColorBrush;
			}
			else if (BackGaming.IsChecked == true)
			{
				SolidColorBrush solidColorBrush2 = new SolidColorBrush(Colors.Gray);
				border1.BorderBrush = solidColorBrush2;
				back1_text.Foreground = solidColorBrush2;
				colorZoneBitmap[2].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
				back1_color2_text.Foreground = solidColorBrush2;
				back1_color2_border.BorderBrush = solidColorBrush2;
				back1_color2_rec.Fill = solidColorBrush2;
				back1_color3_text.Foreground = solidColorBrush2;
				back1_color3_border.BorderBrush = solidColorBrush2;
				back1_color3_rec.Fill = solidColorBrush2;
			}
		}

		private void SetBack1Select()
		{
			stopAllTimer();
			EnableUserSelect();
			if (BackMusic.IsChecked == true)
			{
				SetBack2NoSelect();
				changeRGBText(light[musicRythm]);
				changeLightBrightness(light[musicRythm].L);
				back1_color1.IsChecked = true;
				light[cur].Mode = 2;
				SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.DeepSkyBlue);
				SolidColorBrush fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(light[musicRythm].R, light[musicRythm].G, light[musicRythm].B));
				colorZoneBitmap[4].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Transparent);
				border1.BorderBrush = solidColorBrush;
				back1_text.Foreground = solidColorBrush;
				back1_color1_border.BorderBrush = solidColorBrush;
				back1_color1_rec.Fill = fill;
				back1_color1_text.Foreground = solidColorBrush;
			}
			else if (BackGaming.IsChecked == true)
			{
				SetBack2NoSelect();
				changeRGBText(light[gameRound]);
				if (back1_color3.IsChecked != true)
				{
					back1_color2.IsChecked = true;
				}
				light[cur].Mode = 4;
				SolidColorBrush solidColorBrush2 = new SolidColorBrush(Colors.DeepSkyBlue);
				System.Windows.Media.Color color = System.Windows.Media.Color.FromRgb(light[gameRound].R, light[gameRound].G, light[gameRound].B);
				System.Windows.Media.Color color2 = System.Windows.Media.Color.FromRgb(light[gameRound].backR, light[gameRound].backG, light[gameRound].backB);
				SolidColorBrush fill2 = new SolidColorBrush(color);
				SolidColorBrush fill3 = new SolidColorBrush(color2);
				border1.BorderBrush = solidColorBrush2;
				back1_text.Foreground = solidColorBrush2;
				back1_color2_text.Foreground = solidColorBrush2;
				if (back1_color2.IsChecked == true)
				{
					back1_color2_border.BorderBrush = new SolidColorBrush(Colors.Blue);
				}
				else
				{
					back1_color2_border.BorderBrush = solidColorBrush2;
				}
				back1_color2_rec.Fill = fill2;
				back1_color3_text.Foreground = solidColorBrush2;
				if (back1_color3.IsChecked == true)
				{
					back1_color3_border.BorderBrush = new SolidColorBrush(Colors.Blue);
				}
				else
				{
					back1_color3_border.BorderBrush = solidColorBrush2;
				}
				back1_color3_rec.Fill = fill3;
			}
		}

		private void SetBack2NoSelect()
		{
			if (BackMusic.IsChecked == true)
			{
				SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Gray);
				border2.BorderBrush = solidColorBrush;
				back2_text.Foreground = solidColorBrush;
				ChangeBack2RecColor(128, 128, 128, 100.0);
				colorZoneBitmap[3].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
				back2_color1_text.Foreground = solidColorBrush;
				back2_color1_border.BorderBrush = solidColorBrush;
				back2_color1_rec.Fill = solidColorBrush;
			}
			else if (BackGaming.IsChecked == true)
			{
				SolidColorBrush solidColorBrush2 = new SolidColorBrush(Colors.Gray);
				border2.BorderBrush = solidColorBrush2;
				back2_text.Foreground = solidColorBrush2;
				ChangeBack2RecColor(128, 128, 128, 100.0);
				colorZoneBitmap[3].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
				back2_color2_text.Foreground = solidColorBrush2;
				back2_color2_border.BorderBrush = solidColorBrush2;
				back2_color2_rec.Fill = solidColorBrush2;
				back2_color3_text.Foreground = solidColorBrush2;
				back2_color3_border.BorderBrush = solidColorBrush2;
				back2_color3_rec.Fill = solidColorBrush2;
			}
		}

		private void SetBack2Select()
		{
			stopAllTimer();
			EnableUserSelect();
			if (BackMusic.IsChecked == true)
			{
				SetBack1NoSelect();
				changeRGBText(light[musicJump]);
				changeLightBrightness(light[musicJump].L);
				back2_color1.IsChecked = true;
				light[cur].Mode = 3;
				SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.DeepSkyBlue);
				SolidColorBrush solidColorBrush2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(light[musicJump].R, light[musicJump].G, light[musicJump].B));
				colorZoneBitmap[5].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Transparent);
				border2.BorderBrush = solidColorBrush;
				back2_text.Foreground = solidColorBrush;
				back2_color1.Background = solidColorBrush2;
				back2_color1_text.Foreground = solidColorBrush;
				back2_color1_border.BorderBrush = solidColorBrush;
				back2_color1_rec.Fill = solidColorBrush2;
			}
			else if (BackGaming.IsChecked == true)
			{
				SetBack1NoSelect();
				changeRGBText(light[gameCover]);
				if (back2_color3.IsChecked != true)
				{
					back2_color2.IsChecked = true;
				}
				light[cur].Mode = 5;
				System.Windows.Media.Color color = System.Windows.Media.Color.FromRgb(light[gameCover].R, light[gameCover].G, light[gameCover].B);
				SolidColorBrush solidColorBrush3 = new SolidColorBrush(Colors.DeepSkyBlue);
				SolidColorBrush fill = new SolidColorBrush(color);
				SolidColorBrush fill2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(light[gameCover].backR, light[gameCover].backG, light[gameCover].backB));
				border2.BorderBrush = solidColorBrush3;
				back2_text.Foreground = solidColorBrush3;
				back2_color2_text.Foreground = solidColorBrush3;
				if (back2_color2.IsChecked == true)
				{
					back2_color2_border.BorderBrush = new SolidColorBrush(Colors.Blue);
				}
				else
				{
					back2_color2_border.BorderBrush = solidColorBrush3;
				}
				back2_color2_rec.Fill = fill;
				back2_color3_text.Foreground = solidColorBrush3;
				if (back2_color3.IsChecked == true)
				{
					back2_color3_border.BorderBrush = new SolidColorBrush(Colors.Blue);
				}
				else
				{
					back2_color3_border.BorderBrush = solidColorBrush3;
				}
				back2_color3_rec.Fill = fill2;
			}
		}

		private void back1_color1_Checked(object sender, RoutedEventArgs e)
		{
			if (back1_color1.IsChecked == true)
			{
				if (light[cur].Mode != 2)
				{
					light[cur].Mode = 2;
				}
				changeRGBText(light[musicRythm]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[musicRythm].L);
			}
		}

		private void back1_color2_Checked(object sender, RoutedEventArgs e)
		{
			if (back1_color2.IsChecked == true)
			{
				if (light[cur].Mode != 4)
				{
					light[cur].Mode = 4;
				}
				changeRGBText(light[gameRound]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[gameRound].L);
				back1_color2_border.BorderBrush = new SolidColorBrush(Colors.Blue);
			}
			else
			{
				back1_color2_border.BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
			}
		}

		private void back1_color3_Checked(object sender, RoutedEventArgs e)
		{
			if (back1_color3.IsChecked == true)
			{
				if (light[cur].Mode != 4)
				{
					light[cur].Mode = 4;
				}
				changeRGBText(light[gameRound]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[gameRound].backL);
				back1_color3_border.BorderBrush = new SolidColorBrush(Colors.Blue);
			}
			else
			{
				back1_color3_border.BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
			}
		}

		private void back2_color1_Checked(object sender, RoutedEventArgs e)
		{
			if (back2_color1.IsChecked == true)
			{
				if (light[cur].Mode != 3)
				{
					light[cur].Mode = 3;
				}
				changeRGBText(light[musicJump]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[musicJump].L);
			}
		}

		private void back2_color2_Checked(object sender, RoutedEventArgs e)
		{
			if (back2_color2.IsChecked == true)
			{
				if (light[cur].Mode != 5)
				{
					light[cur].Mode = 5;
				}
				changeRGBText(light[gameCover]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[gameCover].L);
				back2_color2_border.BorderBrush = new SolidColorBrush(Colors.Blue);
			}
			else
			{
				back2_color2_border.BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
			}
		}

		private void back2_color3_Checked(object sender, RoutedEventArgs e)
		{
			if (back2_color3.IsChecked == true)
			{
				if (light[cur].Mode != 5)
				{
					light[cur].Mode = 5;
				}
				changeRGBText(light[gameCover]);
				ColorPanel.SelectValue = CombineColorByText();
				changeLightBrightness(light[gameCover].backL);
				back2_color3_border.BorderBrush = new SolidColorBrush(Colors.Blue);
			}
			else
			{
				back2_color3_border.BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
			}
		}

		private void TextBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void R_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			isTextKeyDown = true;
		}

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				if (isTextKeyDown)
				{
					SaveLightData();
					isTextKeyDown = false;
				}
				System.Windows.Controls.TextBox textBox = (System.Windows.Controls.TextBox)sender;
				string text = textBox.Text;
				if ("" == text || text == null)
				{
					return;
				}
				int num = int.Parse(textBox.Text);
				if (num > 255)
				{
					num = 255;
				}
				textBox.Text = num.ToString();
				textBox.Select(textBox.Text.Length, 0);
				if (BackGaming.IsChecked == true && cur == CurMap["backlight"] && (light[CurMap["backlight"]].Mode == 4 || light[CurMap["backlight"]].Mode == 5))
				{
					if (back1_color3.IsChecked == true)
					{
						ChangeLightBack(textBox, gameRound, num);
					}
					else if (back1_color2.IsChecked == true)
					{
						ChangeLight(textBox, gameRound, num);
					}
					else if (back2_color2.IsChecked == true)
					{
						ChangeLight(textBox, gameCover, num);
					}
					else if (back2_color3.IsChecked == true)
					{
						ChangeLightBack(textBox, gameCover, num);
					}
				}
				else if (BackMusic.IsChecked == true && cur == CurMap["backlight"] && (light[CurMap["backlight"]].Mode == 3 || light[CurMap["backlight"]].Mode == 2))
				{
					if (back1_color1.IsChecked == true)
					{
						ChangeLight(textBox, musicRythm, num);
					}
					else if (back2_color1.IsChecked == true)
					{
						ChangeLight(textBox, musicJump, num);
					}
				}
				else
				{
					ChangeLight(textBox, cur, num);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		private void ChangeLight(System.Windows.Controls.TextBox tb, int index, int num)
		{
			int mode = light[index].Mode;
			if (tb.Name == "R")
			{
				light[index + mode].R = Convert.ToByte(num);
			}
			if (tb.Name == "G")
			{
				light[index + mode].G = Convert.ToByte(num);
			}
			if (tb.Name == "B")
			{
				light[index + mode].B = Convert.ToByte(num);
			}
			if (!brushChange)
			{
				ColorPanel.SelectValue = CombineColorByText();
			}
		}

		private void ChangeLightBack(System.Windows.Controls.TextBox tb, int index, int num)
		{
			if (tb.Name == "R")
			{
				light[index].backR = Convert.ToByte(num);
			}
			if (tb.Name == "G")
			{
				light[index].backG = Convert.ToByte(num);
			}
			if (tb.Name == "B")
			{
				light[index].backB = Convert.ToByte(num);
			}
			if (!brushChange)
			{
				ColorPanel.SelectValue = CombineColorByText();
			}
		}

		private string CombineColorByText()
		{
			StringBuilder stringBuilder = new StringBuilder("#FF");
			stringBuilder.Append(int.Parse(R.Text).ToString("X2"));
			stringBuilder.Append(int.Parse(G.Text).ToString("X2"));
			stringBuilder.Append(int.Parse(B.Text).ToString("X2"));
			return stringBuilder.ToString();
		}

		public void ModeChanged(Light_Infinix light_cur)
		{
			System.Windows.Application.Current.Dispatcher.Invoke(delegate
			{
				SaveLightData();
				if ((cur >= CurMap["backlight"] || light[CurMap["keyboard"]].Switch != 0) && (light_cur.Switch != 0 || BackMusic.IsChecked != false || BackGaming.IsChecked != false))
				{
					AnimationSelect(light_cur);
				}
			});
		}

		private void SetMode(Light_Infinix light_cur)
		{
			if (cur < CurMap["backlight"])
			{
				light_cur = light[cur];
			}
			if (light_cur.Class < CurMap["backlight"])
			{
				switch (light_cur.Mode)
				{
				case 0:
					if (light_cur.Class == 0)
					{
						KeyBoardAlways.IsChecked = true;
					}
					else
					{
						ZoneAlways.IsChecked = true;
					}
					break;
				case 1:
					if (light_cur.Class == 0)
					{
						KeyBoardBreath.IsChecked = true;
					}
					else
					{
						ZoneBreath.IsChecked = true;
					}
					break;
				case 4:
					KeyBoardWave.IsChecked = true;
					break;
				case 2:
					if (light_cur.Class == 0)
					{
						KeyBoardClock.IsChecked = true;
					}
					else
					{
						ZoneClock.IsChecked = true;
					}
					break;
				case 3:
					if (light_cur.Class == 0)
					{
						KeyBoardRainbow.IsChecked = true;
					}
					else
					{
						ZoneRainbow.IsChecked = true;
					}
					break;
				case 5:
					KeyBoardFlow.IsChecked = true;
					break;
				}
				return;
			}
			switch (light_cur.Mode)
			{
			case 0:
				BackAlways.IsChecked = true;
				break;
			case 1:
				BackBreath.IsChecked = true;
				break;
			case 2:
				BackMusic.IsChecked = true;
				showMusic();
				break;
			case 3:
				BackMusic.IsChecked = true;
				showMusic();
				break;
			case 4:
				BackGaming.IsChecked = true;
				showGame();
				back1_color2_border.BorderBrush = new SolidColorBrush(Colors.Blue);
				break;
			case 5:
				BackGaming.IsChecked = true;
				showGame();
				back2_color2_border.BorderBrush = new SolidColorBrush(Colors.Blue);
				break;
			case 6:
				BackAlways.IsChecked = false;
				BackBreath.IsChecked = false;
				ChangeBackColor(0, 0, 0, 0.0);
				SetBack1NoSelect();
				SetBack2NoSelect();
				BackMusic.IsChecked = false;
				BackGaming.IsChecked = false;
				keyBoardView.Visibility = Visibility.Visible;
				backView_2.Visibility = Visibility.Hidden;
				ColorPanel.CanChoose = false;
				DisableUserSelect();
				BackGrid.IsHitTestVisible = false;
				Brightness.IsEnabled = false;
				break;
			}
		}

		private void changeRGBText(Light_Infinix light_cur)
		{
			R.TextChanged -= TextBox_TextChanged;
			G.TextChanged -= TextBox_TextChanged;
			B.TextChanged -= TextBox_TextChanged;
			if (BackGaming.IsChecked == true && (light_cur.Class == gameRound || light_cur.Class == gameCover))
			{
				if (back1_color3.IsChecked == true || back2_color3.IsChecked == true)
				{
					R.Text = light_cur.backR.ToString();
					G.Text = light_cur.backG.ToString();
					B.Text = light_cur.backB.ToString();
				}
				else
				{
					R.Text = light_cur.R.ToString();
					G.Text = light_cur.G.ToString();
					B.Text = light_cur.B.ToString();
				}
			}
			else
			{
				R.Text = light_cur.R.ToString();
				G.Text = light_cur.G.ToString();
				B.Text = light_cur.B.ToString();
			}
			R.TextChanged += TextBox_TextChanged;
			G.TextChanged += TextBox_TextChanged;
			B.TextChanged += TextBox_TextChanged;
			ColorPanel.SelectValue = CombineColorByText();
		}

		private void changeLightBrightness(int val)
		{
			Brightness.ValueChanged -= Brightness_ValueChanged;
			Brightness.Value = val;
			Brightness.ValueChanged += Brightness_ValueChanged;
		}

		private void Light_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Light_Infinix light_cur = (Light_Infinix)sender;
			if (e.PropertyName == "Switch")
			{
				SwitchChanged(light_cur);
			}
			else if (e.PropertyName == "R" || e.PropertyName == "G" || e.PropertyName == "B" || e.PropertyName == "L" || e.PropertyName == "backR" || e.PropertyName == "backG" || e.PropertyName == "backB" || e.PropertyName == "backL")
			{
				RGBLChanged(light_cur);
			}
			else if (e.PropertyName == "Mode")
			{
				ModeChanged(light_cur);
			}
		}

		private void Brightness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (brushChange)
			{
				return;
			}
			int mode = light[cur].Mode;
			if (cur < CurMap["backlight"])
			{
				light[cur + mode].L = (byte)e.NewValue;
			}
			else if (light[cur].Mode == 4)
			{
				if (back1_color2.IsChecked == true)
				{
					light[gameRound].L = (byte)e.NewValue;
				}
				else if (back1_color3.IsChecked == true)
				{
					light[gameRound].backL = (byte)e.NewValue;
				}
			}
			else if (light[cur].Mode == 5)
			{
				if (back2_color2.IsChecked == true)
				{
					light[gameCover].L = (byte)e.NewValue;
				}
				else if (back2_color3.IsChecked == true)
				{
					light[gameCover].backL = (byte)e.NewValue;
				}
			}
			else if (light[cur].Mode == 2)
			{
				light[musicRythm].L = (byte)e.NewValue;
			}
			else if (light[cur].Mode == 3)
			{
				light[musicJump].L = (byte)e.NewValue;
			}
			else
			{
				light[cur + mode].L = (byte)e.NewValue;
			}
			if (!isSlideDrag)
			{
				SaveLightData();
			}
		}

		private void Brightness_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			isSlideDrag = false;
			SaveLightData();
		}

		private void Brightness_DragStarted(object sender, DragStartedEventArgs e)
		{
			isSlideDrag = true;
		}

		private void writeAlwaysData(int index)
		{
			switch (index)
			{
			case 0:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.Always);
				break;
			case 6:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light12.Always1);
				break;
			case 10:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light12.Always2);
				break;
			case 14:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light34.Always1);
				break;
			case 18:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light34.Always2);
				break;
			case 22:
				StartAutoEvent();
				break;
			}
		}

		private void writeBreathData(int index)
		{
			switch (index)
			{
			case 0:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.Breath);
				break;
			case 6:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light12.Breath1);
				break;
			case 10:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light12.Breath2);
				break;
			case 14:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light34.Breath1);
				break;
			case 18:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light34.Breath2);
				break;
			case 22:
				StartAutoEvent();
				break;
			}
		}

		private void writeClockData(int index)
		{
			switch (index)
			{
			case 0:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.GradualChange);
				break;
			case 6:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light12.GradualChange1);
				break;
			case 10:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light12.GradualChange2);
				break;
			case 14:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light34.GradualChange1);
				break;
			case 18:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light34.GradualChange2);
				break;
			}
		}

		private void writeRainbowData(int index)
		{
			switch (index)
			{
			case 0:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.RainBow);
				break;
			case 6:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light12.RainBow1);
				break;
			case 10:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light12.RainBow2);
				break;
			case 14:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light34.RainBow1);
				break;
			case 18:
				WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light34.RainBow2);
				break;
			}
		}

		public void RGBLChanged(Light_Infinix light_cur)
		{
			System.Windows.Application.Current.Dispatcher.Invoke(delegate
			{
				int num = 0;
				if (((light_cur.Class >= CurMap["backlight"]) ? light[CurMap["backlight"]].Switch : light[CurMap["keyboard"]].Switch) != 0)
				{
					if (light[cur].Mode == 1)
					{
						writeBreathData(cur);
						return;
					}
					if (light[cur].Mode == 0)
					{
						ChangeColor(light_cur);
						writeAlwaysData(cur);
						return;
					}
					if (light_cur.Class < CurMap["backlight"])
					{
						if (light[cur].Mode == 4)
						{
							WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.Wave);
							return;
						}
						if (light[cur].Mode == 2)
						{
							writeClockData(cur);
							return;
						}
						if (light[cur].Mode == 3)
						{
							writeRainbowData(cur);
							return;
						}
						if (light[cur].Mode == 5)
						{
							WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.Flow);
							return;
						}
					}
				}
				if (light[cur].Mode == 2)
				{
					ChangeBack1RecColor(light[musicRythm].R, light[musicRythm].G, light[musicRythm].B, (int)light[musicRythm].L);
				}
				else if (light[cur].Mode == 3)
				{
					ChangeBack2RecColor(light[musicJump].R, light[musicJump].G, light[musicJump].B, (int)light[musicJump].L);
				}
				else if (back1_color2.IsChecked == true)
				{
					ChangeBack1RecColor(light[gameRound].R, light[gameRound].G, light[gameRound].B, (int)light[gameRound].L);
					if (light[cur].Switch == 1)
					{
						StartAutoEvent();
					}
				}
				else if (back2_color2.IsChecked == true)
				{
					ChangeBack2RecColor(light[gameCover].R, light[gameCover].G, light[gameCover].B, (int)light[gameCover].L);
					if (light[cur].Switch == 1)
					{
						StartAutoEvent();
					}
				}
				else if (back1_color3.IsChecked == true)
				{
					back1_color3_rec.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(light[gameRound].backR, light[gameRound].backG, light[gameRound].backB));
					colorZoneBitmap[4].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, lightBackColor(light[gameRound]));
					if (light[cur].Switch == 1)
					{
						StartAutoEvent();
					}
				}
				else if (back2_color3.IsChecked == true)
				{
					back2_color3_rec.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(light[gameCover].backR, light[gameCover].backG, light[gameCover].backB));
					colorZoneBitmap[5].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, lightBackColor(light[gameCover]));
					if (light[cur].Switch == 1)
					{
						StartAutoEvent();
					}
				}
			});
		}

		public void SwitchChanged(Light_Infinix light_cur)
		{
			System.Windows.Application.Current.Dispatcher.Invoke(delegate
			{
				SaveLightData();
				if (light_cur.Switch != 0)
				{
					GlobalVars.IsKeyCon = false;
					if (light_cur.Class == CurMap["backlight"] || cur != CurMap["backlight"])
					{
						if (light_cur.Class == CurMap["backlight"] && cur < CurMap["backlight"])
						{
							StartAutoEvent();
						}
						else
						{
							stopAllTimer();
							object obj;
							if (cur < CurMap["backlight"])
							{
								light_cur = light[cur];
								obj = (LightMode)light_cur.Mode;
							}
							else
							{
								obj = (BackLightMode)light_cur.Mode;
							}
							if (light_cur.Mode == 1)
							{
								startTime = DateTime.Now;
								StartBreathAnimation();
								writeBreathData(cur);
							}
							else if (light_cur.Mode == 0)
							{
								writeAlwaysData(cur);
								ChangeColor(light_cur);
							}
							else if (obj is LightMode lightMode)
							{
								switch (lightMode)
								{
								case LightMode.Wave:
									waveIndex = 0;
									StartWaveAnimation();
									WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.Wave);
									break;
								case LightMode.Clock:
									clockIndex = 0;
									StartClockAnimation();
									writeClockData(cur);
									break;
								case LightMode.Rainbow:
									RainbowIndex = 0;
									StartRainbowAnimation();
									writeRainbowData(cur);
									break;
								case LightMode.Flow:
									FlowIndex = 0;
									StartFlowAnimation();
									WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.Flow);
									break;
								}
							}
							else if (obj is BackLightMode backLightMode)
							{
								switch (backLightMode)
								{
								case BackLightMode.Default:
									StartAutoEvent();
									break;
								case BackLightMode.MusicRhythm:
									SetSelectedBack();
									if (light[CurMap["backlight"]].Switch == 1)
									{
										StartSendMusicData();
										if (BackMusic.IsChecked == true)
										{
											ChangeBack1RecColor(light[musicRythm].R, light[musicRythm].G, light[musicRythm].B, (int)light[musicRythm].L);
											StartRythmAnimation();
										}
									}
									break;
								case BackLightMode.MusicJump:
									SetSelectedBack();
									if (light[CurMap["backlight"]].Switch == 1)
									{
										StartSendMusicData();
										if (BackMusic.IsChecked == true)
										{
											ChangeBack2RecColor(light[musicJump].R, light[musicJump].G, light[musicJump].B, (int)light[musicJump].L);
											StartJumpAnimation();
										}
									}
									break;
								case BackLightMode.GameRound:
									SetSelectedBack();
									if (light[CurMap["backlight"]].Switch == 1)
									{
										colorZoneBitmap[4].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, lightColor(light[gameRound]));
										back1_color3_rec.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(light[gameRound].backR, light[gameRound].backG, light[gameRound].backB));
										StartAutoEvent();
										if (BackGaming.IsChecked == true)
										{
											ChangeBack1RecColor(light[gameRound].R, light[gameRound].G, light[gameRound].B, (int)light[gameRound].L);
											StartRoundAnimation();
										}
									}
									break;
								case BackLightMode.GameCovering:
									SetSelectedBack();
									if (light[CurMap["backlight"]].Switch == 1)
									{
										colorZoneBitmap[5].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, lightBackColor(light[gameCover]));
										StartAutoEvent();
										if (BackGaming.IsChecked == true)
										{
											ChangeBack2RecColor(light[gameCover].R, light[gameCover].G, light[gameCover].B, (int)light[gameCover].L);
											StartCoverAnimation();
										}
									}
									break;
								}
							}
						}
					}
				}
				else if (light_cur.Class != CurMap["backlight"] && cur == CurMap["backlight"])
				{
					GlobalVars.IsKeyCon = false;
				}
				else if (light_cur.Class == CurMap["backlight"] && cur < CurMap["backlight"])
				{
					GlobalVars.IsKeyCon = false;
				}
				else
				{
					stopAllTimer();
					if (cur == CurMap["backlight"])
					{
						if (!GlobalVars.IsKeyCon)
						{
							StartAutoEvent();
						}
						GlobalVars.IsKeyCon = false;
						ChangeBackColor(0, 0, 0, 0.0);
						colorZoneBitmap[2].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
						colorZoneBitmap[3].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
						colorZoneBitmap[4].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
						colorZoneBitmap[5].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Colors.Gray);
						ColorZone.Source = colorZoneBitmap[1];
					}
					else
					{
						if (!GlobalVars.IsKeyCon)
						{
							WriteLightDataToUsb(TxBuf.PARAMS.Keyboard_Light.LightOFF);
						}
						GlobalVars.IsKeyCon = false;
						ChangeKeyBoardColor(0, 0, 0, 0.0);
						ColorZone.Source = colorZoneBitmap[0];
					}
				}
			});
		}

		private void SetUI(Light_Infinix light_cur)
		{
			if (light_cur.Class < CurMap["backlight"])
			{
				SetKeyboardUI(light_cur);
			}
			else
			{
				SetBackLightUI(light_cur);
			}
		}

		private void SetKeyboardUI(Light_Infinix light_cur)
		{
			ColorPanel.CanChoose = true;
			Brightness.IsEnabled = true;
			int mode = light_cur.Mode;
			stopAllTimer();
			byte num = light[CurMap["keyboard"]].Switch;
			if (light_cur.Mode == 0 || light_cur.Mode == 1 || light_cur.Mode == 4)
			{
				EnableUserSelect();
			}
			else
			{
				DisableUserSelect();
			}
			if (num == 0)
			{
				Custom_Board.IsChecked = false;
			}
			else
			{
				Custom_Board.IsChecked = true;
			}
			Light_Infinix light_Infinix = light[cur + mode];
			changeRGBText(light_Infinix);
			changeLightBrightness(light_Infinix.L);
			SetMode(light_cur);
			if (light_cur.Mode == 0)
			{
				ChangeColor(light_cur);
			}
			if (num == 0)
			{
				CloseLight();
			}
			else
			{
				AnimationSelect(light_cur);
			}
		}

		private void SetBackLightUI(Light_Infinix light_cur)
		{
			stopAllTimer();
			int mode = light_cur.Mode;
			byte num = light[CurMap["backlight"]].Switch;
			if (num == 0)
			{
				Custom_Back.IsChecked = false;
			}
			else
			{
				Custom_Back.IsChecked = true;
			}
			SetMode(light_cur);
			if (mode != 6)
			{
				Light_Infinix light_Infinix = light[cur + mode];
				changeRGBText(light_Infinix);
				if (back1_color3.IsChecked == true || back2_color3.IsChecked == true)
				{
					changeLightBrightness(light_Infinix.backL);
				}
				else
				{
					changeLightBrightness(light_Infinix.L);
				}
				EnableUserSelect();
			}
			if (num == 0)
			{
				CloseLight();
			}
			else
			{
				AnimationSelect(light_cur);
			}
		}

		public void Dispose()
		{
			try
			{
				StopManualEvent();
				serPort.Dispose();
				GlobalVars.Wmi.setLightPageStatus(isDefault: true);
				WriteLightAutoClose();
			}
			catch
			{
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/views/pages/page2.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				keyboard = (System.Windows.Controls.Button)target;
				keyboard.Click += Keyboard_Click;
				break;
			case 2:
				back = (System.Windows.Controls.Button)target;
				back.Click += back_Click;
				break;
			case 3:
				keyBoardView = (Viewbox)target;
				break;
			case 4:
				KeyboardGrid = (Grid)target;
				break;
			case 5:
				ColorZone_back = (System.Windows.Controls.Image)target;
				break;
			case 6:
				ColorZone = (System.Windows.Controls.Image)target;
				break;
			case 7:
				Zone = (System.Windows.Controls.Image)target;
				Zone.MouseDown += Zone_MouseLeftButtonDown;
				break;
			case 8:
				KeyboardPath1 = (System.Windows.Shapes.Path)target;
				KeyboardPath1.MouseLeftButtonDown += Path_MouseLeftButtonDown;
				break;
			case 9:
				KeyboardPath2 = (System.Windows.Shapes.Path)target;
				KeyboardPath2.MouseLeftButtonDown += Path_MouseLeftButtonDown;
				break;
			case 10:
				KeyboardPath3 = (System.Windows.Shapes.Path)target;
				KeyboardPath3.MouseLeftButtonDown += Path_MouseLeftButtonDown;
				break;
			case 11:
				KeyboardPath4 = (System.Windows.Shapes.Path)target;
				KeyboardPath4.MouseLeftButtonDown += Path_MouseLeftButtonDown;
				break;
			case 12:
				backView_2 = (Viewbox)target;
				break;
			case 13:
				border1 = (Border)target;
				break;
			case 14:
				back1_grid = (Grid)target;
				back1_grid.MouseLeftButtonDown += back1_MouseLeftButtonDown;
				break;
			case 15:
				back1_text = (TextBlock)target;
				break;
			case 16:
				ColorZone_back_1 = (System.Windows.Controls.Image)target;
				break;
			case 17:
				ColorZone_1 = (System.Windows.Controls.Image)target;
				break;
			case 18:
				Zone_1 = (System.Windows.Controls.Image)target;
				Zone_1.MouseDown += Zone_MouseLeftButtonDown;
				break;
			case 19:
				music1 = (Grid)target;
				break;
			case 20:
				back1_color1_text = (TextBlock)target;
				break;
			case 21:
				back1_color1 = (System.Windows.Controls.RadioButton)target;
				back1_color1.Checked += back1_color1_Checked;
				back1_color1.Unchecked += back1_color1_Checked;
				break;
			case 22:
				back1_color1_border = (Border)target;
				break;
			case 23:
				back1_color1_rec = (System.Windows.Shapes.Rectangle)target;
				break;
			case 24:
				game1 = (Grid)target;
				break;
			case 25:
				back1_color2_text = (TextBlock)target;
				break;
			case 26:
				back1_color2 = (System.Windows.Controls.RadioButton)target;
				back1_color2.Checked += back1_color2_Checked;
				back1_color2.Unchecked += back1_color2_Checked;
				break;
			case 27:
				back1_color2_border = (Border)target;
				break;
			case 28:
				back1_color2_rec = (System.Windows.Shapes.Rectangle)target;
				break;
			case 29:
				back1_color3_text = (TextBlock)target;
				break;
			case 30:
				back1_color3 = (System.Windows.Controls.RadioButton)target;
				back1_color3.Checked += back1_color3_Checked;
				back1_color3.Unchecked += back1_color3_Checked;
				break;
			case 31:
				back1_color3_border = (Border)target;
				break;
			case 32:
				back1_color3_rec = (System.Windows.Shapes.Rectangle)target;
				break;
			case 33:
				border2 = (Border)target;
				break;
			case 34:
				((Grid)target).MouseLeftButtonDown += back2_MouseLeftButtonDown;
				break;
			case 35:
				back2_text = (TextBlock)target;
				break;
			case 36:
				ColorZone_back_2 = (System.Windows.Controls.Image)target;
				break;
			case 37:
				ColorZone_2 = (System.Windows.Controls.Image)target;
				break;
			case 38:
				Zone_2 = (System.Windows.Controls.Image)target;
				Zone_2.MouseDown += Zone_MouseLeftButtonDown;
				break;
			case 39:
				music2 = (Grid)target;
				break;
			case 40:
				back2_color1_text = (TextBlock)target;
				break;
			case 41:
				back2_color1 = (System.Windows.Controls.RadioButton)target;
				back2_color1.Checked += back2_color1_Checked;
				back2_color1.Unchecked += back2_color1_Checked;
				break;
			case 42:
				back2_color1_border = (Border)target;
				break;
			case 43:
				back2_color1_rec = (System.Windows.Shapes.Rectangle)target;
				break;
			case 44:
				game2 = (Grid)target;
				break;
			case 45:
				back2_color2_text = (TextBlock)target;
				break;
			case 46:
				back2_color2 = (System.Windows.Controls.RadioButton)target;
				back2_color2.Checked += back2_color2_Checked;
				back2_color2.Unchecked += back2_color2_Checked;
				break;
			case 47:
				back2_color2_border = (Border)target;
				break;
			case 48:
				back2_color2_rec = (System.Windows.Shapes.Rectangle)target;
				break;
			case 49:
				back2_color3_text = (TextBlock)target;
				break;
			case 50:
				back2_color3 = (System.Windows.Controls.RadioButton)target;
				back2_color3.Checked += back2_color3_Checked;
				back2_color3.Unchecked += back2_color3_Checked;
				break;
			case 51:
				back2_color3_border = (Border)target;
				break;
			case 52:
				back2_color3_rec = (System.Windows.Shapes.Rectangle)target;
				break;
			case 53:
				KeyBoardGrid = (Grid)target;
				break;
			case 54:
				KeyBoardAlways = (System.Windows.Controls.RadioButton)target;
				KeyBoardAlways.Checked += KeyBoardAlways_Checked;
				KeyBoardAlways.Unchecked += KeyBoardAlways_Checked;
				break;
			case 55:
				KeyBoardAlwaysImg = (System.Windows.Controls.Image)target;
				break;
			case 56:
				KeyBoardBreath = (System.Windows.Controls.RadioButton)target;
				KeyBoardBreath.Checked += KeyBoardBreath_Checked;
				KeyBoardBreath.Unchecked += KeyBoardBreath_Checked;
				break;
			case 57:
				KeyBoardBreathImg = (System.Windows.Controls.Image)target;
				break;
			case 58:
				KeyBoardWave = (System.Windows.Controls.RadioButton)target;
				KeyBoardWave.Checked += KeyBoardWave_Checked;
				KeyBoardWave.Unchecked += KeyBoardWave_Checked;
				break;
			case 59:
				KeyBoardWaveImg = (System.Windows.Controls.Image)target;
				break;
			case 60:
				KeyBoardClock = (System.Windows.Controls.RadioButton)target;
				KeyBoardClock.Checked += KeyBoardClock_Checked;
				KeyBoardClock.Unchecked += KeyBoardClock_Checked;
				break;
			case 61:
				KeyBoardClockImg = (System.Windows.Controls.Image)target;
				break;
			case 62:
				KeyBoardRainbow = (System.Windows.Controls.RadioButton)target;
				KeyBoardRainbow.Checked += KeyBoardRainbow_Checked;
				KeyBoardRainbow.Unchecked += KeyBoardRainbow_Checked;
				break;
			case 63:
				KeyBoardRainbowImg = (System.Windows.Controls.Image)target;
				break;
			case 64:
				KeyBoardFlow = (System.Windows.Controls.RadioButton)target;
				KeyBoardFlow.Checked += KeyBoardFlow_Checked;
				KeyBoardFlow.Unchecked += KeyBoardFlow_Checked;
				break;
			case 65:
				KeyBoardFlowImg = (System.Windows.Controls.Image)target;
				break;
			case 66:
				ZoneGrid = (Grid)target;
				break;
			case 67:
				ZoneAlways = (System.Windows.Controls.RadioButton)target;
				ZoneAlways.Checked += ZoneAlways_Checked;
				ZoneAlways.Unchecked += ZoneAlways_Checked;
				break;
			case 68:
				ZoneAlwaysImg = (System.Windows.Controls.Image)target;
				break;
			case 69:
				ZoneBreath = (System.Windows.Controls.RadioButton)target;
				ZoneBreath.Checked += ZoneBreath_Checked;
				ZoneBreath.Unchecked += ZoneBreath_Checked;
				break;
			case 70:
				ZoneBreathImg = (System.Windows.Controls.Image)target;
				break;
			case 71:
				ZoneClock = (System.Windows.Controls.RadioButton)target;
				ZoneClock.Checked += ZoneClock_Checked;
				ZoneClock.Unchecked += ZoneClock_Checked;
				break;
			case 72:
				ZoneClockImg = (System.Windows.Controls.Image)target;
				break;
			case 73:
				ZoneRainbow = (System.Windows.Controls.RadioButton)target;
				ZoneRainbow.Checked += ZoneRainbow_Checked;
				ZoneRainbow.Unchecked += ZoneRainbow_Checked;
				break;
			case 74:
				ZoneRainbowImg = (System.Windows.Controls.Image)target;
				break;
			case 75:
				BackGrid = (Grid)target;
				break;
			case 76:
				BackAlways = (System.Windows.Controls.RadioButton)target;
				BackAlways.Checked += BackAlways_Checked;
				BackAlways.Unchecked += BackAlways_Checked;
				break;
			case 77:
				BackAlwaysImg = (System.Windows.Controls.Image)target;
				break;
			case 78:
				BackBreath = (System.Windows.Controls.RadioButton)target;
				BackBreath.Checked += BackBreath_Checked;
				BackBreath.Unchecked += BackBreath_Checked;
				break;
			case 79:
				BackBreathImg = (System.Windows.Controls.Image)target;
				break;
			case 80:
				BackMusic = (System.Windows.Controls.RadioButton)target;
				BackMusic.Checked += BackMusic_Checked;
				BackMusic.Unchecked += BackMusic_Checked;
				break;
			case 81:
				BackMusicImg = (System.Windows.Controls.Image)target;
				break;
			case 82:
				BackGaming = (System.Windows.Controls.RadioButton)target;
				BackGaming.Checked += BackGaming_Checked;
				BackGaming.Unchecked += BackGaming_Checked;
				break;
			case 83:
				BackGameImg = (System.Windows.Controls.Image)target;
				break;
			case 84:
				grid = (Grid)target;
				break;
			case 85:
				viewbox = (Viewbox)target;
				break;
			case 86:
				Custom_Text = (TextBlock)target;
				break;
			case 87:
				Custom_Board = (CustomControlLibrary.Controls.Switch)target;
				Custom_Board.Checked += Custom_Board_Checked;
				Custom_Board.Unchecked += Custom_Board_Checked;
				break;
			case 88:
				Custom_Back = (CustomControlLibrary.Controls.Switch)target;
				Custom_Back.Checked += Custom_Back_Checked;
				Custom_Back.Unchecked += Custom_Back_Checked;
				break;
			case 89:
				Custom_Button = (CustomControlLibrary.Controls.Switch)target;
				Custom_Button.Checked += Custom_Button_Checked;
				Custom_Button.Unchecked += Custom_Button_Checked;
				break;
			case 90:
				PanelGrid = (Grid)target;
				break;
			case 91:
				ColorPanel = (ColorControlPanel)target;
				break;
			case 92:
				ColorTextGrid = (Grid)target;
				break;
			case 93:
				ColorStackPanel = (StackPanel)target;
				break;
			case 94:
				R = (System.Windows.Controls.TextBox)target;
				R.PreviewTextInput += TextBox_PreviewTextInput;
				R.PreviewKeyDown += R_PreviewKeyDown;
				R.TextChanged += TextBox_TextChanged;
				break;
			case 95:
				G = (System.Windows.Controls.TextBox)target;
				G.PreviewTextInput += TextBox_PreviewTextInput;
				G.PreviewKeyDown += R_PreviewKeyDown;
				G.TextChanged += TextBox_TextChanged;
				break;
			case 96:
				B = (System.Windows.Controls.TextBox)target;
				B.PreviewTextInput += TextBox_PreviewTextInput;
				B.PreviewKeyDown += R_PreviewKeyDown;
				B.TextChanged += TextBox_TextChanged;
				break;
			case 97:
				Brightness = (FlatSilder)target;
				Brightness.ValueChanged += Brightness_ValueChanged;
				Brightness.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(Brightness_DragCompleted));
				Brightness.AddHandler(Thumb.DragStartedEvent, new DragStartedEventHandler(Brightness_DragStarted));
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
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

		internal System.Windows.Controls.Label Label3;

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
				System.Windows.Data.Binding binding = new System.Windows.Data.Binding("BatteryStatus");
				binding.Source = source;
				settings.SetBinding(Settings_Dark.ChargeModePorpetry, binding);
				System.Windows.Data.Binding binding2 = new System.Windows.Data.Binding("Battery");
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
			System.Windows.Controls.ComboBox comboBox = (System.Windows.Controls.ComboBox)sender;
			if (origin != comboBox.SelectedIndex)
			{
				comboBox.Show();
				string messageBoxText = (string)System.Windows.Application.Current.FindResource("ResetTips");
				string caption = (string)System.Windows.Application.Current.FindResource("Tips");
				_ = new string[3] { "dGpu only", "Dynamic", "iGpu only" };
				switch (System.Windows.MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
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
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				Label3 = (System.Windows.Controls.Label)target;
				break;
			case 5:
				Switch3 = (CustomControlLibrary.Controls.Switch)target;
				break;
			case 6:
				((System.Windows.Controls.ComboBox)target).DropDownClosed += ComboBox_DropDownClosed;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
	public class Page4 : Page, IComponentConnector, IStyleConnector
	{
		internal System.Windows.Controls.Button btnFirmware;

		internal System.Windows.Controls.Button btnDriver;

		internal System.Windows.Controls.Button check_fw;

		internal System.Windows.Controls.Button check_dr;

		internal Grid Loading;

		internal System.Windows.Shapes.Rectangle rec1;

		internal System.Windows.Shapes.Rectangle rec2;

		internal System.Windows.Shapes.Rectangle rec3;

		internal System.Windows.Shapes.Rectangle rec4;

		internal System.Windows.Shapes.Rectangle rec5;

		internal System.Windows.Controls.ListBox ListBoxDr;

		internal System.Windows.Controls.ListBox ListBoxFw;

		internal System.Windows.Controls.Button AppUpdate;

		private bool _contentLoaded;

		public Page4()
		{
			InitializeComponent();
			base.DataContext = App.Current.Services.GetService(typeof(Page4ViewModel));
		}

		private void check_fw_Click(object sender, RoutedEventArgs e)
		{
			check_fw.IsEnabled = false;
			check_dr.IsEnabled = false;
			btnDriver.IsEnabled = false;
			btnFirmware.IsEnabled = false;
			updateItem(1);
		}

		private async void updateItem(int flag)
		{
			string UpdateNow = (string)System.Windows.Application.Current.FindResource("UpdateNow");
			string LocalUpdate = (string)System.Windows.Application.Current.FindResource("LocalUpdate");
			_ = (string)System.Windows.Application.Current.FindResource("CancelUpdate");
			string NetworkWrong = (string)System.Windows.Application.Current.FindResource("NetworkWrong");
			string NoneLater = (string)System.Windows.Application.Current.FindResource("NoneLater");
			string FindLater = (string)System.Windows.Application.Current.FindResource("FindLater");
			System.Windows.Application.Current.Dispatcher.Invoke(delegate
			{
				ListBoxDr.Visibility = Visibility.Hidden;
				ListBoxFw.Visibility = Visibility.Hidden;
				Loading.Visibility = Visibility.Visible;
			});
			Page4ViewModel viewModel = (Page4ViewModel)base.DataContext;
			Item[] itemArrDR = new Item[100];
			int indexDR = 0;
			Item[] itemArrFW = new Item[3];
			int indexFW = 0;
			viewModel.items.Clear();
			viewModel.FwItems.Clear();
			try
			{
				await System.Threading.Tasks.Task.Run(delegate
				{
					Trace.WriteLine("zqp_test check_dr_Click start");
					(string[], int) tuple = viewModel.clientRequest.checkUpdateServer();
					string[] item = tuple.Item1;
					int item2 = tuple.Item2;
					Dictionary<string, string> allLoacalNameMap = viewModel.clientRequest.getAllLoacalNameMap();
					switch (item2)
					{
					case -1:
					{
						Trace.WriteLine("zqp_test Net error");
						Dictionary<string, string> allLoacalVersionMap2 = viewModel.clientRequest.getAllLoacalVersionMap();
						if (allLoacalVersionMap2.Count != 0)
						{
							foreach (KeyValuePair<string, string> item7 in allLoacalVersionMap2)
							{
								Item item5 = new Item
								{
									Id = item7.Key,
									Name = allLoacalNameMap[item7.Key],
									NowVersion = item7.Value,
									Description = NetworkWrong,
									NewVersion = "-----",
									Tips = "",
									Command = viewModel.DriverCommand,
									btntext = UpdateNow
								};
								if (!(item7.Key == "APPUpdate"))
								{
									if (item7.Key == "BIOS" || item7.Key == "EC" || item7.Key == "PD" || item7.Key == "VideoBIOS")
									{
										itemArrFW[indexFW] = item5;
										indexFW++;
									}
									else
									{
										itemArrDR[indexDR] = item5;
										indexDR++;
									}
								}
							}
							break;
						}
						break;
					}
					case 0:
					{
						Trace.WriteLine("zqp_test server is empty");
						Dictionary<string, string> allLoacalVersionMap3 = viewModel.clientRequest.getAllLoacalVersionMap();
						if (allLoacalVersionMap3.Count != 0)
						{
							foreach (KeyValuePair<string, string> item8 in allLoacalVersionMap3)
							{
								Item item6 = new Item
								{
									Id = item8.Key,
									Name = allLoacalNameMap[item8.Key],
									NowVersion = item8.Value,
									Description = NoneLater,
									Tips = "",
									NewVersion = item8.Value,
									Command = viewModel.DriverCommand,
									btntext = UpdateNow
								};
								if (!(item8.Key == "APPUpdate"))
								{
									if (item8.Key == "BIOS" || item8.Key == "EC" || item8.Key == "PD" || item8.Key == "VideoBIOS")
									{
										itemArrFW[indexFW] = item6;
										indexFW++;
									}
									else
									{
										itemArrDR[indexDR] = item6;
										indexDR++;
									}
								}
							}
							break;
						}
						break;
					}
					case 1:
					{
						for (int i = 0; i < item.Length; i++)
						{
							Item item3 = new Item
							{
								Id = item[i],
								Name = allLoacalNameMap[item[i]]
							};
							IclientRequest.downFileVersionInfo downFileVersionInfo = default(IclientRequest.downFileVersionInfo);
							downFileVersionInfo = viewModel.clientRequest.getGerverVersionMap(item[i]);
							string localVersionMap = viewModel.clientRequest.getLocalVersionMap(item[i]);
							if (downFileVersionInfo.latestVersion == null || localVersionMap == null)
							{
								Trace.WriteLine("未知错误");
							}
							else
							{
								item3.NowVersion = localVersionMap;
								item3.NewVersion = downFileVersionInfo.latestVersion;
								item3.Description = FindLater;
								item3.btntext = UpdateNow;
								item3.Tips = downFileVersionInfo.releaseNote[0];
								item3.Command = viewModel.DriverCommand;
								if (!(item[i] == "APPUpdate"))
								{
									if (item[i] == "BIOS" || item[i] == "EC" || item[i] == "PD" || item[i] == "VideoBIOS")
									{
										itemArrFW[indexFW] = item3;
										indexFW++;
									}
									else
									{
										itemArrDR[indexDR] = item3;
										indexDR++;
									}
								}
							}
						}
						Dictionary<string, string> allLoacalVersionMap = viewModel.clientRequest.getAllLoacalVersionMap();
						if (allLoacalVersionMap.Count != 0)
						{
							foreach (KeyValuePair<string, string> item9 in allLoacalVersionMap)
							{
								if (!item.Contains(item9.Key))
								{
									Item item4 = new Item
									{
										Name = allLoacalNameMap[item9.Key],
										NowVersion = item9.Value,
										Description = NoneLater,
										NewVersion = item9.Value,
										Command = viewModel.DriverCommand,
										btntext = UpdateNow
									};
									if (!(item9.Key == "APPUpdate"))
									{
										if (item9.Key == "BIOS" || item9.Key == "EC" || item9.Key == "PD" || item9.Key == "VideoBIOS")
										{
											itemArrFW[indexFW] = item4;
											indexFW++;
										}
										else
										{
											itemArrDR[indexDR] = item4;
											indexDR++;
										}
									}
								}
							}
							break;
						}
						break;
					}
					}
				}).ContinueWith(delegate
				{
					Trace.WriteLine("zqp_test index:" + indexFW);
					(string[], string[]) tuple = viewModel.clientRequest.checkLoadUpdate();
					string[] item = tuple.Item1;
					string[] item2 = tuple.Item2;
					int i;
					for (i = 0; i < indexDR; i++)
					{
						if (item != null && item.Length != 0 && item2 != null && item2.Length != 0)
						{
							int num = Array.IndexOf<string>(item, itemArrDR[i].Name);
							if (num != -1)
							{
								itemArrDR[i].btntext = LocalUpdate;
								itemArrDR[i].NewVersion = item2[num];
							}
						}
						System.Windows.Application.Current.Dispatcher.Invoke(delegate
						{
							viewModel.AddItem(itemArrDR[i]);
						});
					}
					int i2;
					for (i2 = 0; i2 < indexFW; i2++)
					{
						if (item != null && item.Length != 0 && item2 != null && item2.Length != 0)
						{
							int num2 = Array.IndexOf<string>(item, itemArrFW[i2].Name);
							if (num2 != -1)
							{
								itemArrFW[i2].btntext = LocalUpdate;
								itemArrFW[i2].NewVersion = item2[num2];
							}
						}
						System.Windows.Application.Current.Dispatcher.Invoke(delegate
						{
							viewModel.AddFwItems(itemArrFW[i2]);
						});
					}
					System.Windows.Application.Current.Dispatcher.Invoke(delegate
					{
						Loading.Visibility = Visibility.Hidden;
						if (flag == 1)
						{
							ListBoxFw.Visibility = Visibility.Visible;
						}
						else
						{
							ListBoxDr.Visibility = Visibility.Visible;
						}
						check_fw.IsEnabled = true;
						check_dr.IsEnabled = true;
						btnDriver.IsEnabled = true;
						btnFirmware.IsEnabled = true;
					});
				});
			}
			catch (Exception ex)
			{
				Trace.WriteLine("zqp_test error:" + ex);
			}
		}

		private void check_dr_Click(object sender, RoutedEventArgs e)
		{
			check_fw.IsEnabled = false;
			check_dr.IsEnabled = false;
			btnDriver.IsEnabled = false;
			btnFirmware.IsEnabled = false;
			updateItem(2);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
		}

		private void btnDriver_Click(object sender, RoutedEventArgs e)
		{
			btnDriver.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
			btnFirmware.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF474747"));
			ListBoxFw.Visibility = Visibility.Hidden;
			ListBoxDr.Visibility = Visibility.Visible;
			check_dr.Visibility = Visibility.Visible;
			check_fw.Visibility = Visibility.Hidden;
		}

		private void btnFirmware_Click(object sender, RoutedEventArgs e)
		{
			btnDriver.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF474747"));
			btnFirmware.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF30B3EB"));
			ListBoxFw.Visibility = Visibility.Visible;
			ListBoxDr.Visibility = Visibility.Hidden;
			check_dr.Visibility = Visibility.Hidden;
			check_fw.Visibility = Visibility.Visible;
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			AppUpdate.IsEnabled = false;
			await System.Threading.Tasks.Task.Run(delegate
			{
				string text = (string)System.Windows.Application.Current.FindResource("FindVersion");
				string text2 = (string)System.Windows.Application.Current.FindResource("UpdateQuestion");
				string caption = (string)System.Windows.Application.Current.FindResource("AppUpdate");
				string messageBoxText = (string)System.Windows.Application.Current.FindResource("AppUpdateTips1");
				string messageBoxText2 = (string)System.Windows.Application.Current.FindResource("AppUpdateTips2");
				int num = 0;
				IclientRequest.downFileVersionInfo downFileVersionInfo = default(IclientRequest.downFileVersionInfo);
				clientRequest clientRequest2 = new clientRequest();
				(downFileVersionInfo, num) = clientRequest2.getAppVersion();
				if (num == 1)
				{
					MessageBoxResult num2 = System.Windows.MessageBox.Show(text + downFileVersionInfo.latestVersion + text2, caption, MessageBoxButton.YesNo);
					if (num2 == MessageBoxResult.Yes)
					{
						clientRequest2.UpdateAPP();
					}
					if (num2 == MessageBoxResult.No)
					{
						return;
					}
				}
				if (num == 0)
				{
					System.Windows.MessageBox.Show(messageBoxText, caption, MessageBoxButton.OK);
				}
				if (num == -1)
				{
					System.Windows.MessageBox.Show(messageBoxText2, caption, MessageBoxButton.OK);
				}
			});
			AppUpdate.IsEnabled = true;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/views/pages/page4.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				btnFirmware = (System.Windows.Controls.Button)target;
				btnFirmware.Click += btnFirmware_Click;
				break;
			case 2:
				btnDriver = (System.Windows.Controls.Button)target;
				btnDriver.Click += btnDriver_Click;
				break;
			case 3:
				check_fw = (System.Windows.Controls.Button)target;
				check_fw.Click += check_fw_Click;
				break;
			case 4:
				check_dr = (System.Windows.Controls.Button)target;
				check_dr.Click += check_dr_Click;
				break;
			case 5:
				Loading = (Grid)target;
				break;
			case 6:
				rec1 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 7:
				rec2 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 8:
				rec3 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 9:
				rec4 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 10:
				rec5 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 11:
				ListBoxDr = (System.Windows.Controls.ListBox)target;
				break;
			case 13:
				ListBoxFw = (System.Windows.Controls.ListBox)target;
				break;
			case 15:
				AppUpdate = (System.Windows.Controls.Button)target;
				AppUpdate.Click += Button_Click;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 12:
				((System.Windows.Controls.Button)target).Click += Button_Click_1;
				break;
			case 14:
				((System.Windows.Controls.Button)target).Click += Button_Click_1;
				break;
			}
		}
	}
}
namespace CustomControl
{
	public class ColorControlPanel : System.Windows.Controls.UserControl, IComponentConnector
	{
		private ControlColors ColorPanelColors = new ControlColors();

		private SolidColorBrush savedColor;

		private SolidColorBrush savedColorSV;

		public static readonly DependencyProperty InitialBrushProperty = DependencyProperty.Register("InitialBrush", typeof(SolidColorBrush), typeof(ColorControlPanel), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Red), InitialBrushChanged));

		public static readonly DependencyProperty SelectBrushProperty = DependencyProperty.Register("SelectBrush", typeof(SolidColorBrush), typeof(ColorControlPanel), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Lime), SelectBrushChanged));

		public static readonly DependencyProperty SelectLProperty = DependencyProperty.Register("SelectL", typeof(double), typeof(ColorControlPanel), new FrameworkPropertyMetadata(0.0, SelectLChanged));

		public static readonly DependencyProperty SelectValueProperty = DependencyProperty.Register("SelectValue", typeof(string), typeof(ColorControlPanel), new FrameworkPropertyMetadata("#FFFFFFFF", SelectValueChanged));

		public static readonly DependencyProperty TextForeColorProperty = DependencyProperty.Register("TextForeColor", typeof(SolidColorBrush), typeof(ColorControlPanel), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Lime), SelectBrushChanged));

		public static readonly DependencyProperty CanChooseProperty = DependencyProperty.Register("CanChoose", typeof(bool), typeof(ColorControlPanel), new FrameworkPropertyMetadata(true, CanChooseChanged));

		internal Grid xColorPanelHSV;

		internal Canvas xColorWheel;

		internal Canvas ThumbH;

		internal Ellipse xColorH;

		internal System.Windows.Shapes.Path PanelHue;

		internal Canvas xColorTriangle;

		internal Polygon PanelS;

		internal Polygon PanelV;

		internal Grid ThumbSV;

		internal Ellipse xColorSV;

		internal Polygon PanelSV;

		private bool _contentLoaded;

		public SolidColorBrush InitialBrush
		{
			get
			{
				return (SolidColorBrush)GetValue(InitialBrushProperty);
			}
			set
			{
				SetValue(InitialBrushProperty, value);
			}
		}

		public SolidColorBrush SelectBrush
		{
			get
			{
				return (SolidColorBrush)GetValue(SelectBrushProperty);
			}
			set
			{
				SetValue(SelectBrushProperty, value);
			}
		}

		public double SelectL
		{
			get
			{
				return (double)GetValue(SelectLProperty);
			}
			set
			{
				SetValue(SelectLProperty, value);
				SelectColorChanged();
			}
		}

		public string SelectValue
		{
			get
			{
				return (string)GetValue(SelectValueProperty);
			}
			set
			{
				SetValue(SelectValueProperty, value);
				SelectColorChanged();
			}
		}

		public SolidColorBrush TextForeColor
		{
			get
			{
				return (SolidColorBrush)GetValue(SelectBrushProperty);
			}
			set
			{
				SetValue(SelectBrushProperty, value);
			}
		}

		public bool CanChoose
		{
			get
			{
				return (bool)GetValue(CanChooseProperty);
			}
			set
			{
				SetValue(CanChooseProperty, value);
			}
		}

		public ColorControlPanel()
		{
			InitializeComponent();
			base.DataContext = ColorPanelColors;
			base.Loaded += delegate
			{
				DrawColorWheel();
			};
			RegisterEvent();
			setBind();
		}

		private void setBind()
		{
			SetBinding(SelectBrushProperty, new System.Windows.Data.Binding("SelectedBrush")
			{
				Source = ColorPanelColors,
				Mode = BindingMode.TwoWay
			});
			SetBinding(SelectLProperty, new System.Windows.Data.Binding("SelectedVal")
			{
				Source = ColorPanelColors,
				Mode = BindingMode.TwoWay
			});
			SetBinding(SelectValueProperty, new System.Windows.Data.Binding("SelectedBrushValue")
			{
				Source = ColorPanelColors,
				Mode = BindingMode.TwoWay
			});
		}

		private static void InitialBrushChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			_ = (ColorControlPanel)sender;
		}

		private static void SelectBrushChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			_ = (ColorControlPanel)sender;
		}

		private static void SelectLChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			_ = (ColorControlPanel)sender;
		}

		private static void SelectValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			_ = (ColorControlPanel)sender;
		}

		private static void TextForeColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			((ColorControlPanel)sender).SelectBrush = e.NewValue as SolidColorBrush;
		}

		private static void CanChooseChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ColorControlPanel colorControlPanel = (ColorControlPanel)sender;
			colorControlPanel.DrawColorWheel();
			if (!colorControlPanel.CanChoose)
			{
				colorControlPanel.savedColor = (SolidColorBrush)colorControlPanel.xColorH.Fill;
				colorControlPanel.savedColorSV = (SolidColorBrush)colorControlPanel.xColorSV.Fill;
				colorControlPanel.xColorH.Fill = new SolidColorBrush(Colors.Gray);
				colorControlPanel.xColorSV.Fill = new SolidColorBrush(Colors.Gray);
			}
			else
			{
				System.Windows.Data.Binding binding = new System.Windows.Data.Binding("SelectedBrushH");
				colorControlPanel.xColorH.SetBinding(Shape.FillProperty, binding);
				System.Windows.Data.Binding binding2 = new System.Windows.Data.Binding("SelectedBrush");
				colorControlPanel.xColorSV.SetBinding(Shape.FillProperty, binding2);
			}
		}

		private void RegisterEvent()
		{
			PanelHue.MouseLeftButtonDown += delegate
			{
				HueThumbMove(Mouse.GetPosition(PanelHue));
			};
			PanelHue.MouseMove += delegate(object sender, System.Windows.Input.MouseEventArgs e)
			{
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					if (Mouse.Captured == null)
					{
						Mouse.Capture(PanelHue);
					}
					HueThumbMove(e.GetPosition(PanelHue));
				}
				else
				{
					Mouse.Capture(null);
				}
			};
			PanelSV.MouseLeftButtonDown += delegate
			{
				SvThumbMove(Mouse.GetPosition(this));
			};
			PanelSV.MouseMove += delegate(object sender, System.Windows.Input.MouseEventArgs e)
			{
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					if (Mouse.Captured == null)
					{
						Mouse.Capture(PanelSV);
					}
					SvThumbMove(e.GetPosition(this));
				}
				else
				{
					Mouse.Capture(null);
				}
			};
		}

		private void HueThumbMove(System.Windows.Point point)
		{
			double num = Math.Atan2(point.Y - PanelHue.ActualHeight / 2.0, point.X - PanelHue.ActualWidth / 2.0) * 180.0 / Math.PI;
			if (num < 0.0)
			{
				num += 360.0;
			}
			ColorPanelColors.SelectedHue = num;
		}

		private void SvThumbMove(System.Windows.Point point)
		{
			System.Windows.Point point2 = TranslatePoint(point, PanelSV);
			System.Windows.Point point3 = TranslatePoint(point, PanelS);
			System.Windows.Point point4 = TranslatePoint(point, PanelV);
			System.Windows.Point point5 = PanelSV.Points[2];
			System.Windows.Point point6 = PanelSV.Points[1];
			System.Windows.Point point7 = PanelSV.Points[0];
			if (point2.X < point5.X)
			{
				point2.X = point5.X;
				if (point2.Y < point5.Y)
				{
					point2.Y = point5.Y;
				}
				if (point2.Y > point6.Y)
				{
					point2.Y = point6.Y;
				}
			}
			else if (point3.X < point5.X)
			{
				point3.X = point5.X;
				if (point3.Y < point5.Y)
				{
					point3.Y = point5.Y;
				}
				if (point3.Y > point6.Y)
				{
					point3.Y = point6.Y;
				}
				point2 = PanelS.TranslatePoint(point3, PanelSV);
			}
			else if (point4.X < point5.X)
			{
				point4.X = point5.X;
				if (point4.Y < point5.Y)
				{
					point4.Y = point5.Y;
				}
				if (point4.Y > point6.Y)
				{
					point4.Y = point6.Y;
				}
				point2 = PanelV.TranslatePoint(point4, PanelSV);
			}
			ThumbSV.SetValue(Canvas.LeftProperty, point2.X);
			ThumbSV.SetValue(Canvas.TopProperty, point2.Y);
			ColorHSV selectedHSV = ColorPanelColors.SelectedHSV;
			point4 = PanelSV.TranslatePoint(point2, PanelV);
			double num = point7.X - point5.X;
			double num2 = point7.X - point4.X;
			selectedHSV.V = num2 / num;
			double num3 = point6.Y - point5.Y;
			double num4 = num3 * selectedHSV.V;
			double num5 = point5.Y + (num3 + num4) / 2.0 - point4.Y;
			selectedHSV.S = Math.Round(num5 / num4, 6);
			selectedHSV.V = Math.Round(selectedHSV.V, 6);
			ColorPanelColors.SelectedHSV = selectedHSV;
		}

		private void SelectColorChanged()
		{
			ColorHSV selectedHSV = ColorPanelColors.SelectedHSV;
			System.Windows.Point point = PanelSV.Points[2];
			System.Windows.Point point2 = PanelSV.Points[1];
			System.Windows.Point point3 = PanelSV.Points[0];
			System.Windows.Point point4 = default(System.Windows.Point);
			double num = point3.X - point.X;
			point4.X = point3.X - num * selectedHSV.V;
			double num2 = point2.Y - point.Y;
			double num3 = num2 * selectedHSV.V;
			point4.Y = point.Y + (num2 + num3) / 2.0 - num3 * selectedHSV.S;
			System.Windows.Point point5 = PanelV.TranslatePoint(point4, PanelSV);
			if (point5.X != 0.0 && point5.Y != 0.0)
			{
				ThumbSV.SetValue(Canvas.LeftProperty, point5.X);
				ThumbSV.SetValue(Canvas.TopProperty, point5.Y);
			}
		}

		private void DrawColorWheel()
		{
			Canvas colorWheel = xColorWheel;
			List<System.Windows.Media.Color> list = new List<System.Windows.Media.Color>();
			for (byte b = 0; b < byte.MaxValue; b++)
			{
				list.Add(System.Windows.Media.Color.FromRgb(byte.MaxValue, b, 0));
			}
			for (byte b2 = byte.MaxValue; b2 > 0; b2--)
			{
				list.Add(System.Windows.Media.Color.FromRgb(b2, byte.MaxValue, 0));
			}
			for (byte b3 = 0; b3 < byte.MaxValue; b3++)
			{
				list.Add(System.Windows.Media.Color.FromRgb(0, byte.MaxValue, b3));
			}
			for (byte b4 = byte.MaxValue; b4 > 0; b4--)
			{
				list.Add(System.Windows.Media.Color.FromRgb(0, b4, byte.MaxValue));
			}
			for (byte b5 = 0; b5 < byte.MaxValue; b5++)
			{
				list.Add(System.Windows.Media.Color.FromRgb(b5, 0, byte.MaxValue));
			}
			for (byte b6 = byte.MaxValue; b6 > 0; b6--)
			{
				list.Add(System.Windows.Media.Color.FromRgb(byte.MaxValue, 0, b6));
			}
			double centerX = colorWheel.ActualWidth / 2.0;
			double centerY = colorWheel.ActualHeight / 2.0;
			double radius = ((centerX < centerY) ? centerX : centerY) * 0.8;
			double width = ((centerX < centerY) ? centerX : centerY) * 0.4;
			double angel = Math.PI * 2.0 / (double)list.Count;
			double rotate = 0.0;
			System.Windows.Point pointA = default(System.Windows.Point);
			System.Windows.Point pointB = default(System.Windows.Point);
			colorWheel.Children.Clear();
			list.ForEach(delegate(System.Windows.Media.Color color)
			{
				pointA.X = centerX + radius * Math.Cos(rotate);
				pointA.Y = centerY + radius * Math.Sin(rotate);
				rotate += angel;
				pointB.X = centerX + radius * Math.Cos(rotate + angel);
				pointB.Y = centerY + radius * Math.Sin(rotate + angel);
				PathFigure pathFigure2 = new PathFigure
				{
					StartPoint = pointA
				};
				pathFigure2.Segments.Add(new ArcSegment
				{
					Point = pointB,
					Size = new System.Windows.Size(radius, radius)
				});
				PathGeometry data2 = new PathGeometry(new PathFigureCollection { pathFigure2 });
				if (CanChoose)
				{
					colorWheel.Children.Add(new System.Windows.Shapes.Path
					{
						StrokeThickness = width,
						Stroke = new SolidColorBrush(color),
						Data = data2
					});
				}
				else
				{
					colorWheel.Children.Add(new System.Windows.Shapes.Path
					{
						StrokeThickness = width,
						Stroke = new SolidColorBrush(Colors.Gray),
						Data = data2
					});
				}
			});
			if (rotate > 360.0)
			{
				pointA.X = centerX + radius * Math.Cos(0.0);
				pointA.Y = centerY + radius * Math.Sin(0.0);
				pointB.X = centerX + radius * Math.Cos(angel);
				pointB.Y = centerY + radius * Math.Sin(angel);
				PathFigure pathFigure = new PathFigure
				{
					StartPoint = pointA
				};
				pathFigure.Segments.Add(new ArcSegment
				{
					Point = pointB,
					Size = new System.Windows.Size(radius, radius)
				});
				PathGeometry data = new PathGeometry(new PathFigureCollection { pathFigure });
				colorWheel.Children.Add(new System.Windows.Shapes.Path
				{
					StrokeThickness = width,
					Stroke = new SolidColorBrush(list[0]),
					Data = data
				});
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/customusercontrol/colorcontrolpanel.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				xColorPanelHSV = (Grid)target;
				break;
			case 2:
				xColorWheel = (Canvas)target;
				break;
			case 3:
				ThumbH = (Canvas)target;
				break;
			case 4:
				xColorH = (Ellipse)target;
				break;
			case 5:
				PanelHue = (System.Windows.Shapes.Path)target;
				break;
			case 6:
				xColorTriangle = (Canvas)target;
				break;
			case 7:
				PanelS = (Polygon)target;
				break;
			case 8:
				PanelV = (Polygon)target;
				break;
			case 9:
				ThumbSV = (Grid)target;
				break;
			case 10:
				xColorSV = (Ellipse)target;
				break;
			case 11:
				PanelSV = (Polygon)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
	public class ColorHSV
	{
		public double H;

		public double S = 1.0;

		public double V = 1.0;
	}
	public class ControlColors : INotifyPropertyChanged
	{
		private System.Windows.Media.Color newColor = Colors.Red;

		private ColorHSV hsvColor = new ColorHSV
		{
			H = 0.0,
			S = 1.0,
			V = 1.0
		};

		public SolidColorBrush SelectedBrush
		{
			get
			{
				return new SolidColorBrush(newColor);
			}
			set
			{
				newColor = value.Color;
				OnSelectedChanged();
			}
		}

		public string SelectedBrushValue
		{
			get
			{
				return newColor.ToString();
			}
			set
			{
				try
				{
					newColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(value);
				}
				catch (Exception)
				{
				}
				OnSelectedChanged();
			}
		}

		public ColorHSV SelectedHSV
		{
			get
			{
				return hsvColor;
			}
			set
			{
				hsvColor = value;
				OnSelectedChanged("HSV");
			}
		}

		public double SelectedHue
		{
			get
			{
				return hsvColor.H;
			}
			set
			{
				hsvColor.H = value;
				OnSelectedChanged("HSV");
			}
		}

		public double SelectedSat
		{
			get
			{
				return hsvColor.S * 100.0;
			}
			set
			{
				hsvColor.S = value / 100.0;
				OnSelectedChanged("HSV");
			}
		}

		public double SelectedVal
		{
			get
			{
				return hsvColor.V * 100.0;
			}
			set
			{
				hsvColor.V = value / 100.0;
				OnSelectedChanged("HSV");
			}
		}

		public SolidColorBrush SelectedBrushH => new SolidColorBrush(ConvertHsvToRgb(new ColorHSV
		{
			H = hsvColor.H
		}));

		public string SelectedValH
		{
			get
			{
				if (hsvColor.H < 100.0)
				{
					return Math.Round(hsvColor.H, 1).ToString();
				}
				return Math.Round(hsvColor.H).ToString();
			}
		}

		public string SelectedValS
		{
			get
			{
				if (hsvColor.S < 0.01)
				{
					return Math.Round(hsvColor.S * 100.0, 2).ToString();
				}
				return Math.Round(hsvColor.S * 100.0, 1).ToString();
			}
		}

		public string SelectedValV
		{
			get
			{
				if (hsvColor.V < 0.01)
				{
					return Math.Round(hsvColor.V * 100.0, 2).ToString();
				}
				return Math.Round(hsvColor.V * 100.0, 1).ToString();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void Notify(string notifyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(notifyName));
		}

		private void OnInitialChanged()
		{
			Notify("InitialBrush");
		}

		private void OnSelectedChanged(string mode = "RGB")
		{
			if (mode == "RGB")
			{
				hsvColor = ConvertRgbToHsv(newColor);
			}
			else
			{
				if (hsvColor.S > 1.0)
				{
					hsvColor.S = 0.5;
				}
				newColor = ConvertHsvToRgb(hsvColor);
			}
			List<string> list = new List<string>();
			list.Add("SelectedBrush");
			list.Add("SelectedBrushValue");
			list.Add("SelectedHSV");
			list.Add("SelectedHue");
			list.Add("SelectedSat");
			list.Add("SelectedVal");
			list.Add("SelectedBrushH");
			list.ForEach(delegate(string name)
			{
				Notify(name);
			});
		}

		public static ColorHSV ConvertRgbToHsv(System.Windows.Media.Color color)
		{
			ColorHSV colorHSV = new ColorHSV();
			double num = (double)(int)color.R / 255.0;
			double num2 = (double)(int)color.G / 255.0;
			double num3 = (double)(int)color.B / 255.0;
			double num4 = num;
			double num5 = num;
			if (num2 > num5)
			{
				num5 = num2;
			}
			else if (num2 < num4)
			{
				num4 = num2;
			}
			if (num3 > num5)
			{
				num5 = num3;
			}
			else if (num3 < num4)
			{
				num4 = num3;
			}
			colorHSV.V = num5;
			double num6 = num5 - num4;
			if (colorHSV.V == 0.0)
			{
				colorHSV.S = 0.0;
			}
			else
			{
				colorHSV.S = num6 / num5;
			}
			if (colorHSV.S == 0.0)
			{
				colorHSV.H = 0.0;
			}
			else if (num5 == num)
			{
				colorHSV.H = (num2 - num3) / num6;
			}
			else if (num5 == num2)
			{
				colorHSV.H = 2.0 + (num3 - num) / num6;
			}
			else
			{
				colorHSV.H = 4.0 + (num - num2) / num6;
			}
			colorHSV.H *= 60.0;
			if (colorHSV.H < 0.0)
			{
				colorHSV.H += 360.0;
			}
			return colorHSV;
		}

		public static System.Windows.Media.Color ConvertHsvToRgb(ColorHSV colorhsv)
		{
			if (colorhsv.H > 360.0)
			{
				colorhsv.H = 0.0;
			}
			double num = colorhsv.V * colorhsv.S;
			double num2 = colorhsv.H / 60.0;
			double num3 = num * (1.0 - Math.Abs(num2 % 2.0 - 1.0));
			double num4 = colorhsv.V - num;
			double num5;
			double num6;
			double num7;
			switch ((int)num2)
			{
			case 0:
				num5 = num;
				num6 = num3;
				num7 = 0.0;
				break;
			case 1:
				num5 = num3;
				num6 = num;
				num7 = 0.0;
				break;
			case 2:
				num5 = 0.0;
				num6 = num;
				num7 = num3;
				break;
			case 3:
				num5 = 0.0;
				num6 = num3;
				num7 = num;
				break;
			case 4:
				num5 = num3;
				num6 = 0.0;
				num7 = num;
				break;
			default:
				num5 = num;
				num6 = 0.0;
				num7 = num3;
				break;
			}
			num5 += num4;
			num6 += num4;
			num7 += num4;
			num5 *= 255.0;
			num6 *= 255.0;
			num7 *= 255.0;
			return System.Windows.Media.Color.FromRgb((byte)num5, (byte)num6, (byte)num7);
		}
	}
	public class EqTriangleConver : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values.Length < 1)
			{
				return null;
			}
			double num = (double)values[0] / 2.0;
			double num2 = (double)values[1] / 2.0;
			double num3 = ((num < num2) ? num : num2) * 0.8;
			int num4 = 3;
			double num5 = (double)(360 / num4) * Math.PI / 180.0;
			double num6 = 0.0;
			PointCollection pointCollection = new PointCollection();
			while (--num4 >= 0)
			{
				double x = num + num3 * Math.Cos(num6);
				double y = num2 + num3 * Math.Sin(num6);
				pointCollection.Add(new System.Windows.Point(x, y));
				num6 += num5;
			}
			return pointCollection;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class ColorPicker
	{
		public static System.Windows.Media.Color PointColor(System.Windows.Point point)
		{
			IntPtr dC = GetDC(IntPtr.Zero);
			uint pixel = GetPixel(dC, (int)point.X, (int)point.Y);
			ReleaseDC(IntPtr.Zero, dC);
			return System.Windows.Media.Color.FromRgb((byte)(0xFF & pixel), (byte)(0xFF & (pixel >> 8)), (byte)(0xFF & (pixel >> 16)));
		}

		public static BitmapSource ScreenSnapshot()
		{
			using Bitmap bitmap = new Bitmap((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
			}
			IntPtr hbitmap = bitmap.GetHbitmap();
			try
			{
				return Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			}
			finally
			{
				DeleteObject(hbitmap);
			}
		}

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("user32.dll")]
		private static extern IntPtr GetDC(IntPtr hwnd);

		[DllImport("user32.dll")]
		private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

		[DllImport("gdi32.dll")]
		private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
	}
	public class CPU : System.Windows.Controls.UserControl, IComponentConnector
	{
		private System.Windows.Point lastTmpPoint;

		private System.Windows.Point lastFanPoint;

		private double OldAngle;

		public static readonly DependencyProperty MaxCpuFanPropetry = DependencyProperty.Register("MaxCpuFan", typeof(double), typeof(CPU), new PropertyMetadata(3600.0));

		public static readonly DependencyProperty MaxCpuTemPropetry = DependencyProperty.Register("MaxCpuTem", typeof(double), typeof(CPU), new PropertyMetadata(100.0));

		public static readonly DependencyProperty MaxCpuSpdPropetry = DependencyProperty.Register("MaxCpuSpd", typeof(double), typeof(CPU), new PropertyMetadata(3600.0));

		public static readonly DependencyProperty CpuTemPropetry = DependencyProperty.Register("CpuTem", typeof(double), typeof(CPU), new FrameworkPropertyMetadata(60.0, OnCpuTemPropertyChanged));

		public static readonly DependencyProperty CpuFanPropetry = DependencyProperty.Register("CpuFan", typeof(double), typeof(CPU), new FrameworkPropertyMetadata(0.0, OnCpuFanPropertyChanged));

		public static readonly DependencyProperty CpuSpdPropetry = DependencyProperty.Register("CpuSpd", typeof(double), typeof(CPU), new FrameworkPropertyMetadata(0.0, OnCpuSpdPropertyChanged));

		public static readonly DependencyProperty CPUSpdEndAnglePropetry = DependencyProperty.Register("CPUSpdEndAngle", typeof(double), typeof(CPU), new PropertyMetadata(126.7));

		public static readonly DependencyProperty CPUTemEndAnglePropetry = DependencyProperty.Register("CPUTemEndAngle", typeof(double), typeof(CPU), new PropertyMetadata(-100.0));

		public static readonly DependencyProperty CPUFanEndAnglePropetry = DependencyProperty.Register("CPUFanEndAngle", typeof(double), typeof(CPU), new PropertyMetadata(-100.0));

		private double OldSpdAngle;

		private double OldTemAngle;

		private double OldFanAngle;

		private double lastTem;

		private double lastFan;

		private double lastSpd;

		private double r1 = 157.0;

		private double r2 = 258.0;

		private double r3 = 154.0;

		internal CPU uc;

		internal Arc CpuFanArc;

		internal Arc CpuTemArc;

		internal Arc CpuSpdArc;

		private bool _contentLoaded;

		public double MaxCpuFan
		{
			get
			{
				return (double)GetValue(MaxCpuFanPropetry);
			}
			set
			{
				SetValue(MaxCpuFanPropetry, value);
			}
		}

		public double MaxCpuTem
		{
			get
			{
				return (double)GetValue(MaxCpuTemPropetry);
			}
			set
			{
				SetValue(MaxCpuTemPropetry, value);
			}
		}

		public double MaxCpuSpd
		{
			get
			{
				return (double)GetValue(MaxCpuSpdPropetry);
			}
			set
			{
				SetValue(MaxCpuSpdPropetry, value);
			}
		}

		public double CpuTem
		{
			get
			{
				return (double)GetValue(CpuTemPropetry);
			}
			set
			{
				SetValue(CpuTemPropetry, value);
			}
		}

		public double CpuFan
		{
			get
			{
				return (double)GetValue(CpuFanPropetry);
			}
			set
			{
				SetValue(CpuFanPropetry, value);
			}
		}

		public double CpuSpd
		{
			get
			{
				return (double)GetValue(CpuSpdPropetry);
			}
			set
			{
				SetValue(CpuSpdPropetry, value);
			}
		}

		public double CPUSpdEndAngle
		{
			get
			{
				return (double)GetValue(CPUSpdEndAnglePropetry);
			}
			set
			{
				SetValue(CPUSpdEndAnglePropetry, value);
			}
		}

		public double CPUTemEndAngle
		{
			get
			{
				return (double)GetValue(CPUTemEndAnglePropetry);
			}
			set
			{
				SetValue(CPUTemEndAnglePropetry, value);
			}
		}

		public double CPUFanEndAngle
		{
			get
			{
				return (double)GetValue(CPUFanEndAnglePropetry);
			}
			set
			{
				SetValue(CPUFanEndAnglePropetry, value);
			}
		}

		private static void OnCpuTemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CPU obj = (CPU)d;
			obj.UpdateArcSegment(obj, e.Property.Name);
		}

		private static void OnCpuFanPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CPU obj = (CPU)d;
			obj.UpdateArcSegment(obj, e.Property.Name);
		}

		private static void OnCpuSpdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CPU obj = (CPU)d;
			obj.UpdateArcSegment(obj, e.Property.Name);
		}

		private void TransformCpuSpdAngle()
		{
			DoubleAnimation animation = new DoubleAnimation(OldSpdAngle, CPUSpdEndAngle, new Duration(TimeSpan.FromMilliseconds(500.0)));
			CpuSpdArc.BeginAnimation(Arc.EndAngleProperty, animation);
		}

		private void TransformCpuTemAngle()
		{
			DoubleAnimation animation = new DoubleAnimation(OldTemAngle, CPUTemEndAngle, new Duration(TimeSpan.FromMilliseconds(500.0)));
			CpuTemArc.BeginAnimation(Arc.EndAngleProperty, animation);
		}

		private void TransformCpuFanAngle()
		{
			DoubleAnimation animation = new DoubleAnimation(OldFanAngle, CPUFanEndAngle, new Duration(TimeSpan.FromMilliseconds(500.0)));
			CpuFanArc.BeginAnimation(Arc.EndAngleProperty, animation);
		}

		private void UpdateArcSegment(CPU cpu, string PropertyName)
		{
			switch (PropertyName)
			{
			case "CpuTem":
			{
				if (cpu.CpuTem > MaxCpuTem)
				{
					cpu.SetCurrentValue(CpuTemPropetry, MaxCpuTem);
				}
				else if (cpu.CpuTem < 0.0)
				{
					cpu.SetCurrentValue(CpuTemPropetry, 0.0);
				}
				lastTem = (MaxCpuTem - cpu.CpuTem) / MaxCpuTem;
				double num = 70.0;
				double num2 = cpu.CpuTem / MaxCpuTem;
				CPUTemEndAngle = -170.0 + num2 * num;
				TransformCpuTemAngle();
				OldTemAngle = CPUTemEndAngle;
				break;
			}
			case "CpuFan":
			{
				if (cpu.CpuFan > MaxCpuFan)
				{
					cpu.SetCurrentValue(CpuFanPropetry, MaxCpuFan);
				}
				else if (cpu.CpuFan < 0.0)
				{
					cpu.SetCurrentValue(CpuFanPropetry, 0.0);
				}
				lastFan = (MaxCpuFan - cpu.CpuFan) / MaxCpuFan;
				double num = 89.0;
				double num2 = cpu.CpuFan / MaxCpuFan;
				CPUFanEndAngle = -134.5 + num2 * num;
				TransformCpuFanAngle();
				OldFanAngle = CPUFanEndAngle;
				break;
			}
			case "CpuSpd":
			{
				if (cpu.CpuSpd > MaxCpuSpd)
				{
					cpu.SetCurrentValue(CpuSpdPropetry, MaxCpuSpd);
				}
				else if (cpu.CpuSpd < 0.0)
				{
					cpu.SetCurrentValue(CpuSpdPropetry, 0.0);
				}
				lastSpd = (MaxCpuSpd - cpu.CpuSpd) / MaxCpuSpd;
				double num = 253.4;
				double num2 = cpu.CpuSpd / MaxCpuSpd;
				CPUSpdEndAngle = 126.7 - num2 * num;
				TransformCpuSpdAngle();
				OldSpdAngle = CPUSpdEndAngle;
				break;
			}
			}
		}

		private void increase_Click(object sender, RoutedEventArgs e)
		{
			CpuTem += 20.0;
			CpuFan += 400.0;
			CpuSpd += 200.0;
		}

		private void decrease_Click(object sender, RoutedEventArgs e)
		{
			CpuTem -= 20.0;
			CpuFan -= 400.0;
			CpuSpd -= 200.0;
		}

		public System.Windows.Point Circle_Center1(System.Windows.Point p1, System.Windows.Point p2, double dRadius)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 1.0;
			double num6 = 1.0;
			double num7 = 1.0;
			num = (p2.Y - p1.Y) / (p2.X - p1.X);
			System.Windows.Point point = default(System.Windows.Point);
			System.Windows.Point point2 = default(System.Windows.Point);
			if (num == 0.0)
			{
				point.X = (p1.X + p2.X) / 2.0;
				point2.X = (p1.X + p2.X) / 2.0;
				point.Y = p1.Y + Math.Sqrt(dRadius * dRadius - (p1.X - p2.X) * (p1.X - p2.X) / 4.0);
				point2.Y = p2.Y - Math.Sqrt(dRadius * dRadius - (p1.X - p2.X) * (p1.X - p2.X) / 4.0);
			}
			else
			{
				num2 = -1.0 / num;
				num3 = (p1.X + p2.X) / 2.0;
				num4 = (p1.Y + p2.Y) / 2.0;
				num5 = 1.0 + num2 * num2;
				num6 = -2.0 * num3 - num2 * num2 * (p1.X + p2.X);
				num7 = num3 * num3 + num2 * num2 * (p1.X + p2.X) * (p1.X + p2.X) / 4.0 - (dRadius * dRadius - ((num3 - p1.X) * (num3 - p1.X) + (num4 - p1.Y) * (num4 - p1.Y)));
				point.X = (-1.0 * num6 + Math.Sqrt(num6 * num6 - 4.0 * num5 * num7)) / (2.0 * num5);
				point2.X = (-1.0 * num6 - Math.Sqrt(num6 * num6 - 4.0 * num5 * num7)) / (2.0 * num5);
				point.Y = Y_Coordinates(num3, num4, num2, point.X);
				point2.Y = Y_Coordinates(num3, num4, num2, point2.X);
			}
			System.Windows.Point result = default(System.Windows.Point);
			result.X = point2.X;
			result.Y = point2.Y;
			return result;
		}

		public System.Windows.Point Circle_Center2(System.Windows.Point p1, System.Windows.Point p2, double dRadius)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 1.0;
			double num6 = 1.0;
			double num7 = 1.0;
			num = (p2.Y - p1.Y) / (p2.X - p1.X);
			System.Windows.Point point = default(System.Windows.Point);
			System.Windows.Point point2 = default(System.Windows.Point);
			if (num == 0.0)
			{
				point.X = (p1.X + p2.X) / 2.0;
				point2.X = (p1.X + p2.X) / 2.0;
				point.Y = p1.Y + Math.Sqrt(dRadius * dRadius - (p1.X - p2.X) * (p1.X - p2.X) / 4.0);
				point2.Y = p2.Y - Math.Sqrt(dRadius * dRadius - (p1.X - p2.X) * (p1.X - p2.X) / 4.0);
			}
			else
			{
				num2 = -1.0 / num;
				num3 = (p1.X + p2.X) / 2.0;
				num4 = (p1.Y + p2.Y) / 2.0;
				num5 = 1.0 + num2 * num2;
				num6 = -2.0 * num3 - num2 * num2 * (p1.X + p2.X);
				num7 = num3 * num3 + num2 * num2 * (p1.X + p2.X) * (p1.X + p2.X) / 4.0 - (dRadius * dRadius - ((num3 - p1.X) * (num3 - p1.X) + (num4 - p1.Y) * (num4 - p1.Y)));
				point.X = (-1.0 * num6 + Math.Sqrt(num6 * num6 - 4.0 * num5 * num7)) / (2.0 * num5);
				point2.X = (-1.0 * num6 - Math.Sqrt(num6 * num6 - 4.0 * num5 * num7)) / (2.0 * num5);
				point.Y = Y_Coordinates(num3, num4, num2, point.X);
				point2.Y = Y_Coordinates(num3, num4, num2, point2.X);
			}
			System.Windows.Point result = default(System.Windows.Point);
			result.X = point.X;
			result.Y = point.Y;
			return result;
		}

		public double Y_Coordinates(double x, double y, double k, double x0)
		{
			return k * x0 - k * x + y;
		}

		public CPU()
		{
			InitializeComponent();
			CpuTem = 20.0;
			lastTem = 20.0;
			double num = 70.0;
			double num2 = CpuTem / MaxCpuTem;
			OldSpdAngle = -170.0 + num2 * num;
			CpuFan = 3000.0;
			lastFan = (MaxCpuFan - CpuFan) / MaxCpuFan;
			num = 89.0;
			num2 = CpuFan / MaxCpuFan;
			OldSpdAngle = -134.5 + num2 * num;
			CpuSpd = 1200.0;
			lastSpd = 1200.0;
			num = 252.0;
			num2 = CpuSpd / 3600.0;
			OldSpdAngle = 126.0 - num2 * num;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/customusercontrol/cpu.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				uc = (CPU)target;
				break;
			case 2:
				CpuFanArc = (Arc)target;
				break;
			case 3:
				CpuTemArc = (Arc)target;
				break;
			case 4:
				CpuSpdArc = (Arc)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
	public class GPU : System.Windows.Controls.UserControl, IComponentConnector
	{
		private double OldSpdAngle;

		private double OldTemAngle;

		private double OldFanAngle;

		public static readonly DependencyProperty MaxFanSpdPropetry = DependencyProperty.Register("MaxGpuFan", typeof(double), typeof(GPU), new PropertyMetadata(3600.0));

		public static readonly DependencyProperty MaxGpuTemPropetry = DependencyProperty.Register("MaxGpuTem", typeof(double), typeof(GPU), new PropertyMetadata(100.0));

		public static readonly DependencyProperty MaxGpuSpdPropetry = DependencyProperty.Register("MaxGpuSpd", typeof(double), typeof(GPU), new PropertyMetadata(3600.0));

		public static readonly DependencyProperty GpuTemPropetry = DependencyProperty.Register("GpuTem", typeof(double), typeof(GPU), new FrameworkPropertyMetadata(0.0, OnGpuTemPropertyChanged));

		public static readonly DependencyProperty GpuFanPropetry = DependencyProperty.Register("GpuFan", typeof(double), typeof(GPU), new FrameworkPropertyMetadata(0.0, OnGpuFanPropertyChanged));

		public static readonly DependencyProperty GpuSpdPropetry = DependencyProperty.Register("GpuSpd", typeof(double), typeof(GPU), new FrameworkPropertyMetadata(0.0, OnGpuSpdPropertyChanged));

		public static readonly DependencyProperty GPUSpdEndAnglePropetry = DependencyProperty.Register("GPUSpdEndAngle", typeof(double), typeof(GPU), new PropertyMetadata(126.7));

		public static readonly DependencyProperty GPUTemEndAnglePropetry = DependencyProperty.Register("GPUTemEndAngle", typeof(double), typeof(GPU), new PropertyMetadata(-100.0));

		public static readonly DependencyProperty GPUFanEndAnglePropetry = DependencyProperty.Register("GPUFanEndAngle", typeof(double), typeof(GPU), new PropertyMetadata(-100.0));

		private double lastTem;

		private double lastFan;

		private double lastSpd;

		internal GPU uc;

		internal Arc GpuFanArc;

		internal Arc GpuTemArc;

		internal Arc GpuSpdArc;

		internal TextBlock GpuSpdValue;

		internal TextBlock GpuTemValue;

		internal TextBlock GpuFanValue;

		private bool _contentLoaded;

		public double MaxGpuFan
		{
			get
			{
				return (double)GetValue(MaxFanSpdPropetry);
			}
			set
			{
				SetValue(MaxFanSpdPropetry, value);
			}
		}

		public double MaxGpuTem
		{
			get
			{
				return (double)GetValue(MaxGpuTemPropetry);
			}
			set
			{
				SetValue(MaxGpuTemPropetry, value);
			}
		}

		public double MaxGpuSpd
		{
			get
			{
				return (double)GetValue(MaxGpuSpdPropetry);
			}
			set
			{
				SetValue(MaxGpuSpdPropetry, value);
			}
		}

		public double GpuTem
		{
			get
			{
				return (double)GetValue(GpuTemPropetry);
			}
			set
			{
				SetValue(GpuTemPropetry, value);
			}
		}

		public double GpuFan
		{
			get
			{
				return (double)GetValue(GpuFanPropetry);
			}
			set
			{
				SetValue(GpuFanPropetry, value);
			}
		}

		public double GpuSpd
		{
			get
			{
				return (double)GetValue(GpuSpdPropetry);
			}
			set
			{
				SetValue(GpuSpdPropetry, value);
			}
		}

		public double GPUSpdEndAngle
		{
			get
			{
				return (double)GetValue(GPUSpdEndAnglePropetry);
			}
			set
			{
				SetValue(GPUSpdEndAnglePropetry, value);
			}
		}

		public double GPUTemEndAngle
		{
			get
			{
				return (double)GetValue(GPUTemEndAnglePropetry);
			}
			set
			{
				SetValue(GPUTemEndAnglePropetry, value);
			}
		}

		public double GPUFanEndAngle
		{
			get
			{
				return (double)GetValue(GPUFanEndAnglePropetry);
			}
			set
			{
				SetValue(GPUFanEndAnglePropetry, value);
			}
		}

		private static void OnGpuTemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GPU obj = (GPU)d;
			obj.UpdateArcSegment(obj, e.Property.Name);
		}

		private static void OnGpuFanPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GPU obj = (GPU)d;
			obj.UpdateArcSegment(obj, e.Property.Name);
		}

		private static void OnGpuSpdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GPU obj = (GPU)d;
			obj.UpdateArcSegment(obj, e.Property.Name);
		}

		private void TransformGpuSpdAngle()
		{
			DoubleAnimation animation = new DoubleAnimation(OldSpdAngle, GPUSpdEndAngle, new Duration(TimeSpan.FromMilliseconds(500.0)));
			GpuSpdArc.BeginAnimation(Arc.EndAngleProperty, animation);
		}

		private void TransformGpuTemAngle()
		{
			DoubleAnimation animation = new DoubleAnimation(OldTemAngle, GPUTemEndAngle, new Duration(TimeSpan.FromMilliseconds(500.0)));
			GpuTemArc.BeginAnimation(Arc.EndAngleProperty, animation);
		}

		private void TransformGpuFanAngle()
		{
			DoubleAnimation animation = new DoubleAnimation(OldFanAngle, GPUFanEndAngle, new Duration(TimeSpan.FromMilliseconds(500.0)));
			GpuFanArc.BeginAnimation(Arc.EndAngleProperty, animation);
		}

		private void UpdateArcSegment(GPU gpu, string PropertyName)
		{
			switch (PropertyName)
			{
			case "GpuTem":
			{
				if (gpu.GpuTem > MaxGpuTem)
				{
					gpu.SetCurrentValue(GpuTemPropetry, MaxGpuTem);
				}
				else if (gpu.GpuTem < 0.0)
				{
					gpu.SetCurrentValue(GpuTemPropetry, 0.0);
				}
				lastTem = (MaxGpuTem - gpu.GpuTem) / MaxGpuTem;
				double num = 69.0;
				double num2 = gpu.GpuTem / MaxGpuTem;
				GPUTemEndAngle = -169.0 + num2 * num;
				TransformGpuTemAngle();
				OldTemAngle = GPUTemEndAngle;
				break;
			}
			case "GpuFan":
			{
				if (gpu.GpuFan > MaxGpuFan)
				{
					gpu.SetCurrentValue(GpuFanPropetry, MaxGpuFan);
				}
				else if (gpu.GpuFan < 0.0)
				{
					gpu.SetCurrentValue(GpuFanPropetry, 0.0);
				}
				lastFan = (MaxGpuFan - gpu.GpuFan) / MaxGpuFan;
				double num = 89.0;
				double num2 = gpu.GpuFan / MaxGpuFan;
				GPUFanEndAngle = -134.5 + num2 * num;
				TransformGpuFanAngle();
				OldFanAngle = GPUFanEndAngle;
				break;
			}
			case "GpuSpd":
			{
				if (gpu.GpuSpd > MaxGpuSpd)
				{
					gpu.SetCurrentValue(GpuSpdPropetry, MaxGpuSpd);
				}
				else if (gpu.GpuSpd < 0.0)
				{
					gpu.SetCurrentValue(GpuSpdPropetry, 0.0);
				}
				lastSpd = (MaxGpuSpd - gpu.GpuSpd) / MaxGpuSpd;
				double num = 253.4;
				double num2 = gpu.GpuSpd / MaxGpuSpd;
				GPUSpdEndAngle = 126.7 - num2 * num;
				TransformGpuSpdAngle();
				OldSpdAngle = GPUSpdEndAngle;
				break;
			}
			}
		}

		private void increase_Click(object sender, RoutedEventArgs e)
		{
			GpuTem += 20.0;
			GpuFan += 400.0;
			GpuSpd += 200.0;
		}

		private void decrease_Click(object sender, RoutedEventArgs e)
		{
			GpuTem -= 20.0;
			GpuFan -= 400.0;
			GpuSpd -= 200.0;
		}

		public GPU()
		{
			InitializeComponent();
			GpuTem = 20.0;
			lastTem = 20.0;
			double num = 69.0;
			double num2 = GpuTem / MaxGpuTem;
			OldSpdAngle = -169.0 + num2 * num;
			GpuFan = 3000.0;
			lastFan = (MaxGpuFan - GpuFan) / MaxGpuFan;
			num = 89.0;
			num2 = GpuFan / MaxGpuFan;
			OldSpdAngle = -134.5 + num2 * num;
			GpuSpd = 1200.0;
			lastSpd = 1200.0;
			num = 252.0;
			num2 = GpuSpd / 3600.0;
			OldSpdAngle = 126.0 - num2 * num;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/customusercontrol/gpu.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				uc = (GPU)target;
				break;
			case 2:
				GpuFanArc = (Arc)target;
				break;
			case 3:
				GpuTemArc = (Arc)target;
				break;
			case 4:
				GpuSpdArc = (Arc)target;
				break;
			case 5:
				GpuSpdValue = (TextBlock)target;
				break;
			case 6:
				GpuTemValue = (TextBlock)target;
				break;
			case 7:
				GpuFanValue = (TextBlock)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
	public class moi_progress : System.Windows.Controls.UserControl, IComponentConnector
	{
		public static readonly DependencyProperty ItemValueProperty = DependencyProperty.Register("ItemValue", typeof(int), typeof(moi_progress), new PropertyMetadata(0, OnValuePropertyChanged));

		internal System.Windows.Shapes.Path path;

		internal System.Windows.Controls.Label lbValue;

		private bool _contentLoaded;

		public int ItemValue
		{
			get
			{
				return (int)GetValue(ItemValueProperty);
			}
			set
			{
				SetValue(ItemValueProperty, value);
			}
		}

		private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				moi_progress obj = (moi_progress)d;
				obj.SetValue(obj.ItemValue);
			}
			catch (Exception ex)
			{
				BydCentral.Services.LoggerHelper._.Error("set value err" + ex.Message);
			}
		}

		private void SetValue(int percentValue)
		{
			double num = (double)percentValue * 3.6;
			double num2 = 45.0;
			double num3 = 56.0;
			double num4 = 10.0;
			double num5 = 54.0;
			double num6 = 10.0;
			if (percentValue > 100)
			{
				percentValue = 100;
			}
			if (percentValue < 50)
			{
				path.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(50, 250, 34));
			}
			else if (percentValue < 75)
			{
				path.Stroke = new SolidColorBrush(Colors.Yellow);
			}
			else if (percentValue <= 100)
			{
				path.Stroke = new SolidColorBrush(Colors.Red);
			}
			lbValue.Content = percentValue.ToString("0") + "%";
			bool isLargeArc = false;
			if (num <= 90.0)
			{
				double num7 = (90.0 - num) * Math.PI / 180.0;
				num5 = num3 + Math.Cos(num7) * num2;
				num6 = num4 + num2 - Math.Sin(num7) * num2;
			}
			else if (num <= 180.0)
			{
				double num8 = (num - 90.0) * Math.PI / 180.0;
				num5 = num3 + Math.Cos(num8) * num2;
				num6 = num4 + num2 + Math.Sin(num8) * num2;
			}
			else if (num <= 270.0)
			{
				isLargeArc = true;
				double num9 = (num - 180.0) * Math.PI / 180.0;
				num5 = num3 - Math.Sin(num9) * num2;
				num6 = num4 + num2 + Math.Cos(num9) * num2;
			}
			else if (num < 360.0)
			{
				isLargeArc = true;
				double num10 = (num - 270.0) * Math.PI / 180.0;
				num5 = num3 - Math.Cos(num10) * num2;
				num6 = num4 + num2 - Math.Sin(num10) * num2;
			}
			else
			{
				isLargeArc = true;
				num5 = num3 - 0.001;
				num6 = num4;
			}
			System.Windows.Point point = new System.Windows.Point(num5, num6);
			System.Windows.Size size = new System.Windows.Size(num2, num2);
			SweepDirection sweepDirection = SweepDirection.Clockwise;
			ArcSegment value = new ArcSegment(point, size, 0.0, isLargeArc, sweepDirection, isStroked: true);
			PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
			pathSegmentCollection.Add(value);
			PathFigure pathFigure = new PathFigure();
			pathFigure.StartPoint = new System.Windows.Point(num3, num4);
			pathFigure.Segments = pathSegmentCollection;
			PathFigureCollection pathFigureCollection = new PathFigureCollection();
			pathFigureCollection.Add(pathFigure);
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.Figures = pathFigureCollection;
			path.Data = pathGeometry;
			if (num == 360.0)
			{
				path.Data = Geometry.Combine(path.Data, new PathGeometry(), GeometryCombineMode.Union, null);
			}
		}

		public moi_progress()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/customusercontrol/moi_progress.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				path = (System.Windows.Shapes.Path)target;
				break;
			case 2:
				lbValue = (System.Windows.Controls.Label)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
	public class progress_infinix : System.Windows.Controls.UserControl, IComponentConnector
	{
		public static readonly DependencyProperty ProNameProperty = DependencyProperty.Register("Name", typeof(string), typeof(progress_infinix), new FrameworkPropertyMetadata("cpu", OnNamePropertyChanged));

		public static readonly DependencyProperty MaxFanSpdPropetry = DependencyProperty.Register("MaxFan", typeof(double), typeof(progress_infinix), new PropertyMetadata(4800.0));

		public static readonly DependencyProperty GpuFanPropetry = DependencyProperty.Register("Fan", typeof(double), typeof(progress_infinix), new FrameworkPropertyMetadata(0.0, OnFanPropertyChanged));

		internal System.Windows.Shapes.Rectangle p0;

		internal System.Windows.Shapes.Rectangle p1;

		internal System.Windows.Shapes.Rectangle p2;

		internal System.Windows.Shapes.Rectangle p3;

		internal System.Windows.Shapes.Rectangle p4;

		internal System.Windows.Shapes.Rectangle p5;

		internal System.Windows.Shapes.Rectangle p6;

		internal System.Windows.Shapes.Rectangle p7;

		internal System.Windows.Shapes.Rectangle p8;

		internal System.Windows.Shapes.Rectangle p9;

		internal System.Windows.Shapes.Rectangle p10;

		internal System.Windows.Shapes.Rectangle p11;

		internal System.Windows.Shapes.Rectangle p12;

		internal System.Windows.Shapes.Rectangle p13;

		internal System.Windows.Shapes.Rectangle p14;

		internal System.Windows.Shapes.Rectangle p15;

		internal System.Windows.Shapes.Rectangle p16;

		internal System.Windows.Shapes.Rectangle p17;

		internal System.Windows.Shapes.Rectangle p18;

		internal System.Windows.Shapes.Rectangle p19;

		internal System.Windows.Shapes.Rectangle p20;

		internal System.Windows.Shapes.Rectangle p21;

		internal System.Windows.Shapes.Rectangle p22;

		internal System.Windows.Shapes.Rectangle p23;

		internal TextBlock ProgressName;

		private bool _contentLoaded;

		public string ProName
		{
			get
			{
				return (string)GetValue(ProNameProperty);
			}
			set
			{
				SetValue(ProNameProperty, value);
			}
		}

		public double MaxFan
		{
			get
			{
				return (double)GetValue(MaxFanSpdPropetry);
			}
			set
			{
				SetValue(MaxFanSpdPropetry, value);
			}
		}

		public double Fan
		{
			get
			{
				return (double)GetValue(GpuFanPropetry);
			}
			set
			{
				SetValue(GpuFanPropetry, value);
			}
		}

		private static void OnNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			progress_infinix progress_infinix2 = (progress_infinix)d;
			progress_infinix2.ProgressName.Text = progress_infinix2.ProName;
		}

		private static void OnFanPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			progress_infinix obj = (progress_infinix)d;
			obj.UpdateSpd(obj);
		}

		private async System.Threading.Tasks.Task UpdateSpd(progress_infinix f)
		{
			SolidColorBrush colorS = new SolidColorBrush(System.Windows.Media.Color.FromRgb(48, 179, 235));
			SolidColorBrush colorN = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 106, 149));
			double num = f.Fan * 100.0 / f.MaxFan;
			int s = (int)(num / 4.0);
			for (int i = 23; i >= 0; i--)
			{
				string name = "p" + i;
				System.Windows.Shapes.Rectangle rectangle = FindName(name) as System.Windows.Shapes.Rectangle;
				if (i > s)
				{
					rectangle.Fill = colorN;
				}
				else
				{
					rectangle.Fill = colorS;
				}
				await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.01));
			}
		}

		public progress_infinix()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/customusercontrol/progress_infinix.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				p0 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 2:
				p1 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 3:
				p2 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 4:
				p3 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 5:
				p4 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 6:
				p5 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 7:
				p6 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 8:
				p7 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 9:
				p8 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 10:
				p9 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 11:
				p10 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 12:
				p11 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 13:
				p12 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 14:
				p13 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 15:
				p14 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 16:
				p15 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 17:
				p16 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 18:
				p17 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 19:
				p18 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 20:
				p19 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 21:
				p20 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 22:
				p21 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 23:
				p22 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 24:
				p23 = (System.Windows.Shapes.Rectangle)target;
				break;
			case 25:
				ProgressName = (TextBlock)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
namespace BydCentral
{
	public class App : System.Windows.Application
	{
		private MainWindow mainWindow;

		private const string MemoryMapName = "ControlCenterInt";

		private const int SharedDataSize = 8;

		private MemoryMappedFile memoryMappedFile;

		private AppControl app = new AppControl();

		private PopupImageWindow popupImageWindow;

		public System.Windows.Forms.NotifyIcon notifyIcon;

		private bool _contentLoaded;

		public new static App Current => (App)System.Windows.Application.Current;

		public IServiceProvider Services { get; }

		private static Process RunningInstance()
		{
			Process currentProcess = Process.GetCurrentProcess();
			Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
			foreach (Process process in processesByName)
			{
				if (process.Id != currentProcess.Id)
				{
					return process;
				}
			}
			return null;
		}

		private async System.Threading.Tasks.Task MonitorSharedData()
		{
			while (true)
			{
				using (MemoryMappedViewAccessor memoryMappedViewAccessor = memoryMappedFile.CreateViewAccessor(0L, 4L))
				{
					int value = memoryMappedViewAccessor.ReadInt32(0L);
					if (value != 1)
					{
						base.Dispatcher.Invoke(delegate
						{
							ExecuteMethod(value);
						});
						memoryMappedViewAccessor.Write(0L, 1);
					}
				}
				using (MemoryMappedViewAccessor memoryMappedViewAccessor2 = memoryMappedFile.CreateViewAccessor(4L, 4L))
				{
					if (memoryMappedViewAccessor2.ReadInt32(0L) != 1)
					{
						base.Dispatcher.Invoke(delegate
						{
							mainWindow.Dispose();
							System.Windows.Application.Current.Shutdown();
						});
					}
				}
				await System.Threading.Tasks.Task.Delay(500);
			}
		}

		private void ExecuteMethod(int value)
		{
			ExecuteCustomMethod();
		}

		public void IncreaseSharedData()
		{
			using MemoryMappedViewAccessor memoryMappedViewAccessor = memoryMappedFile.CreateViewAccessor();
			int num = memoryMappedViewAccessor.ReadInt32(0L);
			num++;
			memoryMappedViewAccessor.Write(0L, num);
		}

		public App()
		{
			base.Startup += App_Startup;
			Services = GetServices();
		}

		private void App_Startup(object sender, StartupEventArgs e)
		{
			base.DispatcherUnhandledException += App_DispatcherUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		}

		private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			try
			{
				e.Handled = true;
				BydCentral.Services.LoggerHelper._.Error("UI线程异常:" + e.Exception.Message);
			}
			catch (Exception ex)
			{
				BydCentral.Services.LoggerHelper._.Error("UI线程发生致命错误！" + ex.ToString());
			}
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (e.IsTerminating)
			{
				stringBuilder.Append("非UI线程发生致命错误");
				BydCentral.Services.LoggerHelper._.Error(stringBuilder.ToString());
			}
			stringBuilder.Append("非UI线程异常：");
			if (e.ExceptionObject is Exception)
			{
				stringBuilder.Append(((Exception)e.ExceptionObject).Message);
			}
			else
			{
				stringBuilder.Append(e.ExceptionObject);
			}
			BydCentral.Services.LoggerHelper._.Error(stringBuilder.ToString());
		}

		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			BydCentral.Services.LoggerHelper._.Error("Task线程异常：" + e.Exception.Message);
			e.SetObserved();
		}

		private void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
		{
			mainWindow.Dispose();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			memoryMappedFile = MemoryMappedFile.CreateOrOpen("ControlCenterInt", 8L);
			if (RunningInstance() != null)
			{
				IncreaseSharedData();
				Shutdown();
				return;
			}
			base.OnStartup(e);
			InitLanguage();
			createNotifyIcon();
			NavigationCommands.BrowseBack.InputGestures.Clear();
			using (MemoryMappedViewAccessor memoryMappedViewAccessor = memoryMappedFile.CreateViewAccessor(0L, 4L))
			{
				memoryMappedViewAccessor.Write(0L, 1);
			}
			using (MemoryMappedViewAccessor memoryMappedViewAccessor2 = memoryMappedFile.CreateViewAccessor(4L, 4L))
			{
				memoryMappedViewAccessor2.Write(0L, 1);
			}
			MonitorSharedData();
			mainWindow = new MainWindow();
			popupImageWindow = new PopupImageWindow();
			if (e.Args.Contains("-minimized"))
			{
				mainWindow.WindowState = WindowState.Minimized;
				mainWindow.Show();
				mainWindow.Hide();
			}
			else
			{
				mainWindow.Show();
			}
		}

		private void InitLanguage()
		{
			try
			{
				ResourceDictionary item = new ResourceDictionary
				{
					Source = new Uri("Language\\en-US.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item);
			}
			catch
			{
			}
		}

		private static IServiceProvider GetServices()
		{
			ServiceCollection services = new ServiceCollection();
			services.AddScoped<Page1ViewModel>();
			services.AddScoped<Page2ViewModel>();
			services.AddScoped<Page3ViewModel>();
			return services.BuildServiceProvider();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
		}

		private void ExecuteCustomMethod()
		{
			if (mainWindow != null)
			{
				mainWindow.Show();
				if (mainWindow.WindowState == WindowState.Minimized)
				{
					mainWindow.WindowState = WindowState.Normal;
				}
			}
		}

		private void TaskbarIcon_TrayMouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ExecuteCustomMethod();
		}

		private void createNotifyIcon()
		{
			notifyIcon = new System.Windows.Forms.NotifyIcon();
			notifyIcon.Text = (string)System.Windows.Application.Current.FindResource("Central");
			Icon icon = new Icon(System.Windows.Application.GetResourceStream(new Uri("/favicon.ico", UriKind.Relative)).Stream);
			notifyIcon.Icon = icon;
			notifyIcon.Visible = true;
			notifyIcon.MouseDoubleClick += TaskbarIcon_TrayMouseDoubleClick;
			contextMenu();
		}

		private void contextMenu()
		{
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			notifyIcon.ContextMenuStrip = contextMenuStrip;
			ToolStripMenuItem exitMenuItem = new ToolStripMenuItem((string)System.Windows.Application.Current.FindResource("Exit"), null, exitMenuItem_Click);
			ToolStripMenuItem versionMenuItem = new ToolStripMenuItem((string)System.Windows.Application.Current.FindResource("VersionInfo"));
			versionMenuItem.Enabled = false;
			ToolStripMenuItem LanguageItem = new ToolStripMenuItem((string)System.Windows.Application.Current.FindResource("Language"));
			ToolStripMenuItem showMenuItem = new ToolStripMenuItem((string)System.Windows.Application.Current.FindResource("ShowMainWindow"), null, showMenuItem_Click);
			ToolStripMenuItem autoStartItem = new ToolStripMenuItem((string)System.Windows.Application.Current.FindResource("AutoRestart"), null, AutoStartMenuItem_Click);
			contextMenuStrip.Items.Add(showMenuItem);
			contextMenuStrip.Items.Add(autoStartItem);
			contextMenuStrip.Items.Add(versionMenuItem);
			contextMenuStrip.Items.Add(exitMenuItem);
			if (getAutoStartStatus())
			{
				autoStartItem.Checked = true;
			}
			ToolStripMenuItem en_Item = new ToolStripMenuItem("English");
			ToolStripMenuItem zh_cn_Item = new ToolStripMenuItem("简体中文");
			ToolStripMenuItem ar_Item = new ToolStripMenuItem("العربية");
			ToolStripMenuItem fr_Item = new ToolStripMenuItem("Français");
			ToolStripMenuItem th_Item = new ToolStripMenuItem("ไทย");
			ToolStripMenuItem id_Item = new ToolStripMenuItem("Indonesia");
			ToolStripMenuItem ru_Item = new ToolStripMenuItem("Русский");
			ToolStripMenuItem hi_Item = new ToolStripMenuItem("ह\u093f\u0902द\u0940");
			ToolStripMenuItem en_PH_Item = new ToolStripMenuItem("Pilipino");
			ToolStripMenuItem ms_Item = new ToolStripMenuItem("Bahasa Melayu");
			ToolStripMenuItem tr_Item = new ToolStripMenuItem("Türkçe");
			ToolStripMenuItem pt_Item = new ToolStripMenuItem("Português");
			ToolStripMenuItem pl_Item = new ToolStripMenuItem("Polski");
			LanguageItem.DropDownItems.Add(en_Item);
			LanguageItem.DropDownItems.Add(zh_cn_Item);
			LanguageItem.DropDownItems.Add(ar_Item);
			LanguageItem.DropDownItems.Add(fr_Item);
			LanguageItem.DropDownItems.Add(th_Item);
			LanguageItem.DropDownItems.Add(id_Item);
			LanguageItem.DropDownItems.Add(ru_Item);
			LanguageItem.DropDownItems.Add(hi_Item);
			LanguageItem.DropDownItems.Add(en_PH_Item);
			LanguageItem.DropDownItems.Add(ms_Item);
			LanguageItem.DropDownItems.Add(tr_Item);
			LanguageItem.DropDownItems.Add(pt_Item);
			LanguageItem.DropDownItems.Add(pl_Item);
			en_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, en_Item, 0u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			zh_cn_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, zh_cn_Item, 1u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			ar_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, ar_Item, 2u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			fr_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, fr_Item, 3u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			th_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, th_Item, 4u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			id_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, id_Item, 5u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			ru_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, ru_Item, 6u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			hi_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, hi_Item, 7u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			en_PH_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, en_PH_Item, 8u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			ms_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, ms_Item, 9u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			tr_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, tr_Item, 10u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			pt_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, pt_Item, 11u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			pl_Item.Click += delegate(object? sender, EventArgs e)
			{
				Language_SelectedChanged(sender, e, pl_Item, 12u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
			};
			string text = app.INIRead("system", "language");
			if (text == "no config item")
			{
				text = CultureInfo.CurrentCulture.Name;
			}
			string text2 = text.Substring(0, Math.Min(2, text.Length)).ToLower();
			if (text2 == null)
			{
				return;
			}
			int length = text2.Length;
			if (length != 2)
			{
				return;
			}
			switch (text2[0])
			{
			case 'e':
				if (text2 == "en")
				{
					if (text == "en-US")
					{
						en_Item.Checked = true;
					}
					else if (text == "en-PH")
					{
						en_PH_Item.Checked = true;
					}
				}
				break;
			case 'z':
				if (text2 == "zh")
				{
					zh_cn_Item.Checked = true;
				}
				break;
			case 'a':
				if (text2 == "ar")
				{
					ar_Item.Checked = true;
				}
				break;
			case 'f':
				if (text2 == "fr")
				{
					fr_Item.Checked = true;
				}
				break;
			case 't':
				if (!(text2 == "th"))
				{
					if (text2 == "tr")
					{
						tr_Item.Checked = true;
					}
				}
				else
				{
					th_Item.Checked = true;
				}
				break;
			case 'i':
				if (text2 == "id")
				{
					id_Item.Checked = true;
				}
				break;
			case 'r':
				if (text2 == "ru")
				{
					ru_Item.Checked = true;
				}
				break;
			case 'h':
				if (text2 == "hi")
				{
					hi_Item.Checked = true;
				}
				break;
			case 'm':
				if (text2 == "ms")
				{
					ms_Item.Checked = true;
				}
				break;
			case 'p':
				if (!(text2 == "pt"))
				{
					if (text2 == "pl")
					{
						pl_Item.Checked = true;
					}
				}
				else
				{
					pt_Item.Checked = true;
				}
				break;
			}
		}

		private void Language_SelectedChanged(object sender, EventArgs e, ToolStripMenuItem menuItem, uint temp, ToolStripMenuItem a, ToolStripMenuItem b, ToolStripMenuItem c, ToolStripMenuItem d, ToolStripMenuItem f)
		{
			if (menuItem.Checked)
			{
				return;
			}
			menuItem.Checked = true;
			foreach (ToolStripMenuItem dropDownItem in ((ToolStripMenuItem)menuItem.OwnerItem).DropDownItems)
			{
				if (dropDownItem != menuItem)
				{
					dropDownItem.Checked = false;
				}
			}
			switch (temp)
			{
			case 0u:
			{
				ResourceDictionary item13 = new ResourceDictionary
				{
					Source = new Uri("Language\\en-US.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item13);
				app.INIWrite("system", "language", "en-US");
				break;
			}
			case 1u:
			{
				ResourceDictionary item12 = new ResourceDictionary
				{
					Source = new Uri("Language\\zh.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item12);
				app.INIWrite("system", "language", "zh");
				break;
			}
			case 2u:
			{
				ResourceDictionary item11 = new ResourceDictionary
				{
					Source = new Uri("Language\\ar.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item11);
				app.INIWrite("system", "language", "ar");
				break;
			}
			case 3u:
			{
				ResourceDictionary item10 = new ResourceDictionary
				{
					Source = new Uri("Language\\fr.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item10);
				app.INIWrite("system", "language", "fr");
				break;
			}
			case 4u:
			{
				ResourceDictionary item9 = new ResourceDictionary
				{
					Source = new Uri("Language\\th.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item9);
				app.INIWrite("system", "language", "th");
				break;
			}
			case 5u:
			{
				ResourceDictionary item8 = new ResourceDictionary
				{
					Source = new Uri("Language\\id.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item8);
				app.INIWrite("system", "language", "id");
				break;
			}
			case 6u:
			{
				ResourceDictionary item7 = new ResourceDictionary
				{
					Source = new Uri("Language\\ru.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item7);
				app.INIWrite("system", "language", "ru");
				break;
			}
			case 7u:
			{
				ResourceDictionary item6 = new ResourceDictionary
				{
					Source = new Uri("Language\\hi.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item6);
				app.INIWrite("system", "language", "hi");
				break;
			}
			case 8u:
			{
				ResourceDictionary item5 = new ResourceDictionary
				{
					Source = new Uri("Language\\en-PH.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item5);
				app.INIWrite("system", "language", "en-PH");
				break;
			}
			case 9u:
			{
				ResourceDictionary item4 = new ResourceDictionary
				{
					Source = new Uri("Language\\ms.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item4);
				app.INIWrite("system", "language", "ms");
				break;
			}
			case 10u:
			{
				ResourceDictionary item3 = new ResourceDictionary
				{
					Source = new Uri("Language\\tr.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item3);
				app.INIWrite("system", "language", "tr");
				break;
			}
			case 11u:
			{
				ResourceDictionary item2 = new ResourceDictionary
				{
					Source = new Uri("Language\\pt.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item2);
				app.INIWrite("system", "language", "pt");
				break;
			}
			case 12u:
			{
				ResourceDictionary item = new ResourceDictionary
				{
					Source = new Uri("Language\\pl.xaml", UriKind.Relative)
				};
				base.Resources.MergedDictionaries.Add(item);
				app.INIWrite("system", "language", "pl");
				break;
			}
			}
			a.Text = (string)System.Windows.Application.Current.FindResource("Exit");
			b.Text = (string)System.Windows.Application.Current.FindResource("VersionInfo");
			c.Text = (string)System.Windows.Application.Current.FindResource("Language");
			d.Text = (string)System.Windows.Application.Current.FindResource("ShowMainWindow");
			f.Text = (string)System.Windows.Application.Current.FindResource("AutoRestart");
			notifyIcon.Text = (string)System.Windows.Application.Current.FindResource("Central");
		}

		private void UpdateUI()
		{
		}

		private bool getAutoStartStatus()
		{
			try
			{
				string text = app.INIRead("System", "AutoStart");
				if (text == "Y")
				{
					app.setAutoTask(autoTask: true);
					return true;
				}
				if (text == "no config item")
				{
					app.setAutoTask(autoTask: true);
					return true;
				}
				return false;
			}
			catch (Exception)
			{
				return true;
			}
		}

		private void AutoStartMenuItem_Click(object sender, EventArgs e)
		{
			if (getAutoStartStatus())
			{
				app.INIWrite("System", "AutoStart", "N");
				app.setAutoTask(autoTask: false);
			}
			else
			{
				app.INIWrite("System", "AutoStart", "Y");
				app.setAutoTask(autoTask: true);
			}
			((ToolStripMenuItem)sender).Checked = getAutoStartStatus();
		}

		private void zh_cn_Item_Click(object sender, EventArgs e)
		{
			base.Resources.MergedDictionaries.Clear();
			ResourceDictionary item = new ResourceDictionary
			{
				Source = new Uri("Language\\zh.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item);
		}

		private void en_Item_Click(object sender, EventArgs e)
		{
			base.Resources.MergedDictionaries.Clear();
			ResourceDictionary item = new ResourceDictionary
			{
				Source = new Uri("Language\\en-US.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item);
		}

		private void fr_Item_Click(object sender, EventArgs e)
		{
			base.Resources.MergedDictionaries.Clear();
			ResourceDictionary item = new ResourceDictionary
			{
				Source = new Uri("Language\\fr.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item);
		}

		private void th_Item_Click(object sender, EventArgs e)
		{
		}

		private void showMenuItem_Click(object sender, EventArgs e)
		{
			ExecuteCustomMethod();
		}

		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			mainWindow.Dispose();
			notifyIcon.Dispose();
			Thread.Sleep(300);
			System.Windows.Application.Current.Shutdown();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/app.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[STAThread]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public static void Main()
		{
			App obj = new App();
			obj.InitializeComponent();
			obj.Run();
		}
	}
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

		private HandyControl.Controls.NotifyIcon notifyIcon;

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

		internal System.Windows.Controls.Button minilize;

		internal Grid MainGrid;

		internal Grid GirdSide;

		internal RowDefinition LightGrid;

		internal RowDefinition UpdateGrid;

		internal System.Windows.Controls.Button button1;

		internal ImageBrush image1;

		internal System.Windows.Controls.Button button2;

		internal ImageBrush image2;

		internal System.Windows.Controls.Button button3;

		internal ImageBrush image3;

		internal System.Windows.Controls.Button button4;

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
				System.Windows.Application.Current.Dispatcher.Invoke(delegate
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

		private async System.Threading.Tasks.Task GCCollect()
		{
			int i = 0;
			while (isMini && i < 2)
			{
				await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(20.0));
				await System.Threading.Tasks.Task.Run(delegate
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
					await System.Threading.Tasks.Task.Delay(2000);
					page2.GetVersion();
					GlobalVars.Wmi.inMS(intoMS: false);
				}
			}
			catch (Exception ex)
			{
				BydCentral.Services.LoggerHelper._.Error("cath error" + ex.ToString());
			}
		}

		private void Page1Button_Click(object sender, RoutedEventArgs e)
		{
			cur = 1;
			contentFrame.Navigate(page1);
			image1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/lig_b.png", UriKind.RelativeOrAbsolute));
			image2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/set.png", UriKind.RelativeOrAbsolute));
			image3.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/mod.png", UriKind.RelativeOrAbsolute));
			button1.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(48, 179, 235));
			button2.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(191, 191, 191));
			button3.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(191, 191, 191));
		}

		private void Page2Button_Click(object sender, RoutedEventArgs e)
		{
			cur = 2;
			contentFrame.Navigate(page2);
			image1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/lig.png", UriKind.RelativeOrAbsolute));
			image2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/set_b.png", UriKind.RelativeOrAbsolute));
			image3.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/mod.png", UriKind.RelativeOrAbsolute));
			button1.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(191, 191, 191));
			button2.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(48, 179, 235));
			button3.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(191, 191, 191));
		}

		private void Page3Button_Click(object sender, RoutedEventArgs e)
		{
			cur = 3;
			contentFrame.Navigate(page3);
			image1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/lig.png", UriKind.RelativeOrAbsolute));
			image2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/set.png", UriKind.RelativeOrAbsolute));
			image3.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/mod_b.png", UriKind.RelativeOrAbsolute));
			button1.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(191, 191, 191));
			button2.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(191, 191, 191));
			button3.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(48, 179, 235));
		}

		private void Page4Button_Click(object sender, RoutedEventArgs e)
		{
			cur = 4;
			image1.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/lig.png", UriKind.RelativeOrAbsolute));
			image2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/set.png", UriKind.RelativeOrAbsolute));
			image3.ImageSource = new BitmapImage(new Uri("pack://application:,,,/image/dark/mod.png", UriKind.RelativeOrAbsolute));
			button1.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(191, 191, 191));
			button2.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(191, 191, 191));
			button3.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(191, 191, 191));
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			System.Threading.Tasks.Task.Run(delegate
			{
				Page1ViewModel.commands[0].Execute(null);
				Page2ViewModel.GetCOMCommand.Execute(null);
				Page3ViewModel.GetInfoCommand.Execute(null);
				Page3ViewModel.GpuModeCommand.Execute(null);
			});
			System.Windows.Application.Current.Dispatcher.Invoke(delegate
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
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				minilize = (System.Windows.Controls.Button)target;
				minilize.Click += Button_Click;
				break;
			case 5:
				((System.Windows.Controls.Button)target).Click += Button_Click_1;
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
				button1 = (System.Windows.Controls.Button)target;
				button1.Click += Page1Button_Click;
				break;
			case 11:
				image1 = (ImageBrush)target;
				break;
			case 12:
				button2 = (System.Windows.Controls.Button)target;
				button2.Click += Page2Button_Click;
				break;
			case 13:
				image2 = (ImageBrush)target;
				break;
			case 14:
				button3 = (System.Windows.Controls.Button)target;
				button3.Click += Page3Button_Click;
				break;
			case 15:
				image3 = (ImageBrush)target;
				break;
			case 16:
				button4 = (System.Windows.Controls.Button)target;
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
}
namespace BydCentral.Views
{
	public class DebugWindow : System.Windows.Window, IComponentConnector
	{
		private bool _contentLoaded;

		public DebugWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			WeakReferenceMessenger.Default.Send("SaveKeyboard", "SaveKeyboard");
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			WeakReferenceMessenger.Default.Send("resume", "Resume");
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/views/debugwindow.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				((System.Windows.Controls.Button)target).Click += Button_Click;
				break;
			case 2:
				((System.Windows.Controls.Button)target).Click += Button_Click_1;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
	public class PopupImageWindow : System.Windows.Window, IComponentConnector
	{
		private DispatcherTimer timer;

		private bool iniflag = true;

		internal System.Windows.Controls.Image popupImage;

		private bool _contentLoaded;

		public PopupImageWindow()
		{
			InitializeComponent();
			base.Left = (SystemParameters.PrimaryScreenWidth - base.Width) / 2.0;
			base.Top = SystemParameters.PrimaryScreenHeight - base.Height - 60.0;
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(2.0);
			timer.Tick += Timer_Tick;
			WeakReferenceMessenger.Default.Register<string, string>(this, "SystemMode", Message);
			WeakReferenceMessenger.Default.Register<string, string>(this, "Popup", Message);
		}

		private void Message(object obj, string msg)
		{
			if (iniflag)
			{
				iniflag = false;
			}
			else if (System.Windows.Application.Current.MainWindow.WindowState == WindowState.Minimized && this != null)
			{
				if (base.WindowState != WindowState.Minimized)
				{
					Hide();
					timer.Stop();
				}
				switch (msg)
				{
				case "0":
					popupImage.Source = new BitmapImage(new Uri("/image/切换模式弹窗/office.png", UriKind.RelativeOrAbsolute));
					break;
				case "1":
					popupImage.Source = new BitmapImage(new Uri("/image/切换模式弹窗/per.png", UriKind.RelativeOrAbsolute));
					break;
				case "2":
					popupImage.Source = new BitmapImage(new Uri("/image/切换模式弹窗/game.png", UriKind.RelativeOrAbsolute));
					break;
				case "KbLightOn":
					popupImage.Source = new BitmapImage(new Uri("/image/KbLightOn.png", UriKind.RelativeOrAbsolute));
					break;
				case "KbLightOff":
					popupImage.Source = new BitmapImage(new Uri("/image/KbLightOff.png", UriKind.RelativeOrAbsolute));
					break;
				case "BackLightOn":
					popupImage.Source = new BitmapImage(new Uri("/image/BackLightOn.png", UriKind.RelativeOrAbsolute));
					break;
				case "BackLightOff":
					popupImage.Source = new BitmapImage(new Uri("/image/BackLightOff.png", UriKind.RelativeOrAbsolute));
					break;
				}
				Show();
				timer.Start();
			}
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			Hide();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/views/popupimagewindow.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				popupImage = (System.Windows.Controls.Image)target;
			}
			else
			{
				_contentLoaded = true;
			}
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
				string pathLink = System.IO.Path.Combine(directory, $"{shortcutName}.lnk");
				WshShell wshShell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
				IWshShortcut obj = (IWshShortcut)(dynamic)wshShell.CreateShortcut(pathLink);
				obj.TargetPath = targetPath;
				obj.WorkingDirectory = System.IO.Path.GetDirectoryName(targetPath);
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
			using TaskService taskService = new TaskService();
			Microsoft.Win32.TaskScheduler.Task task = taskService.GetTask("ControlCenter");
			LogonTrigger logonTrigger = new LogonTrigger();
			logonTrigger.Delay = TimeSpan.FromSeconds(3.0);
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
			taskDefinition.Triggers.Add(logonTrigger);
			taskDefinition.Actions.Add(new ExecAction(appAllPath, "-minimized", Directory.GetCurrentDirectory()));
			taskService.RootFolder.RegisterTaskDefinition("ControlCenter", taskDefinition);
		}

		private void delTask()
		{
			using TaskService taskService = new TaskService();
			if (taskService.GetTask("ControlCenter") != null)
			{
				taskService.RootFolder.DeleteTask("ControlCenter");
			}
		}
	}
	public class AudioCapturer : IMMNotificationClient
	{
		private MMDeviceEnumerator enumerator;

		private MMDevice mmDevice;

		private WasapiCapture capture;

		private Visualizer visualizer;

		private double[]? spectrumData;

		private byte T100ms_Recorder;

		private double Recorder_temp;

		private double RecorderDAT;

		private double Sum_RecorderDAT;

		public byte[] Array = new byte[4];

		public byte[] MaxVolumn = new byte[4];

		private double lastMax;

		private System.Threading.Timer Timer;

		public AudioCapturer()
		{
			try
			{
				enumerator = new MMDeviceEnumerator();
				enumerator.RegisterEndpointNotificationCallback(this);
				capture = new WasapiLoopbackCapture();
				visualizer = new Visualizer(256);
				capture.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(8192, 1);
				capture.DataAvailable += Capture_DataAvailable;
				Timer = new System.Threading.Timer(delegate
				{
					DataTimer_Tick();
				}, null, -1, -1);
			}
			catch (Exception)
			{
			}
		}

		public double[] start()
		{
			try
			{
				capture.StartRecording();
				Timer.Change(0, 50);
				_ = spectrumData;
				return spectrumData;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public void stop()
		{
			try
			{
				capture.StopRecording();
				Timer.Change(-1, -1);
			}
			catch (Exception)
			{
			}
		}

		private void Capture_DataAvailable(object? sender, WaveInEventArgs e)
		{
			try
			{
				int num = e.BytesRecorded / 4;
				double[] array = new double[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = BitConverter.ToSingle(e.Buffer, i * 4);
				}
				visualizer.PushSampleData(array);
			}
			catch (Exception)
			{
				capture.StopRecording();
				capture.DataAvailable -= Capture_DataAvailable;
			}
		}

		private bool ByteToFile(double[] byteArray, string fileName)
		{
			bool result = false;
			try
			{
				string[] array = System.Array.ConvertAll(byteArray, (double d) => d.ToString());
				using StreamWriter streamWriter = new StreamWriter(fileName, append: true);
				string[] array2 = array;
				foreach (string value in array2)
				{
					streamWriter.Write(value);
					streamWriter.Write(',');
				}
				streamWriter.Write('\n');
				streamWriter.Write('\n');
				streamWriter.Write('\n');
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public void DataTimer_Tick()
		{
			double[] data = visualizer.GetSpectrumData();
			data = Visualizer.MakeSmooth(data, 2);
			if (data != null)
			{
				spectrumData = data;
				double num = 0.0;
				num = ((!(spectrumData.Sum() < 0.0)) ? spectrumData.Sum() : (0.0 - spectrumData.Sum()));
				Recorder_temp = num;
				if (T100ms_Recorder < 5)
				{
					Sum_RecorderDAT += Recorder_temp;
				}
				if (T100ms_Recorder == 4)
				{
					Sum_RecorderDAT *= 30.0;
					MaxVolumn[0] = (byte)Sum_RecorderDAT;
					MaxVolumn[3] = (byte)Sum_RecorderDAT;
					Sum_RecorderDAT = 0.0;
					T100ms_Recorder = 0;
				}
				T100ms_Recorder++;
			}
		}

		public byte[] DataProcess(byte cmd, byte[] r, byte[] g, byte[] b, byte speed, byte[] br, byte[] AudioData)
		{
			return new byte[17]
			{
				52,
				14,
				cmd,
				r[0],
				r[1],
				g[0],
				g[1],
				b[0],
				b[1],
				speed,
				br[0],
				br[1],
				AudioData[0],
				AudioData[1],
				AudioData[2],
				AudioData[3],
				0
			};
		}

		public void OnDeviceStateChanged(string deviceId, DeviceState newState)
		{
		}

		public void OnDeviceAdded(string pwstrDeviceId)
		{
		}

		public void OnDeviceRemoved(string deviceId)
		{
		}

		private void ChangeCaptureClient()
		{
			capture = new WasapiLoopbackCapture();
			capture.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(8192, 1);
			capture.DataAvailable += Capture_DataAvailable;
		}

		public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
		{
			ChangeCaptureClient();
			if (GlobalVars.IsMusicMode)
			{
				start();
			}
		}

		public void OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
		{
		}
	}
	public class clientRequest : IclientRequest
	{
		[Serializable]
		public struct FileTranslation
		{
			public string pCTypeNum;

			public string fileType;

			public string fileVersion;

			public string hash256;

			public string pW;
		}

		[Serializable]
		public struct PCTypeFileType
		{
			public string[] fileName;

			public string[] driverName;
		}

		public struct ConfigInfo
		{
			public string url;

			public string port;
		}

		[Serializable]
		public struct SHA256STR
		{
			public string res;
		}

		private string URL;

		public string savePath;

		public string configPath;

		public string downloadPath;

		private Wmi wmi = new Wmi();

		public string PCTYPENUM_SSID;

		public string PCTYPENUM_SKU;

		public PCTypeFileType pCTypeFileType1;

		private const int expirationTime = 10000;

		private int lengthDownloadInt = 10485760;

		private const int tryNum = 1;

		public string BIOSVersion = "0.0";

		public string[] nameArr;

		private string APPVersion = "0.0";

		private string ECVersion = "0.0";

		private string clientFilePath;

		private string VideoBIOSVersion = "0.0";

		private string FileTypeFileName = "fileType";

		private IclientRequest.downFileVersionInfo APPVersionInfo;

		public Dictionary<string, string> localVersionMap = new Dictionary<string, string>();

		public Dictionary<string, string> driverNameMap = new Dictionary<string, string>();

		public Dictionary<string, string> allDriverVersionMap = new Dictionary<string, string>();

		private static Mutex mutexFile = new Mutex();

		private static Mutex mutexDownload = new Mutex();

		public int ExistedVersionFlag;

		public bool isTest = true;

		public string fileSplitFlag = " :: ";

		private string[] eliminateFW = new string[3] { "BIOS", "EC", "VideoBIOS" };

		public Dictionary<string, IclientRequest.downFileVersionInfo> serverVersionMap = new Dictionary<string, IclientRequest.downFileVersionInfo>();

		private static object locker = new object();

		private static int downloadNum = 3;

		private static SemaphoreSlim semaphore = new SemaphoreSlim(downloadNum);

		public event Action<int, int> DownloadProgressBar;

		public static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
		{
			Console.WriteLine("Warning, trust any certificate");
			return true;
		}

		public clientRequest()
		{
			configPath = "./config/ClientConfig";
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			clientFilePath = folderPath + "\\ControlFile\\";
			downloadPath = clientFilePath + "File\\";
			Trace.WriteLine("downloadPath:" + downloadPath);
			Directory.CreateDirectory(downloadPath);
			savePath = clientFilePath + "SavePath\\";
			System.Version version = Assembly.GetEntryAssembly().GetName().Version;
			APPVersion = version.ToString();
			PCTYPENUM_SSID = GlobalVars.ssid;
			string phase = GlobalVars.Phase;
			PCTYPENUM_SKU = GlobalVars.skuid.ToString();
			BIOSVersion = wmi.getBIOSver();
			ECVersion = wmi.getECVer();
			VideoBIOSVersion = getVbiosVer();
			if (PCTYPENUM_SKU == null || PCTYPENUM_SKU.Length < 1 || PCTYPENUM_SKU == "0")
			{
				PCTYPENUM_SKU = "";
				Trace.WriteLine("ERROR getSKU IS EMPTY");
			}
			if (PCTYPENUM_SSID == null || PCTYPENUM_SSID.Length == 0 || PCTYPENUM_SSID == "0")
			{
				PCTYPENUM_SSID = "80051B61_5_EVT";
			}
			else
			{
				PCTYPENUM_SSID = PCTYPENUM_SSID + "_" + PCTYPENUM_SKU + "_" + phase;
			}
			Trace.WriteLine("PCTYPENUM_SSID + PCTYPENUM_SKU:" + PCTYPENUM_SSID);
			Trace.WriteLine("BIOSVersion:" + BIOSVersion);
			Trace.WriteLine("ECVersion:" + ECVersion);
			Trace.WriteLine("APPVersion:" + APPVersion);
			Trace.WriteLine("VideoBIOS:" + VideoBIOSVersion);
			localVersionMap.Add("BIOS", BIOSVersion);
			localVersionMap.Add("EC", ECVersion);
			localVersionMap.Add("VideoBIOS", VideoBIOSVersion);
			localVersionMap.Add("APPUpdate", APPVersion);
			driverNameMap["BIOS"] = "BIOS";
			driverNameMap["EC"] = "EC";
			driverNameMap["VideoBIOS"] = "VideoBIOS";
			driverNameMap["APPUpdate"] = "APPUpdate";
			var (array, array2) = getConfigFileType(FileTypeFileName);
			if (array != null && array.Length != 0 && array2 != null && array2.Length != 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					driverNameMap[array[i]] = array2[i];
				}
			}
			getConfig();
			createUpdateFile();
		}

		public string getVbiosVer()
		{
			_ = string.Empty;
			try
			{
				return GPUApi.GetVBIOSVersionString(GPUApi.EnumPhysicalGPUs()[0]);
			}
			catch
			{
				Trace.WriteLine("Get videoBIOS version error");
			}
			return "0.0";
		}

		private string getSKU()
		{
			using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard").Get().GetEnumerator())
			{
				if (managementObjectEnumerator.MoveNext())
				{
					return ((ManagementObject)managementObjectEnumerator.Current)["Version"]?.ToString();
				}
			}
			return "";
		}

		public void getAllDriverVersion()
		{
			allDriverVersionMap = new Dictionary<string, string>();
			try
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPSignedDriver"))
				{
					using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
					foreach (ManagementObject item in managementObjectCollection)
					{
						string text = (string)item["DeviceName"];
						string text2 = (string)item["DriverVersion"];
						if (text != null && text.Length > 0 && text2 != null && text2.Length > 0)
						{
							if (!allDriverVersionMap.ContainsKey(text))
							{
								allDriverVersionMap.Add(text, text2);
							}
							else
							{
								allDriverVersionMap[text] = text2;
							}
						}
					}
				}
				Trace.WriteLine("allDriverVersionMap length:" + allDriverVersionMap.Count);
			}
			catch (Exception)
			{
				Trace.WriteLine("This is a error");
			}
		}

		public string getDriverVersion(string name)
		{
			switch (name)
			{
			case "BIOS":
				return wmi.getBIOSver();
			case "EC":
				return wmi.getECVer();
			case "VideoBIOS":
				return getVbiosVer();
			default:
				if (allDriverVersionMap.ContainsKey(name))
				{
					return allDriverVersionMap[name];
				}
				return "0.0";
			}
		}

		public void getLocation()
		{
			getAllDriverVersion();
			string[] array = nameArr;
			foreach (string text in array)
			{
				string driverVersion = getDriverVersion(text);
				if (localVersionMap.ContainsKey(text))
				{
					localVersionMap[text] = driverVersion;
				}
				else
				{
					localVersionMap.Add(text, driverVersion);
				}
			}
		}

		public void getConfig()
		{
			ConfigInfo configInfo = default(ConfigInfo);
			if (!File.Exists(configPath))
			{
				Trace.WriteLine("文件不存在.");
				System.Windows.Forms.MessageBox.Show("配置文件不存在", "配置文件错误", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
				return;
			}
			FileStream fileStream = new FileStream(configPath, FileMode.Open, FileAccess.Read);
			StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
			string[] array = new string[100];
			ulong num = 0uL;
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				array[num] = text;
				num++;
			}
			fileStream.Close();
			streamReader.Close();
			for (ulong num2 = 0uL; num2 < num; num2++)
			{
				if (array[num2] == "[URL]")
				{
					configInfo.url = array[num2 + 1];
				}
				else if (array[num2] == "[PORT]")
				{
					configInfo.port = array[num2 + 1];
				}
			}
			if (configInfo.url == null || configInfo.url == "" || configInfo.port == null || configInfo.port == "")
			{
				Trace.WriteLine("config file error");
				System.Windows.Forms.MessageBox.Show("配置文件错误，无法读取URL和端口", "配置文件错误", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Hand);
			}
			else
			{
				URL = "https://" + configInfo.url + ":" + configInfo.port;
			}
		}

		public PCTypeFileType getFileType()
		{
			PCTypeFileType result = default(PCTypeFileType);
			try
			{
				if (isTest)
				{
					ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(RemoteCertificateValidate));
				}
				HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(string.Concat(URL + "/file/getFileType", "?pCTypeNum=", PCTYPENUM_SSID).Replace("#", "%23"));
				obj.Method = "GET";
				obj.Timeout = 10000;
				HttpWebResponse httpWebResponse = obj.GetResponse() as HttpWebResponse;
				if (Convert.ToInt32(httpWebResponse.StatusCode) != 200)
				{
					ExistedVersionFlag = -2;
					return result;
				}
				Stream responseStream = httpWebResponse.GetResponseStream();
				string value = "";
				using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
				{
					value = streamReader.ReadToEnd();
				}
				result = JsonConvert.DeserializeObject<PCTypeFileType>(value);
				if (result.fileName == null)
				{
					ExistedVersionFlag = 0;
					result.fileName = new string[1];
					result.fileName[0] = "";
				}
			}
			catch (Exception)
			{
				ExistedVersionFlag = -2;
			}
			return result;
		}

		public IclientRequest.downFileVersionInfo getExistedVersion(string PCTypeNum, string FileType)
		{
			IclientRequest.downFileVersionInfo result = default(IclientRequest.downFileVersionInfo);
			try
			{
				if (isTest)
				{
					ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(RemoteCertificateValidate));
				}
				HttpWebRequest obj = (HttpWebRequest)WebRequest.Create((URL + "/file/getFileVersion?pCTypeNum=" + PCTypeNum + "&fileType=" + FileType).Replace("#", "%23"));
				obj.Method = "GET";
				obj.Timeout = 10000;
				HttpWebResponse httpWebResponse = obj.GetResponse() as HttpWebResponse;
				switch (Convert.ToInt32(httpWebResponse.StatusCode))
				{
				case 501:
					ExistedVersionFlag = -2;
					return result;
				case 500:
					ExistedVersionFlag = -3;
					return result;
				default:
					ExistedVersionFlag = -1;
					return result;
				case 200:
				{
					Stream responseStream = httpWebResponse.GetResponseStream();
					string value = "";
					using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
					{
						value = streamReader.ReadToEnd();
					}
					result = JsonConvert.DeserializeObject<IclientRequest.downFileVersionInfo>(value);
					return result;
				}
				}
			}
			catch (Exception)
			{
				ExistedVersionFlag = -1;
			}
			return result;
		}

		public string GetFileHash(FileTranslation fileInfo)
		{
			string text = URL + "/file/getHash";
			text = text + "?pCTypeNum=" + fileInfo.pCTypeNum;
			text = text + "&fileType=" + fileInfo.fileType;
			text = text + "&fileVersion=" + fileInfo.fileVersion;
			text = text + "&hash256=" + fileInfo.hash256;
			text = text + "&pW=" + fileInfo.pW;
			text = text.Replace("#", "%23");
			try
			{
				if (isTest)
				{
					ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(RemoteCertificateValidate));
				}
				HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(text);
				obj.Method = "GET";
				obj.Timeout = 10000;
				HttpWebResponse httpWebResponse = obj.GetResponse() as HttpWebResponse;
				if (Convert.ToInt32(httpWebResponse.StatusCode) != 200)
				{
					return "";
				}
				Stream responseStream = httpWebResponse.GetResponseStream();
				string value = "";
				using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
				{
					value = streamReader.ReadToEnd();
				}
				return JsonConvert.DeserializeObject<SHA256STR>(value).res;
			}
			catch (Exception)
			{
				return "";
			}
		}

		private string createSha256(string filePath)
		{
			try
			{
				using SHA256 sHA = SHA256.Create();
				using FileStream inputStream = File.OpenRead(filePath);
				return BitConverter.ToString(sHA.ComputeHash(inputStream)).Replace("-", "").ToLower();
			}
			catch (Exception)
			{
				return "";
			}
		}

		public string GetFile(FileTranslation fileInfo, int value)
		{
			try
			{
				string fileName = "";
				string text = driverNameMap[fileInfo.fileType];
				if (text == null || text.Length == 0)
				{
					return null;
				}
				string path = downloadPath + fileInfo.pCTypeNum + "/" + text + "/" + fileInfo.fileVersion;
				if (isTest)
				{
					ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(RemoteCertificateValidate));
				}
				Directory.CreateDirectory(path);
				int second = DateTime.Now.Second;
				int startDownloadPositionInt = 0;
				string startDownloadPosition = startDownloadPositionInt.ToString();
				string text2 = lengthDownloadInt.ToString();
				FileStream file = null;
				bool createFileFlag = true;
				int allTotal = 0;
				int fileTotalSize = 0;
				bool isDownloadOver = true;
				while (isDownloadOver)
				{
					string text3 = URL + "/file/getDownloadFileChunk";
					text3 = text3 + "?pCTypeNum=" + fileInfo.pCTypeNum;
					text3 = text3 + "&fileType=" + fileInfo.fileType;
					text3 = text3 + "&fileVersion=" + fileInfo.fileVersion;
					text3 = text3 + "&hash256=" + fileInfo.hash256;
					text3 = text3 + "&pW=" + fileInfo.pW;
					isDownloadOver = false;
					text3 = text3 + "&startPoint=" + startDownloadPosition;
					text3 = text3 + "&lengthDown=" + text2;
					text3 = text3.Replace("#", "%23");
					HttpWebRequest req = (HttpWebRequest)WebRequest.Create(text3);
					req.Method = "GET";
					req.Timeout = 10000;
					ManualResetEvent downloadCompleteEvent = new ManualResetEvent(initialState: false);
					req.BeginGetResponse(delegate(IAsyncResult ar)
					{
						HttpWebResponse httpWebResponse = (HttpWebResponse)req.EndGetResponse(ar);
						Stream responseStream = httpWebResponse.GetResponseStream();
						if (httpWebResponse.StatusCode != HttpStatusCode.OK)
						{
							isDownloadOver = false;
							downloadCompleteEvent.Set();
						}
						else
						{
							if (createFileFlag)
							{
								fileName = httpWebResponse.GetResponseHeader("Content-Disposition");
								if (fileName != null && fileName.Length > 0)
								{
									file = new FileStream(path + "/" + fileName, FileMode.Create, FileAccess.Write);
									createFileFlag = false;
								}
							}
							if (createFileFlag)
							{
								isDownloadOver = false;
							}
							else
							{
								string responseHeader = httpWebResponse.GetResponseHeader("Content-Length");
								string responseHeader2 = httpWebResponse.GetResponseHeader("fileIsOver");
								string responseHeader3 = httpWebResponse.GetResponseHeader("currentDownLength");
								int num = 0;
								if (responseHeader3 != null && responseHeader3.Length != 0)
								{
									num = int.Parse(responseHeader3);
								}
								if (responseHeader2 != null && responseHeader2.Length != 0 && responseHeader2 == "false")
								{
									isDownloadOver = true;
								}
								else
								{
									isDownloadOver = false;
								}
								if (responseHeader != null || !(responseHeader == ""))
								{
									fileTotalSize = int.Parse(responseHeader);
								}
								int num2 = 0;
								byte[] array = new byte[lengthDownloadInt];
								int num3 = 0;
								int num4 = responseStream.Read(array, 0, array.Length);
								int num5 = 0;
								while (num4 > 0)
								{
									num3 += num4;
									allTotal += num4;
									file.Write(array, 0, num4);
									num5 = (int)((float)allTotal / (float)fileTotalSize * 100f);
									if (num5 - num2 > 1)
									{
										num2 = num5;
										this.DownloadProgressBar?.Invoke(value, num2);
									}
									if (num3 >= num)
									{
										break;
									}
									num4 = responseStream.Read(array, 0, array.Length);
								}
								startDownloadPositionInt += num;
								startDownloadPosition = startDownloadPositionInt.ToString();
								httpWebResponse.Close();
								Trace.WriteLine("下载完成");
							}
							downloadCompleteEvent.Set();
						}
					}, null);
					downloadCompleteEvent.WaitOne();
				}
				if (createFileFlag)
				{
					return null;
				}
				file.Close();
				string text4 = path + "/" + fileName;
				text4 = text4.Replace("./", "");
				text4 = text4.Replace("/", "\\");
				string fileHash = GetFileHash(fileInfo);
				string text5 = createSha256(path + "/" + fileName);
				if (text5 == null)
				{
					Trace.WriteLine("生成sha256失败 fileSHA256：" + text5);
					return null;
				}
				if (!text5.Equals(fileHash))
				{
					Trace.WriteLine("sha256验证失败");
					System.Windows.Forms.MessageBox.Show("sha256验证失败");
					return null;
				}
				Trace.WriteLine("HASH 验证通过");
				int second2 = DateTime.Now.Second;
				Trace.WriteLine("下载时间：" + (second2 - second));
				return System.IO.Path.GetRelativePath(clientFilePath, text4);
			}
			catch (Exception)
			{
				System.Windows.Forms.MessageBox.Show("服务器链接错误", "下载错误", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
				return null;
			}
		}

		public async Task<string> GetFileNew(FileTranslation fileInfo, int value)
		{
			try
			{
				string fileName = "";
				string text = driverNameMap[fileInfo.fileType];
				if (text == null || text.Length == 0)
				{
					return null;
				}
				string path = downloadPath + fileInfo.pCTypeNum + "/" + text + "/" + fileInfo.fileVersion;
				Directory.CreateDirectory(path);
				int currentSeconds = DateTime.Now.Second;
				int startDownloadPositionInt = 0;
				string text2 = startDownloadPositionInt.ToString();
				string lengthDownload = lengthDownloadInt.ToString();
				FileStream file = null;
				bool createFileFlag = true;
				int allTotal = 0;
				int fileTotalSize = 0;
				bool flag = true;
				int currentDownLengthInt = 0;
				int progressBarValue = 0;
				while (flag)
				{
					string text3 = URL + "/file/getDownloadFileChunk";
					text3 = text3 + "?pCTypeNum=" + fileInfo.pCTypeNum;
					text3 = text3 + "&fileType=" + fileInfo.fileType;
					text3 = text3 + "&fileVersion=" + fileInfo.fileVersion;
					text3 = text3 + "&hash256=" + fileInfo.hash256;
					text3 = text3 + "&pW=" + fileInfo.pW;
					flag = false;
					text3 = text3 + "&startPoint=" + text2;
					text3 = text3 + "&lengthDown=" + lengthDownload;
					text3 = text3.Replace("#", "%23");
					HttpClientHandler handler = ((!isTest) ? new HttpClientHandler() : new HttpClientHandler
					{
						ServerCertificateCustomValidationCallback = RemoteCertificateValidate
					});
					using (HttpClient client = new HttpClient(handler))
					{
						try
						{
							string fileSizeStr = "";
							string currentDownLength = "";
							string fileIsOver = "";
							client.Timeout = TimeSpan.FromMilliseconds(10000.0);
							HttpResponseMessage httpResponseMessage = await client.GetAsync(text3);
							httpResponseMessage.EnsureSuccessStatusCode();
							Stream result = httpResponseMessage.Content.ReadAsStreamAsync().Result;
							if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
							{
								flag = false;
								break;
							}
							if (!createFileFlag)
							{
								goto IL_0374;
							}
							if (httpResponseMessage.Headers.TryGetValues("fileName", out IEnumerable<string> values))
							{
								fileName = values.FirstOrDefault();
								Trace.WriteLine("fileName:" + fileName);
							}
							if (fileName != null && fileName.Length > 0)
							{
								file = new FileStream(path + "/" + fileName, FileMode.Create, FileAccess.Write);
								flag = true;
								createFileFlag = false;
								goto IL_0374;
							}
							flag = false;
							goto end_IL_0213;
							IL_0374:
							if (httpResponseMessage.Headers.TryGetValues("fileLength", out IEnumerable<string> values2))
							{
								fileSizeStr = values2.FirstOrDefault();
								Trace.WriteLine("fileLength:" + fileSizeStr);
							}
							if (httpResponseMessage.Headers.TryGetValues("fileIsOver", out IEnumerable<string> values3))
							{
								fileIsOver = values3.FirstOrDefault();
								Trace.WriteLine("fileIsOver:" + fileIsOver);
							}
							if (httpResponseMessage.Headers.TryGetValues("currentDownLength", out IEnumerable<string> values4))
							{
								currentDownLength = values4.FirstOrDefault();
								Trace.WriteLine("currentDownLength:" + currentDownLength);
							}
							if (currentDownLength == null || currentDownLength.Length == 0)
							{
								goto IL_0450;
							}
							currentDownLengthInt = int.Parse(currentDownLength);
							if (currentDownLengthInt > 0)
							{
								goto IL_0450;
							}
							flag = false;
							goto end_IL_0213;
							IL_0450:
							flag = ((fileIsOver != null && fileIsOver.Length != 0 && fileIsOver == "false") ? true : false);
							if (fileSizeStr != null || !(fileSizeStr == ""))
							{
								fileTotalSize = int.Parse(fileSizeStr);
							}
							using (BinaryReader binaryReader = new BinaryReader(result))
							{
								byte[] array = new byte[currentDownLengthInt];
								int num;
								while ((num = binaryReader.Read(array, 0, array.Length)) > 0)
								{
									file.Write(array, 0, num);
									allTotal += num;
									int num2 = (int)((float)allTotal / (float)fileTotalSize * 100f);
									if (num2 - progressBarValue > 1)
									{
										progressBarValue = num2;
										this.DownloadProgressBar?.Invoke(value, progressBarValue);
									}
								}
							}
							startDownloadPositionInt += currentDownLengthInt;
							text2 = startDownloadPositionInt.ToString();
							continue;
							end_IL_0213:;
						}
						catch (HttpRequestException ex)
						{
							Trace.WriteLine("请求错误:\n" + ex);
							flag = false;
						}
					}
					break;
				}
				if (createFileFlag)
				{
					return null;
				}
				file.Close();
				string text4 = path + "/" + fileName;
				text4 = text4.Replace("./", "");
				text4 = text4.Replace("/", "\\");
				string fileHash = GetFileHash(fileInfo);
				string text5 = createSha256(path + "/" + fileName);
				if (text5 == null)
				{
					Trace.WriteLine("生成sha256失败 fileSHA256：" + text5);
					return null;
				}
				if (!text5.Equals(fileHash))
				{
					Trace.WriteLine("sha256验证失败");
					System.Windows.Forms.MessageBox.Show("sha256验证失败");
					return null;
				}
				Trace.WriteLine("HASH 验证通过");
				int second = DateTime.Now.Second;
				Trace.WriteLine("下载时间：" + (second - currentSeconds));
				return System.IO.Path.GetRelativePath(clientFilePath, text4);
			}
			catch (Exception ex2)
			{
				System.Windows.Forms.MessageBox.Show("服务器链接错误:" + ex2, "下载错误", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
				return null;
			}
		}

		public void createUpdateFile()
		{
			mutexFile.WaitOne();
			string path = savePath;
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			if (!File.Exists(savePath + "/UpdateConfig"))
			{
				new FileStream(savePath + "/UpdateConfig", FileMode.OpenOrCreate, FileAccess.Write).Close();
			}
			if (!Directory.Exists(downloadPath + PCTYPENUM_SSID))
			{
				Directory.CreateDirectory(downloadPath + PCTYPENUM_SSID);
			}
			mutexFile.ReleaseMutex();
		}

		public bool checkUpdate()
		{
			bool result = false;
			if (getConfigNotUpdate().Length != 0)
			{
				result = true;
			}
			return result;
		}

		public bool writeConfigNotUpdate(string[] content)
		{
			mutexFile.WaitOne();
			string path = savePath;
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			using (FileStream fileStream = File.Create(savePath + "/UpdateConfig"))
			{
				for (int i = 0; i < content.Length; i++)
				{
					byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(content[i] + "\r\n");
					fileStream.Write(bytes, 0, bytes.Length);
				}
			}
			mutexFile.ReleaseMutex();
			return true;
		}

		public string[] getConfigNotUpdate()
		{
			if (!File.Exists(savePath + "/UpdateConfig"))
			{
				return null;
			}
			mutexFile.WaitOne();
			FileStream fileStream = new FileStream(savePath + "/UpdateConfig", FileMode.Open, FileAccess.Read);
			StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
			List<string> list = new List<string>();
			ulong num = 0uL;
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				if (text.Length > 0)
				{
					list.Add(text);
					num++;
				}
			}
			fileStream.Close();
			streamReader.Close();
			mutexFile.ReleaseMutex();
			return list.ToArray();
		}

		public bool writeConfigFileType(string[] content, string fileName)
		{
			mutexFile.WaitOne();
			string path = savePath;
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			using (FileStream fileStream = File.Create(savePath + fileName))
			{
				for (int i = 0; i < content.Length; i++)
				{
					string text = content[i];
					text = text + fileSplitFlag + driverNameMap[content[i]];
					byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(text + "\r\n");
					fileStream.Write(bytes, 0, bytes.Length);
				}
			}
			mutexFile.ReleaseMutex();
			return true;
		}

		public (string[], string[]) getConfigFileType(string fileName)
		{
			if (!File.Exists(savePath + fileName))
			{
				return (null, null);
			}
			mutexFile.WaitOne();
			FileStream fileStream = new FileStream(savePath + fileName, FileMode.Open, FileAccess.Read);
			StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			ulong num = 0uL;
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				if (text.Length > 0)
				{
					string[] array = text.Split(new string[1] { fileSplitFlag }, StringSplitOptions.None);
					if (array == null || array.Length == 0 || array.Length > 2)
					{
						return (null, null);
					}
					list.Add(array[0]);
					list2.Add(array[1]);
					num++;
				}
			}
			fileStream.Close();
			streamReader.Close();
			mutexFile.ReleaseMutex();
			string[] item = list.ToArray();
			string[] item2 = list2.ToArray();
			return (item, item2);
		}

		public void DeleteDirectory(string targetDir)
		{
			string[] files = Directory.GetFiles(targetDir);
			foreach (string path in files)
			{
				File.SetAttributes(path, FileAttributes.Normal);
				File.Delete(path);
			}
			files = Directory.GetDirectories(targetDir);
			foreach (string text in files)
			{
				DeleteDirectory(text);
				Directory.Delete(text, recursive: true);
			}
		}

		public string downloadOtherFileWPF(string fileType, string version, int value)
		{
			IclientRequest.downFileVersionInfo existedVersion = getExistedVersion(PCTYPENUM_SSID, fileType);
			if (existedVersion.latestVersion != null)
			{
				bool flag = false;
				for (int i = 0; i < existedVersion.allVersion.Length; i++)
				{
					if (string.Compare(existedVersion.allVersion[i], version) == 0)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					version = existedVersion.latestVersion;
				}
				FileTranslation fileInfo = new FileTranslation
				{
					pCTypeNum = PCTYPENUM_SSID,
					fileType = fileType,
					fileVersion = version,
					hash256 = "",
					pW = ""
				};
				string text = null;
				for (int j = 0; j < 1; j++)
				{
					Task<string> fileNew = GetFileNew(fileInfo, value);
					fileNew.Wait();
					text = fileNew.Result;
					if (text != null)
					{
						return text;
					}
				}
				return null;
			}
			return "noUpdate";
		}

		public (IclientRequest.downFileVersionInfo, int) getAppVersion()
		{
			APPVersionInfo = getExistedVersion(PCTYPENUM_SSID, "APPUpdate");
			if (APPVersionInfo.latestVersion == null)
			{
				return (APPVersionInfo, -1);
			}
			if (APPVersionInfo.latestVersion == "")
			{
				return (APPVersionInfo, 0);
			}
			(int, Exception) tuple = Compare(APPVersionInfo.latestVersion, APPVersion);
			var (num, _) = tuple;
			if (tuple.Item2 != null)
			{
				return (APPVersionInfo, -1);
			}
			if (APPVersionInfo.latestVersion != null && num == 1)
			{
				return (APPVersionInfo, 1);
			}
			return (APPVersionInfo, 0);
		}

		private string downloadApp(string version)
		{
			APPVersionInfo = getExistedVersion(PCTYPENUM_SSID, "APPUpdate");
			if (APPVersionInfo.latestVersion != null && string.Compare(APPVersionInfo.latestVersion, version) == 1)
			{
				FileTranslation fileInfo = new FileTranslation
				{
					pCTypeNum = PCTYPENUM_SSID,
					fileType = "APPUpdate",
					fileVersion = APPVersionInfo.latestVersion,
					hash256 = "",
					pW = ""
				};
				string text = null;
				for (int i = 0; i < 1; i++)
				{
					Task<string> fileNew = GetFileNew(fileInfo, 0);
					fileNew.Wait();
					text = fileNew.Result;
					if (text != null)
					{
						Trace.Write("download app success:" + text);
						return text;
					}
				}
				return null;
			}
			return "noUpdate";
		}

		public bool UpdateAPP()
		{
			string text = null;
			text = downloadApp(APPVersion);
			if (text != null)
			{
				if (text == "noUpdate")
				{
					Trace.Write("noUpdate");
					return false;
				}
				if (!updateAppPlan3(text))
				{
					System.Windows.Forms.MessageBox.Show("升级失败");
				}
				return true;
			}
			System.Windows.Forms.MessageBox.Show("download APP ERROR");
			return false;
		}

		private bool updateAppPlan3(string filePath)
		{
			string directoryName = System.IO.Path.GetDirectoryName(filePath);
			Trace.WriteLine("GetFile dirPath:" + directoryName);
			if (File.Exists(directoryName + "\\FlashWin.bat"))
			{
				File.Delete(directoryName + "\\FlashWin.bat");
			}
			Process process = new Process();
			process.StartInfo.FileName = filePath;
			process.StartInfo.Arguments = "-y";
			process.Start();
			process.WaitForExit();
			bool flag = true;
			string text = directoryName + "\\FlashWin.bat";
			foreach (string item in Directory.GetFiles(directoryName, "*", SearchOption.AllDirectories).ToList())
			{
				string fileName = System.IO.Path.GetFileName(item);
				if (fileName == "FlashWin.bat")
				{
					Trace.WriteLine(fileName);
					flag = false;
					text = item;
					break;
				}
			}
			if (flag)
			{
				return false;
			}
			Trace.WriteLine("tmpDir:" + text);
			string location = Assembly.GetExecutingAssembly().Location;
			System.IO.Path.GetDirectoryName(location);
			Trace.WriteLine("本程序的路径：" + location);
			Process obj = new Process
			{
				StartInfo = 
				{
					FileName = text
				}
			};
			process.StartInfo.Arguments = location;
			obj.Start();
			Environment.Exit(0);
			return true;
		}

		public bool getServerVersion(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			IclientRequest.downFileVersionInfo existedVersion = getExistedVersion(PCTYPENUM_SSID, name);
			if (existedVersion.allVersion == null || existedVersion.allVersion.Length == 0)
			{
				return false;
			}
			lock (locker)
			{
				if (serverVersionMap.ContainsKey(name))
				{
					serverVersionMap[name] = existedVersion;
				}
				else
				{
					serverVersionMap.Add(name, existedVersion);
				}
			}
			return true;
		}

		public (string[], int) checkUpdateServer()
		{
			try
			{
				List<string> list = new List<string>();
				ExistedVersionFlag = 1;
				pCTypeFileType1 = getFileType();
				if (ExistedVersionFlag == -1 || pCTypeFileType1.fileName == null)
				{
					var (array, array2) = getConfigFileType(FileTypeFileName);
					if (array == null)
					{
						return (null, -1);
					}
					if (array.Length != 0)
					{
						for (int i = 0; i < array.Length; i++)
						{
							driverNameMap[array[i]] = array2[i];
						}
						nameArr = array;
						getLocation();
					}
					return (null, -1);
				}
				if (pCTypeFileType1.fileName.Length == 0 || pCTypeFileType1.fileName[0] == "")
				{
					return (null, 0);
				}
				nameArr = new string[pCTypeFileType1.fileName.Length];
				if (nameArr.Length != 0)
				{
					for (int j = 0; j < nameArr.Length; j++)
					{
						driverNameMap[pCTypeFileType1.fileName[j]] = pCTypeFileType1.driverName[j];
						nameArr[j] = pCTypeFileType1.fileName[j];
					}
				}
				if (nameArr.Length != 0)
				{
					writeConfigFileType(nameArr, FileTypeFileName);
				}
				int second = DateTime.Now.Second;
				getLocation();
				int second2 = DateTime.Now.Second;
				foreach (string key in localVersionMap.Keys)
				{
					if (!getServerVersion(key))
					{
						continue;
					}
					string text = localVersionMap[key];
					string latestVersion = serverVersionMap[key].latestVersion;
					if (eliminateFW.Contains(key))
					{
						if (text != latestVersion)
						{
							list.Add(key);
						}
						continue;
					}
					(int, Exception) tuple2 = Compare(text, latestVersion);
					var (num, _) = tuple2;
					if (tuple2.Item2 == null && num == -1)
					{
						list.Add(key);
					}
				}
				int second3 = DateTime.Now.Second;
				Trace.WriteLine("时间消耗\n本地时间消耗：" + (second2 - second) + "\n服务器时间消耗：" + (second3 - second2));
				return (list.ToArray(), 1);
			}
			catch (Exception ex)
			{
				Trace.WriteLine("错误！严重错误！！！error:" + ex);
				return (null, -1);
			}
		}

		public bool updateAll()
		{
			bool res = false;
			string[] configNotUpdate = getConfigNotUpdate();
			if (configNotUpdate == null || configNotUpdate.Length == 0)
			{
				return false;
			}
			List<string> strList = new List<string>(configNotUpdate);
			string filePath = "";
			string name = "";
			string version = "";
			List<string> strListTmp = strList;
			return inlineFunc();
			bool inlineFunc()
			{
				string text = "";
				try
				{
					foreach (string item in strList)
					{
						string[] array = item.Split(new string[1] { fileSplitFlag }, StringSplitOptions.None);
						if (array.Length < 4)
						{
							Trace.WriteLine("配置文件出错！！！");
							return false;
						}
						string text2 = array[0].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
						string[] array2 = array[1].Split(new string[1] { "=" }, StringSplitOptions.None);
						name = array2[1];
						string[] array3 = array[2].Split(new string[1] { "=" }, StringSplitOptions.None);
						version = array3[1];
						if (array[3].Split(new string[1] { "=" }, StringSplitOptions.None)[1].Equals("true"))
						{
							if (!getDriverVersion(name).Equals(version))
							{
								filePath = text2;
								text = item.Replace("true", "false");
								strListTmp.Remove(item);
								break;
							}
							strListTmp.Remove(item);
						}
					}
					strList = strListTmp;
					if (filePath.Length == 0)
					{
						string[] content = strListTmp.ToArray();
						writeConfigNotUpdate(content);
						return false;
					}
					res = true;
					filePath = clientFilePath + "\\" + filePath;
					string directoryName = System.IO.Path.GetDirectoryName(filePath);
					Trace.WriteLine("GetFile dirPath:" + directoryName);
					if (File.Exists(directoryName + "\\FlashWin.bat"))
					{
						File.Delete(directoryName + "\\FlashWin.bat");
					}
					Process process = new Process();
					process.StartInfo.FileName = filePath;
					process.StartInfo.Arguments = "-y";
					process.Start();
					process.WaitForExit();
					Trace.WriteLine("解压进程已退出");
					bool flag = true;
					string text3 = directoryName + "\\FlashWin.bat";
					foreach (string item2 in Directory.GetFiles(directoryName, "*", SearchOption.AllDirectories).ToList())
					{
						string fileName = System.IO.Path.GetFileName(item2);
						if (fileName == "FlashWin.bat")
						{
							Console.WriteLine(fileName);
							flag = false;
							text3 = item2;
							break;
						}
					}
					if (flag)
					{
						text = text.Replace("false", "Unable");
						strListTmp.Add(text);
						string[] content2 = strListTmp.ToArray();
						writeConfigNotUpdate(content2);
						System.Windows.Forms.MessageBox.Show("该项不支持自动升级，请进行手动升级");
						string directoryName2 = System.IO.Path.GetDirectoryName(filePath);
						Process.Start("explorer.exe", directoryName2);
						return false;
					}
					Trace.WriteLine("tmpDir:" + text3);
					Process process2 = new Process();
					process2.StartInfo.FileName = text3;
					process2.Start();
					process2.WaitForExit();
					if (text != "")
					{
						if (!getDriverVersion(name).Equals(version))
						{
							strListTmp.Add(text);
							res = false;
						}
						else
						{
							res = true;
							File.SetAttributes(text3, FileAttributes.Normal);
							File.Delete(text3);
						}
						string directoryName3 = System.IO.Path.GetDirectoryName(text3);
						DeleteDirectory(directoryName3);
						Directory.Delete(directoryName3, recursive: true);
					}
					string[] content3 = strListTmp.ToArray();
					writeConfigNotUpdate(content3);
					return res;
				}
				catch
				{
					strListTmp.Add(text);
					string[] content4 = strListTmp.ToArray();
					writeConfigNotUpdate(content4);
					return false;
				}
			}
		}

		public bool updateOther(string fileName, string versionSer)
		{
			bool res = false;
			string[] configNotUpdate = getConfigNotUpdate();
			if (configNotUpdate == null || configNotUpdate.Length == 0)
			{
				return false;
			}
			List<string> strList = new List<string>(configNotUpdate);
			string filePath = "";
			string name = "";
			string version = "";
			List<string> strListTmp = strList;
			inlineFunc();
			return res;
			bool inlineFunc()
			{
				string text = "";
				try
				{
					bool flag;
					foreach (string item in strList)
					{
						string[] array = item.Split(new string[1] { fileSplitFlag }, StringSplitOptions.None);
						if (array.Length < 4)
						{
							Trace.WriteLine("配置文件出错！！！");
							return false;
						}
						string text2 = array[0].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
						string[] array2 = array[1].Split(new string[1] { "=" }, StringSplitOptions.None);
						name = array2[1];
						string[] array3 = array[2].Split(new string[1] { "=" }, StringSplitOptions.None);
						version = array3[1];
						_ = array[3].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
						string text3 = driverNameMap[fileName];
						if (text3 == null || text3.Length == 0)
						{
							System.Windows.Forms.MessageBox.Show("zqperror");
							return false;
						}
						if (!(name != text3))
						{
							if (!getDriverVersion(fileName).Equals(version))
							{
								if (versionSer != null && versionSer != "")
								{
									if (!eliminateFW.Contains(name))
									{
										(int, Exception) tuple = Compare(versionSer, version);
										var (num, _) = tuple;
										if (tuple.Item2 != null && num != 1)
										{
											strListTmp.Remove(item);
											flag = false;
											res = false;
											string[] content = strListTmp.ToArray();
											writeConfigNotUpdate(content);
											return false;
										}
									}
									else if (!versionSer.Equals(version))
									{
										strListTmp.Remove(item);
										flag = false;
										res = false;
										string[] content2 = strListTmp.ToArray();
										writeConfigNotUpdate(content2);
										return false;
									}
								}
								filePath = text2;
								text = item.Replace("true", "false");
								strListTmp.Remove(item);
								flag = true;
								res = true;
								break;
							}
							strListTmp.Remove(item);
						}
					}
					strList = strListTmp;
					if (filePath.Length == 0)
					{
						string[] content3 = strListTmp.ToArray();
						writeConfigNotUpdate(content3);
						return false;
					}
					flag = true;
					filePath = clientFilePath + "\\" + filePath;
					string directoryName = System.IO.Path.GetDirectoryName(filePath);
					Trace.WriteLine("GetFile dirPath:" + directoryName);
					if (File.Exists(directoryName + "\\FlashWin.bat"))
					{
						File.Delete(directoryName + "\\FlashWin.bat");
					}
					Process process = new Process();
					process.StartInfo.FileName = filePath;
					process.StartInfo.Arguments = "-y";
					process.Start();
					process.WaitForExit();
					Trace.WriteLine("解压进程已退出");
					bool flag2 = true;
					string text4 = directoryName + "\\FlashWin.bat";
					foreach (string item2 in Directory.GetFiles(directoryName, "*", SearchOption.AllDirectories).ToList())
					{
						string fileName2 = System.IO.Path.GetFileName(item2);
						if (fileName2 == "FlashWin.bat")
						{
							Console.WriteLine(fileName2);
							flag2 = false;
							text4 = item2;
							break;
						}
					}
					if (flag2)
					{
						text = text.Replace("false", "Unable");
						strListTmp.Add(text);
						string[] content4 = strListTmp.ToArray();
						writeConfigNotUpdate(content4);
						System.Windows.Forms.MessageBox.Show("该项不支持自动升级，请进行手动升级");
						string directoryName2 = System.IO.Path.GetDirectoryName(filePath);
						Process.Start("explorer.exe", directoryName2);
						return false;
					}
					Trace.WriteLine("tmpDir:" + text4);
					Process process2 = new Process();
					process2.StartInfo.FileName = text4;
					process2.Start();
					process2.WaitForExit();
					if (text != "")
					{
						if (!getDriverVersion(name).Equals(version))
						{
							strListTmp.Add(text);
							flag = false;
						}
						else
						{
							flag = true;
							File.SetAttributes(text4, FileAttributes.Normal);
							File.Delete(text4);
						}
						string directoryName3 = System.IO.Path.GetDirectoryName(text4);
						DeleteDirectory(directoryName3);
						Directory.Delete(directoryName3, recursive: true);
					}
					string[] content5 = strListTmp.ToArray();
					writeConfigNotUpdate(content5);
					return flag;
				}
				catch
				{
					strListTmp.Add(text);
					string[] content6 = strListTmp.ToArray();
					writeConfigNotUpdate(content6);
					return false;
				}
			}
		}

		private bool DownloadPool(string fileType, string version, int value)
		{
			try
			{
				Trace.WriteLine("Start downloading:" + fileType + version);
				string text = downloadOtherFileWPF(fileType, version, value);
				Trace.WriteLine("download file over.filePath:" + text);
				if (text == "noUpdate")
				{
					return true;
				}
				string[] configNotUpdate = getConfigNotUpdate();
				List<string> list = new List<string>(configNotUpdate);
				string text2 = driverNameMap[fileType];
				if (text2 == null || text2.Length == 0)
				{
					return false;
				}
				List<string> list2 = list;
				if (configNotUpdate.Length != 0)
				{
					foreach (string item2 in list2)
					{
						string[] array = item2.Split(new string[1] { fileSplitFlag }, StringSplitOptions.None);
						if (array.Length < 4)
						{
							Trace.WriteLine("配置文件出错！！！");
							return false;
						}
						if (array[1].Split(new string[1] { "=" }, StringSplitOptions.None)[1] == text2)
						{
							list.Remove(item2);
						}
					}
				}
				string item = "path=" + text + fileSplitFlag + "type=" + text2 + fileSplitFlag + "version=" + version + fileSplitFlag + "update=true";
				list.Add(item);
				string[] content = list.ToArray();
				writeConfigNotUpdate(content);
				return true;
			}
			catch (Exception)
			{
				Trace.WriteLine("未知错误DownloadPool");
				return false;
			}
		}

		public bool DownloadFile(string fileType, string version, int value)
		{
			if (fileType == null || fileType == "")
			{
				return false;
			}
			mutexDownload.WaitOne();
			if (updateOther(fileType, version))
			{
				mutexDownload.ReleaseMutex();
				return true;
			}
			mutexDownload.ReleaseMutex();
			if (version == null || version == "")
			{
				return false;
			}
			semaphore.Wait();
			bool result = DownloadPool(fileType, version, value);
			semaphore.Release();
			mutexDownload.WaitOne();
			updateAll();
			mutexDownload.ReleaseMutex();
			return result;
		}

		public (string[], string[]) checkLoadUpdate()
		{
			string[] configNotUpdate = getConfigNotUpdate();
			if (configNotUpdate == null || configNotUpdate.Length == 0)
			{
				return (null, null);
			}
			List<string> list = new List<string>(configNotUpdate);
			string text = "";
			string text2 = "";
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			foreach (string item3 in list)
			{
				string[] array = item3.Split(new string[1] { fileSplitFlag }, StringSplitOptions.None);
				if (array.Length < 4)
				{
					Trace.WriteLine("配置文件出错！！！");
					return (null, null);
				}
				_ = array[0].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
				text = array[1].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
				text2 = array[2].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
				_ = array[3].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
				if (text != null && text != "" && text2 != null && text2 != "")
				{
					list2.Add(text);
					list3.Add(text2);
				}
			}
			string[] item = list2.ToArray();
			string[] item2 = list3.ToArray();
			return (item, item2);
		}

		public IclientRequest.downFileVersionInfo getGerverVersionMap(string fileType)
		{
			IclientRequest.downFileVersionInfo value = new IclientRequest.downFileVersionInfo
			{
				type = fileType
			};
			if (serverVersionMap.TryGetValue(fileType, out value))
			{
				return value;
			}
			return default(IclientRequest.downFileVersionInfo);
		}

		public string getLocalVersionMap(string fileType)
		{
			string value = "";
			if (localVersionMap.TryGetValue(fileType, out value))
			{
				return value;
			}
			return null;
		}

		public Dictionary<string, string> getAllLoacalVersionMap()
		{
			return localVersionMap;
		}

		public Dictionary<string, string> getAllLoacalNameMap()
		{
			return driverNameMap;
		}

		public IclientRequest.downFileVersionInfo getAppversion()
		{
			return getExistedVersion(PCTYPENUM_SSID, "APPUpdate");
		}

		public (int, Exception) CompareCor(string s1, string s2)
		{
			int i;
			for (i = 0; i < s1.Length && s1[i] == '0'; i++)
			{
			}
			s1 = ((i != s1.Length) ? s1.Substring(i) : "0");
			for (i = 0; i < s2.Length && s2[i] == '0'; i++)
			{
			}
			s2 = ((i != s2.Length) ? s2.Substring(i) : "0");
			if (s1.Length == 0)
			{
				if (s2.Length == 0)
				{
					return (0, null);
				}
				return (-1, null);
			}
			if (!int.TryParse(s1, out var result))
			{
				return (0, new Exception("Error parsing s1"));
			}
			if (!int.TryParse(s2, out var result2))
			{
				return (0, new Exception("Error parsing s2"));
			}
			if (result == result2)
			{
				return (0, null);
			}
			if (result > result2)
			{
				return (1, null);
			}
			return (-1, null);
		}

		public (int, Exception) Compare(string version1, string version2)
		{
			string[] array = version1.Split('.');
			string[] array2 = version2.Split('.');
			int i;
			for (i = 0; i < array.Length && i < array2.Length; i++)
			{
				var (num, ex) = CompareCor(array[i], array2[i]);
				if (ex != null)
				{
					return (0, ex);
				}
				if (num != 0)
				{
					return (num, null);
				}
			}
			if (array.Length == array2.Length)
			{
				return (0, null);
			}
			if (array.Length > array2.Length)
			{
				for (; i < array.Length; i++)
				{
					var (num2, ex2) = CompareCor(array[i], "0");
					if (ex2 != null)
					{
						return (0, ex2);
					}
					if (num2 != 0)
					{
						return (num2, null);
					}
				}
				return (0, null);
			}
			for (; i < array2.Length; i++)
			{
				var (num3, ex3) = CompareCor("0", array2[i]);
				if (ex3 != null)
				{
					return (0, ex3);
				}
				if (num3 != 0)
				{
					return (num3, null);
				}
			}
			return (0, null);
		}

		public void DeleteAllFile()
		{
			string text = clientFilePath + "\\File";
			string path = clientFilePath + "\\SavePath\\UpdateConfig";
			DeleteDirectory(text);
			Directory.Delete(text, recursive: true);
			Directory.CreateDirectory(text);
			File.Create(path);
		}

		public void OpenLocalFile()
		{
			Process.Start("explorer.exe", downloadPath + PCTYPENUM_SSID);
		}
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
	public class SecondOrderDynamics
	{
		private double xp;

		private double y;

		private double yd;

		private double _w;

		private double _z;

		private double _d;

		private double k1;

		private double k2;

		private double k3;

		public SecondOrderDynamics(double f, double z, double r, double x0)
		{
			_w = Math.PI * 2.0 * f;
			_z = z;
			_d = _w * Math.Sqrt(Math.Abs(z * z - 1.0));
			k1 = z / (Math.PI * f);
			k2 = 1.0 / (Math.PI * 2.0 * f * (Math.PI * 2.0 * f));
			k3 = r * z / (Math.PI * 2.0 * f);
			xp = x0;
			y = x0;
			yd = 0.0;
		}

		public double Update(double deltaTime, double x)
		{
			double num = (x - xp) / deltaTime;
			double num2;
			double num3;
			if (_w * deltaTime < _z)
			{
				num2 = k1;
				num3 = Math.Max(Math.Max(k2, deltaTime * deltaTime / 2.0 + deltaTime * k1 / 2.0), deltaTime * k1);
			}
			else
			{
				double num4 = Math.Exp((0.0 - _z) * _w * deltaTime);
				double num5 = 2.0 * num4 * ((_z <= 1.0) ? Math.Cos(deltaTime * _d) : Math.Cosh(deltaTime * _d));
				double num6 = num4 * num4;
				double num7 = deltaTime / (1.0 + num6 - num5);
				num2 = (1.0 - num6) * num7;
				num3 = deltaTime * num7;
			}
			y += deltaTime * yd;
			yd += deltaTime * (x + k3 * num - y - num2 * yd) / num3;
			xp = x;
			return y;
		}
	}
	public class SecondOrderDynamicsForArray
	{
		private double[] xps;

		private double[] xds;

		private double[] ys;

		private double[] yds;

		private double _w;

		private double _z;

		private double _d;

		private double k1;

		private double k2;

		private double k3;

		public SecondOrderDynamicsForArray(double f, double z, double r, double x0, int size)
		{
			_w = Math.PI * 2.0 * f;
			_z = z;
			_d = _w * Math.Sqrt(Math.Abs(z * z - 1.0));
			k1 = z / (Math.PI * f);
			k2 = 1.0 / (Math.PI * 2.0 * f * (Math.PI * 2.0 * f));
			k3 = r * z / (Math.PI * 2.0 * f);
			xps = new double[size];
			ys = new double[size];
			xds = new double[size];
			yds = new double[size];
			Array.Fill(xps, x0);
			Array.Fill(ys, x0);
		}

		public double[] Update(double deltaTime, double[] xs)
		{
			if (xs.Length != xps.Length)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < xds.Length; i++)
			{
				xds[i] = (xs[i] - xps[i]) / deltaTime;
			}
			double num;
			double num2;
			if (_w * deltaTime < _z)
			{
				num = k1;
				num2 = Math.Max(Math.Max(k2, deltaTime * deltaTime / 2.0 + deltaTime * k1 / 2.0), deltaTime * k1);
			}
			else
			{
				double num3 = Math.Exp((0.0 - _z) * _w * deltaTime);
				double num4 = 2.0 * num3 * ((_z <= 1.0) ? Math.Cos(deltaTime * _d) : Math.Cosh(deltaTime * _d));
				double num5 = num3 * num3;
				double num6 = deltaTime / (1.0 + num5 - num4);
				num = (1.0 - num5) * num6;
				num2 = deltaTime * num6;
			}
			for (int j = 0; j < ys.Length; j++)
			{
				ys[j] += deltaTime * yds[j];
				yds[j] += deltaTime * (xs[j] + k3 * xds[j] - ys[j] - num * yds[j]) / num2;
			}
			for (int k = 0; k < xps.Length; k++)
			{
				xps[k] = xs[k];
			}
			return ys;
		}
	}
	public class SerPort : IDisposable
	{
		private SerialPort port;

		public SerPort()
		{
			try
			{
				port = new SerialPort(GlobalVars.LightCOM, 115200, Parity.None, 8, StopBits.One);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		~SerPort()
		{
			try
			{
				port.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void OpenCom()
		{
			try
			{
				if (port != null)
				{
					port.Open();
				}
			}
			catch (Exception)
			{
			}
		}

		public bool IsComOpen()
		{
			try
			{
				if (port == null)
				{
					return false;
				}
				if (port.IsOpen)
				{
					return true;
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void CloseCom()
		{
			try
			{
				if (port != null)
				{
					port.Close();
				}
			}
			catch (Exception)
			{
			}
		}

		public void DataSendToPorts(byte[] inData)
		{
			try
			{
				if (port != null)
				{
					byte[] array = new byte[inData.Length];
					array = inData;
					port.Write(array, 0, array.Length);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void sendCom(object back_light_bufStruct)
		{
			try
			{
				byte[] array = new byte[Marshal.SizeOf(back_light_bufStruct)];
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				try
				{
					Marshal.StructureToPtr(back_light_bufStruct, intPtr, fDeleteOld: false);
					Marshal.Copy(intPtr, array, 0, array.Length);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				DataSendToPorts(array);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void Dispose()
		{
			port.Close();
		}
	}
	public class Usb : IUSBServices
	{
		public static UsbDeviceFinder MyUsbFinder = new UsbDeviceFinder(13326, 32770);

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
				System.Windows.Forms.MessageBox.Show("发生异常：" + ex.ToString());
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
					System.Windows.Forms.MessageBox.Show("end send data");
					return true;
				}
				error = error2;
				usbDevice.Close();
				System.Windows.Forms.MessageBox.Show("end send data");
				return false;
			}
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show("发生异常" + ex.ToString());
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
	}
	public class Visualizer
	{
		private double[] _sampleData;

		private DateTime _lastTime;

		private SecondOrderDynamicsForArray _dynamics;

		public double[] SampleData => _sampleData;

		public Visualizer(int waveDataSize)
		{
			if (!Get2Flag(waveDataSize))
			{
				throw new ArgumentException("长度必须是 2 的 n 次幂");
			}
			_lastTime = DateTime.Now;
			_sampleData = new double[waveDataSize];
			_dynamics = new SecondOrderDynamicsForArray(1.0, 1.0, 1.0, 0.0, waveDataSize / 2);
		}

		private bool Get2Flag(int num)
		{
			if (num < 1)
			{
				return false;
			}
			return (num & (num - 1)) == 0;
		}

		public void PushSampleData(double[] waveData)
		{
			if (waveData.Length == 0)
			{
				Array.Clear(_sampleData, 0, _sampleData.Length);
				return;
			}
			if (waveData.Length > _sampleData.Length)
			{
				Array.Copy(waveData, waveData.Length - _sampleData.Length, _sampleData, 0, _sampleData.Length);
				return;
			}
			Array.Copy(_sampleData, waveData.Length, _sampleData, 0, _sampleData.Length - waveData.Length);
			Array.Copy(waveData, 0, _sampleData, _sampleData.Length - waveData.Length, waveData.Length);
		}

		public double[] GetSpectrumData()
		{
			DateTime now = DateTime.Now;
			double totalSeconds = (now - _lastTime).TotalSeconds;
			_lastTime = now;
			int num = _sampleData.Length;
			Complex[] array = new Complex[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new Complex(_sampleData[i], 0.0);
			}
			FftSharp.Transform.FFT(array);
			int num2 = num / 2;
			double[] array2 = new double[num2];
			for (int j = 0; j < num2; j++)
			{
				array2[j] = array[j].Magnitude / (double)num;
			}
			Bartlett bartlett = new Bartlett();
			bartlett.Create(num2);
			bartlett.ApplyInPlace(array2);
			return _dynamics.Update(totalSeconds, array2);
		}

		public static double[] TakeSpectrumOfFrequency(double[] spectrum, double sampleRate, double frequency)
		{
			double num = sampleRate / (double)spectrum.Length;
			int num2 = (int)Math.Min(frequency / num, spectrum.Length);
			double[] array = new double[num2];
			Array.Copy(spectrum, 0, array, 0, num2);
			return array;
		}

		public static double[] MakeSmooth(double[] data, int radius)
		{
			double[] weights = GetWeights(radius);
			double[] array = new double[1 + radius * 2];
			double[] array2 = new double[data.Length];
			if (data.Length < radius)
			{
				Array.Fill(array2, data.Average());
				return array2;
			}
			for (int i = 0; i < radius; i++)
			{
				Array.Fill(array, data[i], 0, radius + 1);
				for (int j = 0; j < radius; j++)
				{
					array[radius + 1 + j] = data[i + j];
				}
				ApplyWeights(array, weights);
				array2[i] = array.Sum();
			}
			for (int k = radius; k < data.Length - radius; k++)
			{
				for (int l = 0; l < radius; l++)
				{
					array[l] = data[k - l];
				}
				array[radius] = data[k];
				for (int m = 0; m < radius; m++)
				{
					array[radius + m + 1] = data[k + m];
				}
				ApplyWeights(array, weights);
				array2[k] = array.Sum();
			}
			for (int n = data.Length - radius; n < data.Length; n++)
			{
				Array.Fill(array, data[n], 0, radius + 1);
				for (int num = 0; num < radius; num++)
				{
					array[radius + 1 + num] = data[n - num];
				}
				ApplyWeights(array, weights);
				array2[n] = array.Sum();
			}
			return array2;
			static void ApplyWeights(double[] buffer, double[] array3)
			{
				int num2 = buffer.Length;
				for (int num3 = 0; num3 < num2; num3++)
				{
					buffer[num3] *= array3[num3];
				}
			}
			static double Gaussian(double x)
			{
				return Math.Pow(Math.E, -4.0 * x * x);
			}
			static double[] GetWeights(int num3)
			{
				int num2 = 1 + num3 * 2;
				int num4 = num2 - 1;
				double num5 = num3;
				double[] array3 = new double[num2];
				for (int num6 = 0; num6 <= num3; num6++)
				{
					array3[num3 + num6] = Gaussian((double)num6 / num5);
				}
				for (int num7 = 0; num7 < num3; num7++)
				{
					array3[num7] = array3[num4 - num7];
				}
				double num8 = array3.Sum();
				for (int num9 = 0; num9 < num2; num9++)
				{
					array3[num9] /= num8;
				}
				return array3;
			}
		}
	}
	public class Win32 : IWin32
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

		private CounterSample cval;

		private static int hHook;

		public const int WH_KEYBOARD_LL = 13;

		private static HookProc KeyBoardHookProcedure;

		public string? GetBattery()
		{
			string result = "0";
			try
			{
				using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_Battery").GetInstances().GetEnumerator();
				if (managementObjectEnumerator.MoveNext())
				{
					result = managementObjectEnumerator.Current["EstimatedChargeRemaining"].ToString();
				}
			}
			catch
			{
				result = "0";
			}
			return result;
		}

		public string? GetBatteryStatus()
		{
			string result = "0";
			try
			{
				using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_Battery").GetInstances().GetEnumerator();
				if (managementObjectEnumerator.MoveNext())
				{
					result = managementObjectEnumerator.Current["BatteryStatus"].ToString();
				}
			}
			catch
			{
				result = "0";
			}
			return result;
		}

		public string GetCPUName()
		{
			string result = string.Empty;
			try
			{
				_ = string.Empty;
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

		public int GetCPUOP(int Step)
		{
			string s = string.Empty;
			try
			{
				PerformanceCounter performanceCounter = new PerformanceCounter("Processor Information", "% Processor Utility", "_Total");
				if (Step == 0)
				{
					cval = performanceCounter.NextSample();
				}
				if (Step == 1)
				{
					s = CounterSample.Calculate(cval, performanceCounter.NextSample()).ToString("0.0");
				}
				performanceCounter.Dispose();
				return Convert.ToInt32(Math.Round(float.Parse(s)));
			}
			catch
			{
			}
			return 0;
		}

		public string? GetDiskName()
		{
			string result = string.Empty;
			try
			{
				_ = string.Empty;
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

		public int GetDiskOP()
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
			return Convert.ToInt32(100.0 * array[1] / array[0]);
		}

		public int GetDiskUesd()
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
			return Convert.ToInt32(array[1] / 1024.0 / 1024.0 / 1024.0);
		}

		public string GetDiskSize()
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

		public string? GetGPUName()
		{
			Tuple<string, string> tuple = null;
			try
			{
				ManagementObjectCollection instances = new ManagementClass("Win32_VideoController").GetInstances();
				if (instances.Count >= 2)
				{
					tuple = new Tuple<string, string>("null", "null");
					foreach (ManagementObject item in instances)
					{
						if (!item["Caption"].ToString().Contains("NVIDIA"))
						{
							continue;
						}
						if (item["Status"].ToString() == "OK")
						{
							tuple = new Tuple<string, string>(item["VideoProcessor"].ToString().Replace("Family", ""), ToGB(Convert.ToInt64(item["AdapterRAM"].ToString()), 1024.0));
							break;
						}
						foreach (ManagementObject item2 in instances)
						{
							if (item2["Caption"].ToString().Contains("Graphic"))
							{
								tuple = new Tuple<string, string>(item2["VideoProcessor"].ToString().Replace("Family", ""), ToGB(Convert.ToInt64(item2["AdapterRAM"].ToString()), 1024.0));
								break;
							}
						}
						break;
					}
				}
				else
				{
					using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = instances.GetEnumerator();
					if (managementObjectEnumerator.MoveNext())
					{
						ManagementObject managementObject3 = (ManagementObject)managementObjectEnumerator.Current;
						tuple = new Tuple<string, string>(managementObject3["VideoProcessor"].ToString().Replace("Family", ""), ToGB(Convert.ToInt64(managementObject3["AdapterRAM"].ToString()), 1024.0));
					}
				}
			}
			catch
			{
				tuple = new Tuple<string, string>("null", "null");
			}
			return tuple.Item1 + tuple.Item2;
		}

		public float GetGPUOP()
		{
			return 8.8f;
		}

		public string? GetMemoryName()
		{
			ManagementObjectCollection instances = new ManagementClass("Win32_PhysicalMemory").GetInstances();
			string result = string.Empty;
			try
			{
				using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = instances.GetEnumerator();
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

		public string GetMemoryAvailable()
		{
			string result = string.Empty;
			double num = 0.0;
			try
			{
				using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_PerfFormattedData_PerfOS_Memory").GetInstances().GetEnumerator())
				{
					if (managementObjectEnumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
						num += Math.Round((double)long.Parse(managementObject.Properties["AvailableMBytes"].Value.ToString()) / 1024.0, 1);
					}
				}
				result = num.ToString();
			}
			catch
			{
			}
			return result;
		}

		public string GetMemorySize()
		{
			string result = string.Empty;
			double num = 0.0;
			try
			{
				foreach (ManagementObject instance in new ManagementClass("Win32_PhysicalMemory").GetInstances())
				{
					num += Math.Round((double)(long.Parse(instance.Properties["Capacity"].Value.ToString()) / 1024 / 1024) / 1024.0, 1);
				}
				result = num.ToString();
			}
			catch
			{
			}
			return result;
		}

		public string GetCpuFrequency()
		{
			uint num = 0u;
			try
			{
				PerformanceCounter performanceCounter = new PerformanceCounter("Processor Information", "% Processor Performance", "_Total");
				double num2 = performanceCounter.NextValue();
				Thread.Sleep(1000);
				num2 = performanceCounter.NextValue();
				foreach (ManagementObject item in new ManagementObjectSearcher("SELECT *, Name FROM Win32_Processor").Get())
				{
					num = (uint)(Convert.ToDouble(item["MaxClockSpeed"]) * num2 / 100.0);
				}
			}
			catch
			{
			}
			return num.ToString();
		}

		public string GetGpuFrequency()
		{
			string result = "";
			try
			{
				using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("select * from Win32_VideoController").Get().GetEnumerator();
				if (managementObjectEnumerator.MoveNext())
				{
					result = ((ManagementObject)managementObjectEnumerator.Current)["CurrentRefreshRate"].ToString() ?? "";
				}
			}
			catch
			{
			}
			return result;
		}

		public string GetCpuMaxFrequency()
		{
			string result = "";
			try
			{
				using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("select * from Win32_Processor").Get().GetEnumerator();
				if (managementObjectEnumerator.MoveNext())
				{
					result = ((ManagementObject)managementObjectEnumerator.Current)["MaxClockSpeed"].ToString() ?? "";
				}
			}
			catch
			{
			}
			return result;
		}

		public string GetGpuMaxFrequency()
		{
			string result = "";
			try
			{
				using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("select * from Win32_VideoController").Get().GetEnumerator();
				if (managementObjectEnumerator.MoveNext())
				{
					result = ((ManagementObject)managementObjectEnumerator.Current)["MaxClockSpeed"].ToString() ?? "";
				}
			}
			catch
			{
			}
			return result;
		}

		public string ToGB(double size, double mod)
		{
			string[] array = new string[6] { "B", "K", "M", "G", "T", "P" };
			int num = 0;
			while (size >= mod)
			{
				size /= mod;
				num++;
			}
			return Math.Round(size) + array[num];
		}

		public uint getNVGPUclk()
		{
			uint num = 0u;
			try
			{
				PhysicalGPU[] physicalGPUs = PhysicalGPU.GetPhysicalGPUs();
				PhysicalGPU[] array = physicalGPUs;
				foreach (PhysicalGPU physicalGPU in array)
				{
					num += physicalGPU.CurrentClockFrequencies.GraphicsClock.Frequency / 1000;
				}
				return (uint)(num / physicalGPUs.Length);
			}
			catch
			{
			}
			return 0u;
		}

		public uint getNVGPUMaxFrequency()
		{
			uint num = 0u;
			try
			{
				PhysicalGPU physicalGPU = new PhysicalGPU(GPUApi.EnumPhysicalGPUs()[0]);
				IClockFrequencies baseClockFrequencies = physicalGPU.BaseClockFrequencies;
				IClockFrequencies boostClockFrequencies = physicalGPU.BoostClockFrequencies;
				return baseClockFrequencies.GraphicsClock.Frequency / 1000 + boostClockFrequencies.GraphicsClock.Frequency / 1000;
			}
			catch
			{
				return 1u;
			}
		}

		public int getNVTem()
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

		public string getCom()
		{
			string text = "";
			try
			{
				using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("select * from Win32_PNPEntity where PNPDeviceID ='USB\\\\VID_1A86&PID_7523\\\\5&157263AF&0&4'").Get().GetEnumerator();
				if (managementObjectEnumerator.MoveNext())
				{
					text = ((ManagementObject)managementObjectEnumerator.Current)["Name"].ToString() ?? "";
					text = text.Split('(', ')')[1];
					return text;
				}
				return text;
			}
			catch (Exception ex)
			{
				Console.WriteLine("WMI查询错误: " + ex.Message);
				return text;
			}
		}

		[DllImport("user32.dll")]
		public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern bool UnhookWindowsHookEx(int idHook);

		[DllImport("user32.dll")]
		public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetModuleHandle(string name);

		private static int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
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
			bool flag = true;
			if (hHook != 0)
			{
				flag = UnhookWindowsHookEx(hHook);
				hHook = 0;
			}
			if (!flag)
			{
				System.Windows.MessageBox.Show("hHook error");
			}
		}
	}
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
}
namespace BydCentral.CustomControl
{
	public class Fan : System.Windows.Controls.UserControl, IComponentConnector
	{
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(Fan), new PropertyMetadata(0, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			((Fan)s).Value = (int)e.NewValue;
		}));

		private bool _contentLoaded;

		public int Value
		{
			get
			{
				return (int)GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}

		public Fan()
		{
			InitializeComponent();
			base.DataContext = this;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/fan.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
	public class MonitorItem : System.Windows.Controls.UserControl, IComponentConnector
	{
		public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register("ItemName", typeof(string), typeof(MonitorItem), new PropertyMetadata(null, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			((MonitorItem)s).ItemName = (string)e.NewValue;
		}));

		public static readonly DependencyProperty ItemTitleProperty = DependencyProperty.Register("ItemTitle", typeof(string), typeof(MonitorItem), new PropertyMetadata(null, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			((MonitorItem)s).ItemTitle = (string)e.NewValue;
		}));

		public static readonly DependencyProperty ItemContentProperty = DependencyProperty.Register("ItemContent", typeof(string), typeof(MonitorItem), new PropertyMetadata(null, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			((MonitorItem)s).ItemContent = (string)e.NewValue;
		}));

		public static readonly DependencyProperty ItemValueProperty = DependencyProperty.Register("ItemValue", typeof(int), typeof(MonitorItem), new PropertyMetadata(0, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
		{
			((MonitorItem)s).ItemValue = (int)e.NewValue;
		}));

		private bool _contentLoaded;

		public string ItemName
		{
			get
			{
				return (string)GetValue(ItemNameProperty);
			}
			set
			{
				SetValue(ItemNameProperty, value);
			}
		}

		public string ItemTitle
		{
			get
			{
				return (string)GetValue(ItemTitleProperty);
			}
			set
			{
				SetValue(ItemTitleProperty, value);
			}
		}

		public string ItemContent
		{
			get
			{
				return (string)GetValue(ItemContentProperty);
			}
			set
			{
				SetValue(ItemContentProperty, value);
			}
		}

		public int ItemValue
		{
			get
			{
				return (int)GetValue(ItemValueProperty);
			}
			set
			{
				SetValue(ItemValueProperty, value);
			}
		}

		public MonitorItem()
		{
			InitializeComponent();
			base.DataContext = this;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/monitoritem.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
}
namespace BydCentral.CustomControl.Resource
{
	public class CustomFan : RingProgressBar
	{
		public static readonly DependencyProperty CustomTextProperty = DependencyProperty.Register("CustomText", typeof(string), typeof(CustomFan));

		public string CustomText
		{
			get
			{
				return (string)GetValue(CustomTextProperty);
			}
			set
			{
				SetValue(CustomTextProperty, value);
			}
		}

		public CustomFan()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomFan), new FrameworkPropertyMetadata(typeof(CustomFan)));
		}
	}
	public class FontSizeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double num && parameter is double num2)
			{
				return Math.Round(num * num2);
			}
			return System.Windows.Data.Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
	public class MultiplierConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
namespace BydCentral.CustomControl.Helper
{
	public static class GridHelpers
	{
		public static readonly DependencyProperty AutoFontSizeProperty = DependencyProperty.RegisterAttached("AutoFontSize", typeof(bool), typeof(GridHelpers), new FrameworkPropertyMetadata(false, OnAutoFontSizeChanged));

		public static bool GetAutoFontSize(DependencyObject obj)
		{
			return (bool)obj.GetValue(AutoFontSizeProperty);
		}

		public static void SetAutoFontSize(DependencyObject obj, bool value)
		{
			obj.SetValue(AutoFontSizeProperty, value);
		}

		private static void OnAutoFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is FrameworkElement frameworkElement))
			{
				return;
			}
			object newValue = e.NewValue;
			if (newValue is bool)
			{
				if ((bool)newValue)
				{
					frameworkElement.SizeChanged += Element_SizeChanged;
					AdjustFontSize(frameworkElement);
				}
				else
				{
					frameworkElement.SizeChanged -= Element_SizeChanged;
				}
			}
		}

		private static void Element_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (sender is FrameworkElement element)
			{
				AdjustFontSize(element);
			}
		}

		private static void AdjustFontSize(FrameworkElement element)
		{
			if (element.Parent is Grid grid)
			{
				double num = Math.Min(grid.ActualWidth, grid.ActualHeight);
				if (element is TextBlock textBlock)
				{
					textBlock.FontSize = num * 0.15;
				}
			}
		}
	}
	public static class MultiRegionHelper
	{
		public static readonly DependencyProperty IrregularRegionsProperty = DependencyProperty.RegisterAttached("IrregularRegions", typeof(Geometry[]), typeof(MultiRegionHelper), new PropertyMetadata(null, IrregularRegionsChanged));

		public static readonly DependencyProperty ClickHandlersProperty = DependencyProperty.RegisterAttached("ClickHandlers", typeof(RoutedEventHandler[]), typeof(MultiRegionHelper), new PropertyMetadata(null));

		public static Geometry[] GetIrregularRegions(DependencyObject obj)
		{
			return (Geometry[])obj.GetValue(IrregularRegionsProperty);
		}

		public static void SetIrregularRegions(DependencyObject obj, Geometry[] value)
		{
			obj.SetValue(IrregularRegionsProperty, value);
		}

		public static RoutedEventHandler[] GetClickHandlers(DependencyObject obj)
		{
			return (RoutedEventHandler[])obj.GetValue(ClickHandlersProperty);
		}

		public static void SetClickHandlers(DependencyObject obj, RoutedEventHandler[] value)
		{
			obj.SetValue(ClickHandlersProperty, value);
		}

		private static void IrregularRegionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is System.Windows.Controls.Button button)
			{
				button.MouseLeftButtonDown += Button_MouseLeftButtonDown;
			}
		}

		private static void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
			System.Windows.Point position = e.GetPosition(button);
			Geometry[] irregularRegions = GetIrregularRegions(button);
			RoutedEventHandler[] clickHandlers = GetClickHandlers(button);
			if (irregularRegions == null || clickHandlers == null)
			{
				return;
			}
			for (int i = 0; i < irregularRegions.Length; i++)
			{
				if (irregularRegions[i] != null && irregularRegions[i].FillContains(position) && clickHandlers[i] != null)
				{
					clickHandlers[i](button, new RoutedEventArgs());
					break;
				}
			}
		}
	}
}
namespace BydCentral.CustomControl.CustomUserControl
{
	public class ColorPanelUnabled : System.Windows.Controls.UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public ColorPanelUnabled()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/customusercontrol/colorpanelunabled.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
	public class KeyboardButton : System.Windows.Controls.UserControl, IComponentConnector
	{
		internal System.Windows.Shapes.Path keyboard1;

		internal System.Windows.Shapes.Path keyboard2;

		internal System.Windows.Shapes.Path keyboard3;

		internal System.Windows.Shapes.Path keyboard4;

		private bool _contentLoaded;

		public KeyboardButton()
		{
			InitializeComponent();
		}

		private void PathContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/customusercontrol/keyboardbutton.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				keyboard1 = (System.Windows.Shapes.Path)target;
				break;
			case 2:
				keyboard2 = (System.Windows.Shapes.Path)target;
				break;
			case 3:
				keyboard3 = (System.Windows.Shapes.Path)target;
				break;
			case 4:
				keyboard4 = (System.Windows.Shapes.Path)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
	public class Settings : System.Windows.Controls.UserControl, IComponentConnector
	{
		public static readonly DependencyProperty BatModePorpetry = DependencyProperty.Register("BatMode", typeof(int), typeof(Settings), new FrameworkPropertyMetadata(0, OnBatModePropertyChanged));

		public static readonly DependencyProperty ChargeModePorpetry = DependencyProperty.Register("ChargeMode", typeof(int), typeof(Settings), new FrameworkPropertyMetadata(0, OnChargeModePropertyChanged));

		public static readonly DependencyProperty OutputModePorpetry = DependencyProperty.Register("OutputMode", typeof(int), typeof(Settings), new FrameworkPropertyMetadata(0, OnOutputModePropertyChanged));

		internal System.Windows.Controls.Image gpuMode;

		internal System.Windows.Controls.Image outputMode;

		internal System.Windows.Controls.Image output;

		internal System.Windows.Controls.Image GPUOUT;

		internal System.Windows.Controls.Image IGPUOUT;

		internal System.Windows.Controls.Image GPUIN;

		internal System.Windows.Controls.Image IGPUIN;

		internal System.Windows.Controls.Image ChargingImg;

		internal System.Windows.Controls.Image Charging;

		internal System.Windows.Controls.Image batMode;

		private bool _contentLoaded;

		public int BatMode
		{
			get
			{
				return (int)GetValue(BatModePorpetry);
			}
			set
			{
				SetValue(BatModePorpetry, value);
			}
		}

		public int ChargeMode
		{
			get
			{
				return (int)GetValue(ChargeModePorpetry);
			}
			set
			{
				SetValue(ChargeModePorpetry, value);
			}
		}

		public int OutputMode
		{
			get
			{
				return (int)GetValue(OutputModePorpetry);
			}
			set
			{
				SetValue(OutputModePorpetry, value);
			}
		}

		private static void OnBatModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Settings)d).batChanged();
		}

		private void batChanged()
		{
			double num = MapToRange(BatMode, 0.0, 100.0, 0.0, 10.0);
			if (num == 0.0)
			{
				num = 1.0;
			}
			if (ChargeMode == 1 && num < 5.0)
			{
				num += 10.0;
			}
			Convert.ToInt32(num);
			batMode.Source = new BitmapImage(new Uri("../../image/bat_" + num + ".png", UriKind.Relative));
		}

		public double MapToRange(double value, double minSource, double maxSource, double minTarget, double maxTarget)
		{
			value = Math.Max(minSource, Math.Min(maxSource, value));
			double num = maxSource - minSource;
			double num2 = (maxTarget - minTarget) / num;
			return Math.Round((value - minSource) * num2 + minTarget);
		}

		private static void OnChargeModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Settings)d).chargChanged();
		}

		private void chargChanged()
		{
			if (ChargeMode == 2)
			{
				ChargingImg.Visibility = Visibility.Visible;
				Charging.Visibility = Visibility.Visible;
			}
			else if (ChargeMode == 1)
			{
				ChargingImg.Visibility = Visibility.Hidden;
				Charging.Visibility = Visibility.Hidden;
			}
		}

		private static void OnOutputModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Settings)d).outputChanged();
		}

		private void outputChanged()
		{
			switch (OutputMode)
			{
			case 1:
				dgpuOnlyOutput();
				break;
			case 3:
				dgpuOffOutput();
				break;
			case 2:
				mixOutput();
				break;
			}
		}

		private void dgpuOnlyOutput()
		{
			gpuMode.Source = new BitmapImage(new Uri("../../image/GPU_on.png", UriKind.Relative));
			outputMode.Source = new BitmapImage(new Uri("../../image/gpu_flow.png", UriKind.Relative));
			IGPUOUT.Visibility = Visibility.Hidden;
			GPUIN.Visibility = Visibility.Visible;
			GPUOUT.Visibility = Visibility.Visible;
			IGPUIN.Visibility = Visibility.Visible;
		}

		private void mixOutput()
		{
			gpuMode.Source = new BitmapImage(new Uri("../../image/GPU_on.png", UriKind.Relative));
			outputMode.Source = new BitmapImage(new Uri("../../image/mix.png", UriKind.Relative));
			IGPUOUT.Visibility = Visibility.Visible;
			GPUIN.Visibility = Visibility.Visible;
			GPUOUT.Visibility = Visibility.Visible;
			IGPUIN.Visibility = Visibility.Visible;
		}

		private void dgpuOffOutput()
		{
			gpuMode.Source = new BitmapImage(new Uri("../../image/GPU_off.png", UriKind.Relative));
			outputMode.Source = new BitmapImage(new Uri("../../image/igpu_flow.png", UriKind.Relative));
			IGPUOUT.Visibility = Visibility.Visible;
			GPUIN.Visibility = Visibility.Hidden;
			GPUOUT.Visibility = Visibility.Hidden;
			IGPUIN.Visibility = Visibility.Visible;
		}

		private void SettingsLoad()
		{
			batChanged();
			outputChanged();
			chargChanged();
		}

		public Settings()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/customusercontrol/settings.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				gpuMode = (System.Windows.Controls.Image)target;
				break;
			case 2:
				outputMode = (System.Windows.Controls.Image)target;
				break;
			case 3:
				output = (System.Windows.Controls.Image)target;
				break;
			case 4:
				GPUOUT = (System.Windows.Controls.Image)target;
				break;
			case 5:
				IGPUOUT = (System.Windows.Controls.Image)target;
				break;
			case 6:
				GPUIN = (System.Windows.Controls.Image)target;
				break;
			case 7:
				IGPUIN = (System.Windows.Controls.Image)target;
				break;
			case 8:
				ChargingImg = (System.Windows.Controls.Image)target;
				break;
			case 9:
				Charging = (System.Windows.Controls.Image)target;
				break;
			case 10:
				batMode = (System.Windows.Controls.Image)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
	public class Settings_Dark : System.Windows.Controls.UserControl, IComponentConnector
	{
		public static readonly DependencyProperty BatModePorpetry = DependencyProperty.Register("BatMode", typeof(int), typeof(Settings_Dark), new FrameworkPropertyMetadata(0, OnBatModePropertyChanged));

		public static readonly DependencyProperty ChargeModePorpetry = DependencyProperty.Register("ChargeMode", typeof(int), typeof(Settings_Dark), new FrameworkPropertyMetadata(0, OnChargeModePropertyChanged));

		public static readonly DependencyProperty OutputModePorpetry = DependencyProperty.Register("OutputMode", typeof(int), typeof(Settings_Dark), new FrameworkPropertyMetadata(0, OnOutputModePropertyChanged));

		internal System.Windows.Controls.Image gpuMode;

		internal System.Windows.Controls.Image outputMode;

		internal System.Windows.Controls.Image output;

		internal System.Windows.Controls.Image GPUOUT;

		internal System.Windows.Controls.Image IGPUOUT;

		internal System.Windows.Controls.Image GPUIN;

		internal System.Windows.Controls.Image IGPUIN;

		internal System.Windows.Controls.Image ChargingImg;

		internal System.Windows.Controls.Image Charging;

		internal System.Windows.Controls.Image batMode;

		private bool _contentLoaded;

		public int BatMode
		{
			get
			{
				return (int)GetValue(BatModePorpetry);
			}
			set
			{
				SetValue(BatModePorpetry, value);
			}
		}

		public int ChargeMode
		{
			get
			{
				return (int)GetValue(ChargeModePorpetry);
			}
			set
			{
				SetValue(ChargeModePorpetry, value);
			}
		}

		public int OutputMode
		{
			get
			{
				return (int)GetValue(OutputModePorpetry);
			}
			set
			{
				SetValue(OutputModePorpetry, value);
			}
		}

		private static void OnBatModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Settings_Dark)d).batChanged();
		}

		private void batChanged()
		{
			double num = MapToRange(BatMode, 0.0, 100.0, 0.0, 10.0);
			if (num == 0.0)
			{
				num = 1.0;
			}
			if (ChargeMode == 1 && num < 5.0)
			{
				num += 10.0;
			}
			Convert.ToInt32(num);
			batMode.Source = new BitmapImage(new Uri("../../image/bat_" + num + ".png", UriKind.Relative));
		}

		public double MapToRange(double value, double minSource, double maxSource, double minTarget, double maxTarget)
		{
			value = Math.Max(minSource, Math.Min(maxSource, value));
			double num = maxSource - minSource;
			double num2 = (maxTarget - minTarget) / num;
			return Math.Round((value - minSource) * num2 + minTarget);
		}

		private static void OnChargeModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Settings_Dark)d).chargChanged();
		}

		private void chargChanged()
		{
			if (ChargeMode == 2)
			{
				ChargingImg.Visibility = Visibility.Visible;
				Charging.Visibility = Visibility.Visible;
			}
			else if (ChargeMode == 1)
			{
				ChargingImg.Visibility = Visibility.Hidden;
				Charging.Visibility = Visibility.Hidden;
			}
		}

		private static void OnOutputModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Settings_Dark)d).outputChanged();
		}

		private void outputChanged()
		{
			switch (OutputMode)
			{
			case 1:
				dgpuOnlyOutput();
				break;
			case 3:
				dgpuOffOutput();
				break;
			case 2:
				mixOutput();
				break;
			}
		}

		private void dgpuOnlyOutput()
		{
			gpuMode.Source = new BitmapImage(new Uri("../../image/dark/gpu_on.png", UriKind.Relative));
			outputMode.Source = new BitmapImage(new Uri("../../image/gpu_flow.png", UriKind.Relative));
			IGPUOUT.Visibility = Visibility.Hidden;
			GPUIN.Visibility = Visibility.Visible;
			GPUOUT.Visibility = Visibility.Visible;
			IGPUIN.Visibility = Visibility.Visible;
		}

		private void mixOutput()
		{
			gpuMode.Source = new BitmapImage(new Uri("../../image/dark/gpu_on.png", UriKind.Relative));
			outputMode.Source = new BitmapImage(new Uri("../../image/mix.png", UriKind.Relative));
			IGPUOUT.Visibility = Visibility.Visible;
			GPUIN.Visibility = Visibility.Visible;
			GPUOUT.Visibility = Visibility.Visible;
			IGPUIN.Visibility = Visibility.Visible;
		}

		private void dgpuOffOutput()
		{
			gpuMode.Source = new BitmapImage(new Uri("../../image/dark/GPU_off.png", UriKind.Relative));
			outputMode.Source = new BitmapImage(new Uri("../../image/igpu_flow.png", UriKind.Relative));
			IGPUOUT.Visibility = Visibility.Visible;
			GPUIN.Visibility = Visibility.Hidden;
			GPUOUT.Visibility = Visibility.Hidden;
			IGPUIN.Visibility = Visibility.Visible;
		}

		private void SettingsLoad()
		{
			batChanged();
			outputChanged();
			chargChanged();
		}

		public Settings_Dark()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/customusercontrol/settings_dark.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
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
				gpuMode = (System.Windows.Controls.Image)target;
				break;
			case 2:
				outputMode = (System.Windows.Controls.Image)target;
				break;
			case 3:
				output = (System.Windows.Controls.Image)target;
				break;
			case 4:
				GPUOUT = (System.Windows.Controls.Image)target;
				break;
			case 5:
				IGPUOUT = (System.Windows.Controls.Image)target;
				break;
			case 6:
				GPUIN = (System.Windows.Controls.Image)target;
				break;
			case 7:
				IGPUIN = (System.Windows.Controls.Image)target;
				break;
			case 8:
				ChargingImg = (System.Windows.Controls.Image)target;
				break;
			case 9:
				Charging = (System.Windows.Controls.Image)target;
				break;
			case 10:
				batMode = (System.Windows.Controls.Image)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
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
