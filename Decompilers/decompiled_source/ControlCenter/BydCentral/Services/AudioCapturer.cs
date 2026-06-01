using System;
using System.IO;
using System.Linq;
using System.Threading;
using BydCentral.Core.Models;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;

namespace BydCentral.Services;

public class AudioCapturer : IMMNotificationClient
{
	private MMDeviceEnumerator enumerator;

	private MMDevice mmDevice;

	private WasapiCapture capture;

	private Visualizer visualizer;

	private double[]? spectrumData;

	private byte T100ms_Recorder;

	private double Recorder_temp;

	private double RecorderDAT;

	private double Sum_RecorderDAT;

	public byte[] Array = new byte[4];

	public byte[] MaxVolumn = new byte[4];

	private double lastMax;

	private Timer Timer;

	public AudioCapturer()
	{
		try
		{
			enumerator = new MMDeviceEnumerator();
			enumerator.RegisterEndpointNotificationCallback(this);
			capture = new WasapiLoopbackCapture();
			visualizer = new Visualizer(256);
			capture.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(8192, 1);
			capture.DataAvailable += Capture_DataAvailable;
			Timer = new Timer(delegate
			{
				DataTimer_Tick();
			}, null, -1, -1);
		}
		catch (Exception)
		{
		}
	}

	public double[] start()
	{
		try
		{
			capture.StartRecording();
			Timer.Change(0, 50);
			_ = spectrumData;
			return spectrumData;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public void stop()
	{
		try
		{
			capture.StopRecording();
			Timer.Change(-1, -1);
		}
		catch (Exception)
		{
		}
	}

	private void Capture_DataAvailable(object? sender, WaveInEventArgs e)
	{
		try
		{
			int num = e.BytesRecorded / 4;
			double[] array = new double[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = BitConverter.ToSingle(e.Buffer, i * 4);
			}
			visualizer.PushSampleData(array);
		}
		catch (Exception)
		{
			capture.StopRecording();
			capture.DataAvailable -= Capture_DataAvailable;
		}
	}

	private bool ByteToFile(double[] byteArray, string fileName)
	{
		bool result = false;
		try
		{
			string[] array = System.Array.ConvertAll(byteArray, (double d) => d.ToString());
			using StreamWriter streamWriter = new StreamWriter(fileName, append: true);
			string[] array2 = array;
			foreach (string value in array2)
			{
				streamWriter.Write(value);
				streamWriter.Write(',');
			}
			streamWriter.Write('\n');
			streamWriter.Write('\n');
			streamWriter.Write('\n');
		}
		catch
		{
			result = false;
		}
		return result;
	}

	public void DataTimer_Tick()
	{
		double[] data = visualizer.GetSpectrumData();
		data = Visualizer.MakeSmooth(data, 2);
		if (data != null)
		{
			spectrumData = data;
			double num = 0.0;
			num = ((!(spectrumData.Sum() < 0.0)) ? spectrumData.Sum() : (0.0 - spectrumData.Sum()));
			Recorder_temp = num;
			if (T100ms_Recorder < 5)
			{
				Sum_RecorderDAT += Recorder_temp;
			}
			if (T100ms_Recorder == 4)
			{
				Sum_RecorderDAT *= 30.0;
				MaxVolumn[0] = (byte)Sum_RecorderDAT;
				MaxVolumn[3] = (byte)Sum_RecorderDAT;
				Sum_RecorderDAT = 0.0;
				T100ms_Recorder = 0;
			}
			T100ms_Recorder++;
		}
	}

	public byte[] DataProcess(byte cmd, byte[] r, byte[] g, byte[] b, byte speed, byte[] br, byte[] AudioData)
	{
		return new byte[17]
		{
			52,
			14,
			cmd,
			r[0],
			r[1],
			g[0],
			g[1],
			b[0],
			b[1],
			speed,
			br[0],
			br[1],
			AudioData[0],
			AudioData[1],
			AudioData[2],
			AudioData[3],
			0
		};
	}

	public void OnDeviceStateChanged(string deviceId, DeviceState newState)
	{
	}

	public void OnDeviceAdded(string pwstrDeviceId)
	{
	}

	public void OnDeviceRemoved(string deviceId)
	{
	}

	private void ChangeCaptureClient()
	{
		capture = new WasapiLoopbackCapture();
		capture.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(8192, 1);
		capture.DataAvailable += Capture_DataAvailable;
	}

	public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
	{
		ChangeCaptureClient();
		if (GlobalVars.IsMusicMode)
		{
			start();
		}
	}

	public void OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
	{
	}
}
