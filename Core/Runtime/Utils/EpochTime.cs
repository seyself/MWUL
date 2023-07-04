using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class EpochTime
	{
		private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);	

		/*===========================================================================*/
		/**
		* 現在時刻からUnixTimeを計算する.
		*
		* @return UnixTime.
		*/
		public static long Now()
		{
			return FromDateTime( DateTime.UtcNow );
		}

		/*===========================================================================*/
		/**
		* UnixTimeからDateTimeに変換.
		*
		* @param [in] epochTime 変換したいUnixTime.
		* @return 引数時間のDateTime.
		*/
		public static DateTime FromEpochTime( long epochTime )
		{
			return UNIX_EPOCH.AddMilliseconds( epochTime ).ToLocalTime();
		}

		/*===========================================================================*/
		/**
		* 指定時間をUnixTimeに変換する.
		*
		* @param [in] dateTime DateTimeオブジェクト.
		* @return UnixTime.
		*/
		public static long FromDateTime( DateTime dateTime )
		{
			double nowTicks = ( dateTime.ToUniversalTime() - UNIX_EPOCH ).TotalMilliseconds;
			return (long)nowTicks;
		}
	}
}