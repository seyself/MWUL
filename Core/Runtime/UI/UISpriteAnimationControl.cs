using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App 
{
	public class UISpriteAnimationControl : MonoBehaviour
	{
		[SerializeField] int _fps = 30;
		[SerializeField] Image _image;
		[SerializeField] Sprite[] _sprites;

		int _currentFrameIndex = 0;
		int _totalFrames = 0;
		float _frameTime = 0;
		float _currentTime = 0;
		float _startTime = 0;

		void Start ()
		{
			_totalFrames = _sprites.Length;
			_frameTime = 1f / (float)_fps;
			_startTime = Time.time;

			float tx = 0.0f;
			float ty = 0.0f;
			_image.rectTransform.anchoredPosition = new Vector2(tx, -ty);
		}

		void Update ()
		{
			_currentTime = Time.time - _startTime;
			int index = Mathf.FloorToInt(_currentTime / _frameTime);
			_currentFrameIndex = index % _totalFrames;

			_image.sprite = _sprites[_currentFrameIndex];
		}
	}
}
