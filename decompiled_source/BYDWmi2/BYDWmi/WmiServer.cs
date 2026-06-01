using System;
using System.Management;
using System.Threading;

namespace BYDWmi;

public class WmiServer
{
	public bool mutex;

	private bool isLock;

	private ManagementEventWatcher eWatcher;

	public static bool isMS;

	public uint WmiMethod(string methodName, object inData)
	{
		if (isMS)
		{
			return 5u;
		}
		lock (this)
		{
			uint num = 0u;
			uint num2 = 0u;
			string path = "ACPIMethod";
			string propertyName = "IData";
			string text = "OData";
			ManagementScope managementScope = new ManagementScope("\\root\\wmi");
			managementScope.Options.EnablePrivileges = true;
			managementScope.Connect();
			ManagementPath path2 = new ManagementPath(path);
			ManagementObjectCollection instances = new ManagementClass(managementScope, path2, null).GetInstances();
			ManagementObject managementObject = null;
			ManagementBaseObject managementBaseObject = null;
			ManagementBaseObject managementBaseObject2 = null;
			using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = instances.GetEnumerator())
			{
				if (managementObjectEnumerator.MoveNext())
				{
					managementObject = (ManagementObject)managementObjectEnumerator.Current;
					managementBaseObject = managementObject.GetMethodParameters(methodName);
					managementBaseObject[propertyName] = inData;
				}
			}
			int num3 = 5;
			do
			{
				try
				{
					managementBaseObject2 = managementObject.InvokeMethod(methodName, managementBaseObject, null);
					num2 = 0u;
					num = 0u;
					if (text != "")
					{
						num = (uint)managementBaseObject2[text];
					}
				}
				catch
				{
					num2 = 2147483649u;
					num = 2147483649u;
					Thread.Sleep(new Random().Next(10, 20));
				}
				num3--;
			}
			while (num2 == 2147483649u && num3 > 0);
			return num;
		}
	}

	public uint WmiEvent_Start(string eScope, string eClassName)
	{
		uint result = 0u;
		bool flag = false;
		try
		{
			ManagementScope managementScope = new ManagementScope(eScope);
			managementScope.Options.EnablePrivileges = true;
			managementScope.Connect();
			WqlEventQuery query = new WqlEventQuery("select * from" + eClassName);
			eWatcher = new ManagementEventWatcher(managementScope, query);
			if (!flag)
			{
				if (!isLock)
				{
					isLock = true;
					eWatcher.EventArrived += WmiEvent_Handler;
					eWatcher.Start();
				}
			}
			else
			{
				eWatcher.Options.Timeout = new TimeSpan(0, 0, 20);
				eWatcher.WaitForNextEvent();
			}
		}
		catch
		{
			result = 2147483649u;
		}
		return result;
	}

	public void WmiEvent_Stop()
	{
		if (eWatcher != null)
		{
			isLock = false;
			eWatcher.Stop();
		}
	}

	private void WmiEvent_Handler(object sender, EventArrivedEventArgs e)
	{
		_ = e.NewEvent;
	}
}
