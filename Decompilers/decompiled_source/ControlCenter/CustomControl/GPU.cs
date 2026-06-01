using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using Microsoft.Expression.Shapes;

namespace CustomControl;

public class GPU : UserControl, IComponentConnector
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
