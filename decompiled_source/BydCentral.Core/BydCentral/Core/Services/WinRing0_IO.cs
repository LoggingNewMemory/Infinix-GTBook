using System.Threading;
using OpenLibSys;

namespace BydCentral.Core.Services;

public class WinRing0_IO
{
	private static Ols MyOls;

	public ushort BRAM_Port_0x300 = 768;

	public ushort BRAM_Port_0x302 = 770;

	public WinRing0_IO()
	{
		Initialize();
	}

	public bool Initialize()
	{
		MyOls = new Ols();
		return MyOls.GetStatus() == 0;
	}

	public byte IndexIO_GetRegValue(ushort address, byte offset)
	{
		MyOls.WriteIoPortByte(address, offset);
		return MyOls.ReadIoPortByte((ushort)(address + 1));
	}

	public void IndexIO_SetRegValue(ushort address, byte offset, byte value)
	{
		MyOls.WriteIoPortByte(address, offset);
		MyOls.WriteIoPortByte((ushort)(address + 1), value);
	}

	public void Write_EC(byte command, byte value)
	{
		IndexIO_SetRegValue(BRAM_Port_0x300, 148, 0);
		IndexIO_SetRegValue(BRAM_Port_0x300, 145, 0);
		IndexIO_SetRegValue(BRAM_Port_0x300, 146, 0);
		IndexIO_SetRegValue(BRAM_Port_0x300, 146, 1);
		IndexIO_SetRegValue(BRAM_Port_0x300, 144, 0);
		IndexIO_SetRegValue(BRAM_Port_0x300, 145, command);
		IndexIO_SetRegValue(BRAM_Port_0x300, 160, value);
		IndexIO_SetRegValue(BRAM_Port_0x300, 147, 161);
	}

	public uint Read_EC(byte command)
	{
		uint result = 0u;
		IndexIO_SetRegValue(BRAM_Port_0x300, 148, 0);
		IndexIO_SetRegValue(BRAM_Port_0x300, 146, 1);
		IndexIO_SetRegValue(BRAM_Port_0x300, 144, 0);
		IndexIO_SetRegValue(BRAM_Port_0x300, 145, command);
		IndexIO_SetRegValue(BRAM_Port_0x300, 147, 160);
		int num = 0;
		while (num < 100 && IndexIO_GetRegValue(768, 147) != 0)
		{
			num++;
			Thread.Sleep(2);
		}
		uint num2 = IndexIO_GetRegValue(768, 160);
		if (num2 != 2147483649u)
		{
			result = num2;
		}
		return result;
	}
}
