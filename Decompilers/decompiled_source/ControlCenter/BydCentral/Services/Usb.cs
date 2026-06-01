using System;
using System.Linq;
using System.Windows.Forms;
using BydCentral.Core.Services;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;

namespace BydCentral.Services;

public class Usb : IUSBServices
{
	public static UsbDeviceFinder MyUsbFinder = new UsbDeviceFinder(13326, 32770);

	public IUsbDevice InitUsb(int pid, int vid)
	{
		using UsbContext usbContext = new UsbContext();
		try
		{
			IUsbDevice? usbDevice = usbContext.List().FirstOrDefault((IUsbDevice d) => d.ProductId == pid && d.VendorId == vid);
			usbDevice.Open();
			usbDevice.ClaimInterface(usbDevice.Configs[0].Interfaces[1].Number);
			return usbDevice;
		}
		catch (Exception ex)
		{
			MessageBox.Show("发生异常：" + ex.ToString());
			return null;
		}
	}

	public bool SendData2(byte[] data, out int bytes, out Error error)
	{
		using UsbContext usbContext = new UsbContext();
		try
		{
			UsbDevice obj = (UsbDevice)usbContext.Find(MyUsbFinder);
			((IUsbDevice)obj).TryOpen();
			((IUsbDevice)obj).ClaimInterface(1);
			int transferLength;
			Error error2 = ((IUsbDevice)obj).OpenEndpointWriter(WriteEndpointID.Ep02).Write(data, 3000, out transferLength);
			bytes = transferLength;
			((IUsbDevice)obj).Close();
			((IDisposable)obj).Dispose();
			if (error2 == Error.Success)
			{
				error = Error.Success;
				return true;
			}
			error = error2;
			return false;
		}
		catch (Exception ex)
		{
			Console.WriteLine("发生异常" + ex.ToString());
			bytes = 0;
			error = Error.Success;
			return false;
		}
	}

	public bool ReceiveData2(byte[] indata, out byte[] data, out int bytes, out Error error)
	{
		using UsbContext usbContext = new UsbContext();
		try
		{
			IUsbDevice usbDevice = usbContext.List().FirstOrDefault((IUsbDevice d) => d.ProductId == 32770 && d.VendorId == 13326);
			usbDevice.Open();
			usbDevice.ClaimInterface(usbDevice.Configs[0].Interfaces[1].Number);
			SendData(usbDevice, indata, out var _, out var _);
			UsbEndpointReader usbEndpointReader = usbDevice.OpenEndpointReader(ReadEndpointID.Ep02);
			data = new byte[64];
			int transferLength;
			Error error3 = usbEndpointReader.Read(data, 3000, out transferLength);
			bytes = transferLength;
			if (error3 == Error.Success)
			{
				error = Error.Success;
				usbDevice.Close();
				return true;
			}
			error = error3;
			usbDevice.Close();
			return false;
		}
		catch (Exception ex)
		{
			Console.WriteLine("发生异常" + ex.ToString());
			bytes = 0;
			error = Error.Success;
			data = null;
			return false;
		}
	}

	public bool SendData(IUsbDevice device, byte[] data, out int bytes, out Error error)
	{
		using UsbContext usbContext = new UsbContext();
		try
		{
			IUsbDevice usbDevice = usbContext.List().FirstOrDefault((IUsbDevice d) => d.ProductId == 32770 && d.VendorId == 13326);
			usbDevice.Open();
			usbDevice.ClaimInterface(usbDevice.Configs[0].Interfaces[1].Number);
			int transferLength;
			Error error2 = usbDevice.OpenEndpointWriter(WriteEndpointID.Ep02).Write(data, 3000, out transferLength);
			bytes = transferLength;
			if (error2 == Error.Success)
			{
				error = Error.Success;
				usbDevice.Close();
				MessageBox.Show("end send data");
				return true;
			}
			error = error2;
			usbDevice.Close();
			MessageBox.Show("end send data");
			return false;
		}
		catch (Exception ex)
		{
			MessageBox.Show("发生异常" + ex.ToString());
			bytes = 0;
			error = Error.Success;
			return false;
		}
	}

	public bool ReceiveData(IUsbDevice device, byte[] indata, out byte[] data, out int bytes, out Error error)
	{
		using UsbContext usbContext = new UsbContext();
		try
		{
			IUsbDevice usbDevice = usbContext.List().FirstOrDefault((IUsbDevice d) => d.ProductId == 32770 && d.VendorId == 13326);
			usbDevice.Open();
			usbDevice.ClaimInterface(usbDevice.Configs[0].Interfaces[1].Number);
			SendData(device, indata, out var _, out var _);
			UsbEndpointReader usbEndpointReader = device.OpenEndpointReader(ReadEndpointID.Ep02);
			data = new byte[64];
			int transferLength;
			Error error3 = usbEndpointReader.Read(data, 3000, out transferLength);
			bytes = transferLength;
			if (error3 == Error.Success)
			{
				error = Error.Success;
				usbDevice.Close();
				return true;
			}
			error = error3;
			usbDevice.Close();
			return false;
		}
		catch (Exception ex)
		{
			Console.WriteLine("发生异常" + ex.ToString());
			bytes = 0;
			error = Error.Success;
			data = null;
			return false;
		}
	}
}
