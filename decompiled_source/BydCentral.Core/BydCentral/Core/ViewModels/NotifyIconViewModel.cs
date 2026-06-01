using System.Windows;
using System.Windows.Input;
using HandyControl.Controls;

namespace BydCentral.Core.ViewModels;

public class NotifyIconViewModel
{
	public ICommand ShowWindowCommand => new DelegateCommand
	{
		CanExecuteFunc = () => Application.Current.MainWindow != null,
		CommandAction = delegate
		{
			if (Application.Current.MainWindow.FindName("CentralIcon") is NotifyIcon notifyIcon)
			{
				notifyIcon.ContextMenu.IsOpen = false;
			}
			System.Windows.Window mainWindow = Application.Current.MainWindow;
			if (mainWindow != null)
			{
				if (mainWindow.WindowState == WindowState.Minimized)
				{
					mainWindow.WindowState = WindowState.Normal;
					mainWindow.ShowInTaskbar = true;
				}
				mainWindow.Show();
				mainWindow.Activate();
			}
		}
	};

	public ICommand HideWindowCommand => new DelegateCommand
	{
		CommandAction = delegate
		{
			Application.Current.MainWindow.Hide();
		},
		CanExecuteFunc = () => Application.Current.MainWindow != null
	};

	public ICommand ExitApplicationCommand => new DelegateCommand
	{
		CommandAction = delegate
		{
			Application.Current.Shutdown();
		}
	};
}
