using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CustomControl;

public class progress_infinix : UserControl, IComponentConnector
{
	public static readonly DependencyProperty ProNameProperty = DependencyProperty.Register("Name", typeof(string), typeof(progress_infinix), new FrameworkPropertyMetadata("cpu", OnNamePropertyChanged));

	public static readonly DependencyProperty MaxFanSpdPropetry = DependencyProperty.Register("MaxFan", typeof(double), typeof(progress_infinix), new PropertyMetadata(4800.0));

	public static readonly DependencyProperty GpuFanPropetry = DependencyProperty.Register("Fan", typeof(double), typeof(progress_infinix), new FrameworkPropertyMetadata(0.0, OnFanPropertyChanged));

	internal Rectangle p0;

	internal Rectangle p1;

	internal Rectangle p2;

	internal Rectangle p3;

	internal Rectangle p4;

	internal Rectangle p5;

	internal Rectangle p6;

	internal Rectangle p7;

	internal Rectangle p8;

	internal Rectangle p9;

	internal Rectangle p10;

	internal Rectangle p11;

	internal Rectangle p12;

	internal Rectangle p13;

	internal Rectangle p14;

	internal Rectangle p15;

	internal Rectangle p16;

	internal Rectangle p17;

	internal Rectangle p18;

	internal Rectangle p19;

	internal Rectangle p20;

	internal Rectangle p21;

	internal Rectangle p22;

	internal Rectangle p23;

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

	private async Task UpdateSpd(progress_infinix f)
	{
		SolidColorBrush colorS = new SolidColorBrush(Color.FromRgb(48, 179, 235));
		SolidColorBrush colorN = new SolidColorBrush(Color.FromRgb(0, 106, 149));
		double num = f.Fan * 100.0 / f.MaxFan;
		int s = (int)(num / 4.0);
		for (int i = 23; i >= 0; i--)
		{
			string name = "p" + i;
			Rectangle rectangle = FindName(name) as Rectangle;
			if (i > s)
			{
				rectangle.Fill = colorN;
			}
			else
			{
				rectangle.Fill = colorS;
			}
			await Task.Delay(TimeSpan.FromSeconds(0.01));
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
			p0 = (Rectangle)target;
			break;
		case 2:
			p1 = (Rectangle)target;
			break;
		case 3:
			p2 = (Rectangle)target;
			break;
		case 4:
			p3 = (Rectangle)target;
			break;
		case 5:
			p4 = (Rectangle)target;
			break;
		case 6:
			p5 = (Rectangle)target;
			break;
		case 7:
			p6 = (Rectangle)target;
			break;
		case 8:
			p7 = (Rectangle)target;
			break;
		case 9:
			p8 = (Rectangle)target;
			break;
		case 10:
			p9 = (Rectangle)target;
			break;
		case 11:
			p10 = (Rectangle)target;
			break;
		case 12:
			p11 = (Rectangle)target;
			break;
		case 13:
			p12 = (Rectangle)target;
			break;
		case 14:
			p13 = (Rectangle)target;
			break;
		case 15:
			p14 = (Rectangle)target;
			break;
		case 16:
			p15 = (Rectangle)target;
			break;
		case 17:
			p16 = (Rectangle)target;
			break;
		case 18:
			p17 = (Rectangle)target;
			break;
		case 19:
			p18 = (Rectangle)target;
			break;
		case 20:
			p19 = (Rectangle)target;
			break;
		case 21:
			p20 = (Rectangle)target;
			break;
		case 22:
			p21 = (Rectangle)target;
			break;
		case 23:
			p22 = (Rectangle)target;
			break;
		case 24:
			p23 = (Rectangle)target;
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
