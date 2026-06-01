using System;
using System.Globalization;
using System.Threading;

namespace BYDWmi;

public class Wmi : WmiServer
{
	public void inMS(bool intoMS)
	{
		WmiServer.isMS = intoMS;
	}

	public uint SMI(uint cmd, int rw, int length = 0, object data = null)
	{
		uint result = 0u;
		byte[] array = new byte[24]
		{
			66, 89, 68, 76, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		array[4] = (byte)(cmd & 0xFF);
		array[5] = (byte)((cmd >> 8) & 0xFF);
		array[6] = (byte)((cmd >> 16) & 0xFF);
		array[7] = (byte)((cmd >> 24) & 0xFF);
		array[12] = (byte)rw;
		array[13] = (byte)length;
		if (rw == 1)
		{
			if (data == null)
			{
				return 2147483649u;
			}
			if (data is Array)
			{
				int num = ((byte[])data).Length;
				if (num > 8)
				{
					return 2147483649u;
				}
				for (int i = 0; i < num; i++)
				{
					array[16 + i] = ((byte[])data)[i];
				}
			}
			else
			{
				array[16] = Convert.ToByte(data);
			}
		}
		try
		{
			result = WmiMethod("SMI", array);
		}
		catch
		{
		}
		return result;
	}

	public uint IfNvReady()
	{
		uint num = 0u;
		byte[] inData = new byte[24]
		{
			66, 89, 68, 76, 7, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		try
		{
			uint num2 = WmiMethod("DoMethod", inData);
			if (num2 != 2147483649u)
			{
				num = num2;
			}
		}
		catch
		{
		}
		return num & 2;
	}

	public uint GetSSID()
	{
		uint result = 0u;
		byte[] inData = new byte[24]
		{
			66, 89, 68, 76, 1, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		try
		{
			uint num = WmiMethod("DoMethod", inData);
			if (num != 2147483649u)
			{
				result = num;
			}
		}
		catch
		{
		}
		return result;
	}

	public uint GetSkuId()
	{
		uint num = 0u;
		try
		{
			IO(768u, 1, 148, 0);
			IO(768u, 1, 144, 0);
			IO(768u, 1, 145, 5);
			IO(768u, 1, 146, 1);
			IO(768u, 1, 147, 160);
			return IO(768u, 0, 160, 0);
		}
		catch
		{
			return 2147483651u;
		}
	}

	public string GetPhase()
	{
		string text = string.Empty;
		try
		{
			IO(768u, 1, 148, 0);
			IO(768u, 1, 144, 0);
			IO(768u, 1, 145, 51);
			IO(768u, 1, 146, 1);
			IO(768u, 1, 147, 160);
			string text2 = IO(768u, 0, 160, 0).ToString();
			IO(768u, 1, 148, 0);
			IO(768u, 1, 145, 50);
			IO(768u, 1, 147, 160);
			string text3 = IO(768u, 0, 160, 0).ToString();
			text = text2 + "." + text3;
			return text switch
			{
				"1.0" => "EVT", 
				"1.1" => "EVT1", 
				"2.0" => "DVT", 
				"2.1" => "DVT2", 
				"2.2" => "DVT3", 
				"3.0" => "PVT", 
				"3.1" => "PVT2", 
				"3.2" => "DVT3", 
				"4.0" => "MP", 
				"4.1" => "MP2", 
				"4.2" => "RSV2", 
				_ => text, 
			};
		}
		catch
		{
			return text;
		}
	}

	public uint GetECStatus()
	{
		uint result = 0u;
		byte[] inData = new byte[24]
		{
			66, 89, 68, 76, 2, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		try
		{
			uint num = WmiMethod("DoMethod", inData);
			if (num != 2147483649u)
			{
				result = num;
			}
		}
		catch
		{
		}
		return result;
	}

	public uint GetCPUTem()
	{
		uint result = 0u;
		byte[] inData = new byte[24]
		{
			66, 89, 68, 76, 3, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		try
		{
			uint num = WmiMethod("DoMethod", inData);
			if (num != 2147483649u)
			{
				result = num;
			}
		}
		catch
		{
		}
		return result;
	}

	public uint GetGPUTem()
	{
		uint result = 0u;
		byte[] inData = new byte[24]
		{
			66, 89, 68, 76, 4, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		try
		{
			uint num = WmiMethod("DoMethod", inData);
			if (num != 2147483649u)
			{
				result = num;
			}
		}
		catch
		{
		}
		return result;
	}

	public uint SetAirplaneMode()
	{
		uint result = 0u;
		byte[] inData = new byte[24]
		{
			66, 89, 68, 76, 5, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		try
		{
			result = WmiMethod("DoMethod", inData);
		}
		catch
		{
		}
		return result;
	}

	public uint SetSoundMute()
	{
		uint result = 0u;
		byte[] inData = new byte[24]
		{
			66, 89, 68, 76, 6, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		try
		{
			result = WmiMethod("DoMethod", inData);
		}
		catch
		{
		}
		return result;
	}

	public string GetECVersion()
	{
		string result = string.Empty;
		try
		{
			uint num = 0u;
			IO(768u, 1, 144, 0);
			IO(768u, 1, 145, 0);
			IO(768u, 1, 146, 1);
			IO(768u, 1, 147, 160);
			char c = (char)byte.Parse(IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
			IO(768u, 1, 145, 1);
			IO(768u, 1, 147, 160);
			char c2 = (char)byte.Parse(IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
			IO(768u, 1, 145, 2);
			IO(768u, 1, 147, 160);
			char c3 = (char)byte.Parse(IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
			IO(768u, 1, 145, 3);
			IO(768u, 1, 147, 160);
			char c4 = (char)byte.Parse(IO(768u, 0, 160, 0).ToString(), NumberStyles.Integer);
			result = string.Concat(c, c2, '.', c3, c4);
		}
		catch
		{
		}
		return result;
	}

	public uint SetPerformanceMode(int mode)
	{
		uint num = 0u;
		return mode switch
		{
			3 => ECWriteRamCMD(64, 3), 
			2 => ECWriteRamCMD(64, 2), 
			1 => ECWriteRamCMD(64, 1), 
			_ => ECWriteRamCMD(64, 0), 
		};
	}

	public uint GetPerformanceMode()
	{
		return ECReadRamCMD(64);
	}

	public uint getIgpuTem()
	{
		return 100 - Memory(4275853762u, 0, 8);
	}

	public uint KeyLight(bool isOpen)
	{
		uint light = getLight2();
		uint lightPageStatus = getLightPageStatus();
		try
		{
			if (isOpen)
			{
				ECWriteRamCMD(94, (byte)(0 | light | lightPageStatus));
			}
			else
			{
				ECWriteRamCMD(94, (byte)(1 | light | lightPageStatus));
			}
		}
		catch
		{
		}
		return 0u;
	}

	public uint BackLight(bool isOpen)
	{
		uint light = getLight1();
		uint lightPageStatus = getLightPageStatus();
		try
		{
			if (isOpen)
			{
				ECWriteRamCMD(94, (byte)(0 | light | lightPageStatus));
			}
			else
			{
				ECWriteRamCMD(94, (byte)(2 | light | lightPageStatus));
			}
		}
		catch
		{
		}
		return 0u;
	}

	public uint getLightPageStatus()
	{
		uint result = 0u;
		try
		{
			result = ECReadRamCMD(94) & 0x80;
		}
		catch
		{
		}
		return result;
	}

	public uint setLightPageStatus(bool isDefault)
	{
		uint light = getLight1();
		uint light2 = getLight2();
		try
		{
			if (isDefault)
			{
				ECWriteRamCMD(94, (byte)(0 | light | light2));
			}
			else
			{
				ECWriteRamCMD(94, (byte)(0x80 | light | light2));
			}
		}
		catch
		{
		}
		return 0u;
	}

	public uint getLight1()
	{
		uint result = 0u;
		try
		{
			result = ECReadRamCMD(94) & 1;
		}
		catch
		{
		}
		return result;
	}

	public uint getLight2()
	{
		uint result = 0u;
		try
		{
			result = ECReadRamCMD(94) & 2;
		}
		catch
		{
		}
		return result;
	}

	public uint getLightVer()
	{
		return (ECReadRamCMD(81) << 8) | ECReadRamCMD(80);
	}

	public uint GetGBoxPower()
	{
		return ECReadRamCMD(104);
	}

	public uint GetGBoxFanTable()
	{
		return ECReadRamCMD(106);
	}

	public uint SetGBoxLight(bool flag)
	{
		uint num = 0u;
		if (flag)
		{
			return ECWriteRamCMD(100, 2);
		}
		return ECWriteRamCMD(100, 3);
	}

	public uint GetGBoxLight()
	{
		return ECReadRamCMD(100);
	}

	public uint SetGBoxTurbo(bool flag)
	{
		uint num = 0u;
		if (flag)
		{
			return ECWriteRamCMD(103, 3);
		}
		return ECWriteRamCMD(103, 2);
	}

	public uint GetGBoxTurbo()
	{
		return ECReadRamCMD(103);
	}

	public uint GetGBoxCustomMode()
	{
		return ECReadRamCMD(105);
	}

	public uint GetGBoxCustomLevel()
	{
		return ECReadRamCMD(106);
	}

	public uint SetGBoxCustomMode()
	{
		return ECWriteRamCMD(105, 1);
	}

	public uint SetGBoxCustomLevel(int flag)
	{
		uint num = 0u;
		return flag switch
		{
			4 => ECWriteRamCMD(106, 4), 
			3 => ECWriteRamCMD(106, 3), 
			2 => ECWriteRamCMD(106, 2), 
			1 => ECWriteRamCMD(106, 1), 
			_ => ECWriteRamCMD(106, 5), 
		};
	}

	public uint SetGBoxMode(int flag)
	{
		uint num = 0u;
		num = ECWriteRamCMD(105, 0);
		if (flag != 2 && flag == 3)
		{
			return ECWriteRamCMD(64, 3);
		}
		return ECWriteRamCMD(64, 2);
	}

	public uint GetGBoxChargeMode()
	{
		return ECReadRamCMD(101) * 2 + ECReadRamCMD(102);
	}

	public uint GetUSBCharging()
	{
		uint result = 0u;
		byte[] inData = new byte[24]
		{
			66, 89, 68, 76, 4, 0, 0, 0, 0, 0,
			0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		try
		{
			uint num = WmiMethod("DoMethod", inData);
			if (num != 2147483649u)
			{
				result = num;
			}
		}
		catch
		{
		}
		return result;
	}

	public uint SetUSBCharging(bool isEnable)
	{
		uint result = 0u;
		byte[] array = new byte[24]
		{
			66, 89, 68, 76, 4, 0, 0, 0, 0, 0,
			0, 0, 3, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		try
		{
			if (isEnable)
			{
				array[20] = 1;
			}
			else
			{
				array[20] = 0;
			}
			uint num = WmiMethod("DoMethod", array);
			if (num != 2147483649u)
			{
				result = num;
			}
		}
		catch
		{
		}
		return result;
	}

	public uint SetGPUMode(int mode)
	{
		uint num = 0u;
		return mode switch
		{
			1 => ECWriteRamCMD(192, 1), 
			3 => ECWriteRamCMD(192, 3), 
			_ => ECWriteRamCMD(192, 2), 
		};
	}

	public uint GetGPUMode()
	{
		return ECReadRamCMD(192);
	}

	public uint SetFanFullMode(bool flag)
	{
		uint num = 0u;
		if (flag)
		{
			return ECWriteRamCMD(65, 1);
		}
		return ECWriteRamCMD(65, 0);
	}

	public uint GetFanFullMode()
	{
		return ECReadRamCMD(65);
	}

	public uint GetCPUFanMax()
	{
		return ECReadRamCMD(6);
	}

	public uint GetGPUFanMax()
	{
		return ECReadRamCMD(7);
	}

	public uint GetCPUFan()
	{
		return ECReadRamCMD(8);
	}

	public uint GetGPUFan()
	{
		return ECReadRamCMD(9);
	}

	public uint ECWriteRamCMD(byte address, byte value)
	{
		lock (this)
		{
			mutex = true;
			uint result = 80u;
			byte data = address;
			byte data2 = value;
			try
			{
				IO(768u, 1, 148, 0);
				IO(768u, 1, 145, 0);
				IO(768u, 1, 146, 0);
				IO(768u, 1, 146, 1);
				IO(768u, 1, 144, 0);
				IO(768u, 1, 145, data);
				IO(768u, 1, 160, data2);
				IO(768u, 1, 147, 161);
			}
			catch
			{
			}
			mutex = false;
			return result;
		}
	}

	public uint ECReadRamCMD(byte address)
	{
		lock (this)
		{
			uint result = 0u;
			byte data = address;
			int num = 0;
			while (mutex && num < 200)
			{
				Thread.Sleep(1);
				num++;
			}
			try
			{
				IO(768u, 1, 148, 0);
				IO(768u, 1, 146, 1);
				IO(768u, 1, 144, 0);
				IO(768u, 1, 145, data);
				IO(768u, 1, 147, 160);
				int num2 = 0;
				while (num2 < 100 && IO(768u, 0, 147, 0) != 0)
				{
					num2++;
					Thread.Sleep(2);
				}
				uint num3 = IO(768u, 0, 160, 0);
				if (num3 != 2147483649u)
				{
					result = num3;
				}
			}
			catch
			{
			}
			return result;
		}
	}

	public uint Memory(uint address, int rw, int length, object data = null)
	{
		uint result = 0u;
		byte[] array = new byte[24]
		{
			66, 89, 68, 76, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		array[4] = (byte)(address & 0xFF);
		array[5] = (byte)((address >> 8) & 0xFF);
		array[6] = (byte)((address >> 16) & 0xFF);
		array[7] = (byte)((address >> 24) & 0xFF);
		array[12] = 0;
		array[13] = (byte)rw;
		array[14] = (byte)length;
		if (length != 8 && length != 16 && length != 32)
		{
			return 2147483649u;
		}
		if (rw == 1)
		{
			if (data == null)
			{
				return 2147483649u;
			}
			if (data is Array)
			{
				int num = ((byte[])data).Length;
				for (int i = 0; i < num; i++)
				{
					array[16 + i] = ((byte[])data)[i];
				}
			}
			else
			{
				array[16] = Convert.ToByte(data);
			}
		}
		try
		{
			result = WmiMethod("MemIO", array);
		}
		catch
		{
		}
		return result;
	}

	public uint IO(uint address, int rw, byte index, byte data = 0)
	{
		uint result = 0u;
		byte[] array = new byte[24]
		{
			66, 89, 68, 76, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		};
		array[4] = (byte)(address & 0xFF);
		array[5] = (byte)((address >> 8) & 0xFF);
		array[12] = 1;
		array[13] = (byte)rw;
		array[14] = 8;
		array[15] = index;
		array[16] = data;
		try
		{
			result = WmiMethod("MemIO", array);
		}
		catch
		{
		}
		return result;
	}

	public uint IOPort(uint address, int rw, object data = null)
	{
		uint result = 0u;
		byte[] array = new byte[10020];
		byte[] array2 = new byte[20]
		{
			66, 89, 68, 76, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0
		};
		array2[4] = (byte)(address & 0xFF);
		array2[5] = (byte)((address >> 8) & 0xFF);
		array2[6] = (byte)((address >> 16) & 0xFF);
		array2[7] = (byte)((address >> 24) & 0xFF);
		array2[12] = (byte)rw;
		int num = array2.Length;
		for (int i = 0; i < num; i++)
		{
			array[i] = array2[i];
		}
		if (rw == 1)
		{
			if (data == null)
			{
				return 2147483649u;
			}
			if (data is Array)
			{
				byte[] array3 = (byte[])data;
				int num2 = array3.Length;
				array[16] = (byte)(num2 & 0xFF);
				array[17] = (byte)((num2 >> 8) & 0xFF);
				array[18] = (byte)((num2 >> 16) & 0xFF);
				array[19] = (byte)((num2 >> 24) & 0xFF);
				for (int j = 0; j < num2; j++)
				{
					array[num + j] = array3[j];
				}
			}
			else
			{
				array[16] = 1;
				array[num] = Convert.ToByte(data);
			}
		}
		try
		{
			result = WmiMethod("IOPort", array);
		}
		catch
		{
		}
		return result;
	}
}
