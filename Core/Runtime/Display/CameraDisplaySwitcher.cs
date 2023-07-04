using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class CameraDisplaySwitcher : MonoBehaviour
	{
		public int useDisplayCount = 2;
		public List<Camera> targetCameras = new List<Camera>();
		public List<Canvas> targetCanvases = new List<Canvas>();
		public bool useHotKey = true;

		List<DisplayInfo> infoList;

		void Start ()
		{
			if (targetCameras.Count == 0)
			{
				Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
				targetCameras = new List<Camera>(cameras);
			}
			if (targetCanvases.Count == 0)
			{
				Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
				targetCanvases = new List<Canvas>(canvases);
			}

			// infoList = DisplayInfo.GetDisplayList();
			// displayCount = infoList.Count;
		}

		void Update ()
		{
			if (useHotKey)
			{
				if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
				{
					SwitchDisplay();
				}
			}
		}

		public void SwitchDisplay ()
		{
			for(int i=0; i<targetCameras.Count; i++)
			{
				Camera camera = targetCameras[i];
				int index = camera.targetDisplay;
				index += 1;
				if (index >= useDisplayCount) index = 0;
				camera.targetDisplay = index;
			}

			for(int i=0; i<targetCanvases.Count; i++)
			{
				Canvas canvas = targetCanvases[i];
				int index = canvas.targetDisplay;
				index += 1;
				if (index >= useDisplayCount) index = 0;
				canvas.targetDisplay = index;
			}
		}
	}
}
