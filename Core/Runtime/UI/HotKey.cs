using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace App 
{
	public class HotKey : MonoBehaviour
	{
		[System.Serializable]
		public class ToggleEvent : UnityEvent<bool> {}

		[SerializeField] KeyCode key = KeyCode.None;
		[SerializeField] bool CtrlKey;
		[SerializeField] bool ShiftKey;
		[SerializeField] bool AltKey;

		[SerializeField] bool isOn;
		[SerializeField] public UnityEvent OnSingleAction;
		[SerializeField] public ToggleEvent OnToggleAction;

		void Start ()
		{
			
		}

		void Update ()
		{
			bool isCtrl = !CtrlKey || Input.GetKeyDown(KeyCode.LeftControl);
			bool isShift = !ShiftKey || Input.GetKeyDown(KeyCode.LeftShift);
			bool isAlt = !AltKey || Input.GetKeyDown(KeyCode.LeftAlt);
			
			if (isCtrl && isShift && isAlt && Input.GetKeyDown(key))
			{
				isOn = !isOn;
				OnSingleAction.Invoke();
				OnToggleAction.Invoke(isOn);
			}
		}
	}
}
