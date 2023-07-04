using System.Net;
using UnityEngine;

namespace App 
{
	[System.Serializable]
	public class NetworkInterfaceData
	{
		[SerializeField] public string interfaceName;
		[SerializeField] public string ipString;
		[HideInInspector] public IPAddress ipAddress;

		public NetworkInterfaceData(string interfaceName, IPAddress IpAddress)
		{
			this.interfaceName = interfaceName;
			this.ipAddress = IpAddress;
			this.ipString = IpAddress.ToString();
		}

		public NetworkInterfaceData(string interfaceName, string ipString)
		{
			this.interfaceName = interfaceName;
			this.ipString = ipString;
			this.ipAddress = IPAddress.Parse(ipString);
		}
	}
}