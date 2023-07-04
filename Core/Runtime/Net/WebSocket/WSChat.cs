using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine;

namespace App.API
{
	public class WSChat : WebSocketBehavior
	{
		private static WSChat _instance;
		public static WSChat instance
		{
			get { return _instance; }
		}

		private Queue<string> messageQueue;

		public WSChat()
		{
			_instance = this;
			messageQueue = new Queue<string>();
		}

		public string GetMessage()
		{
			if (messageQueue == null)
			{
				return null;
			}
			if (messageQueue.Count > 0)
			{
				return messageQueue.Dequeue();
			}
			return null;
		}

		protected override void OnClose(CloseEventArgs e)
		{
			Debug.Log("OnClose: ソケットサーバーの接続が切れた時");
		}

		protected override void OnError(ErrorEventArgs e)
		{
			Debug.Log("OnError: ソケットサーバーの接続でエラーが出た時 : " + e);
		}

		protected override void OnOpen()
		{
			Debug.Log("OnOpen: ソケットサーバーに接続が成功した時");
		}

		protected override void OnMessage (MessageEventArgs e)
		{
			if (messageQueue == null)
			{
				messageQueue = new Queue<string>();
			}

			// メッセージの受信はメインスレッドとは別のスレッドで受け取る。
			// そのままではUnityの他の操作が出来ないので、いったんキューにメッセージを溜めて
			// Updateイベント内で取り出して使うとUnityのオブジェクトに反映できる。
			string reseiveText = e.Data;
			Debug.Log(reseiveText);
			try
			{
				messageQueue.Enqueue(reseiveText);
			}
			catch 
			{
				Debug.Log("エラーが発生しました。");
			}

			// 受け取ったメッセージをそのまま接続しているクライアントにバラ撒く。
			Sessions.Broadcast(reseiveText);
		}
	}
}

