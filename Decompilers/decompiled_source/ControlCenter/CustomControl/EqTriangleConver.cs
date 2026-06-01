using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CustomControl;

public class EqTriangleConver : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null || values.Length < 1)
		{
			return null;
		}
		double num = (double)values[0] / 2.0;
		double num2 = (double)values[1] / 2.0;
		double num3 = ((num < num2) ? num : num2) * 0.8;
		int num4 = 3;
		double num5 = (double)(360 / num4) * Math.PI / 180.0;
		double num6 = 0.0;
		PointCollection pointCollection = new PointCollection();
		while (--num4 >= 0)
		{
			double x = num + num3 * Math.Cos(num6);
			double y = num2 + num3 * Math.Sin(num6);
			pointCollection.Add(new Point(x, y));
			num6 += num5;
		}
		return pointCollection;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
