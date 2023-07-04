using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class CursorVisibleControl : MonoBehaviour
	{
		public float waitTimeLimit = 5;
		float _cursorMoveTime = 0;
		Vector3 _mousePosition = Vector3.zero;

		void Start ()
		{
			_cursorMoveTime = Time.time;
		}

		void Update ()
		{
			Vector3 vec3 = Input.mousePosition;
			if (vec3.Equals(_mousePosition) == false)
			{
				_cursorMoveTime = Time.time;
			}
			_mousePosition = Input.mousePosition;

			if (Time.time - _cursorMoveTime >= waitTimeLimit)
			{
				Cursor.visible = false;
			}
			else
			{
				Cursor.visible = true;
			}
		}
	}
}
