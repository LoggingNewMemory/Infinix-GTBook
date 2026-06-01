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

public class CPU : UserControl, IComponentConnector
{
	private Point lastTmpPoint;

	private Point lastFanPoint;

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

	public Point Circle_Center1(Point p1, Point p2, double dRadius)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 1.0;
		double num6 = 1.0;
		double num7 = 1.0;
		num = (p2.Y - p1.Y) / (p2.X - p1.X);
		Point point = default(Point);
		Point point2 = default(Point);
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
		Point result = default(Point);
		result.X = point2.X;
		result.Y = point2.Y;
		return result;
	}

	public Point Circle_Center2(Point p1, Point p2, double dRadius)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 1.0;
		double num6 = 1.0;
		double num7 = 1.0;
		num = (p2.Y - p1.Y) / (p2.X - p1.X);
		Point point = default(Point);
		Point point2 = default(Point);
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
		Point result = default(Point);
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
