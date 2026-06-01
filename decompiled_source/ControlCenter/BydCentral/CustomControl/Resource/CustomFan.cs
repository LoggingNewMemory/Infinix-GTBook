using System.Windows;
using Panuon.WPF.UI;

namespace BydCentral.CustomControl.Resource;

public class CustomFan : RingProgressBar
{
	public static readonly DependencyProperty CustomTextProperty = DependencyProperty.Register("CustomText", typeof(string), typeof(CustomFan));

	public string CustomText
	{
		get
		{
			return (string)GetValue(CustomTextProperty);
		}
		set
		{
			SetValue(CustomTextProperty, value);
		}
	}

	public CustomFan()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomFan), new FrameworkPropertyMetadata(typeof(CustomFan)));
	}
}
