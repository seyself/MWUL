using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class WordChecker
	{
		public static string HIRA = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんがぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽヴぁぃぅぇぉゃゅょっ";
		public static string KANA = "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲンガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポヴァィゥェォャュョッ";
		public static string LARGE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public static string SMALL = "abcdefghijklmnopqrstuvwxyz";

		string[] _bgWords = new string[]{};

		public WordChecker()
		{
			_bgWords = NGWord.words;
		}

		public bool IsOK(string text)
		{
			return Check(text);
		}

		public bool HasNgWord(string text)
		{
			return Check(text) == false;
		}

		public string Convert(string text)
		{
			int len = text.Length;
			string text2 = "";
			for(int i=0; i<len; i++)
			{
				string str = text.Substring(i, 1);
				int n = KANA.IndexOf(str);
				if (n >= 0)
				{
					str = HIRA.Substring(n, 1);
				}
				n = LARGE.IndexOf(str);
				if (n >= 0)
				{
					str = SMALL.Substring(n, 1);
				}
				text2 += str;
			}
			return text2;
		}

		public bool Check(string text)
		{
			string text2 = Convert(text);
			bool isOK = true;
			int len = _bgWords.Length;
			for(int i=0; i<len; i++)
			{
				if ( text2.IndexOf(_bgWords[i]) >= 0)
				{
					isOK = false;
					break;
				}
			}
			return isOK;
		}
	}
}