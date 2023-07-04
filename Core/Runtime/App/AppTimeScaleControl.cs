using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class AppTimeScaleControl : MonoBehaviour
	{
		public KeyCode toggleHotKey = KeyCode.F3;
		public KeyCode fastForwardHotKey = KeyCode.LeftShift;
		public float fastForwardTimeScale = 10f;

		public bool paused { get; private set; }
		public bool boosted { get; private set; }

		void Update ()
		{
			if (Input.GetKeyUp(toggleHotKey))
			{
				ToggleTimeScale();
			}

			if (Input.GetKey(fastForwardHotKey))
			{
				boosted = true;
				Time.timeScale = fastForwardTimeScale;
			}
			else if (boosted)
			{
				boosted = false;
				Time.timeScale = 1f;
			}
		}
		
		public void ToggleTimeScale()
		{
			if (paused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}

		public void Pause()
		{
			paused = true;
			Time.timeScale = 0f;
		}
		
		public void Resume()
		{
			paused = false;
			Time.timeScale = 1f;
		}
	}
}
