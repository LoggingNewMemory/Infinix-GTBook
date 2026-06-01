using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using BydCentral;
using BydCentral.Core.Models;
using BydCentral.Core.ViewModels;
using BydCentral.Services;
using CommunityToolkit.Mvvm.Messaging;
using CustomControl;
using CustomControlLibrary.Controls;
using ProtoBuf;

namespace BydContral;

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

	private Usb? usb;

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

	private Color keyboardColor = Colors.Green;

	private Color backColor = Colors.Green;

	private Color sideColor = Colors.Green;

	private Color keyboardColorRecord = Colors.Green;

	private Color backColorRecord = Colors.Green;

	private Color sideColorRecord = Colors.Green;

	private DateTime startTime = DateTime.Now;

	private int waveIndex;

	private int clockIndex;

	private int RainbowIndex;

	private volatile Color[] rainbowColors = new Color[7]
	{
		Colors.Red,
		Colors.Orange,
		Colors.Yellow,
		Colors.Green,
		Colors.Blue,
		Colors.Indigo,
		Colors.Violet
	};

	private List<Color> flowColors;

	private int FlowIndex;

	internal System.Windows.Controls.Button keyboard;

	internal System.Windows.Controls.Button back;

	internal Viewbox keyBoardView;

	internal Grid KeyboardGrid;

	internal Image ColorZone_back;

	internal Image ColorZone;

	internal Image Zone;

	internal System.Windows.Shapes.Path KeyboardPath1;

	internal System.Windows.Shapes.Path KeyboardPath2;

	internal System.Windows.Shapes.Path KeyboardPath3;

	internal System.Windows.Shapes.Path KeyboardPath4;

	internal Viewbox backView_2;

	internal Border border1;

	internal Grid back1_grid;

	internal TextBlock back1_text;

	internal Image ColorZone_back_1;

	internal Image ColorZone_1;

	internal Image Zone_1;

	internal Grid music1;

	internal TextBlock back1_color1_text;

	internal System.Windows.Controls.RadioButton back1_color1;

	internal Border back1_color1_border;

	internal Rectangle back1_color1_rec;

	internal Grid game1;

	internal TextBlock back1_color2_text;

	internal System.Windows.Controls.RadioButton back1_color2;

	internal Border back1_color2_border;

	internal Rectangle back1_color2_rec;

	internal TextBlock back1_color3_text;

	internal System.Windows.Controls.RadioButton back1_color3;

	internal Border back1_color3_border;

	internal Rectangle back1_color3_rec;

	internal Border border2;

	internal TextBlock back2_text;

	internal Image ColorZone_back_2;

	internal Image ColorZone_2;

	internal Image Zone_2;

	internal Grid music2;

	internal TextBlock back2_color1_text;

	internal System.Windows.Controls.RadioButton back2_color1;

	internal Border back2_color1_border;

	internal Rectangle back2_color1_rec;

	internal Grid game2;

	internal TextBlock back2_color2_text;

	internal System.Windows.Controls.RadioButton back2_color2;

	internal Border back2_color2_border;

	internal Rectangle back2_color2_rec;

	internal TextBlock back2_color3_text;

	internal System.Windows.Controls.RadioButton back2_color3;

	internal Border back2_color3_border;

	internal Rectangle back2_color3_rec;

	internal Grid KeyBoardGrid;

	internal System.Windows.Controls.RadioButton KeyBoardAlways;

	internal Image KeyBoardAlwaysImg;

	internal System.Windows.Controls.RadioButton KeyBoardBreath;

	internal Image KeyBoardBreathImg;

	internal System.Windows.Controls.RadioButton KeyBoardWave;

	internal Image KeyBoardWaveImg;

	internal System.Windows.Controls.RadioButton KeyBoardClock;

	internal Image KeyBoardClockImg;

	internal System.Windows.Controls.RadioButton KeyBoardRainbow;

	internal Image KeyBoardRainbowImg;

	internal System.Windows.Controls.RadioButton KeyBoardFlow;

	internal Image KeyBoardFlowImg;

	internal Grid ZoneGrid;

	internal System.Windows.Controls.RadioButton ZoneAlways;

	internal Image ZoneAlwaysImg;

	internal System.Windows.Controls.RadioButton ZoneBreath;

	internal Image ZoneBreathImg;

	internal System.Windows.Controls.RadioButton ZoneClock;

	internal Image ZoneClockImg;

	internal System.Windows.Controls.RadioButton ZoneRainbow;

	internal Image ZoneRainbowImg;

	internal Grid BackGrid;

	internal System.Windows.Controls.RadioButton BackAlways;

	internal Image BackAlwaysImg;

	internal System.Windows.Controls.RadioButton BackBreath;

	internal Image BackBreathImg;

	internal System.Windows.Controls.RadioButton BackMusic;

	internal Image BackMusicImg;

	internal System.Windows.Controls.RadioButton BackGaming;

	internal Image BackGameImg;

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
		usb = new Usb();
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
				Color color = ColorPanel.SelectBrush.Color;
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

	private async Task startComTick()
	{
		if (!serPort.IsComOpen())
		{
			await Task.Delay(50);
			serPort.OpenCom();
			dispatcherTimer.Stop();
			dispatcherTimer.Start();
			await Task.Delay(50);
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

	public async Task SaveLightData()
	{
		using (FileStream destination = new FileStream(System.Windows.Forms.Application.StartupPath + "\\LightConfig.dat", FileMode.Create))
		{
			Serializer.Serialize((Stream)destination, light);
		}
		await Task.CompletedTask;
	}

	private List<Color> LoadFlowColorsFromFile(string relativeResourcePath)
	{
		List<Color> list = new List<Color>();
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
					Color item = Color.FromRgb(r, g, b);
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
			keyboard.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			back.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
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
			keyboard.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			back.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
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

	public static ColorHSV ConvertRgbToHsv(Color color)
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

	private Color lightColor(Light_Infinix light_cur)
	{
		return Color.FromRgb((byte)CombineRGBWithBrightness(light_cur.R, (int)light_cur.L), (byte)CombineRGBWithBrightness(light_cur.G, (int)light_cur.L), (byte)CombineRGBWithBrightness(light_cur.B, (int)light_cur.L));
	}

	private Color lightBackColor(Light_Infinix light_cur)
	{
		return Color.FromRgb((byte)CombineRGBWithBrightness(light_cur.backR, (int)light_cur.backL), (byte)CombineRGBWithBrightness(light_cur.backG, (int)light_cur.backL), (byte)CombineRGBWithBrightness(light_cur.backB, (int)light_cur.backL));
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
			back1_color1_rec.Fill = new SolidColorBrush(Color.FromRgb((byte)ColorR, (byte)ColorG, (byte)ColorB));
		}
		else if (light[cur].Mode == 4)
		{
			back1_color2_rec.Fill = new SolidColorBrush(Color.FromRgb((byte)ColorR, (byte)ColorG, (byte)ColorB));
		}
	}

	private void ChangeBack2RecColor(int ColorR, int ColorG, int ColorB, double brightness)
	{
		if (light[cur].Mode == 3)
		{
			back2_color1_rec.Fill = new SolidColorBrush(Color.FromRgb((byte)ColorR, (byte)ColorG, (byte)ColorB));
		}
		else if (light[cur].Mode == 5)
		{
			back2_color2_rec.Fill = new SolidColorBrush(Color.FromRgb((byte)ColorR, (byte)ColorG, (byte)ColorB));
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
			KeyBoardAlways.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			KeyBoardAlwaysImg.Source = new BitmapImage(new Uri("/image/ao_on.png", UriKind.RelativeOrAbsolute));
			changeRGBText(light[cur]);
			ColorPanel.SelectValue = CombineColorByText();
			changeLightBrightness(light[cur].L);
			EnableUserSelect();
		}
		else
		{
			KeyBoardAlways.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			KeyBoardAlwaysImg.Source = new BitmapImage(new Uri("/image/ao_off.png", UriKind.RelativeOrAbsolute));
		}
	}

	private void ZoneAlways_Checked(object sender, RoutedEventArgs e)
	{
		if (ZoneAlways.IsChecked == true)
		{
			light[cur].Mode = 0;
			ZoneAlways.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			ZoneAlwaysImg.Source = new BitmapImage(new Uri("/image/dark/ao_on.png", UriKind.RelativeOrAbsolute));
			changeRGBText(light[cur]);
			ColorPanel.SelectValue = CombineColorByText();
			changeLightBrightness(light[cur].L);
			EnableUserSelect();
		}
		else
		{
			ZoneAlways.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
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
			BackAlways.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			BackAlwaysImg.Source = new BitmapImage(new Uri("/image/ao_on.png", UriKind.RelativeOrAbsolute));
			EnableUserSelect();
		}
		else
		{
			BackAlways.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			BackAlwaysImg.Source = new BitmapImage(new Uri("/image/ao_off.png", UriKind.RelativeOrAbsolute));
		}
	}

	private void KeyBoardBreath_Checked(object sender, RoutedEventArgs e)
	{
		if (KeyBoardBreath.IsChecked == true)
		{
			KeyBoardBreath.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			KeyBoardBreathImg.Source = new BitmapImage(new Uri("/image/br_on.png", UriKind.RelativeOrAbsolute));
			light[cur].Mode = 1;
			changeRGBText(light[cur + 1]);
			ColorPanel.SelectValue = CombineColorByText();
			changeLightBrightness(light[cur + 1].L);
			EnableUserSelect();
		}
		else
		{
			KeyBoardBreath.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			KeyBoardBreathImg.Source = new BitmapImage(new Uri("/image/br_off.png", UriKind.RelativeOrAbsolute));
			StopAnimation(breathTokenSource);
		}
	}

	private void ZoneBreath_Checked(object sender, RoutedEventArgs e)
	{
		if (ZoneBreath.IsChecked == true)
		{
			light[cur].Mode = 1;
			ZoneBreath.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
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
			ZoneBreath.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
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
			BackBreath.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			BackBreathImg.Source = new BitmapImage(new Uri("/image/br_on.png", UriKind.RelativeOrAbsolute));
			if (Custom_Back.IsChecked == true)
			{
				startTime = DateTime.Now;
				StartBreathAnimation();
			}
		}
		else
		{
			BackBreath.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			BackBreathImg.Source = new BitmapImage(new Uri("/image/br_off.png", UriKind.RelativeOrAbsolute));
			StopAnimation(breathTokenSource);
		}
	}

	private void KeyBoardWave_Checked(object sender, RoutedEventArgs e)
	{
		if (KeyBoardWave.IsChecked == true)
		{
			KeyBoardWave.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			KeyBoardWaveImg.Source = new BitmapImage(new Uri("/image/wa_on.png", UriKind.RelativeOrAbsolute));
			light[cur].Mode = 4;
			changeRGBText(light[cur + 4]);
			ColorPanel.SelectValue = CombineColorByText();
			changeLightBrightness(light[cur + 4].L);
			EnableUserSelect();
		}
		else
		{
			KeyBoardWave.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			KeyBoardWaveImg.Source = new BitmapImage(new Uri("/image/wa_off.png", UriKind.RelativeOrAbsolute));
			StopAnimation(waveTokenSource);
		}
	}

	private void KeyBoardClock_Checked(object sender, RoutedEventArgs e)
	{
		if (KeyBoardClock.IsChecked == true)
		{
			KeyBoardClock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			KeyBoardClockImg.Source = new BitmapImage(new Uri("/image/cl_on.png", UriKind.RelativeOrAbsolute));
			light[cur].Mode = 2;
			DisableUserSelect();
			changeLightBrightness(light[cur + 2].L);
		}
		else
		{
			KeyBoardClock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			KeyBoardClockImg.Source = new BitmapImage(new Uri("/image/cl_off.png", UriKind.RelativeOrAbsolute));
			StopAnimation(clockTokenSource);
		}
	}

	private void ZoneClock_Checked(object sender, RoutedEventArgs e)
	{
		if (ZoneClock.IsChecked == true)
		{
			light[cur].Mode = 2;
			ZoneClock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
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
			ZoneClock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			ZoneClockImg.Source = new BitmapImage(new Uri("/image/dark/cl_off.png", UriKind.RelativeOrAbsolute));
			StopAnimation(clockTokenSource);
		}
	}

	private void KeyBoardRainbow_Checked(object sender, RoutedEventArgs e)
	{
		if (KeyBoardRainbow.IsChecked == true)
		{
			KeyBoardRainbow.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			KeyBoardRainbowImg.Source = new BitmapImage(new Uri("/image/rb_on.png", UriKind.RelativeOrAbsolute));
			light[cur].Mode = 3;
			DisableUserSelect();
			changeLightBrightness(light[cur + 3].L);
		}
		else
		{
			KeyBoardRainbow.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			KeyBoardRainbowImg.Source = new BitmapImage(new Uri("/image/rb_off.png", UriKind.RelativeOrAbsolute));
			StopAnimation(rainbowTokenSource);
		}
	}

	private void ZoneRainbow_Checked(object sender, RoutedEventArgs e)
	{
		if (ZoneRainbow.IsChecked == true)
		{
			light[cur].Mode = 3;
			ZoneRainbow.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
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
			ZoneRainbow.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			ZoneRainbowImg.Source = new BitmapImage(new Uri("/image/dark/rb_off.png", UriKind.RelativeOrAbsolute));
			StopAnimation(rainbowTokenSource);
		}
	}

	private void KeyBoardFlow_Checked(object sender, RoutedEventArgs e)
	{
		if (KeyBoardFlow.IsChecked == true)
		{
			KeyBoardFlow.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			KeyBoardFlowImg.Source = new BitmapImage(new Uri("/image/fl_on.png", UriKind.RelativeOrAbsolute));
			light[0].Mode = 5;
			DisableUserSelect();
			changeLightBrightness(light[cur + 5].L);
		}
		else
		{
			KeyBoardFlow.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			KeyBoardFlowImg.Source = new BitmapImage(new Uri("/image/fl_off.png", UriKind.RelativeOrAbsolute));
			StopAnimation(flowTokenSource);
		}
	}

	private void BackMusic_Checked(object sender, RoutedEventArgs e)
	{
		stopAllTimer();
		if (BackMusic.IsChecked == true)
		{
			BackMusic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
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
			BackMusic.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
			BackMusicImg.Source = new BitmapImage(new Uri("/image/mu_off.png", UriKind.RelativeOrAbsolute));
		}
	}

	private void BackGaming_Checked(object sender, RoutedEventArgs e)
	{
		stopAllTimer();
		if (BackGaming.IsChecked == true)
		{
			BackGaming.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
			BackGameImg.Source = new BitmapImage(new Uri("/image/ga_on.png", UriKind.RelativeOrAbsolute));
			showGame();
			SetSelectedBack();
			if (light[CurMap["backlight"]].Switch == 1 && light[CurMap["backlight"]].Mode == 4)
			{
				ChangeBack1RecColor(light[gameRound].R, light[gameRound].G, light[gameRound].B, (int)light[gameRound].L);
				colorZoneBitmap[4].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Color.FromRgb(light[gameRound].backR, light[gameRound].backG, light[gameRound].backB));
				StartRoundAnimation();
			}
			if (light[CurMap["backlight"]].Switch == 1 && light[CurMap["backlight"]].Mode == 5)
			{
				ChangeBack2RecColor(light[gameCover].R, light[gameCover].G, light[gameCover].B, (int)light[gameCover].L);
				colorZoneBitmap[5].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Color.FromRgb(light[gameCover].backR, light[gameCover].backG, light[gameCover].backB));
				StartCoverAnimation();
			}
		}
		else
		{
			BackGaming.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBFBFBF"));
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
			Task.Run(delegate
			{
				GlobalVars.Wmi.KeyLight(isOpen: true);
			});
		}
		else
		{
			light[CurMap["keyboard"]].Switch = 0;
			Task.Run(delegate
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
			Task.Run(delegate
			{
				GlobalVars.Wmi.BackLight(isOpen: true);
			});
		}
		else
		{
			light[CurMap["backlight"]].Switch = 0;
			Task.Run(delegate
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
			Task.Run(delegate
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
		Task.Run(delegate
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

	private async Task SendComData()
	{
		_ = 1;
		try
		{
			isSendingData = true;
			await startComTick();
			setComData();
			serPort.sendCom(back_light_bufStruct);
			await Task.Delay(50);
		}
		finally
		{
			isSendingData = false;
		}
	}

	private async Task StartSendMusicData()
	{
		if (!semaphore.Wait(0))
		{
			return;
		}
		try
		{
			GlobalVars.IsMusicMode = true;
			capture.start();
			await Task.Delay(TimeSpan.FromSeconds(0.2));
			StartManualEvent();
			while (light[CurMap["backlight"]].Switch == 1 && (light[CurMap["backlight"]].Mode == 2 || light[CurMap["backlight"]].Mode == 3))
			{
				await Task.Delay(TimeSpan.FromSeconds(0.1));
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
				colorZoneBitmap[4].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Color.FromRgb(light[gameRound].backR, light[gameRound].backG, light[gameRound].backB));
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
				colorZoneBitmap[5].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, Color.FromRgb(light[gameCover].backR, light[gameCover].backG, light[gameCover].backB));
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

	public async Task StartBreathAnimation()
	{
		if (breathTokenSource != null)
		{
			breathTokenSource.Cancel();
			await Task.Delay(100);
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
				await Task.Delay(TimeSpan.FromMilliseconds(50.0), breathToken);
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

	public async Task StartWaveAnimation()
	{
		if (waveTokenSource != null)
		{
			waveTokenSource.Cancel();
			await Task.Delay(150);
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
				await Task.Delay(TimeSpan.FromSeconds(0.1), waveToken);
			}
		}
		finally
		{
			waveTokenSource.Cancel();
		}
	}

	public async Task StartClockAnimation()
	{
		if (clockTokenSource != null)
		{
			clockTokenSource.Cancel();
			await Task.Delay(150);
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
				keyboardColor = Color.FromArgb(byte.MaxValue, (byte)num2, (byte)num3, (byte)num4);
				if (titleFlag == 0)
				{
					ChangeKeyBoardColor(num2, num3, num4, (int)light[cur + 2].L);
					ColorZone.Source = colorZoneBitmap[0];
				}
				await Task.Delay(TimeSpan.FromSeconds(0.1), clockToken);
			}
		}
		finally
		{
			clockTokenSource.Cancel();
		}
	}

	public async Task StartRainbowAnimation()
	{
		if (rainbowTokenSource != null)
		{
			rainbowTokenSource.Cancel();
			await Task.Delay(1150);
		}
		rainbowTokenSource = new CancellationTokenSource();
		rainbowToken = rainbowTokenSource.Token;
		try
		{
			while (!rainbowToken.IsCancellationRequested)
			{
				Color color = rainbowColors[RainbowIndex];
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
				await Task.Delay(TimeSpan.FromSeconds(1.0), rainbowToken);
			}
		}
		finally
		{
			rainbowTokenSource.Cancel();
		}
	}

	private void DrawColorBlock(WriteableBitmap bitmap, int x, int y, int width, int height, Color color)
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
			DrawColorBlock(colorZoneBitmap[0], x1 + i * num, y1, num2, num3, Color.FromRgb((byte)CombineRGBWithBrightness(flowColors[(i + 21 - FlowIndex) % 21].R, (int)light[cur + 5].L), (byte)CombineRGBWithBrightness(flowColors[(i + 21 - FlowIndex) % 21].G, (int)light[cur + 5].L), (byte)CombineRGBWithBrightness(flowColors[(i + 21 - FlowIndex) % 21].B, (int)light[cur + 5].L)));
		}
	}

	private async Task StartFlowAnimation()
	{
		if (flowTokenSource != null)
		{
			flowTokenSource.Cancel();
			await Task.Delay(200);
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
				await Task.Delay(TimeSpan.FromSeconds(0.15), flowToken);
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

	private async Task StartRythmAnimation()
	{
		if (musicRythmTokenSource != null)
		{
			musicRythmTokenSource.Cancel();
			await Task.Delay(150);
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
					await Task.Delay(TimeSpan.FromSeconds(0.01), rythmToken);
				}
			}
		}
		finally
		{
			musicRythmTokenSource.Cancel();
		}
	}

	private async Task StartJumpAnimation()
	{
		if (musicJumpTokenSource != null)
		{
			musicJumpTokenSource.Cancel();
			await Task.Delay(150);
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
				await Task.Delay(TimeSpan.FromSeconds(0.08), jumpToken);
			}
		}
		finally
		{
			musicJumpTokenSource.Cancel();
		}
	}

	private async Task StartRoundAnimation()
	{
		if (gameRoundTokenSource != null)
		{
			gameRoundTokenSource.Cancel();
			await Task.Delay(100);
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
				await Task.Delay(TimeSpan.FromSeconds(0.005), roundToken);
			}
		}
		finally
		{
			gameRoundTokenSource.Cancel();
		}
	}

	private async Task StartCoverAnimation()
	{
		if (gameCoverTokenSource != null)
		{
			gameCoverTokenSource.Cancel();
			await Task.Delay(100);
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
				await Task.Delay(TimeSpan.FromSeconds(0.01), coverToken);
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
			SolidColorBrush fill = new SolidColorBrush(Color.FromRgb(light[musicRythm].R, light[musicRythm].G, light[musicRythm].B));
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
			Color color = Color.FromRgb(light[gameRound].R, light[gameRound].G, light[gameRound].B);
			Color color2 = Color.FromRgb(light[gameRound].backR, light[gameRound].backG, light[gameRound].backB);
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
			SolidColorBrush solidColorBrush2 = new SolidColorBrush(Color.FromRgb(light[musicJump].R, light[musicJump].G, light[musicJump].B));
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
			Color color = Color.FromRgb(light[gameCover].R, light[gameCover].G, light[gameCover].B);
			SolidColorBrush solidColorBrush3 = new SolidColorBrush(Colors.DeepSkyBlue);
			SolidColorBrush fill = new SolidColorBrush(color);
			SolidColorBrush fill2 = new SolidColorBrush(Color.FromRgb(light[gameCover].backR, light[gameCover].backG, light[gameCover].backB));
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
				back1_color3_rec.Fill = new SolidColorBrush(Color.FromRgb(light[gameRound].backR, light[gameRound].backG, light[gameRound].backB));
				colorZoneBitmap[4].FillRectangle(back2X1, back2Y1, back2X2, back2Y2, lightBackColor(light[gameRound]));
				if (light[cur].Switch == 1)
				{
					StartAutoEvent();
				}
			}
			else if (back2_color3.IsChecked == true)
			{
				back2_color3_rec.Fill = new SolidColorBrush(Color.FromRgb(light[gameCover].backR, light[gameCover].backG, light[gameCover].backB));
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
									back1_color3_rec.Fill = new SolidColorBrush(Color.FromRgb(light[gameRound].backR, light[gameRound].backG, light[gameRound].backB));
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
			ColorZone_back = (Image)target;
			break;
		case 6:
			ColorZone = (Image)target;
			break;
		case 7:
			Zone = (Image)target;
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
			ColorZone_back_1 = (Image)target;
			break;
		case 17:
			ColorZone_1 = (Image)target;
			break;
		case 18:
			Zone_1 = (Image)target;
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
			back1_color1_rec = (Rectangle)target;
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
			back1_color2_rec = (Rectangle)target;
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
			back1_color3_rec = (Rectangle)target;
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
			ColorZone_back_2 = (Image)target;
			break;
		case 37:
			ColorZone_2 = (Image)target;
			break;
		case 38:
			Zone_2 = (Image)target;
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
			back2_color1_rec = (Rectangle)target;
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
			back2_color2_rec = (Rectangle)target;
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
			back2_color3_rec = (Rectangle)target;
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
			KeyBoardAlwaysImg = (Image)target;
			break;
		case 56:
			KeyBoardBreath = (System.Windows.Controls.RadioButton)target;
			KeyBoardBreath.Checked += KeyBoardBreath_Checked;
			KeyBoardBreath.Unchecked += KeyBoardBreath_Checked;
			break;
		case 57:
			KeyBoardBreathImg = (Image)target;
			break;
		case 58:
			KeyBoardWave = (System.Windows.Controls.RadioButton)target;
			KeyBoardWave.Checked += KeyBoardWave_Checked;
			KeyBoardWave.Unchecked += KeyBoardWave_Checked;
			break;
		case 59:
			KeyBoardWaveImg = (Image)target;
			break;
		case 60:
			KeyBoardClock = (System.Windows.Controls.RadioButton)target;
			KeyBoardClock.Checked += KeyBoardClock_Checked;
			KeyBoardClock.Unchecked += KeyBoardClock_Checked;
			break;
		case 61:
			KeyBoardClockImg = (Image)target;
			break;
		case 62:
			KeyBoardRainbow = (System.Windows.Controls.RadioButton)target;
			KeyBoardRainbow.Checked += KeyBoardRainbow_Checked;
			KeyBoardRainbow.Unchecked += KeyBoardRainbow_Checked;
			break;
		case 63:
			KeyBoardRainbowImg = (Image)target;
			break;
		case 64:
			KeyBoardFlow = (System.Windows.Controls.RadioButton)target;
			KeyBoardFlow.Checked += KeyBoardFlow_Checked;
			KeyBoardFlow.Unchecked += KeyBoardFlow_Checked;
			break;
		case 65:
			KeyBoardFlowImg = (Image)target;
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
			ZoneAlwaysImg = (Image)target;
			break;
		case 69:
			ZoneBreath = (System.Windows.Controls.RadioButton)target;
			ZoneBreath.Checked += ZoneBreath_Checked;
			ZoneBreath.Unchecked += ZoneBreath_Checked;
			break;
		case 70:
			ZoneBreathImg = (Image)target;
			break;
		case 71:
			ZoneClock = (System.Windows.Controls.RadioButton)target;
			ZoneClock.Checked += ZoneClock_Checked;
			ZoneClock.Unchecked += ZoneClock_Checked;
			break;
		case 72:
			ZoneClockImg = (Image)target;
			break;
		case 73:
			ZoneRainbow = (System.Windows.Controls.RadioButton)target;
			ZoneRainbow.Checked += ZoneRainbow_Checked;
			ZoneRainbow.Unchecked += ZoneRainbow_Checked;
			break;
		case 74:
			ZoneRainbowImg = (Image)target;
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
			BackAlwaysImg = (Image)target;
			break;
		case 78:
			BackBreath = (System.Windows.Controls.RadioButton)target;
			BackBreath.Checked += BackBreath_Checked;
			BackBreath.Unchecked += BackBreath_Checked;
			break;
		case 79:
			BackBreathImg = (Image)target;
			break;
		case 80:
			BackMusic = (System.Windows.Controls.RadioButton)target;
			BackMusic.Checked += BackMusic_Checked;
			BackMusic.Unchecked += BackMusic_Checked;
			break;
		case 81:
			BackMusicImg = (Image)target;
			break;
		case 82:
			BackGaming = (System.Windows.Controls.RadioButton)target;
			BackGaming.Checked += BackGaming_Checked;
			BackGaming.Unchecked += BackGaming_Checked;
			break;
		case 83:
			BackGameImg = (Image)target;
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
