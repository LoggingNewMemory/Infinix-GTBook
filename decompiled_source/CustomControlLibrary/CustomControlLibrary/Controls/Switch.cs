using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CustomControlLibrary.Controls;

public class Switch : ToggleButton
{
	private TranslateTransform ballMoveX;

	public static readonly DependencyProperty BallonHeightProperty;

	public static readonly DependencyProperty CheckedTipProperty;

	public static readonly DependencyProperty UncheckedTipProperty;

	public static readonly DependencyProperty BallColorProperty;

	private DoubleAnimation ballAnima = new DoubleAnimation(0.0, new Duration(new TimeSpan(0, 0, 0, 0, 200)))
	{
		EasingFunction = new CubicEase
		{
			EasingMode = EasingMode.EaseInOut
		}
	};

	public int BallonHeight
	{
		get
		{
			return (int)GetValue(BallonHeightProperty);
		}
		set
		{
			SetValue(BallonHeightProperty, value);
		}
	}

	public string CheckedTip
	{
		get
		{
			return (string)GetValue(CheckedTipProperty);
		}
		set
		{
			SetValue(CheckedTipProperty, value);
		}
	}

	public string UncheckedTip
	{
		get
		{
			return (string)GetValue(UncheckedTipProperty);
		}
		set
		{
			SetValue(UncheckedTipProperty, value);
		}
	}

	public Brush BallColor
	{
		get
		{
			return (Brush)GetValue(BallColorProperty);
		}
		set
		{
			SetValue(BallColorProperty, value);
		}
	}

	static Switch()
	{
		BallonHeightProperty = DependencyProperty.Register("BallonHeight", typeof(int), typeof(Switch), new PropertyMetadata(10));
		CheckedTipProperty = DependencyProperty.Register("CheckedTip", typeof(string), typeof(Switch), new PropertyMetadata("ON"));
		UncheckedTipProperty = DependencyProperty.Register("UncheckedTip", typeof(string), typeof(Switch), new PropertyMetadata("OFF"));
		BallColorProperty = DependencyProperty.Register("BallColor", typeof(Brush), typeof(Switch), new PropertyMetadata(new SolidColorBrush(Colors.White)));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Switch), new FrameworkPropertyMetadata(typeof(Switch)));
	}

	public Switch()
	{
		base.Checked += CheckedAnimation;
		base.Unchecked += UncheckedAnimation;
		base.Loaded += delegate(object e, RoutedEventArgs s)
		{
			ballMoveX = ((Switch)e).GetTemplateChild("trans") as TranslateTransform;
			if (base.IsChecked.HasValue)
			{
				ballMoveX.X = (double)((base.IsChecked == true) ? 1 : (-1)) * (base.ActualWidth - base.ActualHeight) / 2.0;
			}
		};
	}

	private void CheckedAnimation(object sender, RoutedEventArgs e)
	{
		if (!base.IsLoaded)
		{
			e.Handled = true;
		}
		if (ballMoveX != null)
		{
			ballAnima.To = (base.ActualWidth - base.ActualHeight) / 2.0;
			ballMoveX.BeginAnimation(TranslateTransform.XProperty, ballAnima);
		}
	}

	private void UncheckedAnimation(object sender, RoutedEventArgs e)
	{
		if (!base.IsLoaded)
		{
			e.Handled = true;
		}
		if (ballMoveX != null)
		{
			ballAnima.To = (0.0 - (base.ActualWidth - base.ActualHeight)) / 2.0;
			ballMoveX.BeginAnimation(TranslateTransform.XProperty, ballAnima);
		}
	}
}
