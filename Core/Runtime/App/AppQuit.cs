using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class AppQuit : MonoBehaviour
	{
		public KeyCode hotKey = KeyCode.Escape;
		public int inputCount = 3;
		public float inputIntervalSec = 0.5f;

		private int _count = 0;
		private float _latestInputTime = 0;
		

		void Update ()
		{
			if (Input.GetKeyDown(hotKey))
			{
				float time = Time.time;
				if (time - _latestInputTime <= inputIntervalSec)
				{
					_count += 1;
					if (_count >= inputCount)
					{
						_count = 0;
						Application.Quit();
					}
				}
				else
				{
					_count = 1;
				}
				_latestInputTime = time;
				
			}
		}
	}
}
