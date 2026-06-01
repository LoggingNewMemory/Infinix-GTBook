using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BydCentral.CustomControl.Helper;

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
		if (d is Button button)
		{
			button.MouseLeftButtonDown += Button_MouseLeftButtonDown;
		}
	}

	private static void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		Button button = sender as Button;
		Point position = e.GetPosition(button);
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
