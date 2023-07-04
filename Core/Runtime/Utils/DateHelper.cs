using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class DateHelper
	{
		public DateHelper()
		{
			
		}

		public static System.DateTime Now() 
		{
			return System.DateTime.Now;
		}

		public static string ToString(System.DateTime date, int formatType=0)
		{
			switch(formatType)
			{
				case 1 : return date.ToString("yyyy.MM.dd HH:mm");
				case 2 : return date.ToString("yyyy.MM.dd HH:mm:ss");
				case 3 : return date.ToString("yyyy/MM/dd");
				case 4 : return date.ToString("HH:mm:ss");
			}
			return date.ToString("yyyy/MM/dd HH:mm:ss");
		}
	}
}