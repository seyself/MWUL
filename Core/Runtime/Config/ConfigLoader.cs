using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class ConfigLoader
	{
		public ConfigLoader()
		{
			
		}

		public static T Load<T>(string filePath="Data/config.yaml")
		{
			// #if UNITY_EDITOR_OSX

			// #elif UNITY_STANDALONE_OSX
			// filePath = "../" + filePath;
			// #endif
			
			string configYaml = FileIO.ReadText(filePath);
			return Yaml.Deserialize<T>(configYaml);
		}
		
		public static T LoadYAML<T>(string filePath="Data/config.yaml")
		{
			// #if UNITY_EDITOR_OSX

			// #elif UNITY_STANDALONE_OSX
			// filePath = "../" + filePath;
			// #endif
			
			string configYaml = FileIO.ReadText(filePath);
			return Yaml.Deserialize<T>(configYaml);
		}
		
		public static T LoadJSON<T>(string filePath="Data/config.json")
		{
			string configJson = FileIO.ReadText(filePath);
			return JsonUtility.FromJson<T>(configJson);
		}
	}
}