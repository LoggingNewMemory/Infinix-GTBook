using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CustomControl;

public class ColorControlPanel : UserControl, IComponentConnector
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

	internal Path PanelHue;

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
		SetBinding(SelectBrushProperty, new Binding("SelectedBrush")
		{
			Source = ColorPanelColors,
			Mode = BindingMode.TwoWay
		});
		SetBinding(SelectLProperty, new Binding("SelectedVal")
		{
			Source = ColorPanelColors,
			Mode = BindingMode.TwoWay
		});
		SetBinding(SelectValueProperty, new Binding("SelectedBrushValue")
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
			Binding binding = new Binding("SelectedBrushH");
			colorControlPanel.xColorH.SetBinding(Shape.FillProperty, binding);
			Binding binding2 = new Binding("SelectedBrush");
			colorControlPanel.xColorSV.SetBinding(Shape.FillProperty, binding2);
		}
	}

	private void RegisterEvent()
	{
		PanelHue.MouseLeftButtonDown += delegate
		{
			HueThumbMove(Mouse.GetPosition(PanelHue));
		};
		PanelHue.MouseMove += delegate(object sender, MouseEventArgs e)
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
		PanelSV.MouseMove += delegate(object sender, MouseEventArgs e)
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

	private void HueThumbMove(Point point)
	{
		double num = Math.Atan2(point.Y - PanelHue.ActualHeight / 2.0, point.X - PanelHue.ActualWidth / 2.0) * 180.0 / Math.PI;
		if (num < 0.0)
		{
			num += 360.0;
		}
		ColorPanelColors.SelectedHue = num;
	}

	private void SvThumbMove(Point point)
	{
		Point point2 = TranslatePoint(point, PanelSV);
		Point point3 = TranslatePoint(point, PanelS);
		Point point4 = TranslatePoint(point, PanelV);
		Point point5 = PanelSV.Points[2];
		Point point6 = PanelSV.Points[1];
		Point point7 = PanelSV.Points[0];
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
		Point point = PanelSV.Points[2];
		Point point2 = PanelSV.Points[1];
		Point point3 = PanelSV.Points[0];
		Point point4 = default(Point);
		double num = point3.X - point.X;
		point4.X = point3.X - num * selectedHSV.V;
		double num2 = point2.Y - point.Y;
		double num3 = num2 * selectedHSV.V;
		point4.Y = point.Y + (num2 + num3) / 2.0 - num3 * selectedHSV.S;
		Point point5 = PanelV.TranslatePoint(point4, PanelSV);
		if (point5.X != 0.0 && point5.Y != 0.0)
		{
			ThumbSV.SetValue(Canvas.LeftProperty, point5.X);
			ThumbSV.SetValue(Canvas.TopProperty, point5.Y);
		}
	}

	private void DrawColorWheel()
	{
		Canvas colorWheel = xColorWheel;
		List<Color> list = new List<Color>();
		for (byte b = 0; b < byte.MaxValue; b++)
		{
			list.Add(Color.FromRgb(byte.MaxValue, b, 0));
		}
		for (byte b2 = byte.MaxValue; b2 > 0; b2--)
		{
			list.Add(Color.FromRgb(b2, byte.MaxValue, 0));
		}
		for (byte b3 = 0; b3 < byte.MaxValue; b3++)
		{
			list.Add(Color.FromRgb(0, byte.MaxValue, b3));
		}
		for (byte b4 = byte.MaxValue; b4 > 0; b4--)
		{
			list.Add(Color.FromRgb(0, b4, byte.MaxValue));
		}
		for (byte b5 = 0; b5 < byte.MaxValue; b5++)
		{
			list.Add(Color.FromRgb(b5, 0, byte.MaxValue));
		}
		for (byte b6 = byte.MaxValue; b6 > 0; b6--)
		{
			list.Add(Color.FromRgb(byte.MaxValue, 0, b6));
		}
		double centerX = colorWheel.ActualWidth / 2.0;
		double centerY = colorWheel.ActualHeight / 2.0;
		double radius = ((centerX < centerY) ? centerX : centerY) * 0.8;
		double width = ((centerX < centerY) ? centerX : centerY) * 0.4;
		double angel = Math.PI * 2.0 / (double)list.Count;
		double rotate = 0.0;
		Point pointA = default(Point);
		Point pointB = default(Point);
		colorWheel.Children.Clear();
		list.ForEach(delegate(Color color)
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
				Size = new Size(radius, radius)
			});
			PathGeometry data2 = new PathGeometry(new PathFigureCollection { pathFigure2 });
			if (CanChoose)
			{
				colorWheel.Children.Add(new Path
				{
					StrokeThickness = width,
					Stroke = new SolidColorBrush(color),
					Data = data2
				});
			}
			else
			{
				colorWheel.Children.Add(new Path
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
				Size = new Size(radius, radius)
			});
			PathGeometry data = new PathGeometry(new PathFigureCollection { pathFigure });
			colorWheel.Children.Add(new Path
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
			PanelHue = (Path)target;
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
