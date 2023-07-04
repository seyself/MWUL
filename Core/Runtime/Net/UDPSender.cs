using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace App.Net 
{
	public class UDPSender
	{
		public string host = "172.0.0.1";
		public int port = 5000;

		UdpClient _client;
		IPEndPoint _remoteEndPoint;

		public UDPSender(string host, int port)
		{
			this.host = host;
			this.port = port;
			_remoteEndPoint = new IPEndPoint( IPAddress.Parse(host), port );
			_client = new UdpClient();
		}

		public void SendMessage(string text)
		{
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				_client.Send(bytes, bytes.Length, _remoteEndPoint);
			}
			catch(System.Exception e)
			{
				Debug.LogError(e);
			}
		}

		public static void SendMessage(string text, string host, int port)
		{
			IPEndPoint remoteEndPoint = new IPEndPoint( IPAddress.Parse(host), port );
			SendMessage(text, remoteEndPoint);
		}
		
		public static void SendMessage(string text, IPEndPoint remoteEndPoint)
		{
			Thread thread = new Thread(()=>{
				try
				{
					UdpClient client = new UdpClient();
					byte[] bytes = Encoding.UTF8.GetBytes(text);
					// client.Send(bytes, bytes.Length, remoteEndPoint);
					client.SendAsync(bytes, bytes.Length, remoteEndPoint);
				}
				catch(System.Exception e)
				{
					Debug.LogError(e);
				}
			});
			thread.IsBackground = true;
			thread.Start();
		}

		public static void SendBytes(byte[] bytes, string host, int port)
		{
			IPEndPoint remoteEndPoint = new IPEndPoint( IPAddress.Parse(host), port );
			SendBytes(bytes, remoteEndPoint);
		}
		
		public static void SendBytes(byte[] bytes, IPEndPoint remoteEndPoint)
		{
			Thread thread = new Thread(()=>{
				try
				{
					UdpClient client = new UdpClient();
					// client.Send(bytes, bytes.Length, remoteEndPoint);
					client.SendAsync(bytes, bytes.Length, remoteEndPoint);
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
