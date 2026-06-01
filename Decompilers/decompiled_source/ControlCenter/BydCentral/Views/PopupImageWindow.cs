using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Messaging;

namespace BydCentral.Views;

public class PopupImageWindow : Window, IComponentConnector
{
	private DispatcherTimer timer;

	private bool iniflag = true;

	internal Image popupImage;

	private bool _contentLoaded;

	public PopupImageWindow()
	{
		InitializeComponent();
		base.Left = (SystemParameters.PrimaryScreenWidth - base.Width) / 2.0;
		base.Top = SystemParameters.PrimaryScreenHeight - base.Height - 60.0;
		timer = new DispatcherTimer();
		timer.Interval = TimeSpan.FromSeconds(2.0);
		timer.Tick += Timer_Tick;
		WeakReferenceMessenger.Default.Register<string, string>(this, "SystemMode", Message);
		WeakReferenceMessenger.Default.Register<string, string>(this, "Popup", Message);
	}

	private void Message(object obj, string msg)
	{
		if (iniflag)
		{
			iniflag = false;
		}
		else if (Application.Current.MainWindow.WindowState == WindowState.Minimized && this != null)
		{
			if (base.WindowState != WindowState.Minimized)
			{
				Hide();
				timer.Stop();
			}
			switch (msg)
			{
			case "0":
				popupImage.Source = new BitmapImage(new Uri("/image/切换模式弹窗/office.png", UriKind.RelativeOrAbsolute));
				break;
			case "1":
				popupImage.Source = new BitmapImage(new Uri("/image/切换模式弹窗/per.png", UriKind.RelativeOrAbsolute));
				break;
			case "2":
				popupImage.Source = new BitmapImage(new Uri("/image/切换模式弹窗/game.png", UriKind.RelativeOrAbsolute));
				break;
			case "KbLightOn":
				popupImage.Source = new BitmapImage(new Uri("/image/KbLightOn.png", UriKind.RelativeOrAbsolute));
				break;
			case "KbLightOff":
				popupImage.Source = new BitmapImage(new Uri("/image/KbLightOff.png", UriKind.RelativeOrAbsolute));
				break;
			case "BackLightOn":
				popupImage.Source = new BitmapImage(new Uri("/image/BackLightOn.png", UriKind.RelativeOrAbsolute));
				break;
			case "BackLightOff":
				popupImage.Source = new BitmapImage(new Uri("/image/BackLightOff.png", UriKind.RelativeOrAbsolute));
				break;
			}
			Show();
			timer.Start();
		}
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		Hide();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/views/popupimagewindow.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		if (connectionId == 1)
		{
			popupImage = (Image)target;
		}
		else
		{
			_contentLoaded = true;
		}
	}
}
