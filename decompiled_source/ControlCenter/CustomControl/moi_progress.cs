using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using BydCentral.Services;

namespace CustomControl;

public class moi_progress : UserControl, IComponentConnector
{
	public static readonly DependencyProperty ItemValueProperty = DependencyProperty.Register("ItemValue", typeof(int), typeof(moi_progress), new PropertyMetadata(0, OnValuePropertyChanged));

	internal Path path;

	internal Label lbValue;

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
			LoggerHelper._.Error("set value err" + ex.Message);
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
			path.Stroke = new SolidColorBrush(Color.FromRgb(50, 250, 34));
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
		Point point = new Point(num5, num6);
		Size size = new Size(num2, num2);
		SweepDirection sweepDirection = SweepDirection.Clockwise;
		ArcSegment value = new ArcSegment(point, size, 0.0, isLargeArc, sweepDirection, isStroked: true);
		PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
		pathSegmentCollection.Add(value);
		PathFigure pathFigure = new PathFigure();
		pathFigure.StartPoint = new Point(num3, num4);
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
			path = (Path)target;
			break;
		case 2:
			lbValue = (Label)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
