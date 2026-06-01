using System;
using System.Globalization;
using System.Windows.Data;

namespace CustomControlLibrary.Controls;

public class HeightCornerRadiusConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return 0.5 * (double)int.Parse(value.ToString());
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
