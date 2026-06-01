using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace BydCentral.CustomControl;

public class Fan : UserControl, IComponentConnector
{
	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(Fan), new PropertyMetadata(0, delegate(DependencyObject s, DependencyPropertyChangedEventArgs e)
	{
		((Fan)s).Value = (int)e.NewValue;
	}));

	private bool _contentLoaded;

	public int Value
	{
		get
		{
			return (int)GetValue(ValueProperty);
		}
		set
		{
			SetValue(ValueProperty, value);
		}
	}

	public Fan()
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
			Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/fan.xaml", UriKind.Relative);
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
