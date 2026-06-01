using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace BydCentral.Services;

public class AppControl
{
	private const string QuickName = "ControlCenter";

	private string appAllPath => Process.GetCurrentProcess().MainModule.FileName;

	private string iniPath => Directory.GetCurrentDirectory() + "/config.ini";

	private string systemStartPath => Environment.GetFolderPath(Environment.SpecialFolder.Startup);

	private string desktopPath => Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

	[DllImport("kernel32")]
	private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

	[DllImport("kernel32")]
	private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

	public void CreateIniFile()
	{
		if (!File.Exists(iniPath))
		{
			File.Create(iniPath);
		}
	}

	public string GetPath()
	{
		return iniPath;
	}

	public void INIWrite(string section, string key, string value)
	{
		WritePrivateProfileString(section, key, value, iniPath);
	}

	public string INIRead(string section, string key)
	{
		StringBuilder stringBuilder = new StringBuilder(255);
		GetPrivateProfileString(section, key, "", stringBuilder, 255, iniPath);
		if (stringBuilder.Length == 0)
		{
			return "no config item";
		}
		return stringBuilder.ToString();
	}

	public void INIDelete(string FilePath)
	{
		File.Delete(FilePath);
	}

	public void SetMeAutoStart(bool onOff = true)
	{
		if (onOff)
		{
			List<string> quickFromFolder = GetQuickFromFolder(systemStartPath, appAllPath);
			if (quickFromFolder.Count >= 2)
			{
				for (int i = 1; i < quickFromFolder.Count; i++)
				{
					DeleteFile(quickFromFolder[i]);
				}
			}
			else if (quickFromFolder.Count < 1)
			{
				CreateShortcut(systemStartPath, "ControlCenter", appAllPath, "ControlCenter");
			}
		}
		else
		{
			List<string> quickFromFolder2 = GetQuickFromFolder(systemStartPath, appAllPath);
			if (quickFromFolder2.Count > 0)
			{
				for (int j = 0; j < quickFromFolder2.Count; j++)
				{
					DeleteFile(quickFromFolder2[j]);
				}
			}
		}
		_ = Process.GetCurrentProcess().MainModule.FileName;
	}

	private bool CreateShortcut(string directory, string shortcutName, string targetPath, string description = null, string iconLocation = null)
	{
		try
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			string pathLink = Path.Combine(directory, $"{shortcutName}.lnk");
			WshShell wshShell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
			IWshShortcut obj = (IWshShortcut)(dynamic)wshShell.CreateShortcut(pathLink);
			obj.TargetPath = targetPath;
			obj.WorkingDirectory = Path.GetDirectoryName(targetPath);
			obj.WindowStyle = 1;
			obj.Description = description;
			obj.IconLocation = (string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation);
			obj.Save();
			return true;
		}
		catch (Exception ex)
		{
			_ = ex.Message;
		}
		return false;
	}

	private List<string> GetQuickFromFolder(string directory, string targetPath)
	{
		List<string> list = new List<string>();
		list.Clear();
		string[] files = Directory.GetFiles(directory, "*.lnk");
		if (files == null || files.Length < 1)
		{
			return list;
		}
		for (int i = 0; i < files.Length; i++)
		{
			if (GetAppPathFromQuick(files[i]) == targetPath)
			{
				list.Add(files[i]);
			}
		}
		return list;
	}

	private string GetAppPathFromQuick(string shortcutPath)
	{
		if (File.Exists(shortcutPath))
		{
			WshShell wshShell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
			return ((IWshShortcut)(dynamic)wshShell.CreateShortcut(shortcutPath)).TargetPath;
		}
		return "";
	}

	private void DeleteFile(string path)
	{
		if (File.GetAttributes(path) == FileAttributes.Directory)
		{
			Directory.Delete(path, recursive: true);
		}
		else
		{
			File.Delete(path);
		}
	}

	public void CreateDesktopQuick(string desktopPath = "", string quickName = "", string appPath = "")
	{
		if (GetQuickFromFolder(desktopPath, appPath).Count < 1)
		{
			CreateShortcut(desktopPath, quickName, appPath, "软件描述");
		}
	}

	public bool SetMeAutoStartRegistry(bool onOff)
	{
		string appName = "ControlCenter";
		string fileName = Process.GetCurrentProcess().MainModule.FileName;
		return SetAutoStart(onOff, appName, fileName);
	}

	public bool SetAutoStart(bool onOff, string appName, string appPath)
	{
		bool result = true;
		if (!IsExistKey(appName) && onOff)
		{
			result = SelfRunning(onOff, appName, appPath);
		}
		else if (IsExistKey(appName) && !onOff)
		{
			result = SelfRunning(onOff, appName, appPath);
		}
		return result;
	}

	private bool IsExistKey(string keyName)
	{
		try
		{
			bool result = false;
			RegistryKey localMachine = Registry.LocalMachine;
			RegistryKey registryKey = localMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
			if (registryKey == null)
			{
				registryKey = localMachine.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows")
					.CreateSubKey("CurrentVersion")
					.CreateSubKey("Run");
			}
			string[] valueNames = registryKey.GetValueNames();
			for (int i = 0; i < valueNames.Length; i++)
			{
				if (valueNames[i].ToUpper() == keyName.ToUpper())
				{
					return true;
				}
			}
			return result;
		}
		catch
		{
			return false;
		}
	}

	private bool SelfRunning(bool isStart, string exeName, string path)
	{
		try
		{
			RegistryKey localMachine = Registry.LocalMachine;
			RegistryKey registryKey = localMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
			if (registryKey == null)
			{
				localMachine.CreateSubKey("SOFTWARE//Microsoft//Windows//CurrentVersion//Run");
			}
			if (isStart)
			{
				registryKey.SetValue(exeName, path);
				registryKey.Close();
			}
			else
			{
				string[] valueNames = registryKey.GetValueNames();
				for (int i = 0; i < valueNames.Length; i++)
				{
					if (valueNames[i].ToUpper() == exeName.ToUpper())
					{
						registryKey.DeleteValue(exeName);
						registryKey.Close();
					}
				}
			}
		}
		catch (Exception ex)
		{
			_ = ex.Message;
			return false;
		}
		return true;
	}

	public void setAutoTask(bool autoTask)
	{
		if (autoTask)
		{
			CreateTask();
		}
		else
		{
			delTask();
		}
	}

	private void CreateTask()
	{
		using TaskService taskService = new TaskService();
		Task task = taskService.GetTask("ControlCenter");
		LogonTrigger logonTrigger = new LogonTrigger();
		logonTrigger.Delay = TimeSpan.FromSeconds(3.0);
		if (task != null)
		{
			TaskDefinition definition = task.Definition;
			definition.Actions.Clear();
			definition.Actions.Add(new ExecAction(appAllPath, "-minimized", Directory.GetCurrentDirectory()));
			taskService.RootFolder.RegisterTaskDefinition("ControlCenter", definition);
			return;
		}
		TaskDefinition taskDefinition = taskService.NewTask();
		taskDefinition.RegistrationInfo.Description = "ControlCenter";
		taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;
		taskDefinition.Settings.DisallowStartIfOnBatteries = false;
		taskDefinition.Settings.StopIfGoingOnBatteries = false;
		taskDefinition.Triggers.Add(logonTrigger);
		taskDefinition.Actions.Add(new ExecAction(appAllPath, "-minimized", Directory.GetCurrentDirectory()));
		taskService.RootFolder.RegisterTaskDefinition("ControlCenter", taskDefinition);
	}

	private void delTask()
	{
		using TaskService taskService = new TaskService();
		if (taskService.GetTask("ControlCenter") != null)
		{
			taskService.RootFolder.DeleteTask("ControlCenter");
		}
	}
}
