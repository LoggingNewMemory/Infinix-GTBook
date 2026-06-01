using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using AngleSharp.Text;
using BydCentral.Core.ViewModels;
using BydCentral.Services;
using BydCentral.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BydCentral;

public class App : System.Windows.Application
{
	private MainWindow mainWindow;

	private const string MemoryMapName = "ControlCenterInt";

	private const int SharedDataSize = 8;

	private MemoryMappedFile memoryMappedFile;

	private AppControl app = new AppControl();

	private PopupImageWindow popupImageWindow;

	public NotifyIcon notifyIcon;

	private bool _contentLoaded;

	public new static App Current => (App)System.Windows.Application.Current;

	public IServiceProvider Services { get; }

	private static Process RunningInstance()
	{
		Process currentProcess = Process.GetCurrentProcess();
		Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
		foreach (Process process in processesByName)
		{
			if (process.Id != currentProcess.Id)
			{
				return process;
			}
		}
		return null;
	}

	private async Task MonitorSharedData()
	{
		while (true)
		{
			using (MemoryMappedViewAccessor memoryMappedViewAccessor = memoryMappedFile.CreateViewAccessor(0L, 4L))
			{
				int value = memoryMappedViewAccessor.ReadInt32(0L);
				if (value != 1)
				{
					base.Dispatcher.Invoke(delegate
					{
						ExecuteMethod(value);
					});
					memoryMappedViewAccessor.Write(0L, 1);
				}
			}
			using (MemoryMappedViewAccessor memoryMappedViewAccessor2 = memoryMappedFile.CreateViewAccessor(4L, 4L))
			{
				if (memoryMappedViewAccessor2.ReadInt32(0L) != 1)
				{
					base.Dispatcher.Invoke(delegate
					{
						mainWindow.Dispose();
						System.Windows.Application.Current.Shutdown();
					});
				}
			}
			await Task.Delay(500);
		}
	}

	private void ExecuteMethod(int value)
	{
		ExecuteCustomMethod();
	}

	public void IncreaseSharedData()
	{
		using MemoryMappedViewAccessor memoryMappedViewAccessor = memoryMappedFile.CreateViewAccessor();
		int num = memoryMappedViewAccessor.ReadInt32(0L);
		num++;
		memoryMappedViewAccessor.Write(0L, num);
	}

	public App()
	{
		base.Startup += App_Startup;
		Services = GetServices();
	}

	private void App_Startup(object sender, StartupEventArgs e)
	{
		base.DispatcherUnhandledException += App_DispatcherUnhandledException;
		TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
	}

	private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		try
		{
			e.Handled = true;
			LoggerHelper._.Error("UI线程异常:" + e.Exception.Message);
		}
		catch (Exception ex)
		{
			LoggerHelper._.Error("UI线程发生致命错误！" + ex.ToString());
		}
	}

	private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (e.IsTerminating)
		{
			stringBuilder.Append("非UI线程发生致命错误");
			LoggerHelper._.Error(stringBuilder.ToString());
		}
		stringBuilder.Append("非UI线程异常：");
		if (e.ExceptionObject is Exception)
		{
			stringBuilder.Append(((Exception)e.ExceptionObject).Message);
		}
		else
		{
			stringBuilder.Append(e.ExceptionObject);
		}
		LoggerHelper._.Error(stringBuilder.ToString());
	}

	private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
	{
		LoggerHelper._.Error("Task线程异常：" + e.Exception.Message);
		e.SetObserved();
	}

	private void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
	{
		mainWindow.Dispose();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		memoryMappedFile = MemoryMappedFile.CreateOrOpen("ControlCenterInt", 8L);
		if (RunningInstance() != null)
		{
			IncreaseSharedData();
			Shutdown();
			return;
		}
		base.OnStartup(e);
		InitLanguage();
		createNotifyIcon();
		NavigationCommands.BrowseBack.InputGestures.Clear();
		using (MemoryMappedViewAccessor memoryMappedViewAccessor = memoryMappedFile.CreateViewAccessor(0L, 4L))
		{
			memoryMappedViewAccessor.Write(0L, 1);
		}
		using (MemoryMappedViewAccessor memoryMappedViewAccessor2 = memoryMappedFile.CreateViewAccessor(4L, 4L))
		{
			memoryMappedViewAccessor2.Write(0L, 1);
		}
		MonitorSharedData();
		mainWindow = new MainWindow();
		popupImageWindow = new PopupImageWindow();
		if (e.Args.Contains("-minimized"))
		{
			mainWindow.WindowState = WindowState.Minimized;
			mainWindow.Show();
			mainWindow.Hide();
		}
		else
		{
			mainWindow.Show();
		}
	}

	private void InitLanguage()
	{
		try
		{
			ResourceDictionary item = new ResourceDictionary
			{
				Source = new Uri("Language\\en-US.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item);
		}
		catch
		{
		}
	}

	private static IServiceProvider GetServices()
	{
		ServiceCollection services = new ServiceCollection();
		services.AddScoped<Page1ViewModel>();
		services.AddScoped<Page2ViewModel>();
		services.AddScoped<Page3ViewModel>();
		return services.BuildServiceProvider();
	}

	protected override void OnExit(ExitEventArgs e)
	{
		base.OnExit(e);
	}

	private void ExecuteCustomMethod()
	{
		if (mainWindow != null)
		{
			mainWindow.Show();
			if (mainWindow.WindowState == WindowState.Minimized)
			{
				mainWindow.WindowState = WindowState.Normal;
			}
		}
	}

	private void TaskbarIcon_TrayMouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
	{
		ExecuteCustomMethod();
	}

	private void createNotifyIcon()
	{
		notifyIcon = new NotifyIcon();
		notifyIcon.Text = (string)System.Windows.Application.Current.FindResource("Central");
		Icon icon = new Icon(System.Windows.Application.GetResourceStream(new Uri("/favicon.ico", UriKind.Relative)).Stream);
		notifyIcon.Icon = icon;
		notifyIcon.Visible = true;
		notifyIcon.MouseDoubleClick += TaskbarIcon_TrayMouseDoubleClick;
		contextMenu();
	}

	private void contextMenu()
	{
		ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
		notifyIcon.ContextMenuStrip = contextMenuStrip;
		ToolStripMenuItem exitMenuItem = new ToolStripMenuItem((string)System.Windows.Application.Current.FindResource("Exit"), null, exitMenuItem_Click);
		ToolStripMenuItem versionMenuItem = new ToolStripMenuItem((string)System.Windows.Application.Current.FindResource("VersionInfo"));
		versionMenuItem.Enabled = false;
		ToolStripMenuItem LanguageItem = new ToolStripMenuItem((string)System.Windows.Application.Current.FindResource("Language"));
		ToolStripMenuItem showMenuItem = new ToolStripMenuItem((string)System.Windows.Application.Current.FindResource("ShowMainWindow"), null, showMenuItem_Click);
		ToolStripMenuItem autoStartItem = new ToolStripMenuItem((string)System.Windows.Application.Current.FindResource("AutoRestart"), null, AutoStartMenuItem_Click);
		contextMenuStrip.Items.Add(showMenuItem);
		contextMenuStrip.Items.Add(autoStartItem);
		contextMenuStrip.Items.Add(versionMenuItem);
		contextMenuStrip.Items.Add(exitMenuItem);
		if (getAutoStartStatus())
		{
			autoStartItem.Checked = true;
		}
		ToolStripMenuItem en_Item = new ToolStripMenuItem("English");
		ToolStripMenuItem zh_cn_Item = new ToolStripMenuItem("简体中文");
		ToolStripMenuItem ar_Item = new ToolStripMenuItem("العربية");
		ToolStripMenuItem fr_Item = new ToolStripMenuItem("Français");
		ToolStripMenuItem th_Item = new ToolStripMenuItem("ไทย");
		ToolStripMenuItem id_Item = new ToolStripMenuItem("Indonesia");
		ToolStripMenuItem ru_Item = new ToolStripMenuItem("Русский");
		ToolStripMenuItem hi_Item = new ToolStripMenuItem("ह\u093f\u0902द\u0940");
		ToolStripMenuItem en_PH_Item = new ToolStripMenuItem("Pilipino");
		ToolStripMenuItem ms_Item = new ToolStripMenuItem("Bahasa Melayu");
		ToolStripMenuItem tr_Item = new ToolStripMenuItem("Türkçe");
		ToolStripMenuItem pt_Item = new ToolStripMenuItem("Português");
		ToolStripMenuItem pl_Item = new ToolStripMenuItem("Polski");
		LanguageItem.DropDownItems.Add(en_Item);
		LanguageItem.DropDownItems.Add(zh_cn_Item);
		LanguageItem.DropDownItems.Add(ar_Item);
		LanguageItem.DropDownItems.Add(fr_Item);
		LanguageItem.DropDownItems.Add(th_Item);
		LanguageItem.DropDownItems.Add(id_Item);
		LanguageItem.DropDownItems.Add(ru_Item);
		LanguageItem.DropDownItems.Add(hi_Item);
		LanguageItem.DropDownItems.Add(en_PH_Item);
		LanguageItem.DropDownItems.Add(ms_Item);
		LanguageItem.DropDownItems.Add(tr_Item);
		LanguageItem.DropDownItems.Add(pt_Item);
		LanguageItem.DropDownItems.Add(pl_Item);
		en_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, en_Item, 0u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		zh_cn_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, zh_cn_Item, 1u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		ar_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, ar_Item, 2u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		fr_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, fr_Item, 3u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		th_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, th_Item, 4u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		id_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, id_Item, 5u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		ru_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, ru_Item, 6u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		hi_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, hi_Item, 7u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		en_PH_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, en_PH_Item, 8u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		ms_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, ms_Item, 9u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		tr_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, tr_Item, 10u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		pt_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, pt_Item, 11u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		pl_Item.Click += delegate(object? sender, EventArgs e)
		{
			Language_SelectedChanged(sender, e, pl_Item, 12u, exitMenuItem, versionMenuItem, LanguageItem, showMenuItem, autoStartItem);
		};
		string text = app.INIRead("system", "language");
		if (text == "no config item")
		{
			text = CultureInfo.CurrentCulture.Name;
		}
		string text2 = text.Substring(0, Math.Min(2, text.Length)).ToLower();
		if (text2 == null)
		{
			return;
		}
		int length = text2.Length;
		if (length != 2)
		{
			return;
		}
		switch (text2[0])
		{
		case 'e':
			if (text2 == "en")
			{
				if (text == "en-US")
				{
					en_Item.Checked = true;
				}
				else if (text == "en-PH")
				{
					en_PH_Item.Checked = true;
				}
			}
			break;
		case 'z':
			if (text2 == "zh")
			{
				zh_cn_Item.Checked = true;
			}
			break;
		case 'a':
			if (text2 == "ar")
			{
				ar_Item.Checked = true;
			}
			break;
		case 'f':
			if (text2 == "fr")
			{
				fr_Item.Checked = true;
			}
			break;
		case 't':
			if (!(text2 == "th"))
			{
				if (text2 == "tr")
				{
					tr_Item.Checked = true;
				}
			}
			else
			{
				th_Item.Checked = true;
			}
			break;
		case 'i':
			if (text2 == "id")
			{
				id_Item.Checked = true;
			}
			break;
		case 'r':
			if (text2 == "ru")
			{
				ru_Item.Checked = true;
			}
			break;
		case 'h':
			if (text2 == "hi")
			{
				hi_Item.Checked = true;
			}
			break;
		case 'm':
			if (text2 == "ms")
			{
				ms_Item.Checked = true;
			}
			break;
		case 'p':
			if (!(text2 == "pt"))
			{
				if (text2 == "pl")
				{
					pl_Item.Checked = true;
				}
			}
			else
			{
				pt_Item.Checked = true;
			}
			break;
		}
	}

	private void Language_SelectedChanged(object sender, EventArgs e, ToolStripMenuItem menuItem, uint temp, ToolStripMenuItem a, ToolStripMenuItem b, ToolStripMenuItem c, ToolStripMenuItem d, ToolStripMenuItem f)
	{
		if (menuItem.Checked)
		{
			return;
		}
		menuItem.Checked = true;
		foreach (ToolStripMenuItem dropDownItem in ((ToolStripMenuItem)menuItem.OwnerItem).DropDownItems)
		{
			if (dropDownItem != menuItem)
			{
				dropDownItem.Checked = false;
			}
		}
		switch (temp)
		{
		case 0u:
		{
			ResourceDictionary item13 = new ResourceDictionary
			{
				Source = new Uri("Language\\en-US.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item13);
			app.INIWrite("system", "language", "en-US");
			break;
		}
		case 1u:
		{
			ResourceDictionary item12 = new ResourceDictionary
			{
				Source = new Uri("Language\\zh.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item12);
			app.INIWrite("system", "language", "zh");
			break;
		}
		case 2u:
		{
			ResourceDictionary item11 = new ResourceDictionary
			{
				Source = new Uri("Language\\ar.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item11);
			app.INIWrite("system", "language", "ar");
			break;
		}
		case 3u:
		{
			ResourceDictionary item10 = new ResourceDictionary
			{
				Source = new Uri("Language\\fr.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item10);
			app.INIWrite("system", "language", "fr");
			break;
		}
		case 4u:
		{
			ResourceDictionary item9 = new ResourceDictionary
			{
				Source = new Uri("Language\\th.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item9);
			app.INIWrite("system", "language", "th");
			break;
		}
		case 5u:
		{
			ResourceDictionary item8 = new ResourceDictionary
			{
				Source = new Uri("Language\\id.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item8);
			app.INIWrite("system", "language", "id");
			break;
		}
		case 6u:
		{
			ResourceDictionary item7 = new ResourceDictionary
			{
				Source = new Uri("Language\\ru.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item7);
			app.INIWrite("system", "language", "ru");
			break;
		}
		case 7u:
		{
			ResourceDictionary item6 = new ResourceDictionary
			{
				Source = new Uri("Language\\hi.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item6);
			app.INIWrite("system", "language", "hi");
			break;
		}
		case 8u:
		{
			ResourceDictionary item5 = new ResourceDictionary
			{
				Source = new Uri("Language\\en-PH.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item5);
			app.INIWrite("system", "language", "en-PH");
			break;
		}
		case 9u:
		{
			ResourceDictionary item4 = new ResourceDictionary
			{
				Source = new Uri("Language\\ms.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item4);
			app.INIWrite("system", "language", "ms");
			break;
		}
		case 10u:
		{
			ResourceDictionary item3 = new ResourceDictionary
			{
				Source = new Uri("Language\\tr.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item3);
			app.INIWrite("system", "language", "tr");
			break;
		}
		case 11u:
		{
			ResourceDictionary item2 = new ResourceDictionary
			{
				Source = new Uri("Language\\pt.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item2);
			app.INIWrite("system", "language", "pt");
			break;
		}
		case 12u:
		{
			ResourceDictionary item = new ResourceDictionary
			{
				Source = new Uri("Language\\pl.xaml", UriKind.Relative)
			};
			base.Resources.MergedDictionaries.Add(item);
			app.INIWrite("system", "language", "pl");
			break;
		}
		}
		a.Text = (string)System.Windows.Application.Current.FindResource("Exit");
		b.Text = (string)System.Windows.Application.Current.FindResource("VersionInfo");
		c.Text = (string)System.Windows.Application.Current.FindResource("Language");
		d.Text = (string)System.Windows.Application.Current.FindResource("ShowMainWindow");
		f.Text = (string)System.Windows.Application.Current.FindResource("AutoRestart");
		notifyIcon.Text = (string)System.Windows.Application.Current.FindResource("Central");
	}

	private void UpdateUI()
	{
	}

	private bool getAutoStartStatus()
	{
		try
		{
			string text = app.INIRead("System", "AutoStart");
			if (text == "Y")
			{
				app.setAutoTask(autoTask: true);
				return true;
			}
			if (text == "no config item")
			{
				app.setAutoTask(autoTask: true);
				return true;
			}
			return false;
		}
		catch (Exception)
		{
			return true;
		}
	}

	private void AutoStartMenuItem_Click(object sender, EventArgs e)
	{
		if (getAutoStartStatus())
		{
			app.INIWrite("System", "AutoStart", "N");
			app.setAutoTask(autoTask: false);
		}
		else
		{
			app.INIWrite("System", "AutoStart", "Y");
			app.setAutoTask(autoTask: true);
		}
		((ToolStripMenuItem)sender).Checked = getAutoStartStatus();
	}

	private void zh_cn_Item_Click(object sender, EventArgs e)
	{
		base.Resources.MergedDictionaries.Clear();
		ResourceDictionary item = new ResourceDictionary
		{
			Source = new Uri("Language\\zh.xaml", UriKind.Relative)
		};
		base.Resources.MergedDictionaries.Add(item);
	}

	private void en_Item_Click(object sender, EventArgs e)
	{
		base.Resources.MergedDictionaries.Clear();
		ResourceDictionary item = new ResourceDictionary
		{
			Source = new Uri("Language\\en-US.xaml", UriKind.Relative)
		};
		base.Resources.MergedDictionaries.Add(item);
	}

	private void fr_Item_Click(object sender, EventArgs e)
	{
		base.Resources.MergedDictionaries.Clear();
		ResourceDictionary item = new ResourceDictionary
		{
			Source = new Uri("Language\\fr.xaml", UriKind.Relative)
		};
		base.Resources.MergedDictionaries.Add(item);
	}

	private void th_Item_Click(object sender, EventArgs e)
	{
	}

	private void showMenuItem_Click(object sender, EventArgs e)
	{
		ExecuteCustomMethod();
	}

	private void exitMenuItem_Click(object sender, EventArgs e)
	{
		mainWindow.Dispose();
		notifyIcon.Dispose();
		Thread.Sleep(300);
		System.Windows.Application.Current.Shutdown();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri resourceLocator = new Uri("/ControlCenter;V1.2.2;component/app.xaml", UriKind.Relative);
			System.Windows.Application.LoadComponent(this, resourceLocator);
		}
	}

	[STAThread]
	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "7.0.5.0")]
	public static void Main()
	{
		App obj = new App();
		obj.InitializeComponent();
		obj.Run();
	}
}
