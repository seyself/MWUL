using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils 
{
	public class CommandLineArgs
	{
		private static CommandLineArgs _args;

		private Dictionary<string, CommandLineArgument> _dict;

		public static CommandLineArgs GetArgs()
		{
			if (_args == null)
			{
				_args = new CommandLineArgs();
				_args._Init();
			}
			return _args;
		}

		public List<string> Keys {
			get {
				List<string> _keys = new List<string>();

				foreach(string key in _dict.Keys)
				{
					_keys.Add(key);
				}
				return _keys;
			}
		}

		public bool ContainsKey(string key)
		{
			return _dict.ContainsKey(key);
		}

		public bool IsString(string key)
		{
			if (_dict.ContainsKey(key))
			{
				return _dict[key].IsString;
			}
			return false;
		}

		public bool IsFloat(string key)
		{
			if (_dict.ContainsKey(key))
			{
				return _dict[key].IsFloat;
			}
			return false;
		}

		public bool IsInt(string key)
		{
			if (_dict.ContainsKey(key))
			{
				return _dict[key].IsInt;
			}
			return false;
		}

		public bool IsBool(string key)
		{
			if (_dict.ContainsKey(key))
			{
				return _dict[key].IsBool;
			}
			return false;
		}

		public string GetString(string key)
		{
			if (_dict.ContainsKey(key))
			{
				return _dict[key].str;
			}
			return null;
		}

		public float GetFloat(string key)
		{
			if (_dict.ContainsKey(key))
			{
				return _dict[key].f;
			}
			return float.NaN;
		}

		public int GetInt(string key)
		{
			if (_dict.ContainsKey(key))
			{
				return _dict[key].i;
			}
			return 0;
		}

		public bool GetBool(string key)
		{
			if (_dict.ContainsKey(key))
			{
				return _dict[key].b;
			}
			return false;
		}

		private void _Init() 
		{
			_dict = new Dictionary<string, CommandLineArgument>();

			string[] args = System.Environment.GetCommandLineArgs();
			
			string keyName = null;
			for (var i = 1; i < args.Length; i++)
			{
				bool isKey = args[i].IndexOf("-") == 0;
				if (isKey == true)
				{
					if (keyName != null)
					{
						if (keyName.IndexOf("--") == 0) keyName = keyName.Substring(2);
						if (keyName.IndexOf("-") == 0) keyName = keyName.Substring(1);
						_dict.Add( keyName, CommandLineArgument.Create("true") );
						keyName = args[i];
					}
					else
					{
						keyName = args[i];
					}
				}
				else
				{
					string value = args[i];
					if (keyName.IndexOf("--") == 0) keyName = keyName.Substring(2);
					if (keyName.IndexOf("-") == 0) keyName = keyName.Substring(1);
					_dict.Add( keyName, CommandLineArgument.Create(value) );
					keyName = null;
				}
			}

			if (keyName != null)
			{
				if (keyName.IndexOf("--") == 0) keyName = keyName.Substring(2);
				if (keyName.IndexOf("-") == 0) keyName = keyName.Substring(1);
				_dict.Add( keyName, CommandLineArgument.Create("true") );
				keyName = null;
			}
		}

		
	}

	

	class CommandLineArgument 
	{
		public string str;
		public int i;
		public float f;
		public bool b;

		public bool IsString;
		public bool IsInt;
		public bool IsFloat;
		public bool IsBool;

		override public string ToString()
		{
			if (IsInt) return i.ToString();
			if (IsFloat) return f.ToString();
			if (IsBool) return b.ToString();
			return str;
		}

		public static CommandLineArgument Create(string value) 
		{
			CommandLineArgument arg = new CommandLineArgument();

			if ( _IsFloat(value) )
			{
				arg.IsFloat = true;
				arg.f = float.Parse(value);
			}
			else if ( _IsInt(value) ) 
			{
				arg.IsInt = true;
				arg.i = int.Parse(value);
			}
			else if ( _IsBool(value) ) 
			{
				arg.IsBool = true;
				arg.b = bool.Parse(value);
			}
			else 
			{
				arg.IsString = true;
				arg.str = value;
			}
			return arg;
		}

		private static bool _IsBool(string value)
		{
			if (value == "true") return true;
			if (value == "True") return true;
			if (value == "TRUE") return true;
			if (value == "false") return true;
			if (value == "False") return true;
			if (value == "FALSE") return true;
			return false;
		}

		private static bool _IsFloat(string value)
		{
			try 
			{
				if (value.IndexOf(".") >= 0)
				{
					float f = float.Parse(value);
				}
				else
				{
					return false;
				}
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e);
				return false;
			}
			return true;
		}

		private static bool _IsInt(string value)
		{
			if (value == null) return false;
			if (value == "") return false;
			try 
			{
				float f = int.Parse(value);
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e);
				return false;
			}
			return true;
		}

	}
}
