using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App 
{
	public class NetworkInfo : MonoBehaviour
	{
		//obtained values
		[SerializeField] private string _ipAddress;    //This device ip
		//predefined values
		[SerializeField] private string _priorityIntafaceName;    //もし参照を優先したいインターフェースがあったらココに書く．なければ空白
		//link components
		[SerializeField] private Text _output;
		[SerializeField] private Dropdown _dropdown;
		[SerializeField, Space(15f)] private List<NetworkInterfaceData> _networkInterfaces = new List<NetworkInterfaceData>();

		public List<NetworkInterfaceData> networkInterfaces { get { return _networkInterfaces; } }

		// Use this for initialization
		void Start () 
		{
			if (_output == null) _output = GetComponent<Text>();
			if (_dropdown == null) _dropdown = GetComponent<Dropdown>();

			GetIpAddress();
		}
		
		/// <summary>
		/// Fetch the IP address of this terminal
		/// </summary>
		private void GetIpAddress()
		{
			if (_dropdown) _dropdown.options.Clear();
			_networkInterfaces.Clear();
			//string hostname = Dns.GetHostName();
			NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
			bool isfoundPriorityInterface = false;

			foreach (var ni in nis)
			{
				IPInterfaceProperties ipip = ni.GetIPProperties();
				UnicastIPAddressInformationCollection uipaic = ipip.UnicastAddresses;
				foreach (var uipai in uipaic)
				{
					if (ni.NetworkInterfaceType != NetworkInterfaceType.Loopback
						&& ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel
						&& uipai.Address.AddressFamily.ToString() == "InterNetwork"
						&& ni.Name != "lo"
						&& ni.Name != "lo0"
						&& uipai.Address.ToString().IndexOf("169.254") == -1)
					{
						_networkInterfaces.Add(new NetworkInterfaceData(ni.Name, uipai.Address));

						if (_dropdown) 
						{
							var option = new Dropdown.OptionData(_networkInterfaces[_networkInterfaces.Count - 1].interfaceName);
							_dropdown.options.Add(option);
						}

						//priority interface
						if (ni.Name == _priorityIntafaceName)
						{
							//Debug.Log(uipai.Address);
							_ipAddress = uipai.Address.ToString();
							if (_dropdown) 
							{
								_dropdown.value = _dropdown.options.Count - 1;
							}
							isfoundPriorityInterface = true;
						}
						if (_dropdown) 
						{
							_dropdown.RefreshShownValue();
						}
					}
				}
			}
			
			if (!isfoundPriorityInterface && _networkInterfaces.Count > 0 && _ipAddress == string.Empty)
			{
				if (_dropdown) _dropdown.value = 0;
				_ipAddress = _networkInterfaces[0].ipAddress.ToString();
			}

			if (_output)
			{
				string log = "";
				int len = _networkInterfaces.Count;
				for(int i=0; i<len; i++)
				{
					log += _networkInterfaces[i].interfaceName + " : \n" + _networkInterfaces[i].ipString + "\n\n";
				}
				_output.text = log;
			}
		}

		/// <summary>
		/// Select other network interface by drop down
		/// </summary>
		public void OnUpdatedSelectInterfaceDropdownValue()
		{
			Debug.Log("Select interface : " + _networkInterfaces[_dropdown.value].interfaceName);
			_ipAddress = _networkInterfaces[_dropdown.value].ipString;
		}
	}
}
