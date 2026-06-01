using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BydCentral.Core.ViewModels;

public class MainViewModel : ObservableObject
{
	private int _value;

	public int Value
	{
		get
		{
			return _value;
		}
		set
		{
			SetProperty(ref _value, value, "Value");
		}
	}

	public ICommand BtnCommand { get; set; }

	public MainViewModel()
	{
		BtnCommand = new RelayCommand<object>(RunTest);
	}

	private void Message(object obj, string message)
	{
	}

	private async void RunTest(object obj)
	{
		int i = 0;
		while (i < 10)
		{
			await Task.Delay(1000);
			Value = i++;
		}
	}
}
