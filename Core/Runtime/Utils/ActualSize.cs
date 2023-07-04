using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class ActualSize
	{
		public static float GetScale(SpriteRenderer spriteRenderer, Camera camera)
		{
			float ppu = spriteRenderer.sprite.pixelsPerUnit;
            float f = camera.fieldOfView * Mathf.PI / 180;
            float d = (spriteRenderer.transform.position - camera.transform.position).magnitude;
            float s = (Mathf.Tan(f / 2) * d * ppu) / Screen.height * 2;
            return s;
		}

		public static float GetPosition(SpriteRenderer spriteRenderer, Camera camera)
		{
			float ppu = spriteRenderer.sprite.pixelsPerUnit;
			float f = camera.fieldOfView * Mathf.PI / 180;
			float z = -(Screen.height / 2 / ppu) / Mathf.Tan(f / 2);
			return z;
		}
	}
}