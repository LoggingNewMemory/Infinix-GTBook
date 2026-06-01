#define TRACE
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using AngleSharp.Text;
using BydCentral;
using BydCentral.Core.Models;
using BydCentral.Core.Services;
using BydCentral.Core.ViewModels;
using BydCentral.Services;

namespace BydContral;

public class Page4 : Page, IComponentConnector, IStyleConnector
{
	internal Button btnFirmware;

	internal Button btnDriver;

	internal Button check_fw;

	internal Button check_dr;

	internal Grid Loading;

	internal Rectangle rec1;

	internal Rectangle rec2;

	internal Rectangle rec3;

	internal Rectangle rec4;

	internal Rectangle rec5;

	internal ListBox ListBoxDr;

	internal ListBox ListBoxFw;

	internal Button AppUpdate;

	private bool _contentLoaded;

	public Page4()
	{
		InitializeComponent();
		base.DataContext = App.Current.Services.GetService(typeof(Page4ViewModel));
	}

	private void check_fw_Click(object sender, RoutedEventArgs e)
	{
		check_fw.IsEnabled = false;
		check_dr.IsEnabled = false;
		btnDriver.IsEnabled = false;
		btnFirmware.IsEnabled = false;
		updateItem(1);
	}

	private async void updateItem(int flag)
	{
		string UpdateNow = (string)Application.Current.FindResource("UpdateNow");
		string LocalUpdate = (string)Application.Current.FindResource("LocalUpdate");
		_ = (string)Application.Current.FindResource("CancelUpdate");
		string NetworkWrong = (string)Application.Current.FindResource("NetworkWrong");
		string NoneLater = (string)Application.Current.FindResource("NoneLater");
		string FindLater = (string)Application.Current.FindResource("FindLater");
		Application.Current.Dispatcher.Invoke(delegate
		{
			ListBoxDr.Visibility = Visibility.Hidden;
			ListBoxFw.Visibility = Visibility.Hidden;
			Loading.Visibility = Visibility.Visible;
		});
		Page4ViewModel viewModel = (Page4ViewModel)base.DataContext;
		Item[] itemArrDR = new Item[100];
		int indexDR = 0;
		Item[] itemArrFW = new Item[3];
		int indexFW = 0;
		viewModel.items.Clear();
		viewModel.FwItems.Clear();
		try
		{
			await Task.Run(delegate
			{
				Trace.WriteLine("zqp_test check_dr_Click start");
				(string[], int) tuple = viewModel.clientRequest.checkUpdateServer();
				string[] item = tuple.Item1;
				int item2 = tuple.Item2;
				Dictionary<string, string> allLoacalNameMap = viewModel.clientRequest.getAllLoacalNameMap();
				switch (item2)
				{
				case -1:
				{
					Trace.WriteLine("zqp_test Net error");
					Dictionary<string, string> allLoacalVersionMap2 = viewModel.clientRequest.getAllLoacalVersionMap();
					if (allLoacalVersionMap2.Count != 0)
					{
						foreach (KeyValuePair<string, string> item7 in allLoacalVersionMap2)
						{
							Item item5 = new Item
							{
								Id = item7.Key,
								Name = allLoacalNameMap[item7.Key],
								NowVersion = item7.Value,
								Description = NetworkWrong,
								NewVersion = "-----",
								Tips = "",
								Command = viewModel.DriverCommand,
								btntext = UpdateNow
							};
							if (!(item7.Key == "APPUpdate"))
							{
								if (item7.Key == "BIOS" || item7.Key == "EC" || item7.Key == "PD" || item7.Key == "VideoBIOS")
								{
									itemArrFW[indexFW] = item5;
									indexFW++;
								}
								else
								{
									itemArrDR[indexDR] = item5;
									indexDR++;
								}
							}
						}
						break;
					}
					break;
				}
				case 0:
				{
					Trace.WriteLine("zqp_test server is empty");
					Dictionary<string, string> allLoacalVersionMap3 = viewModel.clientRequest.getAllLoacalVersionMap();
					if (allLoacalVersionMap3.Count != 0)
					{
						foreach (KeyValuePair<string, string> item8 in allLoacalVersionMap3)
						{
							Item item6 = new Item
							{
								Id = item8.Key,
								Name = allLoacalNameMap[item8.Key],
								NowVersion = item8.Value,
								Description = NoneLater,
								Tips = "",
								NewVersion = item8.Value,
								Command = viewModel.DriverCommand,
								btntext = UpdateNow
							};
							if (!(item8.Key == "APPUpdate"))
							{
								if (item8.Key == "BIOS" || item8.Key == "EC" || item8.Key == "PD" || item8.Key == "VideoBIOS")
								{
									itemArrFW[indexFW] = item6;
									indexFW++;
								}
								else
								{
									itemArrDR[indexDR] = item6;
									indexDR++;
								}
							}
						}
						break;
					}
					break;
				}
				case 1:
				{
					for (int i = 0; i < item.Length; i++)
					{
						Item item3 = new Item
						{
							Id = item[i],
							Name = allLoacalNameMap[item[i]]
						};
						IclientRequest.downFileVersionInfo downFileVersionInfo = default(IclientRequest.downFileVersionInfo);
						downFileVersionInfo = viewModel.clientRequest.getGerverVersionMap(item[i]);
						string localVersionMap = viewModel.clientRequest.getLocalVersionMap(item[i]);
						if (downFileVersionInfo.latestVersion == null || localVersionMap == null)
						{
							Trace.WriteLine("未知错误");
						}
						else
						{
							item3.NowVersion = localVersionMap;
							item3.NewVersion = downFileVersionInfo.latestVersion;
							item3.Description = FindLater;
							item3.btntext = UpdateNow;
							item3.Tips = downFileVersionInfo.releaseNote[0];
							item3.Command = viewModel.DriverCommand;
							if (!(item[i] == "APPUpdate"))
							{
								if (item[i] == "BIOS" || item[i] == "EC" || item[i] == "PD" || item[i] == "VideoBIOS")
								{
									itemArrFW[indexFW] = item3;
									indexFW++;
								}
								else
								{
									itemArrDR[indexDR] = item3;
									indexDR++;
								}
							}
						}
					}
					Dictionary<string, string> allLoacalVersionMap = viewModel.clientRequest.getAllLoacalVersionMap();
					if (allLoacalVersionMap.Count != 0)
					{
						foreach (KeyValuePair<string, string> item9 in allLoacalVersionMap)
						{
							if (!item.Contains(item9.Key))
							{
								Item item4 = new Item
								{
									Name = allLoacalNameMap[item9.Key],
									NowVersion = item9.Value,
									Description = NoneLater,
									NewVersion = item9.Value,
									Command = viewModel.DriverCommand,
									btntext = UpdateNow
								};
								if (!(item9.Key == "APPUpdate"))
								{
									if (item9.Key == "BIOS" || item9.Key == "EC" || item9.Key == "PD" || item9.Key == "VideoBIOS")
									{
										itemArrFW[indexFW] = item4;
										indexFW++;
									}
									else
									{
										itemArrDR[indexDR] = item4;
										indexDR++;
									}
								}
							}
						}
						break;
					}
					break;
				}
				}
			}).ContinueWith(delegate
			{
				Trace.WriteLine("zqp_test index:" + indexFW);
				(string[], string[]) tuple = viewModel.clientRequest.checkLoadUpdate();
				string[] item = tuple.Item1;
				string[] item2 = tuple.Item2;
				int i;
				for (i = 0; i < indexDR; i++)
				{
					if (item != null && item.Length != 0 && item2 != null && item2.Length != 0)
					{
						int num = Array.IndexOf<string>(item, itemArrDR[i].Name);
						if (num != -1)
						{
							itemArrDR[i].btntext = LocalUpdate;
							itemArrDR[i].NewVersion = item2[num];
						}
					}
					Application.Current.Dispatcher.Invoke(delegate
					{
						viewModel.AddItem(itemArrDR[i]);
					});
				}
				int i2;
				for (i2 = 0; i2 < indexFW; i2++)
				{
					if (item != null && item.Length != 0 && item2 != null && item2.Length != 0)
					{
						int num2 = Array.IndexOf<string>(item, itemArrFW[i2].Name);
						if (num2 != -1)
						{
							itemArrFW[i2].btntext = LocalUpdate;
							itemArrFW[i2].NewVersion = item2[num2];
						}
					}
					Application.Current.Dispatcher.Invoke(delegate
					{
						viewModel.AddFwItems(itemArrFW[i2]);
					});
				}
				Application.Current.Dispatcher.Invoke(delegate
				{
					Loading.Visibility = Visibility.Hidden;
					if (flag == 1)
					{
						ListBoxFw.Visibility = Visibility.Visible;
					}
					else
					{
						ListBoxDr.Visibility = Visibility.Visible;
					}
					check_fw.IsEnabled = true;
					check_dr.IsEnabled = true;
					btnDriver.IsEnabled = true;
					btnFirmware.IsEnabled = true;
				});
			});
		}
		catch (Exception ex)
		{
			Trace.WriteLine("zqp_test error:" + ex);
		}
	}

	private void check_dr_Click(object sender, RoutedEventArgs e)
	{
		check_fw.IsEnabled = false;
		check_dr.IsEnabled = false;
		btnDriver.IsEnabled = false;
		btnFirmware.IsEnabled = false;
		updateItem(2);
	}

	private void Button_Click_1(object sender, RoutedEventArgs e)
	{
	}

	private void btnDriver_Click(object sender, RoutedEventArgs e)
	{
		btnDriver.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
		btnFirmware.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF474747"));
		ListBoxFw.Visibility = Visibility.Hidden;
		ListBoxDr.Visibility = Visibility.Visible;
		check_dr.Visibility = Visibility.Visible;
		check_fw.Visibility = Visibility.Hidden;
	}

	private void btnFirmware_Click(object sender, RoutedEventArgs e)
	{
		btnDriver.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF474747"));
		btnFirmware.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF30B3EB"));
		ListBoxFw.Visibility = Visibility.Visible;
		ListBoxDr.Visibility = Visibility.Hidden;
		check_dr.Visibility = Visibility.Hidden;
		check_fw.Visibility = Visibility.Visible;
	}

	private async void Button_Click(object sender, RoutedEventArgs e)
	{
		AppUpdate.IsEnabled = false;
		await Task.Run(delegate
		{
			string text = (string)Application.Current.FindResource("FindVersion");
			string text2 = (string)Application.Current.FindResource("UpdateQuestion");
			string caption = (string)Application.Current.FindResource("AppUpdate");
			string messageBoxText = (string)Application.Current.FindResource("AppUpdateTips1");
			string messageBoxText2 = (string)Application.Current.FindResource("AppUpdateTips2");
			int num = 0;
			IclientRequest.downFileVersionInfo downFileVersionInfo = default(IclientRequest.downFileVersionInfo);
			clientRequest clientRequest2 = new clientRequest();
			(downFileVersionInfo, num) = clientRequest2.getAppVersion();
			if (num == 1)
			{
				MessageBoxResult num2 = MessageBox.Show(text + downFileVersionInfo.latestVersion + text2, caption, MessageBoxButton.YesNo);
				if (num2 == MessageBoxResult.Yes)
				{
					clientRequest2.UpdateAPP();
				}
				if (num2 == MessageBoxResult.No)
				{
					return;
				}
			}
			if (num == 0)
			{
				MessageBox.Show(messageBoxText, caption, MessageBoxButton.OK);
			}
			if (num == -1)
			{
				MessageBox.Show(messageBoxText2, caption, MessageBoxButton.OK);
			}
		});
		AppUpdate.IsEnabled = true;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/views/pages/page4.xaml", UriKind.Relative);
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
			btnFirmware = (Button)target;
			btnFirmware.Click += btnFirmware_Click;
			break;
		case 2:
			btnDriver = (Button)target;
			btnDriver.Click += btnDriver_Click;
			break;
		case 3:
			check_fw = (Button)target;
			check_fw.Click += check_fw_Click;
			break;
		case 4:
			check_dr = (Button)target;
			check_dr.Click += check_dr_Click;
			break;
		case 5:
			Loading = (Grid)target;
			break;
		case 6:
			rec1 = (Rectangle)target;
			break;
		case 7:
			rec2 = (Rectangle)target;
			break;
		case 8:
			rec3 = (Rectangle)target;
			break;
		case 9:
			rec4 = (Rectangle)target;
			break;
		case 10:
			rec5 = (Rectangle)target;
			break;
		case 11:
			ListBoxDr = (ListBox)target;
			break;
		case 13:
			ListBoxFw = (ListBox)target;
			break;
		case 15:
			AppUpdate = (Button)target;
			AppUpdate.Click += Button_Click;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IStyleConnector.Connect(int connectionId, object target)
	{
		switch (connectionId)
		{
		case 12:
			((Button)target).Click += Button_Click_1;
			break;
		case 14:
			((Button)target).Click += Button_Click_1;
			break;
		}
	}
}
