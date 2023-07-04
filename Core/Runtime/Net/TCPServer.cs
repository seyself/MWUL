using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Text.RegularExpressions;

namespace App.Net 
{
	public class TCPServer : MonoBehaviour
	{
		public delegate void TCPServerTextHandler(string text);
		public delegate void TCPServerByteHandler(byte[] bytes);
		public TCPServerTextHandler OnReceiveMessage;
		public TCPServerByteHandler OnReceiveBytes;

		[SerializeField] public string host = "*";
		[SerializeField] public int port = 55555;
		[HideInInspector] public int receiveTimeout = int.MaxValue;
		[SerializeField] public bool autoOpen = false;
		[SerializeField] public bool useMainThread = true;
		[SerializeField] public bool receiveStringMessage = true;
		[SerializeField] public bool outputLog = false;
		[SerializeField] public int receiveBufferSize = 8192;

		ManualResetEvent _tcpClientConnected = new ManualResetEvent(false);
		TcpListener _tcpListener;
		TcpClient _tcpClient;
		Thread _thread;
		bool _isOpen;
		List<string> _receiveText = new List<string>();
		List<byte[]> _receiveBytes = new List<byte[]>();

		static readonly string EMPTY_BYTE_STRING = Encoding.UTF8.GetString(new byte[]{0}, 0, 1);

		void Start() 
		{
			if (autoOpen)
			{
				Open(host, port);
			}
		}

		public void Open() 
		{
			Open(host, port);
		}

		// ソケット接続準備、待機
		public void Open(string host, int port) 
		{
			this.host = host;
			this.port = port;
			if (host == "*" || host == "" || host == "any" || host == null) 
			{
				_tcpListener = new TcpListener(IPAddress.Any, port);
			}
			else
			{
				_tcpListener = new TcpListener(IPAddress.Parse(host), port);
			}
			_tcpListener.Start();
			Debug.Log("TCP Server Start");

			_isOpen = true;
			_thread = new Thread( ReceiveThread );
			_thread.IsBackground = true;
			_thread.Start();
		}

		void ReceiveThread()
		{
			while( _isOpen && _tcpListener != null )
			{
				try
				{
					_tcpClientConnected.Reset();
					_tcpListener.BeginAcceptSocket(new System.AsyncCallback(AcceptCallback), _tcpListener);
					_tcpClientConnected.WaitOne();
				}
				catch(System.Threading.ThreadAbortException e)
				{
					Debug.LogWarning(e);
					Debug.Log("TCPServer / Threadを中断しました。");
				}
				catch(System.Exception e)
				{
					Debug.LogError(e);
				}
			}
		}

		void AcceptCallback(System.IAsyncResult asyncResult)
		{
			Debug.Log("TCP Server AcceptCallback");
			_tcpClientConnected.Set();
			TcpListener tcpListener = (TcpListener)asyncResult.AsyncState;
			Socket socketClient = tcpListener.EndAcceptSocket(asyncResult);
			
			var state = new StateObject(receiveBufferSize);
        	state.ClientSocket = socketClient;
			socketClient.ReceiveBufferSize = receiveBufferSize;
			socketClient.BeginReceive(state.Buffer, 0, state.BufferSize, 0, new System.AsyncCallback(ReceiveCallback), state);
		}

		void ReceiveCallback(System.IAsyncResult asyncResult)
		{
			Debug.Log("TCP Server ReceiveCallback");
			var state = asyncResult.AsyncState as StateObject;
			var clientSocket = state.ClientSocket;

			int bufferSize = clientSocket.EndReceive(asyncResult);

			if (bufferSize > 0)
			{
				if (receiveStringMessage)
				{
					ReadTextBuffer(state.Buffer);
				}
				else
				{
					ReadByteBuffer(state.Buffer);
				}


				if (!ClientTimeout(clientSocket))
				{
					clientSocket.BeginReceive(state.Buffer, 0, state.BufferSize, 0, new System.AsyncCallback(ReceiveCallback), state);
				}
			}
			else
			{
				Debug.Log("TCP Server Socket close");
				clientSocket.Close();
			}
		}

		void ReadTextBuffer(byte[] buffer) 
		{
			string text = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
			text = Regex.Replace( text, EMPTY_BYTE_STRING + "+$", "" );
			
			if (useMainThread)
			{
				_receiveText.Add( text );
			}
			else
			{
				if (OnReceiveMessage != null) OnReceiveMessage(text);
			}
			if (outputLog) Debug.Log("TCP Server Received : " + text);
		}

		void ReadByteBuffer(byte[] buffer) 
		{
			if (useMainThread)
			{
				_receiveBytes.Add( buffer );
			}
			else
			{
				if (OnReceiveBytes != null) OnReceiveBytes(buffer);
			}
		}

		bool ClientTimeout(Socket client) 
		{
			if (client.Poll(1000, SelectMode.SelectRead) && (client.Available == 0)) 
			{
				client.Close();
				return true;
			}
			return false;
		}

		void Update ()
		{
			if (!useMainThread) return;

			if (receiveStringMessage)
			{
				while (_receiveText.Count > 0)
				{
					string text = _receiveText[0];
					_receiveText.RemoveAt(0);
					if (OnReceiveMessage != null) OnReceiveMessage(text);
				}
			}
			else
			{
				while (_receiveBytes.Count > 0)
				{
					int num = _receiveBytes.Count; 
					byte[] bytes = _receiveBytes[num - 1];
					for(int i=0; i<num; i++) _receiveBytes.RemoveAt(0);
					if (OnReceiveBytes != null) OnReceiveBytes(bytes);
				}
			}
		}

		public void Close()
		{
			if (!_isOpen) return;
			Debug.Log("TCP Server Close");
			_isOpen = false;
			if (_thread != null)
			{
				_thread.Abort();
				_thread = null;
			}
			if (_tcpListener != null)
			{
				_tcpListener.Stop();
				_tcpListener = null;
			}
			if (_tcpClient != null)
			{
				_tcpClient.Close();
				_tcpClient = null;
			}
		}

		void OnDisable()
		{
			Close();
		}

		void OnDestroy()
		{
			Close();
		}

		void OnApplicationQuit() 
		{
			Close();
		}

		class StateObject
		{
			public Socket ClientSocket { get; set; }
			public int BufferSize = 1024;
			public byte[] Buffer { get; }

			public StateObject(int BufferSize)
			{
				this.BufferSize = BufferSize;
				Buffer = new byte[BufferSize];
			}
		}
	}

	
}
