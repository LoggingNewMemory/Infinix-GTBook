using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace BydCentral.CustomControl.CustomUserControl;

public class Settings : UserControl, IComponentConnector
{
	public static readonly DependencyProperty BatModePorpetry = DependencyProperty.Register("BatMode", typeof(int), typeof(Settings), new FrameworkPropertyMetadata(0, OnBatModePropertyChanged));

	public static readonly DependencyProperty ChargeModePorpetry = DependencyProperty.Register("ChargeMode", typeof(int), typeof(Settings), new FrameworkPropertyMetadata(0, OnChargeModePropertyChanged));

	public static readonly DependencyProperty OutputModePorpetry = DependencyProperty.Register("OutputMode", typeof(int), typeof(Settings), new FrameworkPropertyMetadata(0, OnOutputModePropertyChanged));

	internal Image gpuMode;

	internal Image outputMode;

	internal Image output;

	internal Image GPUOUT;

	internal Image IGPUOUT;

	internal Image GPUIN;

	internal Image IGPUIN;

	internal Image ChargingImg;

	internal Image Charging;

	internal Image batMode;

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
			gpuMode = (Image)target;
			break;
		case 2:
			outputMode = (Image)target;
			break;
		case 3:
			output = (Image)target;
			break;
		case 4:
			GPUOUT = (Image)target;
			break;
		case 5:
			IGPUOUT = (Image)target;
			break;
		case 6:
			GPUIN = (Image)target;
			break;
		case 7:
			IGPUIN = (Image)target;
			break;
		case 8:
			ChargingImg = (Image)target;
			break;
		case 9:
			Charging = (Image)target;
			break;
		case 10:
			batMode = (Image)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
