using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class InputHelper : MonoBehaviour
	{
		void Start ()
		{
			
		}

		void Update ()
		{
			
		}

		public static bool ShiftKey(bool keyDownOnly=false)
		{
			if (keyDownOnly) return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
			return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		}

		public static bool AltKey(bool keyDownOnly=false)
		{
			if (keyDownOnly) return Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt);
			return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
		}

		public static bool ControlKey(bool keyDownOnly=false)
		{
			if (keyDownOnly) return Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);
			return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		}

		public static bool CommandKey(bool keyDownOnly=false)
		{
			if (keyDownOnly) return Input.GetKeyDown(KeyCode.LeftCommand) || Input.GetKeyDown(KeyCode.RightCommand);
			return Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);
		}
	}
}
