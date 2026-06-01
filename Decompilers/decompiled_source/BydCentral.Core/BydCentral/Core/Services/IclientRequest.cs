using System;
using System.Collections.Generic;

namespace BydCentral.Core.Services;

public interface IclientRequest
{
	[Serializable]
	public struct downFileVersionInfo
	{
		public string type;

		public string latestVersion;

		public string[] allVersion;

		public string[] releaseNote;

		public string[] updateTime;
	}

	event Action<int, int> DownloadProgressBar;

	(string[], string[]) checkLoadUpdate();

	(string[], int) checkUpdateServer();

	downFileVersionInfo getGerverVersionMap(string fileType);

	string getLocalVersionMap(string fileType);

	Dictionary<string, string> getAllLoacalVersionMap();

	Dictionary<string, string> getAllLoacalNameMap();

	bool DownloadFile(string fileType, string version, int value);

	(downFileVersionInfo, int) getAppVersion();

	bool UpdateAPP();

	(int, Exception) Compare(string version1, string version2);

	void DeleteAllFile();

	void OpenLocalFile();
}
