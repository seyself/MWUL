using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class FilePath
	{
		public static string ProjectBase(string path)
		{
			#if UNITY_EDITOR
			return Combine(Application.dataPath, "../" + path);
			#else
			return Combine(Application.persistentDataPath, path);
			#endif
		}

		public static string StreamingAssets(string path)
		{
			return Combine(Application.streamingAssetsPath, path);
		}

		public static string DataDirectory(string path)
		{
			return Combine(Application.dataPath, path);
		}

		public static string Combine(string path1, string path2)
		{
			path1 = path1.Replace(@"\", "/");
			path2 = path2.Replace(@"\", "/");
			return System.IO.Path.Combine(path1, path2);
			// return System.IO.Path.Combine(path1, path2).Replace("/", @"\");
		}
	}
}