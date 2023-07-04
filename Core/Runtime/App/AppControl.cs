using System.Collections;
using System.Collections.Generic;
using App.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Tayx.Graphy;

namespace App 
{
	public class AppControl : MonoBehaviour
	{
		[SerializeField] Camera _camera;
		public string mainScene = "App/Scenes/Main";
		public bool useShiftKey = false;
		public ContentsSync contentsSync;
		public GraphyManager _graphy;

		void Awake ()
		{
			
		}

		void Update ()
		{
			bool enableHotKey = true;
			if (useShiftKey)
			{
				enableHotKey = Input.GetKey(KeyCode.LeftShift);
			}
			
			if (enableHotKey && Input.GetKeyUp(KeyCode.F5))
			{
				SceneReload();
			}
			if (enableHotKey && Input.GetKeyUp(KeyCode.F4))
			{
				Quit();
			}
			if (enableHotKey && Input.GetKeyUp(KeyCode.F7))
			{
				ChangeTimeScale();
			}
			if (enableHotKey && Input.GetKeyUp(KeyCode.F2))
			{
				ToggleMouseCursor();
			}
		}

		public void ToggleMouseCursor()
		{
			Cursor.visible = !Cursor.visible;
		}

		public void ChangeTimeScale()
		{
			if (Time.timeScale == 1)
			{
				Time.timeScale = 2;
			}
			else if (Time.timeScale == 2)
			{
				Time.timeScale = 5;
			}
			else if (Time.timeScale == 5)
			{
				Time.timeScale = 10;
			}
			else
			{
				Time.timeScale = 1;
			}
		}

		public void SceneReload()
		{
			SceneManager.LoadScene(mainScene, LoadSceneMode.Single);
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}
