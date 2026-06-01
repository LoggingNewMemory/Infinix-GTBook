using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace BydCentral.CustomControl;

public class MonitorItem : UserControl, IComponentConnector
{
	public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register("ItemName", typeof(string), typeof(MonitorItem), new PropertyMetadata(null, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
	{
		((MonitorItem)s).ItemName = (string)e.NewValue;
	}));

	public static readonly DependencyProperty ItemTitleProperty = DependencyProperty.Register("ItemTitle", typeof(string), typeof(MonitorItem), new PropertyMetadata(null, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
	{
		((MonitorItem)s).ItemTitle = (string)e.NewValue;
	}));

	public static readonly DependencyProperty ItemContentProperty = DependencyProperty.Register("ItemContent", typeof(string), typeof(MonitorItem), new PropertyMetadata(null, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
	{
		((MonitorItem)s).ItemContent = (string)e.NewValue;
	}));

	public static readonly DependencyProperty ItemValueProperty = DependencyProperty.Register("ItemValue", typeof(int), typeof(MonitorItem), new PropertyMetadata(0, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
	{
		((MonitorItem)s).ItemValue = (int)e.NewValue;
	}));

	private bool _contentLoaded;

	public string ItemName
	{
		get
		{
			return (string)GetValue(ItemNameProperty);
		}
		set
		{
			SetValue(ItemNameProperty, value);
		}
	}

	public string ItemTitle
	{
		get
		{
			return (string)GetValue(ItemTitleProperty);
		}
		set
		{
			SetValue(ItemTitleProperty, value);
		}
	}

	public string ItemContent
	{
		get
		{
			return (string)GetValue(ItemContentProperty);
		}
		set
		{
			SetValue(ItemContentProperty, value);
		}
	}

	public int ItemValue
	{
		get
		{
			return (int)GetValue(ItemValueProperty);
		}
		set
		{
			SetValue(ItemValueProperty, value);
		}
	}

	public MonitorItem()
	{
		InitializeComponent();
		base.DataContext = this;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/monitoritem.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		_contentLoaded = true;
	}
}
