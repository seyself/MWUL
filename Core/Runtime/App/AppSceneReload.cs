using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace App 
{
	public class AppSceneReload : MonoBehaviour
	{
		public string mainScene = "App/Scenes/Main";

		public KeyCode hotKey = KeyCode.F5;
		

		void Update ()
		{
			if (Input.GetKeyUp(hotKey))
			{
				SceneReload();
			}
		}
		
		public void SceneReload()
		{
			SceneManager.LoadScene(mainScene, LoadSceneMode.Single);
		}
	}
}
