using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine;

namespace App.API
{
	public class WSEcho : WebSocketBehavior
	{
		protected override void OnMessage(MessageEventArgs e)
		{
			Debug.Log(e.Data);
			var message = e.Data;
			// 送信してきた相手にそのまま同じメッセージを返す
			Send(message);
		}
	}
}

