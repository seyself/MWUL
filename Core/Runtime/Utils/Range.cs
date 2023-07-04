using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public struct Range
	{
		public float min;
		public float max;

		public float size { get { return max - min; } }

		public Range(float min, float max)
		{
			this.min = min;
			this.max = max;
		}
	}
}