using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace App 
{
	public class Reboot
	{
		/// <summary>
		/// reboot.bat:
		/// <code>
		/// timeout /t 3 /nobreak >nul
		/// call dev_607x1080.bat
		/// </code>
		/// </summary>
		public static void Execute ()
		{
			UnityEngine.Debug.Log("Reboot / Ececute");

			#if UNITY_EDITOR

			UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			UnityEditor.EditorApplication.isPlaying = false;
			
			#elif UNITY_STANDALONE_WIN
			
			Application.Quit();
			Process p = new Process();
			p.StartInfo.FileName = "reboot.bat";
			p.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
			p.Start();
			
			#endif
		}

		#if UNITY_EDITOR

		private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
		{
			UnityEngine.Debug.Log(state);
			if (state == UnityEditor.PlayModeStateChange.EnteredEditMode)
			{
				UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
				UnityEditor.EditorApplication.isPlaying = true;
			}
		}

		#endif
	}
}
