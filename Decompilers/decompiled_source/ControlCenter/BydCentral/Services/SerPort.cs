using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using BydCentral.Core.Models;

namespace BydCentral.Services;

public class SerPort : IDisposable
{
	private SerialPort port;

	public SerPort()
	{
		try
		{
			port = new SerialPort(GlobalVars.LightCOM, 115200, Parity.None, 8, StopBits.One);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}

	~SerPort()
	{
		try
		{
			port.Close();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}

	public void OpenCom()
	{
		try
		{
			if (port != null)
			{
				port.Open();
			}
		}
		catch (Exception)
		{
		}
	}

	public bool IsComOpen()
	{
		try
		{
			if (port == null)
			{
				return false;
			}
			if (port.IsOpen)
			{
				return true;
			}
			return false;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public void CloseCom()
	{
		try
		{
			if (port != null)
			{
				port.Close();
			}
		}
		catch (Exception)
		{
		}
	}

	public void DataSendToPorts(byte[] inData)
	{
		try
		{
			if (port != null)
			{
				byte[] array = new byte[inData.Length];
				array = inData;
				port.Write(array, 0, array.Length);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}

	public void sendCom(object back_light_bufStruct)
	{
		try
		{
			byte[] array = new byte[Marshal.SizeOf(back_light_bufStruct)];
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			try
			{
				Marshal.StructureToPtr(back_light_bufStruct, intPtr, fDeleteOld: false);
				Marshal.Copy(intPtr, array, 0, array.Length);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			DataSendToPorts(array);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}

	public void Dispose()
	{
		port.Close();
	}
}
