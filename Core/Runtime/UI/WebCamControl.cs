using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App 
{
	public class WebCamControl : MonoBehaviour
	{
		[SerializeField] RawImage _output;
		[SerializeField] int _width  = 1920;
		[SerializeField] int _height = 1080;
		[SerializeField] Dropdown _devices;
		[SerializeField] GameObject _uiContainer;

		WebCamTexture _webCamTexture = null;

		private IEnumerator Start()
		{
			if( WebCamTexture.devices.Length == 0 )
			{
				Debug.LogWarning( "!! Not Camera Device" );
				yield break;
			}

			yield return Application.RequestUserAuthorization( UserAuthorization.WebCam );
			if( !Application.HasUserAuthorization( UserAuthorization.WebCam ) )
			{
				Debug.LogWarning( "!! Unusable Camera Device" );
				yield break;
			}

			int len = WebCamTexture.devices.Length;
			List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
			for(int i=0; i<len; i++)
			{
				options.Add( new Dropdown.OptionData(WebCamTexture.devices[i].name) );
			}
			_devices.AddOptions(options);
		}

		public void OnStartCamera()
		{
			try
			{
				WebCamDevice userCameraDevice = WebCamTexture.devices[ _devices.value ];
				_webCamTexture = new WebCamTexture( userCameraDevice.name, _width, _height );
				_output.texture = _webCamTexture;
				_webCamTexture.Play();

				_uiContainer.SetActive(false);
			}
			catch(System.Exception error)
			{
				Debug.LogError(error);
			}
		}

		void OnApplicationQuit()
		{
			if (_webCamTexture != null)
			{
				_webCamTexture.Stop();
				Destroy(_webCamTexture);
				_webCamTexture = null;
			}
		}
	}
}
