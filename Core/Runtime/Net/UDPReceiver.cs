using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace App.Net 
{
	public class UDPReceiver : MonoBehaviour
	{
		public delegate void UDPReceiverTextHandler(string text);
		public delegate void UDPReceiverHandler(byte[] bytes);
		public UDPReceiverTextHandler OnReceiveMessage;
		public UDPReceiverHandler OnReceiveBytes;

		public int port = 5000;
		public int receiveTimeout = 24 * 60 * 60 * 1000;
		public bool autoOpen = false;
		public bool useMainThread = true;
		public bool receiveStringMessage = true;

		UdpClient _udpClient;
		Thread _thread;
		bool _isOpen;
		List<string> _receiveData = new List<string>();
		List<byte[]> _receiveBytes = new List<byte[]>();

		void Start() 
		{
			if (autoOpen)
			{
				Open(port);
			}
		}

		public void Open() 
		{
			Open(port);
		}

		public void Open(int port) 
		{
			this.port = port;
			_udpClient = new UdpClient(port);
			// _udpClient.Client.ReceiveTimeout = receiveTimeout;

			_isOpen = true;
			_thread = new Thread( ReceiveThread );
			_thread.IsBackground = true;
			_thread.Start();
		}

		void Update ()
		{
			if (receiveStringMessage)
			{
				while (_receiveData.Count > 0)
				{
					string text = _receiveData[0];
					_receiveData.RemoveAt(0);
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

		private void OnApplicationQuit()
		{
			OnDestroy();
		}

		void OnDestroy()
		{
			if (_thread != null)
			{
				_isOpen = false;
				_thread.Abort();
				_thread = null;
			}
			if (_udpClient != null)
			{
				_udpClient.Close();
				_udpClient = null;
			}
		}

		void ReceiveThread()
		{
			while( _isOpen && _udpClient != null )
			{
				try
				{
					IPEndPoint remoteEndPoint = null;
					byte[] data = _udpClient.Receive(ref remoteEndPoint);
					if (receiveStringMessage)
					{
						string text = Encoding.UTF8.GetString(data);
						if (useMainThread)
						{
							_receiveData.Add( text );
						}
						else
						{
							if (OnReceiveMessage != null) OnReceiveMessage(text);
						}
					}
					else
					{
						if (useMainThread)
						{
							_receiveBytes.Add( data );
						}
						else
						{
							if (OnReceiveBytes != null) OnReceiveBytes(data);
						}
					}
				}
				catch(System.Threading.ThreadAbortException e)
				{
					Debug.Log(e);
					Debug.Log("UDPReceiver / Threadを中断しました。");
				}
				catch(System.Exception e)
				{
					Debug.LogError(e);
				}
			}
		}
	}
}
