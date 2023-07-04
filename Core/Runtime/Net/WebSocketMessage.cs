using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine;


namespace App 
{
	[System.Serializable]
	public class WebSocketMessage
	{
		public string type;
		public string uid;
		public string text;
		public float value;
	}
}