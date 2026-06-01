using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace BydCentral.CustomControl.CustomUserControl;

public class KeyboardButton : UserControl, IComponentConnector
{
	internal Path keyboard1;

	internal Path keyboard2;

	internal Path keyboard3;

	internal Path keyboard4;

	private bool _contentLoaded;

	public KeyboardButton()
	{
		InitializeComponent();
	}

	private void PathContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/customcontrol/customusercontrol/keyboardbutton.xaml", UriKind.Relative);
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
			keyboard1 = (Path)target;
			break;
		case 2:
			keyboard2 = (Path)target;
			break;
		case 3:
			keyboard3 = (Path)target;
			break;
		case 4:
			keyboard4 = (Path)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
