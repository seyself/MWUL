using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;
using App.API;

namespace App 
{
	public class WebSocketServer : MonoBehaviour 
	{
		public delegate void OnMessageHandler(string message);
		public OnMessageHandler OnMessage;

		public delegate void OnStartHandler();
		public OnStartHandler OnStart;

		public delegate void OnStopHandler();
		public OnStopHandler OnStop;

		// ソケット通信の受け口となるポート番号
		public int port = 55555;

		// 自動で実行を開始するかどうか
		public bool autoStart = false;

		WebSocketSharp.Server.WebSocketServer server;
		bool isStarted;
		
		public bool ServerToggleStart()
		{
			if (isStarted)
			{
				ServerStop();
			}
			else
			{
				ServerStart();
			}
			return isStarted;
		}

		// サーバーを起動する
		public void ServerStart()
		{
			// 接続先のアドレスは
			// ws://<ホスト名>:<ポート番号>/<API名>
			// となります。
			server = new WebSocketSharp.Server.WebSocketServer( port );
			
			// 送受信のテスト用のエコーサーバーAPI
			server.AddWebSocketService<WSEcho>("/echo");
			
			// チャットAPI
			server.AddWebSocketService<WSChat>("/chat");
			
			// サーバーを起動する
			server.Start();

			isStarted = true;
			if (OnStart != null)
			{
				OnStart();
			}
		}

		// サーバーを停止する
		public void ServerStop()
		{
			if (server != null)
			{
				server.Stop();
				server = null;
				isStarted = false;
				if (OnStop != null)
				{
					OnStop();
				}
			}
		}

		void Start ()
		{
			if (autoStart)
			{
				ServerStart();
			}
		}
		
		void Update () 
		{
			if (WSChat.instance != null)
			{
				string msg = WSChat.instance.GetMessage();
				if (msg != null && OnMessage != null)
				{
					//メッセージが溜まっていたら、メッセージ受信イベントを発行する
					OnMessage(msg);
				}
			}
		}

		void OnDestroy()
		{
			ServerStop();
		}

	}
}