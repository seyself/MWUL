using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class MultiDisplayActivator : MonoBehaviour
	{
		[SerializeField] public DisplaySwitcher displaySwitcher;
		[SerializeField] int _displayCount = 2;

		public int displayCount {
			get { return _displayCount; }
			set { _displayCount = value; }
		}

		public List<int> activeDisplayList = new List<int>();

		public bool saveDisplay;
		public bool autoStart;

		void Start()
		{
			if (autoStart)
			{
				Activate();
			}
		}
		
		public void Activate()
		{
			StartCoroutine(StartEnumerator());
		}
		
		IEnumerator StartEnumerator ()
		{
			if (activeDisplayList.Count > 0)
			{
				int displayCount = Display.displays.Length;
				int len = activeDisplayList.Count;
				for(int i=0; i<len; i++)
				{
					int k = activeDisplayList[i];
					if (k < displayCount)
					{
						if (i == 0 && displaySwitcher != null)
						{
							// not operation
						}
						else
						{
							Display.displays[k].Activate();
						}
						
					}
				}

				yield return null;

				if (displaySwitcher != null)
				{
					displaySwitcher.ChangeDisplay( activeDisplayList[0] );
				}
				
				if (saveDisplay)
				{
					PlayerPrefs.SetInt("UnitySelectMonitor", activeDisplayList[0]);
				}
				
			}
			else
			{
				int len = Display.displays.Length;
				if (len > _displayCount)
				{
					len = _displayCount;
				}
				for(int i=0; i<len; i++)
				{
					Display.displays[i].Activate();
				}
			}
		}
	}
}
