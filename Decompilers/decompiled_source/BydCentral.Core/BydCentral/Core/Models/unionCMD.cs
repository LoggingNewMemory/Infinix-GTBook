using System;
using System.Runtime.InteropServices;

namespace BydCentral.Core.Models;

public class TxBuf
{
	public struct outerstruct
	{
		public int outervar;

		public int othervar;

		public union union;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct union
	{
		[FieldOffset(0)]
		public byte Address;

		[FieldOffset(0)]
		public Address1 Address1;

		[FieldOffset(0)]
		public Address2 Address2;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct Address1
	{
		[FieldOffset(0)]
		public byte Address;

		[FieldOffset(0)]
		public byte Byte1;

		[FieldOffset(1)]
		public byte Byte2;

		[FieldOffset(2)]
		public byte Byte3;

		[FieldOffset(3)]
		public byte Byte4;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct Address2
	{
		[FieldOffset(0)]
		public byte Address;

		[FieldOffset(0)]
		public byte Byte1;

		[FieldOffset(1)]
		public byte Byte2;

		[FieldOffset(2)]
		public byte Byte3;

		[FieldOffset(3)]
		public byte Byte4;
	}

	public enum COMMAND : byte
	{
		Device_Info,
		KeyBoard_Light,
		Side_Light,
		Logo_Light,
		Fan_Ctrl,
		Periph_Ctrl,
		KeyBoard_Light12,
		KeyBoard_Light34
	}

	public enum ACTION
	{
		Set,
		Get
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct PARAMS
	{
		public enum Device_Info
		{
			Reset,
			SaveKeyboard,
			Version,
			Battery,
			Update
		}

		public enum Keyboard_Light
		{
			LightOFF,
			Always,
			Breath,
			GradualChange,
			RainBow,
			Flow,
			Wave,
			Light_Auto_Close,
			Light_Open
		}

		public enum Side_Light
		{
			LightOFF,
			Always,
			Breath,
			GradualChange,
			RainBow,
			TemperatureControl
		}

		public enum Logo_Light
		{
			LightOFF,
			Always,
			Breath,
			GradualChange,
			RainBow,
			PowerLight
		}

		public enum Fan_Ctrl
		{
			OfficeMode,
			PerformanceMode,
			GamingMode,
			FullSpeed,
			FullSpeedOff
		}

		public enum Periph_Ctrl
		{
			AGPU,
			GPUOnly,
			DDS,
			APUOnly,
			CharingON,
			CharingOFF,
			WinKeyOFF,
			WinKeyON
		}

		public enum Keyboard_Light12
		{
			Always1,
			Breath1,
			GradualChange1,
			RainBow1,
			Always2,
			Breath2,
			GradualChange2,
			RainBow2
		}

		public enum Keyboard_Light34
		{
			Always1,
			Breath1,
			GradualChange1,
			RainBow1,
			Always2,
			Breath2,
			GradualChange2,
			RainBow2
		}
	}

	public enum RXField : byte
	{
		CPU_FAN1_Speed = 9,
		DGPU_FAN2_Speed = 10,
		CPU_Temperature = 11,
		DGPU_Temperature = 12,
		System_Power_State = 13,
		Adapter_Current = 41,
		Adapter_Voltage = 43,
		Thermal_Flag1_State = 23,
		Thermal_Control_Mode = 24,
		Color_R = 51,
		Color_G = 52,
		Color_B = 53,
		Fan_Mode = 50
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct unionCMD
	{
		[FieldOffset(0)]
		public PositionCMD PositionCMD;

		[FieldOffset(0)]
		public byte Value;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct unionSize
	{
		[FieldOffset(0)]
		public PositionSize PositionSize;

		[FieldOffset(0)]
		public byte Value;
	}

	[StructLayout(LayoutKind.Explicit, Size = 4)]
	public struct unionReserved
	{
		[FieldOffset(0)]
		public byte C1;

		[FieldOffset(1)]
		public byte C2;

		[FieldOffset(2)]
		public byte C3;

		[FieldOffset(3)]
		public byte C4;
	}

	[StructLayout(LayoutKind.Explicit, Size = 56)]
	public struct unionData
	{
		[FieldOffset(0)]
		public Color Color;

		[FieldOffset(0)]
		public Other Other;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct CheckSum
	{
		[FieldOffset(0)]
		public byte CheckSum_num1;

		[FieldOffset(0)]
		public byte CheckSum_num2;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct PositionCMD
	{
		[FieldOffset(0)]
		public byte PARAMS;

		[FieldOffset(0)]
		public byte COMMAND;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct PositionSize
	{
		[FieldOffset(0)]
		public byte size;

		[FieldOffset(0)]
		public byte isEmpty;
	}

	[StructLayout(LayoutKind.Explicit, Size = 56)]
	public struct Color
	{
		[FieldOffset(0)]
		public byte R;

		[FieldOffset(1)]
		public byte G;

		[FieldOffset(2)]
		public byte B;

		[FieldOffset(3)]
		public byte L;

		[FieldOffset(4)]
		public unsafe fixed byte Name[52];
	}

	[StructLayout(LayoutKind.Explicit, Size = 56)]
	public struct Other
	{
		[FieldOffset(0)]
		public byte index;

		[FieldOffset(1)]
		public unsafe fixed byte Name[55];
	}

	public struct txBuF
	{
		public byte ID;

		public unionCMD unionCMD;

		public unionSize unionSize;

		public unionReserved unionReserved;

		public unionData unionData;

		public CheckSum CheckSum;
	}

	public byte[] rxBuf = new byte[64];

	public int tx_actual_length;

	public int rx_actual_length;

	public int hid_timeout = 50;

	public static byte SetbitValue(byte data, int index, bool flag)
	{
		int num;
		switch (index)
		{
		default:
			throw new ArgumentOutOfRangeException();
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
			num = 2 << index - 2;
			break;
		case 1:
			num = index;
			break;
		}
		int num2 = num;
		if (!flag)
		{
			return (byte)(data & ~num2);
		}
		return (byte)(data | num2);
	}

	public byte GetByte(byte data, int index, byte count)
	{
		return (byte)((data >> 8 - index - count) % (1 << (int)count));
	}

	public byte SetByte(int value, byte data, byte index, byte count)
	{
		int num = data % (1 << 8 - index - count);
		return (byte)(((data >> 8 - index << (int)count) + value << (int)index) + num);
	}

	public bool TestBit(byte data, int index)
	{
		return GetByte(data, index, 1) == 1;
	}

	public byte SetBit(bool value, byte data, byte index, byte count)
	{
		return SetByte(value ? 1 : 0, data, index, count);
	}

	public bool TypeFlagsAudio(byte flags, int index)
	{
		return TestBit(flags, index);
	}

	public void TenToTwo(int value)
	{
		int num = 0;
		int[] array = new int[20];
		bool[] array2 = new bool[4];
		while (value / 2 == 1)
		{
			array[num++] = value % 2;
			array2[num++] = true;
			value /= 2;
		}
		array[num++] = value % 2;
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
		}
	}

	public txBuF setID(txBuF setID)
	{
		byte iD = 6;
		setID.ID = iD;
		return setID;
	}

	public txBuF setCommand(COMMAND arg, txBuF setCMD)
	{
		Convert.ToString((int)arg, 2);
		bool[] array = new bool[4];
		byte[] array2 = new byte[4];
		string text = Convert.ToString((byte)arg, 2);
		for (int i = 0; i < text.Length; i++)
		{
			array2[i] = Convert.ToByte(text[i].ToString());
			array[i] = Convert.ToBoolean(array2[i]);
			setCMD.unionCMD.Value = SetbitValue(setCMD.unionCMD.Value, 4 + text.Length - i, array[i]);
			TypeFlagsAudio(setCMD.unionCMD.Value, 3 + text.Length - i);
		}
		return setCMD;
	}

	public txBuF setAction(ACTION arg, txBuF setAction)
	{
		Convert.ToString((int)arg, 2);
		bool[] array = new bool[1];
		byte[] array2 = new byte[1];
		string text = Convert.ToString((byte)arg, 2);
		for (int i = 0; i < text.Length; i++)
		{
			array2[i] = Convert.ToByte(text[i].ToString());
			array[i] = Convert.ToBoolean(array2[i]);
			setAction.unionCMD.Value = SetbitValue(setAction.unionCMD.Value, 3 + text.Length - i, array[i]);
			TypeFlagsAudio(setAction.unionCMD.Value, 3 + text.Length - 1 - i);
		}
		return setAction;
	}

	public txBuF setParams(int arg, txBuF setParams)
	{
		Convert.ToString(arg, 2);
		bool[] array = new bool[4];
		byte[] array2 = new byte[4];
		string text = Convert.ToString((byte)arg, 2);
		for (int i = 0; i < text.Length; i++)
		{
			array2[i] = Convert.ToByte(text[i].ToString());
			array[i] = Convert.ToBoolean(array2[i]);
			setParams.unionCMD.Value = SetbitValue(setParams.unionCMD.Value, text.Length - i, array[i]);
			TypeFlagsAudio(setParams.unionCMD.Value, text.Length - 1 - i);
		}
		return setParams;
	}

	public txBuF setDataRGBL(byte R, byte G, byte B, byte L, txBuF setRGBL)
	{
		setRGBL.unionData.Color.R = R;
		setRGBL.unionData.Color.G = G;
		setRGBL.unionData.Color.B = B;
		setRGBL.unionData.Color.L = L;
		return setRGBL;
	}

	public txBuF setDataSize(byte size, txBuF setDataSize)
	{
		if (size == 0)
		{
			return setDataSize;
		}
		if (size > 56)
		{
			size = 56;
		}
		setDataSize.unionSize.PositionSize.isEmpty = 0;
		setDataSize.unionSize.PositionSize.size = size;
		return setDataSize;
	}

	public txBuF clearData(txBuF setDataSize)
	{
		setDataSize.unionData.Other.index = 0;
		return setDataSize;
	}

	public txBuF setCheckSum(txBuF setCheckSum)
	{
		byte[] array = new byte[64];
		IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(setCheckSum));
		try
		{
			Marshal.StructureToPtr(setCheckSum, intPtr, fDeleteOld: false);
			Marshal.Copy(intPtr, array, 0, Marshal.SizeOf(setCheckSum));
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
		uint num = 0u;
		for (int i = 1; i < 63; i++)
		{
			num += array[i];
		}
		setCheckSum.CheckSum.CheckSum_num1 = (byte)num;
		return setCheckSum;
	}

	public byte getUSBFiled(RXField field)
	{
		return rxBuf[(uint)field];
	}

	private void send()
	{
	}

	public txBuF on_setKeyboardSave()
	{
		txBuF setCMD = setID(default(txBuF));
		setCMD = setCommand(COMMAND.Device_Info, setCMD);
		setCMD = setParams(1, setCMD);
		return setCheckSum(setCMD);
	}

	public txBuF on_getVersion()
	{
		txBuF setCMD = setID(default(txBuF));
		setCMD = setCommand(COMMAND.Device_Info, setCMD);
		setCMD = setParams(2, setCMD);
		return setCheckSum(setCMD);
	}

	public txBuF On_ChatEC_setKeyboardLED(txBuF setKeyboardLED, byte R, byte G, byte B, byte L)
	{
		setKeyboardLED = setID(setKeyboardLED);
		setKeyboardLED = setCommand(COMMAND.KeyBoard_Light, setKeyboardLED);
		setKeyboardLED = setParams(1, setKeyboardLED);
		setKeyboardLED = setDataSize(4, setKeyboardLED);
		setKeyboardLED = clearData(setKeyboardLED);
		setKeyboardLED = setDataRGBL(R, G, B, L, setKeyboardLED);
		setKeyboardLED = setCheckSum(setKeyboardLED);
		return setKeyboardLED;
	}

	public txBuF on_setKeyboardLED(PARAMS.Keyboard_Light state, byte R, byte G, byte B, byte L)
	{
		txBuF setCMD = setID(default(txBuF));
		setCMD = setCommand(COMMAND.KeyBoard_Light, setCMD);
		setCMD = setParams((int)state, setCMD);
		setCMD = setDataSize(4, setCMD);
		setCMD = clearData(setCMD);
		setCMD = setDataRGBL(R, G, B, L, setCMD);
		return setCheckSum(setCMD);
	}

	public txBuF on_setKeyboard12LED(PARAMS.Keyboard_Light12 state, byte R, byte G, byte B, byte L)
	{
		txBuF setCMD = setID(default(txBuF));
		setCMD = setCommand(COMMAND.KeyBoard_Light12, setCMD);
		setCMD = setParams((int)state, setCMD);
		setCMD = setDataSize(4, setCMD);
		setCMD = clearData(setCMD);
		setCMD = setDataRGBL(R, G, B, L, setCMD);
		return setCheckSum(setCMD);
	}

	public txBuF on_setKeyboard34LED(PARAMS.Keyboard_Light34 state, byte R, byte G, byte B, byte L)
	{
		txBuF setCMD = setID(default(txBuF));
		setCMD = setCommand(COMMAND.KeyBoard_Light34, setCMD);
		setCMD = setParams((int)state, setCMD);
		setCMD = setDataSize(4, setCMD);
		setCMD = clearData(setCMD);
		setCMD = setDataRGBL(R, G, B, L, setCMD);
		return setCheckSum(setCMD);
	}

	public txBuF on_setLogoLED(PARAMS.Logo_Light state, byte R, byte G, byte B, byte L)
	{
		txBuF setCMD = setID(default(txBuF));
		setCMD = setCommand(COMMAND.Logo_Light, setCMD);
		setCMD = setParams((int)state, setCMD);
		setCMD = setDataSize(4, setCMD);
		setCMD = clearData(setCMD);
		setCMD = setDataRGBL(R, G, B, L, setCMD);
		return setCheckSum(setCMD);
	}

	public txBuF on_setCircleLED(PARAMS.Side_Light state, byte R, byte G, byte B, byte L)
	{
		txBuF setCMD = setID(default(txBuF));
		setCMD = setCommand(COMMAND.Side_Light, setCMD);
		setCMD = setParams((int)state, setCMD);
		setCMD = setDataSize(4, setCMD);
		setCMD = clearData(setCMD);
		setCMD = setDataRGBL(R, G, B, L, setCMD);
		return setCheckSum(setCMD);
	}

	public txBuF on_setFanControl(PARAMS.Fan_Ctrl state)
	{
		txBuF setCMD = setID(default(txBuF));
		setCMD = setCommand(COMMAND.Fan_Ctrl, setCMD);
		setCMD = setParams((int)state, setCMD);
		setCMD = setDataSize(0, setCMD);
		setCMD = clearData(setCMD);
		return setCheckSum(setCMD);
	}

	public txBuF on_setPraphicsControl(PARAMS.Periph_Ctrl state, byte R, byte G, byte B, byte L)
	{
		txBuF setCMD = setID(default(txBuF));
		setCMD = setCommand(COMMAND.Periph_Ctrl, setCMD);
		setCMD = setParams((int)state, setCMD);
		setCMD = setDataSize(0, setCMD);
		setCMD = clearData(setCMD);
		return setCheckSum(setCMD);
	}

	public txBuF on_setDeviceInfo(PARAMS.Device_Info state, byte R, byte G, byte B, byte L)
	{
		txBuF setCMD = setID(default(txBuF));
		setCMD = setCommand(COMMAND.Device_Info, setCMD);
		setCMD = setParams((int)state, setCMD);
		setCMD = setDataSize(0, setCMD);
		setCMD = clearData(setCMD);
		return setCheckSum(setCMD);
	}

	public txBuF on_setKeyboardLightState(PARAMS.Keyboard_Light state)
	{
		txBuF setCMD = setID(default(txBuF));
		setCMD = setCommand(COMMAND.KeyBoard_Light, setCMD);
		setCMD = setParams((int)state, setCMD);
		setCMD = setDataSize(4, setCMD);
		setCMD = clearData(setCMD);
		setCMD = setDataRGBL(0, 0, 0, 0, setCMD);
		return setCheckSum(setCMD);
	}
}
