using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;


namespace App 
{
	public class WebSocketClient : MonoBehaviour 
	{
		public delegate void OnMessageHandler(string message);
		public OnMessageHandler OnMessage;

		public delegate void OnOpenHandler();
		public OnOpenHandler OnOpen;

		public delegate void OnCloseHandler();
		public OnCloseHandler OnClose;

		// 接続先のホスト名（ドメイン or IPアドレスを指定）
		public string host = "localhost";

		// 接続先のポート番号を指定
		public int port = 8443;

		// 接続先のAPIのパスを指定する
		public string api = "";

		// 自動で接続を開始するかどうか
		public bool autoStart = false;

		// ユーザーのID
		public string uid = "";

		WebSocket client;
		public bool isConnected;

		void Start () 
		{
			// ユーザーのユニークなIDを発行する
			uid = System.Guid.NewGuid().ToString();

			if (autoStart)
			{
				Connect();
			}
		}
		
		public void Connect()
		{
			if (client != null)
			{
				Disconnect();
			}

			//接続先URL
			string url = string.Format("wss://{0}:{1}/{2}", host, port, api);
			Debug.Log("接続を開始 : " + url);
			
			client = new WebSocket(url);
			
			// 接続開始時のイベント
			client.OnOpen += (sender, e) =>
			{
				isConnected = true;
				if (OnOpen != null)
				{
					OnOpen();
				}
			};

			// 切断時のイベント
			client.OnClose += (sender, e) =>
			{
				isConnected = false;
				client = null;
				if (OnClose != null)
				{
					OnClose();
				}
			};
			
			// メッセージ受信時のイベント
			client.OnMessage += (sender, e) =>
			{
				if (OnMessage != null)
				{
					string data = e.Data;
					OnMessage( data );
				}
			};

			// 接続を開始する
			client.Connect();
		}

		// ソケットを切断する
		public void Disconnect()
		{
			if (isConnected)
			{
				client.Close();
				client = null;
				isConnected = false;
			}
		}

		// メッセージを送信する
		public void Send(string text)
		{
			if (isConnected)
			{
				WebSocketMessage msg = new WebSocketMessage();
				msg.uid = uid;
				msg.type = "message";
				msg.text = text;

				// メッセージ送信
				client.Send( msg.ToString() );
			}
		}

		void OnDestroy()
		{
			Disconnect();
		}
	}
}
