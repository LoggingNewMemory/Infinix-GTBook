using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControlLibrary.Controls;

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
