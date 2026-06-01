using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AssemblyTitle("CustomControlLibrary")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Razer")]
[assembly: AssemblyProduct("CustomControlLibrary")]
[assembly: AssemblyCopyright("Copyright © Razer 2023")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: TargetFramework(".NETFramework,Version=v4.8", FrameworkDisplayName = ".NET Framework 4.8")]
[assembly: AssemblyVersion("1.0.0.0")]
namespace CustomControlLibrary.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					resourceMan = new ResourceManager("CustomControlLibrary.Properties.Resources", typeof(Resources).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal Resources()
		{
		}
	}
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default => defaultInstance;
	}
}
namespace CustomControlLibrary.Controls
{
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
	public class CheckedToVisbilityConvert : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return Visibility.Hidden;
			}
			return ((bool)value) ? Visibility.Hidden : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	public class CheckedToHidenConvert : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return Visibility.Hidden;
			}
			return (!(bool)value) ? Visibility.Hidden : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
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
	public class FlatSilder : Slider
	{
		private Thumb PART_Thumb;

		private Track PART_Track;

		private bool _thumbIsPressed;

		public static readonly DependencyProperty DecreaseColorProperty;

		public static readonly DependencyProperty IncreaseColorProperty;

		public static readonly DependencyProperty IsVideoVisibleWhenPressThumbProperty;

		public static readonly RoutedEvent DropValueChangedEvent;

		public Brush DecreaseColor
		{
			get
			{
				return (Brush)GetValue(DecreaseColorProperty);
			}
			set
			{
				SetValue(DecreaseColorProperty, value);
			}
		}

		public Brush IncreaseColor
		{
			get
			{
				return (Brush)GetValue(IncreaseColorProperty);
			}
			set
			{
				SetValue(IncreaseColorProperty, value);
			}
		}

		public bool IsVideoVisibleWhenPressThumb
		{
			get
			{
				return (bool)GetValue(IsVideoVisibleWhenPressThumbProperty);
			}
			set
			{
				SetValue(IsVideoVisibleWhenPressThumbProperty, value);
			}
		}

		public event RoutedPropertyChangedEventHandler<double> DropValueChanged
		{
			add
			{
				AddHandler(DropValueChangedEvent, value);
			}
			remove
			{
				RemoveHandler(DropValueChangedEvent, value);
			}
		}

		public virtual void OnDropValueChanged(double oldValue, double newValue)
		{
			RoutedPropertyChangedEventArgs<double> e = new RoutedPropertyChangedEventArgs<double>(oldValue, newValue, DropValueChangedEvent);
			RaiseEvent(e);
		}

		static FlatSilder()
		{
			DecreaseColorProperty = DependencyProperty.Register("DecreaseColor", typeof(Brush), typeof(FlatSilder));
			IncreaseColorProperty = DependencyProperty.Register("IncreaseColor", typeof(Brush), typeof(FlatSilder));
			IsVideoVisibleWhenPressThumbProperty = DependencyProperty.Register("IsVideoVisibleWhenPressThumb", typeof(bool), typeof(FlatSilder), new PropertyMetadata(false));
			DropValueChangedEvent = EventManager.RegisterRoutedEvent("DropValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(FlatSilder));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatSilder), new FrameworkPropertyMetadata(typeof(FlatSilder)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			PART_Thumb = GetTemplateChild("PART_Thumb") as Thumb;
			PART_Track = GetTemplateChild("PART_Track") as Track;
			if (PART_Thumb != null)
			{
				PART_Thumb.PreviewMouseLeftButtonDown += PART_Thumb_PreviewMouseLeftButtonDown;
				PART_Thumb.PreviewMouseLeftButtonUp += PART_Thumb_PreviewMouseLeftButtonUp;
			}
			if (PART_Track != null)
			{
				PART_Track.MouseLeftButtonDown += PART_Track_MouseLeftButtonDown;
				PART_Track.MouseLeftButtonUp += PART_Track_MouseLeftButtonUp;
			}
			base.ValueChanged += FlatSilder_ValueChanged;
		}

		private void PART_Track_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_thumbIsPressed = IsVideoVisibleWhenPressThumb;
		}

		private void PART_Thumb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_thumbIsPressed = IsVideoVisibleWhenPressThumb;
		}

		private void FlatSilder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (IsVideoVisibleWhenPressThumb && _thumbIsPressed)
			{
				OnDropValueChanged(base.Value, base.Value);
			}
		}

		private void PART_Thumb_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (!IsVideoVisibleWhenPressThumb)
			{
				OnDropValueChanged(base.Value, base.Value);
			}
		}

		private void PART_Track_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (!IsVideoVisibleWhenPressThumb)
			{
				OnDropValueChanged(base.Value, base.Value);
			}
		}
	}
}
