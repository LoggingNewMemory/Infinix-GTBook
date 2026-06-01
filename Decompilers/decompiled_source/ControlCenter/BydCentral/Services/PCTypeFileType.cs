#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AngleSharp.Text;
using BydCentral.Core.Models;
using BydCentral.Core.Services;
using Newtonsoft.Json;
using NvAPIWrapper.Native;

namespace BydCentral.Services;

public class clientRequest : IclientRequest
{
	[Serializable]
	public struct FileTranslation
	{
		public string pCTypeNum;

		public string fileType;

		public string fileVersion;

		public string hash256;

		public string pW;
	}

	[Serializable]
	public struct PCTypeFileType
	{
		public string[] fileName;

		public string[] driverName;
	}

	public struct ConfigInfo
	{
		public string url;

		public string port;
	}

	[Serializable]
	public struct SHA256STR
	{
		public string res;
	}

	private string URL;

	public string savePath;

	public string configPath;

	public string downloadPath;

	private Wmi wmi = new Wmi();

	public string PCTYPENUM_SSID;

	public string PCTYPENUM_SKU;

	public PCTypeFileType pCTypeFileType1;

	private const int expirationTime = 10000;

	private int lengthDownloadInt = 10485760;

	private const int tryNum = 1;

	public string BIOSVersion = "0.0";

	public string[] nameArr;

	private string APPVersion = "0.0";

	private string ECVersion = "0.0";

	private string clientFilePath;

	private string VideoBIOSVersion = "0.0";

	private string FileTypeFileName = "fileType";

	private IclientRequest.downFileVersionInfo APPVersionInfo;

	public Dictionary<string, string> localVersionMap = new Dictionary<string, string>();

	public Dictionary<string, string> driverNameMap = new Dictionary<string, string>();

	public Dictionary<string, string> allDriverVersionMap = new Dictionary<string, string>();

	private static Mutex mutexFile = new Mutex();

	private static Mutex mutexDownload = new Mutex();

	public int ExistedVersionFlag;

	public bool isTest = true;

	public string fileSplitFlag = " :: ";

	private string[] eliminateFW = new string[3] { "BIOS", "EC", "VideoBIOS" };

	public Dictionary<string, IclientRequest.downFileVersionInfo> serverVersionMap = new Dictionary<string, IclientRequest.downFileVersionInfo>();

	private static object locker = new object();

	private static int downloadNum = 3;

	private static SemaphoreSlim semaphore = new SemaphoreSlim(downloadNum);

	public event Action<int, int> DownloadProgressBar;

	public static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
	{
		Console.WriteLine("Warning, trust any certificate");
		return true;
	}

	public clientRequest()
	{
		configPath = "./config/ClientConfig";
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		clientFilePath = folderPath + "\\ControlFile\\";
		downloadPath = clientFilePath + "File\\";
		Trace.WriteLine("downloadPath:" + downloadPath);
		Directory.CreateDirectory(downloadPath);
		savePath = clientFilePath + "SavePath\\";
		Version version = Assembly.GetEntryAssembly().GetName().Version;
		APPVersion = version.ToString();
		PCTYPENUM_SSID = GlobalVars.ssid;
		string phase = GlobalVars.Phase;
		PCTYPENUM_SKU = GlobalVars.skuid.ToString();
		BIOSVersion = wmi.getBIOSver();
		ECVersion = wmi.getECVer();
		VideoBIOSVersion = getVbiosVer();
		if (PCTYPENUM_SKU == null || PCTYPENUM_SKU.Length < 1 || PCTYPENUM_SKU == "0")
		{
			PCTYPENUM_SKU = "";
			Trace.WriteLine("ERROR getSKU IS EMPTY");
		}
		if (PCTYPENUM_SSID == null || PCTYPENUM_SSID.Length == 0 || PCTYPENUM_SSID == "0")
		{
			PCTYPENUM_SSID = "80051B61_5_EVT";
		}
		else
		{
			PCTYPENUM_SSID = PCTYPENUM_SSID + "_" + PCTYPENUM_SKU + "_" + phase;
		}
		Trace.WriteLine("PCTYPENUM_SSID + PCTYPENUM_SKU:" + PCTYPENUM_SSID);
		Trace.WriteLine("BIOSVersion:" + BIOSVersion);
		Trace.WriteLine("ECVersion:" + ECVersion);
		Trace.WriteLine("APPVersion:" + APPVersion);
		Trace.WriteLine("VideoBIOS:" + VideoBIOSVersion);
		localVersionMap.Add("BIOS", BIOSVersion);
		localVersionMap.Add("EC", ECVersion);
		localVersionMap.Add("VideoBIOS", VideoBIOSVersion);
		localVersionMap.Add("APPUpdate", APPVersion);
		driverNameMap["BIOS"] = "BIOS";
		driverNameMap["EC"] = "EC";
		driverNameMap["VideoBIOS"] = "VideoBIOS";
		driverNameMap["APPUpdate"] = "APPUpdate";
		var (array, array2) = getConfigFileType(FileTypeFileName);
		if (array != null && array.Length != 0 && array2 != null && array2.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				driverNameMap[array[i]] = array2[i];
			}
		}
		getConfig();
		createUpdateFile();
	}

	public string getVbiosVer()
	{
		_ = string.Empty;
		try
		{
			return GPUApi.GetVBIOSVersionString(GPUApi.EnumPhysicalGPUs()[0]);
		}
		catch
		{
			Trace.WriteLine("Get videoBIOS version error");
		}
		return "0.0";
	}

	private string getSKU()
	{
		using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard").Get().GetEnumerator())
		{
			if (managementObjectEnumerator.MoveNext())
			{
				return ((ManagementObject)managementObjectEnumerator.Current)["Version"]?.ToString();
			}
		}
		return "";
	}

	public void getAllDriverVersion()
	{
		allDriverVersionMap = new Dictionary<string, string>();
		try
		{
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPSignedDriver"))
			{
				using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
				foreach (ManagementObject item in managementObjectCollection)
				{
					string text = (string)item["DeviceName"];
					string text2 = (string)item["DriverVersion"];
					if (text != null && text.Length > 0 && text2 != null && text2.Length > 0)
					{
						if (!allDriverVersionMap.ContainsKey(text))
						{
							allDriverVersionMap.Add(text, text2);
						}
						else
						{
							allDriverVersionMap[text] = text2;
						}
					}
				}
			}
			Trace.WriteLine("allDriverVersionMap length:" + allDriverVersionMap.Count);
		}
		catch (Exception)
		{
			Trace.WriteLine("This is a error");
		}
	}

	public string getDriverVersion(string name)
	{
		switch (name)
		{
		case "BIOS":
			return wmi.getBIOSver();
		case "EC":
			return wmi.getECVer();
		case "VideoBIOS":
			return getVbiosVer();
		default:
			if (allDriverVersionMap.ContainsKey(name))
			{
				return allDriverVersionMap[name];
			}
			return "0.0";
		}
	}

	public void getLocation()
	{
		getAllDriverVersion();
		string[] array = nameArr;
		foreach (string text in array)
		{
			string driverVersion = getDriverVersion(text);
			if (localVersionMap.ContainsKey(text))
			{
				localVersionMap[text] = driverVersion;
			}
			else
			{
				localVersionMap.Add(text, driverVersion);
			}
		}
	}

	public void getConfig()
	{
		ConfigInfo configInfo = default(ConfigInfo);
		if (!File.Exists(configPath))
		{
			Trace.WriteLine("文件不存在.");
			MessageBox.Show("配置文件不存在", "配置文件错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		FileStream fileStream = new FileStream(configPath, FileMode.Open, FileAccess.Read);
		StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
		string[] array = new string[100];
		ulong num = 0uL;
		string text;
		while ((text = streamReader.ReadLine()) != null)
		{
			array[num] = text;
			num++;
		}
		fileStream.Close();
		streamReader.Close();
		for (ulong num2 = 0uL; num2 < num; num2++)
		{
			if (array[num2] == "[URL]")
			{
				configInfo.url = array[num2 + 1];
			}
			else if (array[num2] == "[PORT]")
			{
				configInfo.port = array[num2 + 1];
			}
		}
		if (configInfo.url == null || configInfo.url == "" || configInfo.port == null || configInfo.port == "")
		{
			Trace.WriteLine("config file error");
			MessageBox.Show("配置文件错误，无法读取URL和端口", "配置文件错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		else
		{
			URL = "https://" + configInfo.url + ":" + configInfo.port;
		}
	}

	public PCTypeFileType getFileType()
	{
		PCTypeFileType result = default(PCTypeFileType);
		try
		{
			if (isTest)
			{
				ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(RemoteCertificateValidate));
			}
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(string.Concat(URL + "/file/getFileType", "?pCTypeNum=", PCTYPENUM_SSID).Replace("#", "%23"));
			obj.Method = "GET";
			obj.Timeout = 10000;
			HttpWebResponse httpWebResponse = obj.GetResponse() as HttpWebResponse;
			if (Convert.ToInt32(httpWebResponse.StatusCode) != 200)
			{
				ExistedVersionFlag = -2;
				return result;
			}
			Stream responseStream = httpWebResponse.GetResponseStream();
			string value = "";
			using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
			{
				value = streamReader.ReadToEnd();
			}
			result = JsonConvert.DeserializeObject<PCTypeFileType>(value);
			if (result.fileName == null)
			{
				ExistedVersionFlag = 0;
				result.fileName = new string[1];
				result.fileName[0] = "";
			}
		}
		catch (Exception)
		{
			ExistedVersionFlag = -2;
		}
		return result;
	}

	public IclientRequest.downFileVersionInfo getExistedVersion(string PCTypeNum, string FileType)
	{
		IclientRequest.downFileVersionInfo result = default(IclientRequest.downFileVersionInfo);
		try
		{
			if (isTest)
			{
				ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(RemoteCertificateValidate));
			}
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create((URL + "/file/getFileVersion?pCTypeNum=" + PCTypeNum + "&fileType=" + FileType).Replace("#", "%23"));
			obj.Method = "GET";
			obj.Timeout = 10000;
			HttpWebResponse httpWebResponse = obj.GetResponse() as HttpWebResponse;
			switch (Convert.ToInt32(httpWebResponse.StatusCode))
			{
			case 501:
				ExistedVersionFlag = -2;
				return result;
			case 500:
				ExistedVersionFlag = -3;
				return result;
			default:
				ExistedVersionFlag = -1;
				return result;
			case 200:
			{
				Stream responseStream = httpWebResponse.GetResponseStream();
				string value = "";
				using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
				{
					value = streamReader.ReadToEnd();
				}
				result = JsonConvert.DeserializeObject<IclientRequest.downFileVersionInfo>(value);
				return result;
			}
			}
		}
		catch (Exception)
		{
			ExistedVersionFlag = -1;
		}
		return result;
	}

	public string GetFileHash(FileTranslation fileInfo)
	{
		string text = URL + "/file/getHash";
		text = text + "?pCTypeNum=" + fileInfo.pCTypeNum;
		text = text + "&fileType=" + fileInfo.fileType;
		text = text + "&fileVersion=" + fileInfo.fileVersion;
		text = text + "&hash256=" + fileInfo.hash256;
		text = text + "&pW=" + fileInfo.pW;
		text = text.Replace("#", "%23");
		try
		{
			if (isTest)
			{
				ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(RemoteCertificateValidate));
			}
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(text);
			obj.Method = "GET";
			obj.Timeout = 10000;
			HttpWebResponse httpWebResponse = obj.GetResponse() as HttpWebResponse;
			if (Convert.ToInt32(httpWebResponse.StatusCode) != 200)
			{
				return "";
			}
			Stream responseStream = httpWebResponse.GetResponseStream();
			string value = "";
			using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
			{
				value = streamReader.ReadToEnd();
			}
			return JsonConvert.DeserializeObject<SHA256STR>(value).res;
		}
		catch (Exception)
		{
			return "";
		}
	}

	private string createSha256(string filePath)
	{
		try
		{
			using SHA256 sHA = SHA256.Create();
			using FileStream inputStream = File.OpenRead(filePath);
			return BitConverter.ToString(sHA.ComputeHash(inputStream)).Replace("-", "").ToLower();
		}
		catch (Exception)
		{
			return "";
		}
	}

	public string GetFile(FileTranslation fileInfo, int value)
	{
		try
		{
			string fileName = "";
			string text = driverNameMap[fileInfo.fileType];
			if (text == null || text.Length == 0)
			{
				return null;
			}
			string path = downloadPath + fileInfo.pCTypeNum + "/" + text + "/" + fileInfo.fileVersion;
			if (isTest)
			{
				ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(RemoteCertificateValidate));
			}
			Directory.CreateDirectory(path);
			int second = DateTime.Now.Second;
			int startDownloadPositionInt = 0;
			string startDownloadPosition = startDownloadPositionInt.ToString();
			string text2 = lengthDownloadInt.ToString();
			FileStream file = null;
			bool createFileFlag = true;
			int allTotal = 0;
			int fileTotalSize = 0;
			bool isDownloadOver = true;
			while (isDownloadOver)
			{
				string text3 = URL + "/file/getDownloadFileChunk";
				text3 = text3 + "?pCTypeNum=" + fileInfo.pCTypeNum;
				text3 = text3 + "&fileType=" + fileInfo.fileType;
				text3 = text3 + "&fileVersion=" + fileInfo.fileVersion;
				text3 = text3 + "&hash256=" + fileInfo.hash256;
				text3 = text3 + "&pW=" + fileInfo.pW;
				isDownloadOver = false;
				text3 = text3 + "&startPoint=" + startDownloadPosition;
				text3 = text3 + "&lengthDown=" + text2;
				text3 = text3.Replace("#", "%23");
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(text3);
				req.Method = "GET";
				req.Timeout = 10000;
				ManualResetEvent downloadCompleteEvent = new ManualResetEvent(initialState: false);
				req.BeginGetResponse(delegate(IAsyncResult ar)
				{
					HttpWebResponse httpWebResponse = (HttpWebResponse)req.EndGetResponse(ar);
					Stream responseStream = httpWebResponse.GetResponseStream();
					if (httpWebResponse.StatusCode != HttpStatusCode.OK)
					{
						isDownloadOver = false;
						downloadCompleteEvent.Set();
					}
					else
					{
						if (createFileFlag)
						{
							fileName = httpWebResponse.GetResponseHeader("Content-Disposition");
							if (fileName != null && fileName.Length > 0)
							{
								file = new FileStream(path + "/" + fileName, FileMode.Create, FileAccess.Write);
								createFileFlag = false;
							}
						}
						if (createFileFlag)
						{
							isDownloadOver = false;
						}
						else
						{
							string responseHeader = httpWebResponse.GetResponseHeader("Content-Length");
							string responseHeader2 = httpWebResponse.GetResponseHeader("fileIsOver");
							string responseHeader3 = httpWebResponse.GetResponseHeader("currentDownLength");
							int num = 0;
							if (responseHeader3 != null && responseHeader3.Length != 0)
							{
								num = int.Parse(responseHeader3);
							}
							if (responseHeader2 != null && responseHeader2.Length != 0 && responseHeader2 == "false")
							{
								isDownloadOver = true;
							}
							else
							{
								isDownloadOver = false;
							}
							if (responseHeader != null || !(responseHeader == ""))
							{
								fileTotalSize = int.Parse(responseHeader);
							}
							int num2 = 0;
							byte[] array = new byte[lengthDownloadInt];
							int num3 = 0;
							int num4 = responseStream.Read(array, 0, array.Length);
							int num5 = 0;
							while (num4 > 0)
							{
								num3 += num4;
								allTotal += num4;
								file.Write(array, 0, num4);
								num5 = (int)((float)allTotal / (float)fileTotalSize * 100f);
								if (num5 - num2 > 1)
								{
									num2 = num5;
									this.DownloadProgressBar?.Invoke(value, num2);
								}
								if (num3 >= num)
								{
									break;
								}
								num4 = responseStream.Read(array, 0, array.Length);
							}
							startDownloadPositionInt += num;
							startDownloadPosition = startDownloadPositionInt.ToString();
							httpWebResponse.Close();
							Trace.WriteLine("下载完成");
						}
						downloadCompleteEvent.Set();
					}
				}, null);
				downloadCompleteEvent.WaitOne();
			}
			if (createFileFlag)
			{
				return null;
			}
			file.Close();
			string text4 = path + "/" + fileName;
			text4 = text4.Replace("./", "");
			text4 = text4.Replace("/", "\\");
			string fileHash = GetFileHash(fileInfo);
			string text5 = createSha256(path + "/" + fileName);
			if (text5 == null)
			{
				Trace.WriteLine("生成sha256失败 fileSHA256：" + text5);
				return null;
			}
			if (!text5.Equals(fileHash))
			{
				Trace.WriteLine("sha256验证失败");
				MessageBox.Show("sha256验证失败");
				return null;
			}
			Trace.WriteLine("HASH 验证通过");
			int second2 = DateTime.Now.Second;
			Trace.WriteLine("下载时间：" + (second2 - second));
			return Path.GetRelativePath(clientFilePath, text4);
		}
		catch (Exception)
		{
			MessageBox.Show("服务器链接错误", "下载错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return null;
		}
	}

	public async Task<string> GetFileNew(FileTranslation fileInfo, int value)
	{
		try
		{
			string fileName = "";
			string text = driverNameMap[fileInfo.fileType];
			if (text == null || text.Length == 0)
			{
				return null;
			}
			string path = downloadPath + fileInfo.pCTypeNum + "/" + text + "/" + fileInfo.fileVersion;
			Directory.CreateDirectory(path);
			int currentSeconds = DateTime.Now.Second;
			int startDownloadPositionInt = 0;
			string text2 = startDownloadPositionInt.ToString();
			string lengthDownload = lengthDownloadInt.ToString();
			FileStream file = null;
			bool createFileFlag = true;
			int allTotal = 0;
			int fileTotalSize = 0;
			bool flag = true;
			int currentDownLengthInt = 0;
			int progressBarValue = 0;
			while (flag)
			{
				string text3 = URL + "/file/getDownloadFileChunk";
				text3 = text3 + "?pCTypeNum=" + fileInfo.pCTypeNum;
				text3 = text3 + "&fileType=" + fileInfo.fileType;
				text3 = text3 + "&fileVersion=" + fileInfo.fileVersion;
				text3 = text3 + "&hash256=" + fileInfo.hash256;
				text3 = text3 + "&pW=" + fileInfo.pW;
				flag = false;
				text3 = text3 + "&startPoint=" + text2;
				text3 = text3 + "&lengthDown=" + lengthDownload;
				text3 = text3.Replace("#", "%23");
				HttpClientHandler handler = ((!isTest) ? new HttpClientHandler() : new HttpClientHandler
				{
					ServerCertificateCustomValidationCallback = RemoteCertificateValidate
				});
				using (HttpClient client = new HttpClient(handler))
				{
					try
					{
						string fileSizeStr = "";
						string currentDownLength = "";
						string fileIsOver = "";
						client.Timeout = TimeSpan.FromMilliseconds(10000.0);
						HttpResponseMessage httpResponseMessage = await client.GetAsync(text3);
						httpResponseMessage.EnsureSuccessStatusCode();
						Stream result = httpResponseMessage.Content.ReadAsStreamAsync().Result;
						if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
						{
							flag = false;
							break;
						}
						if (!createFileFlag)
						{
							goto IL_0374;
						}
						if (httpResponseMessage.Headers.TryGetValues("fileName", out IEnumerable<string> values))
						{
							fileName = values.FirstOrDefault();
							Trace.WriteLine("fileName:" + fileName);
						}
						if (fileName != null && fileName.Length > 0)
						{
							file = new FileStream(path + "/" + fileName, FileMode.Create, FileAccess.Write);
							flag = true;
							createFileFlag = false;
							goto IL_0374;
						}
						flag = false;
						goto end_IL_0213;
						IL_0374:
						if (httpResponseMessage.Headers.TryGetValues("fileLength", out IEnumerable<string> values2))
						{
							fileSizeStr = values2.FirstOrDefault();
							Trace.WriteLine("fileLength:" + fileSizeStr);
						}
						if (httpResponseMessage.Headers.TryGetValues("fileIsOver", out IEnumerable<string> values3))
						{
							fileIsOver = values3.FirstOrDefault();
							Trace.WriteLine("fileIsOver:" + fileIsOver);
						}
						if (httpResponseMessage.Headers.TryGetValues("currentDownLength", out IEnumerable<string> values4))
						{
							currentDownLength = values4.FirstOrDefault();
							Trace.WriteLine("currentDownLength:" + currentDownLength);
						}
						if (currentDownLength == null || currentDownLength.Length == 0)
						{
							goto IL_0450;
						}
						currentDownLengthInt = int.Parse(currentDownLength);
						if (currentDownLengthInt > 0)
						{
							goto IL_0450;
						}
						flag = false;
						goto end_IL_0213;
						IL_0450:
						flag = ((fileIsOver != null && fileIsOver.Length != 0 && fileIsOver == "false") ? true : false);
						if (fileSizeStr != null || !(fileSizeStr == ""))
						{
							fileTotalSize = int.Parse(fileSizeStr);
						}
						using (BinaryReader binaryReader = new BinaryReader(result))
						{
							byte[] array = new byte[currentDownLengthInt];
							int num;
							while ((num = binaryReader.Read(array, 0, array.Length)) > 0)
							{
								file.Write(array, 0, num);
								allTotal += num;
								int num2 = (int)((float)allTotal / (float)fileTotalSize * 100f);
								if (num2 - progressBarValue > 1)
								{
									progressBarValue = num2;
									this.DownloadProgressBar?.Invoke(value, progressBarValue);
								}
							}
						}
						startDownloadPositionInt += currentDownLengthInt;
						text2 = startDownloadPositionInt.ToString();
						continue;
						end_IL_0213:;
					}
					catch (HttpRequestException ex)
					{
						Trace.WriteLine("请求错误:\n" + ex);
						flag = false;
					}
				}
				break;
			}
			if (createFileFlag)
			{
				return null;
			}
			file.Close();
			string text4 = path + "/" + fileName;
			text4 = text4.Replace("./", "");
			text4 = text4.Replace("/", "\\");
			string fileHash = GetFileHash(fileInfo);
			string text5 = createSha256(path + "/" + fileName);
			if (text5 == null)
			{
				Trace.WriteLine("生成sha256失败 fileSHA256：" + text5);
				return null;
			}
			if (!text5.Equals(fileHash))
			{
				Trace.WriteLine("sha256验证失败");
				MessageBox.Show("sha256验证失败");
				return null;
			}
			Trace.WriteLine("HASH 验证通过");
			int second = DateTime.Now.Second;
			Trace.WriteLine("下载时间：" + (second - currentSeconds));
			return Path.GetRelativePath(clientFilePath, text4);
		}
		catch (Exception ex2)
		{
			MessageBox.Show("服务器链接错误:" + ex2, "下载错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return null;
		}
	}

	public void createUpdateFile()
	{
		mutexFile.WaitOne();
		string path = savePath;
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		if (!File.Exists(savePath + "/UpdateConfig"))
		{
			new FileStream(savePath + "/UpdateConfig", FileMode.OpenOrCreate, FileAccess.Write).Close();
		}
		if (!Directory.Exists(downloadPath + PCTYPENUM_SSID))
		{
			Directory.CreateDirectory(downloadPath + PCTYPENUM_SSID);
		}
		mutexFile.ReleaseMutex();
	}

	public bool checkUpdate()
	{
		bool result = false;
		if (getConfigNotUpdate().Length != 0)
		{
			result = true;
		}
		return result;
	}

	public bool writeConfigNotUpdate(string[] content)
	{
		mutexFile.WaitOne();
		string path = savePath;
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		using (FileStream fileStream = File.Create(savePath + "/UpdateConfig"))
		{
			for (int i = 0; i < content.Length; i++)
			{
				byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(content[i] + "\r\n");
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}
		mutexFile.ReleaseMutex();
		return true;
	}

	public string[] getConfigNotUpdate()
	{
		if (!File.Exists(savePath + "/UpdateConfig"))
		{
			return null;
		}
		mutexFile.WaitOne();
		FileStream fileStream = new FileStream(savePath + "/UpdateConfig", FileMode.Open, FileAccess.Read);
		StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
		List<string> list = new List<string>();
		ulong num = 0uL;
		string text;
		while ((text = streamReader.ReadLine()) != null)
		{
			if (text.Length > 0)
			{
				list.Add(text);
				num++;
			}
		}
		fileStream.Close();
		streamReader.Close();
		mutexFile.ReleaseMutex();
		return list.ToArray();
	}

	public bool writeConfigFileType(string[] content, string fileName)
	{
		mutexFile.WaitOne();
		string path = savePath;
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		using (FileStream fileStream = File.Create(savePath + fileName))
		{
			for (int i = 0; i < content.Length; i++)
			{
				string text = content[i];
				text = text + fileSplitFlag + driverNameMap[content[i]];
				byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(text + "\r\n");
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}
		mutexFile.ReleaseMutex();
		return true;
	}

	public (string[], string[]) getConfigFileType(string fileName)
	{
		if (!File.Exists(savePath + fileName))
		{
			return (null, null);
		}
		mutexFile.WaitOne();
		FileStream fileStream = new FileStream(savePath + fileName, FileMode.Open, FileAccess.Read);
		StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		ulong num = 0uL;
		string text;
		while ((text = streamReader.ReadLine()) != null)
		{
			if (text.Length > 0)
			{
				string[] array = text.Split(new string[1] { fileSplitFlag }, StringSplitOptions.None);
				if (array == null || array.Length == 0 || array.Length > 2)
				{
					return (null, null);
				}
				list.Add(array[0]);
				list2.Add(array[1]);
				num++;
			}
		}
		fileStream.Close();
		streamReader.Close();
		mutexFile.ReleaseMutex();
		string[] item = list.ToArray();
		string[] item2 = list2.ToArray();
		return (item, item2);
	}

	public void DeleteDirectory(string targetDir)
	{
		string[] files = Directory.GetFiles(targetDir);
		foreach (string path in files)
		{
			File.SetAttributes(path, FileAttributes.Normal);
			File.Delete(path);
		}
		files = Directory.GetDirectories(targetDir);
		foreach (string text in files)
		{
			DeleteDirectory(text);
			Directory.Delete(text, recursive: true);
		}
	}

	public string downloadOtherFileWPF(string fileType, string version, int value)
	{
		IclientRequest.downFileVersionInfo existedVersion = getExistedVersion(PCTYPENUM_SSID, fileType);
		if (existedVersion.latestVersion != null)
		{
			bool flag = false;
			for (int i = 0; i < existedVersion.allVersion.Length; i++)
			{
				if (string.Compare(existedVersion.allVersion[i], version) == 0)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				version = existedVersion.latestVersion;
			}
			FileTranslation fileInfo = new FileTranslation
			{
				pCTypeNum = PCTYPENUM_SSID,
				fileType = fileType,
				fileVersion = version,
				hash256 = "",
				pW = ""
			};
			string text = null;
			for (int j = 0; j < 1; j++)
			{
				Task<string> fileNew = GetFileNew(fileInfo, value);
				fileNew.Wait();
				text = fileNew.Result;
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}
		return "noUpdate";
	}

	public (IclientRequest.downFileVersionInfo, int) getAppVersion()
	{
		APPVersionInfo = getExistedVersion(PCTYPENUM_SSID, "APPUpdate");
		if (APPVersionInfo.latestVersion == null)
		{
			return (APPVersionInfo, -1);
		}
		if (APPVersionInfo.latestVersion == "")
		{
			return (APPVersionInfo, 0);
		}
		(int, Exception) tuple = Compare(APPVersionInfo.latestVersion, APPVersion);
		var (num, _) = tuple;
		if (tuple.Item2 != null)
		{
			return (APPVersionInfo, -1);
		}
		if (APPVersionInfo.latestVersion != null && num == 1)
		{
			return (APPVersionInfo, 1);
		}
		return (APPVersionInfo, 0);
	}

	private string downloadApp(string version)
	{
		APPVersionInfo = getExistedVersion(PCTYPENUM_SSID, "APPUpdate");
		if (APPVersionInfo.latestVersion != null && string.Compare(APPVersionInfo.latestVersion, version) == 1)
		{
			FileTranslation fileInfo = new FileTranslation
			{
				pCTypeNum = PCTYPENUM_SSID,
				fileType = "APPUpdate",
				fileVersion = APPVersionInfo.latestVersion,
				hash256 = "",
				pW = ""
			};
			string text = null;
			for (int i = 0; i < 1; i++)
			{
				Task<string> fileNew = GetFileNew(fileInfo, 0);
				fileNew.Wait();
				text = fileNew.Result;
				if (text != null)
				{
					Trace.Write("download app success:" + text);
					return text;
				}
			}
			return null;
		}
		return "noUpdate";
	}

	public bool UpdateAPP()
	{
		string text = null;
		text = downloadApp(APPVersion);
		if (text != null)
		{
			if (text == "noUpdate")
			{
				Trace.Write("noUpdate");
				return false;
			}
			if (!updateAppPlan3(text))
			{
				MessageBox.Show("升级失败");
			}
			return true;
		}
		MessageBox.Show("download APP ERROR");
		return false;
	}

	private bool updateAppPlan3(string filePath)
	{
		string directoryName = Path.GetDirectoryName(filePath);
		Trace.WriteLine("GetFile dirPath:" + directoryName);
		if (File.Exists(directoryName + "\\FlashWin.bat"))
		{
			File.Delete(directoryName + "\\FlashWin.bat");
		}
		Process process = new Process();
		process.StartInfo.FileName = filePath;
		process.StartInfo.Arguments = "-y";
		process.Start();
		process.WaitForExit();
		bool flag = true;
		string text = directoryName + "\\FlashWin.bat";
		foreach (string item in Directory.GetFiles(directoryName, "*", SearchOption.AllDirectories).ToList())
		{
			string fileName = Path.GetFileName(item);
			if (fileName == "FlashWin.bat")
			{
				Trace.WriteLine(fileName);
				flag = false;
				text = item;
				break;
			}
		}
		if (flag)
		{
			return false;
		}
		Trace.WriteLine("tmpDir:" + text);
		string location = Assembly.GetExecutingAssembly().Location;
		Path.GetDirectoryName(location);
		Trace.WriteLine("本程序的路径：" + location);
		Process obj = new Process
		{
			StartInfo = 
			{
				FileName = text
			}
		};
		process.StartInfo.Arguments = location;
		obj.Start();
		Environment.Exit(0);
		return true;
	}

	public bool getServerVersion(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		IclientRequest.downFileVersionInfo existedVersion = getExistedVersion(PCTYPENUM_SSID, name);
		if (existedVersion.allVersion == null || existedVersion.allVersion.Length == 0)
		{
			return false;
		}
		lock (locker)
		{
			if (serverVersionMap.ContainsKey(name))
			{
				serverVersionMap[name] = existedVersion;
			}
			else
			{
				serverVersionMap.Add(name, existedVersion);
			}
		}
		return true;
	}

	public (string[], int) checkUpdateServer()
	{
		try
		{
			List<string> list = new List<string>();
			ExistedVersionFlag = 1;
			pCTypeFileType1 = getFileType();
			if (ExistedVersionFlag == -1 || pCTypeFileType1.fileName == null)
			{
				var (array, array2) = getConfigFileType(FileTypeFileName);
				if (array == null)
				{
					return (null, -1);
				}
				if (array.Length != 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						driverNameMap[array[i]] = array2[i];
					}
					nameArr = array;
					getLocation();
				}
				return (null, -1);
			}
			if (pCTypeFileType1.fileName.Length == 0 || pCTypeFileType1.fileName[0] == "")
			{
				return (null, 0);
			}
			nameArr = new string[pCTypeFileType1.fileName.Length];
			if (nameArr.Length != 0)
			{
				for (int j = 0; j < nameArr.Length; j++)
				{
					driverNameMap[pCTypeFileType1.fileName[j]] = pCTypeFileType1.driverName[j];
					nameArr[j] = pCTypeFileType1.fileName[j];
				}
			}
			if (nameArr.Length != 0)
			{
				writeConfigFileType(nameArr, FileTypeFileName);
			}
			int second = DateTime.Now.Second;
			getLocation();
			int second2 = DateTime.Now.Second;
			foreach (string key in localVersionMap.Keys)
			{
				if (!getServerVersion(key))
				{
					continue;
				}
				string text = localVersionMap[key];
				string latestVersion = serverVersionMap[key].latestVersion;
				if (eliminateFW.Contains(key))
				{
					if (text != latestVersion)
					{
						list.Add(key);
					}
					continue;
				}
				(int, Exception) tuple2 = Compare(text, latestVersion);
				var (num, _) = tuple2;
				if (tuple2.Item2 == null && num == -1)
				{
					list.Add(key);
				}
			}
			int second3 = DateTime.Now.Second;
			Trace.WriteLine("时间消耗\n本地时间消耗：" + (second2 - second) + "\n服务器时间消耗：" + (second3 - second2));
			return (list.ToArray(), 1);
		}
		catch (Exception ex)
		{
			Trace.WriteLine("错误！严重错误！！！error:" + ex);
			return (null, -1);
		}
	}

	public bool updateAll()
	{
		bool res = false;
		string[] configNotUpdate = getConfigNotUpdate();
		if (configNotUpdate == null || configNotUpdate.Length == 0)
		{
			return false;
		}
		List<string> strList = new List<string>(configNotUpdate);
		string filePath = "";
		string name = "";
		string version = "";
		List<string> strListTmp = strList;
		return inlineFunc();
		bool inlineFunc()
		{
			string text = "";
			try
			{
				foreach (string item in strList)
				{
					string[] array = item.Split(new string[1] { fileSplitFlag }, StringSplitOptions.None);
					if (array.Length < 4)
					{
						Trace.WriteLine("配置文件出错！！！");
						return false;
					}
					string text2 = array[0].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
					string[] array2 = array[1].Split(new string[1] { "=" }, StringSplitOptions.None);
					name = array2[1];
					string[] array3 = array[2].Split(new string[1] { "=" }, StringSplitOptions.None);
					version = array3[1];
					if (array[3].Split(new string[1] { "=" }, StringSplitOptions.None)[1].Equals("true"))
					{
						if (!getDriverVersion(name).Equals(version))
						{
							filePath = text2;
							text = item.Replace("true", "false");
							strListTmp.Remove(item);
							break;
						}
						strListTmp.Remove(item);
					}
				}
				strList = strListTmp;
				if (filePath.Length == 0)
				{
					string[] content = strListTmp.ToArray();
					writeConfigNotUpdate(content);
					return false;
				}
				res = true;
				filePath = clientFilePath + "\\" + filePath;
				string directoryName = Path.GetDirectoryName(filePath);
				Trace.WriteLine("GetFile dirPath:" + directoryName);
				if (File.Exists(directoryName + "\\FlashWin.bat"))
				{
					File.Delete(directoryName + "\\FlashWin.bat");
				}
				Process process = new Process();
				process.StartInfo.FileName = filePath;
				process.StartInfo.Arguments = "-y";
				process.Start();
				process.WaitForExit();
				Trace.WriteLine("解压进程已退出");
				bool flag = true;
				string text3 = directoryName + "\\FlashWin.bat";
				foreach (string item2 in Directory.GetFiles(directoryName, "*", SearchOption.AllDirectories).ToList())
				{
					string fileName = Path.GetFileName(item2);
					if (fileName == "FlashWin.bat")
					{
						Console.WriteLine(fileName);
						flag = false;
						text3 = item2;
						break;
					}
				}
				if (flag)
				{
					text = text.Replace("false", "Unable");
					strListTmp.Add(text);
					string[] content2 = strListTmp.ToArray();
					writeConfigNotUpdate(content2);
					MessageBox.Show("该项不支持自动升级，请进行手动升级");
					string directoryName2 = Path.GetDirectoryName(filePath);
					Process.Start("explorer.exe", directoryName2);
					return false;
				}
				Trace.WriteLine("tmpDir:" + text3);
				Process process2 = new Process();
				process2.StartInfo.FileName = text3;
				process2.Start();
				process2.WaitForExit();
				if (text != "")
				{
					if (!getDriverVersion(name).Equals(version))
					{
						strListTmp.Add(text);
						res = false;
					}
					else
					{
						res = true;
						File.SetAttributes(text3, FileAttributes.Normal);
						File.Delete(text3);
					}
					string directoryName3 = Path.GetDirectoryName(text3);
					DeleteDirectory(directoryName3);
					Directory.Delete(directoryName3, recursive: true);
				}
				string[] content3 = strListTmp.ToArray();
				writeConfigNotUpdate(content3);
				return res;
			}
			catch
			{
				strListTmp.Add(text);
				string[] content4 = strListTmp.ToArray();
				writeConfigNotUpdate(content4);
				return false;
			}
		}
	}

	public bool updateOther(string fileName, string versionSer)
	{
		bool res = false;
		string[] configNotUpdate = getConfigNotUpdate();
		if (configNotUpdate == null || configNotUpdate.Length == 0)
		{
			return false;
		}
		List<string> strList = new List<string>(configNotUpdate);
		string filePath = "";
		string name = "";
		string version = "";
		List<string> strListTmp = strList;
		inlineFunc();
		return res;
		bool inlineFunc()
		{
			string text = "";
			try
			{
				bool flag;
				foreach (string item in strList)
				{
					string[] array = item.Split(new string[1] { fileSplitFlag }, StringSplitOptions.None);
					if (array.Length < 4)
					{
						Trace.WriteLine("配置文件出错！！！");
						return false;
					}
					string text2 = array[0].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
					string[] array2 = array[1].Split(new string[1] { "=" }, StringSplitOptions.None);
					name = array2[1];
					string[] array3 = array[2].Split(new string[1] { "=" }, StringSplitOptions.None);
					version = array3[1];
					_ = array[3].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
					string text3 = driverNameMap[fileName];
					if (text3 == null || text3.Length == 0)
					{
						MessageBox.Show("zqperror");
						return false;
					}
					if (!(name != text3))
					{
						if (!getDriverVersion(fileName).Equals(version))
						{
							if (versionSer != null && versionSer != "")
							{
								if (!eliminateFW.Contains(name))
								{
									(int, Exception) tuple = Compare(versionSer, version);
									var (num, _) = tuple;
									if (tuple.Item2 != null && num != 1)
									{
										strListTmp.Remove(item);
										flag = false;
										res = false;
										string[] content = strListTmp.ToArray();
										writeConfigNotUpdate(content);
										return false;
									}
								}
								else if (!versionSer.Equals(version))
								{
									strListTmp.Remove(item);
									flag = false;
									res = false;
									string[] content2 = strListTmp.ToArray();
									writeConfigNotUpdate(content2);
									return false;
								}
							}
							filePath = text2;
							text = item.Replace("true", "false");
							strListTmp.Remove(item);
							flag = true;
							res = true;
							break;
						}
						strListTmp.Remove(item);
					}
				}
				strList = strListTmp;
				if (filePath.Length == 0)
				{
					string[] content3 = strListTmp.ToArray();
					writeConfigNotUpdate(content3);
					return false;
				}
				flag = true;
				filePath = clientFilePath + "\\" + filePath;
				string directoryName = Path.GetDirectoryName(filePath);
				Trace.WriteLine("GetFile dirPath:" + directoryName);
				if (File.Exists(directoryName + "\\FlashWin.bat"))
				{
					File.Delete(directoryName + "\\FlashWin.bat");
				}
				Process process = new Process();
				process.StartInfo.FileName = filePath;
				process.StartInfo.Arguments = "-y";
				process.Start();
				process.WaitForExit();
				Trace.WriteLine("解压进程已退出");
				bool flag2 = true;
				string text4 = directoryName + "\\FlashWin.bat";
				foreach (string item2 in Directory.GetFiles(directoryName, "*", SearchOption.AllDirectories).ToList())
				{
					string fileName2 = Path.GetFileName(item2);
					if (fileName2 == "FlashWin.bat")
					{
						Console.WriteLine(fileName2);
						flag2 = false;
						text4 = item2;
						break;
					}
				}
				if (flag2)
				{
					text = text.Replace("false", "Unable");
					strListTmp.Add(text);
					string[] content4 = strListTmp.ToArray();
					writeConfigNotUpdate(content4);
					MessageBox.Show("该项不支持自动升级，请进行手动升级");
					string directoryName2 = Path.GetDirectoryName(filePath);
					Process.Start("explorer.exe", directoryName2);
					return false;
				}
				Trace.WriteLine("tmpDir:" + text4);
				Process process2 = new Process();
				process2.StartInfo.FileName = text4;
				process2.Start();
				process2.WaitForExit();
				if (text != "")
				{
					if (!getDriverVersion(name).Equals(version))
					{
						strListTmp.Add(text);
						flag = false;
					}
					else
					{
						flag = true;
						File.SetAttributes(text4, FileAttributes.Normal);
						File.Delete(text4);
					}
					string directoryName3 = Path.GetDirectoryName(text4);
					DeleteDirectory(directoryName3);
					Directory.Delete(directoryName3, recursive: true);
				}
				string[] content5 = strListTmp.ToArray();
				writeConfigNotUpdate(content5);
				return flag;
			}
			catch
			{
				strListTmp.Add(text);
				string[] content6 = strListTmp.ToArray();
				writeConfigNotUpdate(content6);
				return false;
			}
		}
	}

	private bool DownloadPool(string fileType, string version, int value)
	{
		try
		{
			Trace.WriteLine("Start downloading:" + fileType + version);
			string text = downloadOtherFileWPF(fileType, version, value);
			Trace.WriteLine("download file over.filePath:" + text);
			if (text == "noUpdate")
			{
				return true;
			}
			string[] configNotUpdate = getConfigNotUpdate();
			List<string> list = new List<string>(configNotUpdate);
			string text2 = driverNameMap[fileType];
			if (text2 == null || text2.Length == 0)
			{
				return false;
			}
			List<string> list2 = list;
			if (configNotUpdate.Length != 0)
			{
				foreach (string item2 in list2)
				{
					string[] array = item2.Split(new string[1] { fileSplitFlag }, StringSplitOptions.None);
					if (array.Length < 4)
					{
						Trace.WriteLine("配置文件出错！！！");
						return false;
					}
					if (array[1].Split(new string[1] { "=" }, StringSplitOptions.None)[1] == text2)
					{
						list.Remove(item2);
					}
				}
			}
			string item = "path=" + text + fileSplitFlag + "type=" + text2 + fileSplitFlag + "version=" + version + fileSplitFlag + "update=true";
			list.Add(item);
			string[] content = list.ToArray();
			writeConfigNotUpdate(content);
			return true;
		}
		catch (Exception)
		{
			Trace.WriteLine("未知错误DownloadPool");
			return false;
		}
	}

	public bool DownloadFile(string fileType, string version, int value)
	{
		if (fileType == null || fileType == "")
		{
			return false;
		}
		mutexDownload.WaitOne();
		if (updateOther(fileType, version))
		{
			mutexDownload.ReleaseMutex();
			return true;
		}
		mutexDownload.ReleaseMutex();
		if (version == null || version == "")
		{
			return false;
		}
		semaphore.Wait();
		bool result = DownloadPool(fileType, version, value);
		semaphore.Release();
		mutexDownload.WaitOne();
		updateAll();
		mutexDownload.ReleaseMutex();
		return result;
	}

	public (string[], string[]) checkLoadUpdate()
	{
		string[] configNotUpdate = getConfigNotUpdate();
		if (configNotUpdate == null || configNotUpdate.Length == 0)
		{
			return (null, null);
		}
		List<string> list = new List<string>(configNotUpdate);
		string text = "";
		string text2 = "";
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		foreach (string item3 in list)
		{
			string[] array = item3.Split(new string[1] { fileSplitFlag }, StringSplitOptions.None);
			if (array.Length < 4)
			{
				Trace.WriteLine("配置文件出错！！！");
				return (null, null);
			}
			_ = array[0].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
			text = array[1].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
			text2 = array[2].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
			_ = array[3].Split(new string[1] { "=" }, StringSplitOptions.None)[1];
			if (text != null && text != "" && text2 != null && text2 != "")
			{
				list2.Add(text);
				list3.Add(text2);
			}
		}
		string[] item = list2.ToArray();
		string[] item2 = list3.ToArray();
		return (item, item2);
	}

	public IclientRequest.downFileVersionInfo getGerverVersionMap(string fileType)
	{
		IclientRequest.downFileVersionInfo value = new IclientRequest.downFileVersionInfo
		{
			type = fileType
		};
		if (serverVersionMap.TryGetValue(fileType, out value))
		{
			return value;
		}
		return default(IclientRequest.downFileVersionInfo);
	}

	public string getLocalVersionMap(string fileType)
	{
		string value = "";
		if (localVersionMap.TryGetValue(fileType, out value))
		{
			return value;
		}
		return null;
	}

	public Dictionary<string, string> getAllLoacalVersionMap()
	{
		return localVersionMap;
	}

	public Dictionary<string, string> getAllLoacalNameMap()
	{
		return driverNameMap;
	}

	public IclientRequest.downFileVersionInfo getAppversion()
	{
		return getExistedVersion(PCTYPENUM_SSID, "APPUpdate");
	}

	public (int, Exception) CompareCor(string s1, string s2)
	{
		int i;
		for (i = 0; i < s1.Length && s1[i] == '0'; i++)
		{
		}
		s1 = ((i != s1.Length) ? s1.Substring(i) : "0");
		for (i = 0; i < s2.Length && s2[i] == '0'; i++)
		{
		}
		s2 = ((i != s2.Length) ? s2.Substring(i) : "0");
		if (s1.Length == 0)
		{
			if (s2.Length == 0)
			{
				return (0, null);
			}
			return (-1, null);
		}
		if (!int.TryParse(s1, out var result))
		{
			return (0, new Exception("Error parsing s1"));
		}
		if (!int.TryParse(s2, out var result2))
		{
			return (0, new Exception("Error parsing s2"));
		}
		if (result == result2)
		{
			return (0, null);
		}
		if (result > result2)
		{
			return (1, null);
		}
		return (-1, null);
	}

	public (int, Exception) Compare(string version1, string version2)
	{
		string[] array = version1.Split('.');
		string[] array2 = version2.Split('.');
		int i;
		for (i = 0; i < array.Length && i < array2.Length; i++)
		{
			var (num, ex) = CompareCor(array[i], array2[i]);
			if (ex != null)
			{
				return (0, ex);
			}
			if (num != 0)
			{
				return (num, null);
			}
		}
		if (array.Length == array2.Length)
		{
			return (0, null);
		}
		if (array.Length > array2.Length)
		{
			for (; i < array.Length; i++)
			{
				var (num2, ex2) = CompareCor(array[i], "0");
				if (ex2 != null)
				{
					return (0, ex2);
				}
				if (num2 != 0)
				{
					return (num2, null);
				}
			}
			return (0, null);
		}
		for (; i < array2.Length; i++)
		{
			var (num3, ex3) = CompareCor("0", array2[i]);
			if (ex3 != null)
			{
				return (0, ex3);
			}
			if (num3 != 0)
			{
				return (num3, null);
			}
		}
		return (0, null);
	}

	public void DeleteAllFile()
	{
		string text = clientFilePath + "\\File";
		string path = clientFilePath + "\\SavePath\\UpdateConfig";
		DeleteDirectory(text);
		Directory.Delete(text, recursive: true);
		Directory.CreateDirectory(text);
		File.Create(path);
	}

	public void OpenLocalFile()
	{
		Process.Start("explorer.exe", downloadPath + PCTYPENUM_SSID);
	}
}
