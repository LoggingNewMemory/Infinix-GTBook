using System.ComponentModel;
using System.Windows.Input;

namespace BydCentral.Core.Models;

public class Item : INotifyPropertyChanged
{
	private string _NowVersion;

	private string _NewVersion;

	private int _Progress;

	private string _btntext;

	private string _tips = "用于显示更新提示的内容...";

	public string? Id { get; set; }

	public string? Name { get; set; }

	public string? Description { get; set; }

	public string NowVersion
	{
		get
		{
			return _NowVersion;
		}
		set
		{
			if (_NowVersion != value)
			{
				_NowVersion = value;
				OnPropertyChanged("NowVersion");
			}
		}
	}

	public string NewVersion
	{
		get
		{
			return _NewVersion;
		}
		set
		{
			if (_NewVersion != value)
			{
				_NewVersion = value;
				OnPropertyChanged("NewVersion");
			}
		}
	}

	public int Progress
	{
		get
		{
			return _Progress;
		}
		set
		{
			if (_Progress != value)
			{
				_Progress = value;
				OnPropertyChanged("Progress");
			}
		}
	}

	public string btntext
	{
		get
		{
			return _btntext;
		}
		set
		{
			if (_btntext != value)
			{
				_btntext = value;
				OnPropertyChanged("btntext");
			}
		}
	}

	public string Tips
	{
		get
		{
			return _tips;
		}
		set
		{
			if (_tips != value)
			{
				_tips = value;
				OnPropertyChanged("Tips");
			}
		}
	}

	public ICommand? Command { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
