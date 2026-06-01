using System;
using System.Globalization;
using System.Windows.Data;

namespace BydCentral.CustomControl.Resource;

public class FontSizeConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is double num && parameter is double num2)
		{
			return Math.Round(num * num2);
		}
		return Binding.DoNothing;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
