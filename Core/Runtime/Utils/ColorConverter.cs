using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class ColorConverter
	{

		public static Color ToColor(Color32 color) 
		{
			return new Color(
				(float)color.r / 255.0f,
				(float)color.g / 255.0f,
				(float)color.b / 255.0f,
				(float)color.a / 255.0f
			);
		}

		public static Color32 ToColor32(Color color) 
		{
			return new Color32(
				(byte)Mathf.RoundToInt(color.r * 255),
				(byte)Mathf.RoundToInt(color.g * 255),
				(byte)Mathf.RoundToInt(color.b * 255),
				(byte)Mathf.RoundToInt(color.a * 255)
			);
		}
	}
}