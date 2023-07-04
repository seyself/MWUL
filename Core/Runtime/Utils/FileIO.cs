using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;

public class FileIO
{
	public FileIO()
	{
		
	}

	public static int FileCount(string directoryPath, string searchPattern)
	{
		return Find(directoryPath, searchPattern).Length;
	}
	
	public static string[] Find(string filePath, string searchPattern)
	{
		return Directory.GetFiles(filePath, searchPattern, SearchOption.AllDirectories);
	}

	public static bool ExistsDirectory(string directoryPath)
	{
		return Directory.Exists(directoryPath);
	}

	public static DirectoryInfo CreateDirectory(string directoryPath)
	{
		return Directory.CreateDirectory(directoryPath);
	}

	public static bool Exists(string filePath)
	{
		return File.Exists(filePath);
	}
	
	public static void RemoveFile(string filePath)
	{
		File.Delete(filePath);
	}
	
	public static void RemoveDirectory(string targetDirectoryPath)
	{
		if (!Directory.Exists(targetDirectoryPath))
		{
			Debug.Log("Directoryが無い");
			return;
		}

		//ディレクトリ以外の全ファイルを削除
		string[] filePaths = Directory.GetFiles(targetDirectoryPath);
		foreach (string filePath in filePaths)
		{
			File.SetAttributes(filePath, FileAttributes.Normal);
			File.Delete(filePath);
		}

		//ディレクトリの中のディレクトリも再帰的に削除
		string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
		foreach (string directoryPath in directoryPaths)
		{
			RemoveDirectory(directoryPath);
		}

		//中が空になったらディレクトリ自身も削除
		Directory.Delete(targetDirectoryPath, false);
	}
	
	public static string GetFileName(string filePath)
	{
		return System.IO.Path.GetFileName(filePath);
	}

	public static void WriteText(string path, string text, bool append=false)
	{
		// Thread thread = new Thread(()=>{
		// 	StreamWriter sw = new StreamWriter(path, append);
		// 	sw.WriteLine(text);
		// 	sw.Flush();
		// 	sw.Close();
		// });
		// thread.IsBackground = true;
		// thread.Start();

		using (StreamWriter sw = new StreamWriter(path, append))
		{
			sw.WriteLine(text);
		}
		// sw.WriteLine(text);
		// sw.Flush();
		// sw.Close();
	}

	public static string ReadText(string path)
	{
		string text = null;
		using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
		{
			using (StreamReader reader = new StreamReader(fileStream))
			{
				text = reader.ReadToEnd();
			}
		}
		return text;
	}

	public static void WriteBinary(string path, byte[] bytes)
	{
		Thread thread = new Thread(() =>
		{
			File.WriteAllBytes(path, bytes);
		});
		thread.IsBackground = true;
		thread.Start();
	}

	public static byte[] ReadBinary(string path)
	{
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		BinaryReader bin = new BinaryReader(fileStream);
		byte[] bytes = bin.ReadBytes((int)bin.BaseStream.Length);
		bin.Close();
		fileStream.Close();
		return bytes;
	}

	public static void WriteImage(string path, Texture2D texture, string fileType=null)
	{
		if (fileType != null)
		{
			fileType = fileType.ToLower();
		}
		bool isJPEG = fileType == "jpeg" || fileType == "jpg" || path.Contains(".jpg");
		if (isJPEG)
		{
			byte[] bytes = ImageConversion.EncodeToJPG(texture);
			WriteBinary(path, bytes);
		}
		else
		{
			byte[] bytes = ImageConversion.EncodeToPNG(texture);
			WriteBinary(path, bytes);
		}
	}

	public static Texture2D ReadImage(string path)
	{
		byte[] bytes = ReadBinary( path );
		return BinaryToTexture( bytes );
	}
	
	public static Texture2D BinaryToTexture(byte[] bytes)
	{
		Texture2D tex = new Texture2D(2, 2);
		ImageConversion.LoadImage(tex, bytes);
		return tex;
	}

	public static bool IsPNG(byte[] bytes)
	{
		if (bytes[1] != 0x50) return false;
		if (bytes[2] != 0x4E) return false;
		if (bytes[3] != 0x47) return false;
		return true;
	}

	public static bool IsJPEG(byte[] bytes)
	{
		if (bytes[6] != 0x4A) return false;
		if (bytes[7] != 0x46) return false;
		if (bytes[8] != 0x49) return false;
		if (bytes[9] != 0x46) return false;
		return true;
	}
	
	public static void Serialize<T>(string path, T obj)
	{
		Serialize<T>(path, obj, false);
	}

	public static void Serialize<T>(string path, T obj, bool prettyPrint)
	{
		string json = JsonUtility.ToJson(obj, prettyPrint);
		WriteText(path, json);
	}

	public static T Deserialize<T>(string path)
	{
		try 
		{
			string json = ReadText(path);
			return JsonUtility.FromJson<T>(json);
		}
		catch (System.Exception e) 
		{
			Debug.LogWarning(e);
		}
		return default(T);
	}
}