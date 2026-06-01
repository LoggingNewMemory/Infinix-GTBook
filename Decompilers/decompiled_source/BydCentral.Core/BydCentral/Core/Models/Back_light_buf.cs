using System.Runtime.InteropServices;

namespace BydCentral.Core.Models;

public class Back_light_buf
{
	public enum COMMAND : byte
	{
		Light_Close = 0,
		Light_AlwaysOn = 1,
		Light_Breath = 2,
		Light_Rythm = 3,
		Light_Jump = 4,
		Light_Round = 5,
		Light_Cover = 6,
		SliceMode = 7,
		BalanceMode = 8,
		GameMode = 9,
		Save = 124,
		Back_Light = 15,
		ReadBuf = 144
	}

	[StructLayout(LayoutKind.Explicit, Size = 17)]
	public struct PARAMS
	{
		[FieldOffset(0)]
		public byte h1;

		[FieldOffset(1)]
		public byte h2;

		[FieldOffset(2)]
		public COMMAND EffectCMD;

		[FieldOffset(3)]
		public byte r1;

		[FieldOffset(4)]
		public byte r2;

		[FieldOffset(5)]
		public byte g1;

		[FieldOffset(6)]
		public byte g2;

		[FieldOffset(7)]
		public byte b1;

		[FieldOffset(8)]
		public byte b2;

		[FieldOffset(9)]
		public byte Spd;

		[FieldOffset(10)]
		public byte l1;

		[FieldOffset(11)]
		public byte l2;

		[FieldOffset(12)]
		public byte fd1;

		[FieldOffset(13)]
		public byte fd2;

		[FieldOffset(14)]
		public byte fd3;

		[FieldOffset(15)]
		public byte fd4;

		[FieldOffset(16)]
		public byte checkSum;
	}

	public PARAMS setBackLight(COMMAND mode, byte r, byte g, byte b, byte speed, byte l, byte[] AudioData)
	{
		PARAMS pARAMS = new PARAMS
		{
			h1 = 52,
			h2 = 14,
			EffectCMD = mode,
			Spd = speed
		};
		switch (mode)
		{
		case COMMAND.Light_AlwaysOn:
		case COMMAND.Light_Breath:
		case COMMAND.Back_Light:
			return setRGBL(pARAMS, r, g, b, l);
		case COMMAND.Light_Rythm:
		case COMMAND.Light_Jump:
		case COMMAND.Light_Round:
		case COMMAND.Light_Cover:
			return setBackLightWithAudio(pARAMS, r, g, b, l, AudioData);
		default:
			return pARAMS;
		}
	}

	public PARAMS setRGBL(PARAMS p, byte r, byte g, byte b, byte l)
	{
		p.r1 = r;
		p.g1 = g;
		p.b1 = b;
		p.l1 = (byte)((double)(int)l * 0.44);
		return p;
	}

	public PARAMS setBackLightWithAudio(PARAMS p, byte r, byte g, byte b, byte l, byte[] AudioData)
	{
		p.r1 = r;
		p.g1 = g;
		p.b1 = b;
		p.l1 = (byte)((double)(int)l * 0.44);
		p.fd1 = AudioData[0];
		p.fd2 = AudioData[1];
		p.fd3 = AudioData[2];
		p.fd4 = AudioData[3];
		return p;
	}
}
