using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace App.Net 
{
	public class TCPClient
	{
		public string host = "127.0.0.1";
		public int port = 55555;

		public static int readTimeout = 1000;
		public static int writeTimeout = 1000;
		public static int sendBufferSize = 8192;

		TcpClient _client;
		NetworkStream _stream;

		public TCPClient(string host, int port)
		{
			this.host = host;
			this.port = port;
		}

		public void Open() 
		{
			try
			{
				// _client = new TcpClient(new IPEndPoint(IPAddress.Parse(host), port));
				_client = new TcpClient(host, port);
				_client.Client.SendBufferSize = sendBufferSize;
				_stream = _client.GetStream();
				_stream.ReadTimeout = readTimeout;
				_stream.WriteTimeout = writeTimeout;
			}
			catch(System.Exception e)
			{
				Debug.LogError(e);
			}
		}

		public void Close() 
		{
			try
			{
				_stream.Close();
			}
			catch(System.Exception e)
			{
				Debug.LogError(e);
			}
			
			try
			{
				_client.Close();
			}
			catch(System.Exception e)
			{
				Debug.LogError(e);
			}
		}

		public void SendMessage(string text)
		{
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				SendBytes(bytes);
			}
			catch(System.Exception e)
			{
				Debug.LogError(e);
			}
		}

		public void SendBytes(byte[] bytes)
		{
			try
			{
				_stream.Write(bytes, 0, bytes.Length);
			}
			catch(System.Exception e)
			{
				Debug.LogError(e);
			}
		}

		public static void SendMessage(string text, string host, int port)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			SendBytes(bytes, host, port);
		}

		public static void SendBytes(byte[] bytes, string host, int port)
		{
			Thread thread = new Thread(()=>{
				try
				{
					// TcpClient client = new TcpClient(new IPEndPoint(IPAddress.Parse(host), port));
					TcpClient client = new TcpClient(host, port);
					client.NoDelay = true;
					client.Client.SendBufferSize = sendBufferSize;
					NetworkStream ns = client.GetStream();
					ns.ReadTimeout = readTimeout;
					ns.WriteTimeout = writeTimeout;
					ns.Write(bytes, 0, bytes.Length);
					ns.Flush();
					ns.Close();
					client.Close();
				}
				catch(System.Exception e)
				{
					Debug.LogError(e);
				}
			});
			thread.IsBackground = true;
			thread.Start();
		}
	}
}
