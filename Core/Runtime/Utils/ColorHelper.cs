using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class ColorHelper
	{
		public ColorHelper()
		{
			
		}

		public static Color ToColor(string hexColor)
		{
			uint color = System.Convert.ToUInt32(hexColor, 16);
			Color _color = new Color();
			_color.a = 1;
			_color.r = (float)((color >> 16) & 0xFF) / 255f;
			_color.g = (float)((color >> 8) & 0xFF) / 255f;
			_color.b = (float)((color >> 0) & 0xFF) / 255f;
			return _color;
		}
		
		public static Color FromHexString(string hexColor)
		{
			uint color = System.Convert.ToUInt32(hexColor, 16);
			Color _color = new Color();
			_color.a = 1;
			_color.r = (float)((color >> 16) & 0xFF) / 255f;
			_color.g = (float)((color >> 8) & 0xFF) / 255f;
			_color.b = (float)((color >> 0) & 0xFF) / 255f;
			return _color;
		}
	}
}