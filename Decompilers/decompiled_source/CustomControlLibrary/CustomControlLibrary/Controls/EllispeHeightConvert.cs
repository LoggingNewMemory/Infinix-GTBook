using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControlLibrary.Controls;

public class EllispeHeightConvert : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (double)int.Parse(value.ToString()) * 0.5;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
