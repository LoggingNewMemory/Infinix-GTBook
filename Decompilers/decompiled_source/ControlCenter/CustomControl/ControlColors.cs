using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace CustomControl;

public class ControlColors : INotifyPropertyChanged
{
	private Color newColor = Colors.Red;

	private ColorHSV hsvColor = new ColorHSV
	{
		H = 0.0,
		S = 1.0,
		V = 1.0
	};

	public SolidColorBrush SelectedBrush
	{
		get
		{
			return new SolidColorBrush(newColor);
		}
		set
		{
			newColor = value.Color;
			OnSelectedChanged();
		}
	}

	public string SelectedBrushValue
	{
		get
		{
			return newColor.ToString();
		}
		set
		{
			try
			{
				newColor = (Color)ColorConverter.ConvertFromString(value);
			}
			catch (Exception)
			{
			}
			OnSelectedChanged();
		}
	}

	public ColorHSV SelectedHSV
	{
		get
		{
			return hsvColor;
		}
		set
		{
			hsvColor = value;
			OnSelectedChanged("HSV");
		}
	}

	public double SelectedHue
	{
		get
		{
			return hsvColor.H;
		}
		set
		{
			hsvColor.H = value;
			OnSelectedChanged("HSV");
		}
	}

	public double SelectedSat
	{
		get
		{
			return hsvColor.S * 100.0;
		}
		set
		{
			hsvColor.S = value / 100.0;
			OnSelectedChanged("HSV");
		}
	}

	public double SelectedVal
	{
		get
		{
			return hsvColor.V * 100.0;
		}
		set
		{
			hsvColor.V = value / 100.0;
			OnSelectedChanged("HSV");
		}
	}

	public SolidColorBrush SelectedBrushH => new SolidColorBrush(ConvertHsvToRgb(new ColorHSV
	{
		H = hsvColor.H
	}));

	public string SelectedValH
	{
		get
		{
			if (hsvColor.H < 100.0)
			{
				return Math.Round(hsvColor.H, 1).ToString();
			}
			return Math.Round(hsvColor.H).ToString();
		}
	}

	public string SelectedValS
	{
		get
		{
			if (hsvColor.S < 0.01)
			{
				return Math.Round(hsvColor.S * 100.0, 2).ToString();
			}
			return Math.Round(hsvColor.S * 100.0, 1).ToString();
		}
	}

	public string SelectedValV
	{
		get
		{
			if (hsvColor.V < 0.01)
			{
				return Math.Round(hsvColor.V * 100.0, 2).ToString();
			}
			return Math.Round(hsvColor.V * 100.0, 1).ToString();
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void Notify(string notifyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(notifyName));
	}

	private void OnInitialChanged()
	{
		Notify("InitialBrush");
	}

	private void OnSelectedChanged(string mode = "RGB")
	{
		if (mode == "RGB")
		{
			hsvColor = ConvertRgbToHsv(newColor);
		}
		else
		{
			if (hsvColor.S > 1.0)
			{
				hsvColor.S = 0.5;
			}
			newColor = ConvertHsvToRgb(hsvColor);
		}
		List<string> list = new List<string>();
		list.Add("SelectedBrush");
		list.Add("SelectedBrushValue");
		list.Add("SelectedHSV");
		list.Add("SelectedHue");
		list.Add("SelectedSat");
		list.Add("SelectedVal");
		list.Add("SelectedBrushH");
		list.ForEach(delegate(string name)
		{
			Notify(name);
		});
	}

	public static ColorHSV ConvertRgbToHsv(Color color)
	{
		ColorHSV colorHSV = new ColorHSV();
		double num = (double)(int)color.R / 255.0;
		double num2 = (double)(int)color.G / 255.0;
		double num3 = (double)(int)color.B / 255.0;
		double num4 = num;
		double num5 = num;
		if (num2 > num5)
		{
			num5 = num2;
		}
		else if (num2 < num4)
		{
			num4 = num2;
		}
		if (num3 > num5)
		{
			num5 = num3;
		}
		else if (num3 < num4)
		{
			num4 = num3;
		}
		colorHSV.V = num5;
		double num6 = num5 - num4;
		if (colorHSV.V == 0.0)
		{
			colorHSV.S = 0.0;
		}
		else
		{
			colorHSV.S = num6 / num5;
		}
		if (colorHSV.S == 0.0)
		{
			colorHSV.H = 0.0;
		}
		else if (num5 == num)
		{
			colorHSV.H = (num2 - num3) / num6;
		}
		else if (num5 == num2)
		{
			colorHSV.H = 2.0 + (num3 - num) / num6;
		}
		else
		{
			colorHSV.H = 4.0 + (num - num2) / num6;
		}
		colorHSV.H *= 60.0;
		if (colorHSV.H < 0.0)
		{
			colorHSV.H += 360.0;
		}
		return colorHSV;
	}

	public static Color ConvertHsvToRgb(ColorHSV colorhsv)
	{
		if (colorhsv.H > 360.0)
		{
			colorhsv.H = 0.0;
		}
		double num = colorhsv.V * colorhsv.S;
		double num2 = colorhsv.H / 60.0;
		double num3 = num * (1.0 - Math.Abs(num2 % 2.0 - 1.0));
		double num4 = colorhsv.V - num;
		double num5;
		double num6;
		double num7;
		switch ((int)num2)
		{
		case 0:
			num5 = num;
			num6 = num3;
			num7 = 0.0;
			break;
		case 1:
			num5 = num3;
			num6 = num;
			num7 = 0.0;
			break;
		case 2:
			num5 = 0.0;
			num6 = num;
			num7 = num3;
			break;
		case 3:
			num5 = 0.0;
			num6 = num3;
			num7 = num;
			break;
		case 4:
			num5 = num3;
			num6 = 0.0;
			num7 = num;
			break;
		default:
			num5 = num;
			num6 = 0.0;
			num7 = num3;
			break;
		}
		num5 += num4;
		num6 += num4;
		num7 += num4;
		num5 *= 255.0;
		num6 *= 255.0;
		num7 *= 255.0;
		return Color.FromRgb((byte)num5, (byte)num6, (byte)num7);
	}
}
