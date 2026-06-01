using System;
using System.Linq;
using FftSharp;
using FftSharp.Windows;

namespace BydCentral.Services;

public class Visualizer
{
	private double[] _sampleData;

	private DateTime _lastTime;

	private SecondOrderDynamicsForArray _dynamics;

	public double[] SampleData => _sampleData;

	public Visualizer(int waveDataSize)
	{
		if (!Get2Flag(waveDataSize))
		{
			throw new ArgumentException("长度必须是 2 的 n 次幂");
		}
		_lastTime = DateTime.Now;
		_sampleData = new double[waveDataSize];
		_dynamics = new SecondOrderDynamicsForArray(1.0, 1.0, 1.0, 0.0, waveDataSize / 2);
	}

	private bool Get2Flag(int num)
	{
		if (num < 1)
		{
			return false;
		}
		return (num & (num - 1)) == 0;
	}

	public void PushSampleData(double[] waveData)
	{
		if (waveData.Length == 0)
		{
			Array.Clear(_sampleData, 0, _sampleData.Length);
			return;
		}
		if (waveData.Length > _sampleData.Length)
		{
			Array.Copy(waveData, waveData.Length - _sampleData.Length, _sampleData, 0, _sampleData.Length);
			return;
		}
		Array.Copy(_sampleData, waveData.Length, _sampleData, 0, _sampleData.Length - waveData.Length);
		Array.Copy(waveData, 0, _sampleData, _sampleData.Length - waveData.Length, waveData.Length);
	}

	public double[] GetSpectrumData()
	{
		DateTime now = DateTime.Now;
		double totalSeconds = (now - _lastTime).TotalSeconds;
		_lastTime = now;
		int num = _sampleData.Length;
		Complex[] array = new Complex[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = new Complex(_sampleData[i], 0.0);
		}
		Transform.FFT(array);
		int num2 = num / 2;
		double[] array2 = new double[num2];
		for (int j = 0; j < num2; j++)
		{
			array2[j] = array[j].Magnitude / (double)num;
		}
		Bartlett bartlett = new Bartlett();
		bartlett.Create(num2);
		bartlett.ApplyInPlace(array2);
		return _dynamics.Update(totalSeconds, array2);
	}

	public static double[] TakeSpectrumOfFrequency(double[] spectrum, double sampleRate, double frequency)
	{
		double num = sampleRate / (double)spectrum.Length;
		int num2 = (int)Math.Min(frequency / num, spectrum.Length);
		double[] array = new double[num2];
		Array.Copy(spectrum, 0, array, 0, num2);
		return array;
	}

	public static double[] MakeSmooth(double[] data, int radius)
	{
		double[] weights = GetWeights(radius);
		double[] array = new double[1 + radius * 2];
		double[] array2 = new double[data.Length];
		if (data.Length < radius)
		{
			Array.Fill(array2, data.Average());
			return array2;
		}
		for (int i = 0; i < radius; i++)
		{
			Array.Fill(array, data[i], 0, radius + 1);
			for (int j = 0; j < radius; j++)
			{
				array[radius + 1 + j] = data[i + j];
			}
			ApplyWeights(array, weights);
			array2[i] = array.Sum();
		}
		for (int k = radius; k < data.Length - radius; k++)
		{
			for (int l = 0; l < radius; l++)
			{
				array[l] = data[k - l];
			}
			array[radius] = data[k];
			for (int m = 0; m < radius; m++)
			{
				array[radius + m + 1] = data[k + m];
			}
			ApplyWeights(array, weights);
			array2[k] = array.Sum();
		}
		for (int n = data.Length - radius; n < data.Length; n++)
		{
			Array.Fill(array, data[n], 0, radius + 1);
			for (int num = 0; num < radius; num++)
			{
				array[radius + 1 + num] = data[n - num];
			}
			ApplyWeights(array, weights);
			array2[n] = array.Sum();
		}
		return array2;
		static void ApplyWeights(double[] buffer, double[] array3)
		{
			int num2 = buffer.Length;
			for (int num3 = 0; num3 < num2; num3++)
			{
				buffer[num3] *= array3[num3];
			}
		}
		static double Gaussian(double x)
		{
			return Math.Pow(Math.E, -4.0 * x * x);
		}
		static double[] GetWeights(int num3)
		{
			int num2 = 1 + num3 * 2;
			int num4 = num2 - 1;
			double num5 = num3;
			double[] array3 = new double[num2];
			for (int num6 = 0; num6 <= num3; num6++)
			{
				array3[num3 + num6] = Gaussian((double)num6 / num5);
			}
			for (int num7 = 0; num7 < num3; num7++)
			{
				array3[num7] = array3[num4 - num7];
			}
			double num8 = array3.Sum();
			for (int num9 = 0; num9 < num2; num9++)
			{
				array3[num9] /= num8;
			}
			return array3;
		}
	}
}
