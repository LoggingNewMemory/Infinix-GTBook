using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using CommunityToolkit.Mvvm.Messaging;

namespace BydCentral.Views;

public class DebugWindow : Window, IComponentConnector
{
	private bool _contentLoaded;

	public DebugWindow()
	{
		InitializeComponent();
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		WeakReferenceMessenger.Default.Send("SaveKeyboard", "SaveKeyboard");
	}

	private void Button_Click_1(object sender, RoutedEventArgs e)
	{
		WeakReferenceMessenger.Default.Send("resume", "Resume");
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/views/debugwindow.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 1:
			((Button)target).Click += Button_Click;
			break;
		case 2:
			((Button)target).Click += Button_Click_1;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
