using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace App 
{
	public class BackupLogFile : MonoBehaviour
	{
		public string logDirectory;
		public string backupDirectory = "Data/Logs/";
		public string logFile;
		public bool autoBackup;
		public int maxFileCount = 100;

		void Start ()
		{
			if (autoBackup)
			{
				AutoFilePath();
				Execute();
			}
		}

		public void AutoFilePath()
		{
#if UNITY_EDITOR_WIN
			Debug.Log("BACKUP LOG : UNITY_EDITOR_WIN");
			if (logDirectory.IsNullOrEmpty())
			{
				var regex = new Regex(@"/LocalLow/.+");
				logDirectory = regex.Replace(Application.persistentDataPath, "/Local/Unity/Editor/");
			}
			if (logFile.IsNullOrEmpty())
			{
				logFile = "Editor-prev.log";
			}
#elif UNITY_STANDALONE_WIN
			Debug.Log("BACKUP LOG : UNITY_STANDALONE_WIN");
			if (logDirectory.IsNullOrEmpty())
			{
				logDirectory = Application.persistentDataPath + "/";
			}
			if (logFile.IsNullOrEmpty())
			{
				logFile = "Player-prev.log";
			}
#else
			Debug.Log("BACKUP LOG : Other");
#endif
		}
		
		public void Execute ()
		{
			Debug.Log("enabled = " + enabled);
			Debug.Log("logDirectory = " + logDirectory);
			Debug.Log("backupDirectory = " + backupDirectory);
			Debug.Log("logFile = " + logFile);
			Debug.Log("maxFileCount = " + maxFileCount);
			
			if (!enabled) return;
			if (logDirectory.IsNullOrEmpty()) return;
			if (backupDirectory.IsNullOrEmpty()) return;
			if (logFile.IsNullOrEmpty()) return;
			
			Debug.Log("############################## Execute Backup LogFile. ##############################");
			
			try
			{
				string prevLog = logDirectory + logFile;
				if (FileIO.Exists(prevLog))
				{
					string datetime = System.DateTime.Now.ToString("_yyyyMMdd_HHmmss");
					string copyFile = backupDirectory + logFile.Replace(".", datetime + ".");
					string text = FileIO.ReadText(prevLog);
					FileIO.WriteText(copyFile, text);
					Debug.Log("##### Success Backup LogFile >>> " + copyFile);
				}
				else
				{
					Debug.LogWarning("!!! Not Found Target LogFile >>> " + prevLog);
				}
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e);
			}

			try
			{
				string[] files = FileIO.Find(backupDirectory, "*.log");
				if (files.Length > maxFileCount)
				{
					int len = files.Length - maxFileCount;
					for (int i = 0; i < len; i++)
					{
						Debug.Log("REMOVE LOG FILE : " + files[i]);
						FileIO.RemoveFile(files[i]);
					}
				}
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e);
			}
		}
	}
}
