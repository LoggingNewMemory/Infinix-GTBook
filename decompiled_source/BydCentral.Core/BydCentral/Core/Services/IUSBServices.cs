using LibUsbDotNet;
using LibUsbDotNet.LibUsb;

namespace BydCentral.Core.Services;

public interface IUSBServices
{
	IUsbDevice InitUsb(int pid, int vid);

	bool SendData(IUsbDevice device, byte[] data, out int bytes, out Error error);

	bool ReceiveData(IUsbDevice device, byte[] indata, out byte[] data, out int bytes, out Error error);

	bool SendData2(byte[] data, out int bytes, out Error error);

	bool ReceiveData2(byte[] indata, out byte[] data, out int bytes, out Error error);
}
