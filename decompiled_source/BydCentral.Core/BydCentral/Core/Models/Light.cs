using System.ComponentModel;
using ProtoBuf;

namespace BydCentral.Core.Models;

[ProtoContract]
[ProtoInclude(8, typeof(Light_Infinix))]
public class Light : INotifyPropertyChanged
{
	private byte _class;

	private byte _switch;

	private byte _mode;

	private byte _r;

	private byte _g;

	private byte _b;

	private byte _l;

	[ProtoMember(1)]
	public byte Class
	{
		get
		{
			return _class;
		}
		set
		{
			if (_class != value)
			{
				_class = value;
				OnPropertyChanged("Class");
			}
		}
	}

	[ProtoMember(2)]
	public byte Switch
	{
		get
		{
			return _switch;
		}
		set
		{
			if (_switch != value)
			{
				_switch = value;
				OnPropertyChanged("Switch");
			}
		}
	}

	[ProtoMember(3)]
	public byte Mode
	{
		get
		{
			return _mode;
		}
		set
		{
			if (_mode != value)
			{
				_mode = value;
				OnPropertyChanged("Mode");
			}
		}
	}

	[ProtoMember(4)]
	public byte R
	{
		get
		{
			return _r;
		}
		set
		{
			if (_r != value)
			{
				_r = value;
				OnPropertyChanged("R");
			}
		}
	}

	[ProtoMember(5)]
	public byte G
	{
		get
		{
			return _g;
		}
		set
		{
			if (_g != value)
			{
				_g = value;
				OnPropertyChanged("G");
			}
		}
	}

	[ProtoMember(6)]
	public byte B
	{
		get
		{
			return _b;
		}
		set
		{
			if (_b != value)
			{
				_b = value;
				OnPropertyChanged("B");
			}
		}
	}

	[ProtoMember(7)]
	public byte L
	{
		get
		{
			return _l;
		}
		set
		{
			if (_l != value)
			{
				_l = value;
				OnPropertyChanged("L");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
