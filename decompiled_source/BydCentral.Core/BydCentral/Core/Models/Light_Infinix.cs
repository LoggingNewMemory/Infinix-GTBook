using System.ComponentModel;
using ProtoBuf;

namespace BydCentral.Core.Models;

[ProtoContract]
public class Light_Infinix : Light, INotifyPropertyChanged
{
	private byte _backr;

	private byte _backg;

	private byte _backb;

	private byte _backl;

	[ProtoMember(1)]
	public byte backR
	{
		get
		{
			return _backr;
		}
		set
		{
			if (_backr != value)
			{
				_backr = value;
				OnPropertyChanged("backR");
			}
		}
	}

	[ProtoMember(2)]
	public byte backG
	{
		get
		{
			return _backg;
		}
		set
		{
			if (_backg != value)
			{
				_backg = value;
				OnPropertyChanged("backG");
			}
		}
	}

	[ProtoMember(3)]
	public byte backB
	{
		get
		{
			return _backb;
		}
		set
		{
			if (_backb != value)
			{
				_backb = value;
				OnPropertyChanged("backB");
			}
		}
	}

	[ProtoMember(4)]
	public byte backL
	{
		get
		{
			return _backl;
		}
		set
		{
			if (_backl != value)
			{
				_backl = value;
				OnPropertyChanged("backL");
			}
		}
	}
}
