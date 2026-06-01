using System;
using System.Windows;
using System.Windows.Controls;

namespace BydCentral.CustomControl.Helper;

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
