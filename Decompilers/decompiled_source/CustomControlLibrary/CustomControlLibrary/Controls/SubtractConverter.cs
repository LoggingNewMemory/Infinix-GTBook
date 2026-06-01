using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomControlLibrary.Controls;

public class SubtractConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values.Length == 2 && values[0] is double && values[1] is double)
		{
			double num = (double)values[0];
			double num2 = (double)values[1];
			return num - num2 - 3.0;
		}
		return DependencyProperty.UnsetValue;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
