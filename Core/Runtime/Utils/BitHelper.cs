using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class BitHelper
	{
		public static float ReadFloat(byte[] bytes, ref int index)
		{
			float value = System.BitConverter.ToSingle(bytes, index);
			index += 4;
			if (float.IsNaN(value)) return 0;
			return value;
		}

		public static short ReadInt16(byte[] bytes, ref int index)
		{
			short value = System.BitConverter.ToInt16(bytes, index);
			index += 2;
			return value;
		}

		public static int ReadInt32(byte[] bytes, ref int index)
		{
			int value = System.BitConverter.ToInt32(bytes, index);
			index += 4;
			return value;
		}

		public static long ReadInt64(byte[] bytes, ref int index)
		{
			long value = System.BitConverter.ToInt64(bytes, index);
			index += 8;
			return value;
		}
		
		public static void WriteFloat(byte[] bytes, ref int index, float value)
		{
			if (float.IsNaN(value)) value = 0;
			byte[] num = System.BitConverter.GetBytes(value);
			int n = index;
			bytes[n + 0] = num[0];
			bytes[n + 1] = num[1];
			bytes[n + 2] = num[2];
			bytes[n + 3] = num[3];
			index += 4;
		}

		public static void WriteInt16(byte[] bytes, ref int index, short value)
		{
			byte[] num = System.BitConverter.GetBytes(value);
			int n = index;
			bytes[n + 0] = num[0];
			bytes[n + 1] = num[1];
			index += 2;
		}

		public static void WriteInt32(byte[] bytes, ref int index, int value)
		{
			byte[] num = System.BitConverter.GetBytes(value);
			int n = index;
			bytes[n + 0] = num[0];
			bytes[n + 1] = num[1];
			bytes[n + 2] = num[2];
			bytes[n + 3] = num[3];
			index += 4;
		}

		public static void WriteInt64(byte[] bytes, ref int index, long value)
		{
			byte[] num = System.BitConverter.GetBytes(value);
			int n = index;
			bytes[n + 0] = num[0];
			bytes[n + 1] = num[1];
			bytes[n + 2] = num[2];
			bytes[n + 3] = num[3];
			bytes[n + 4] = num[4];
			bytes[n + 5] = num[5];
			bytes[n + 6] = num[6];
			bytes[n + 7] = num[7];
			index += 8;
		}

		public static byte[] Serialize(object src)
		{
			var memoryStream = new System.IO.MemoryStream();
			var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, src);
			memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
			byte[] bytes = memoryStream.GetBuffer();
			return bytes;
		}

		public static T Deserialize<T>(byte[] bytes)
		{
			var memoryStream = new System.IO.MemoryStream();
			var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			memoryStream.Write(bytes, 0, bytes.Length);
			memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
			return (T)binaryFormatter.Deserialize(memoryStream);
		}

		public static byte[] SerializeVector3Array(Vector3[] src)
		{
			int len = src.Length;
			float[] vArray = new float[len * 3];
			for (int i=0; i<len; i++)
			{
				int k = i * 3;
				vArray[k  ] = src[i].x;
				vArray[k+1] = src[i].y;
				vArray[k+2] = src[i].z;
			}
			return Serialize(vArray);
		}

		public static Vector3[] DeserializeVector3Array(byte[] bytes)
		{
			float[] fArray = Deserialize<float[]>(bytes);
			Vector3[] vArray = new Vector3[fArray.Length / 3];
			int len = vArray.Length;
			for (int i=0; i<len; i++)
			{
				int k = i * 3;
				vArray[i] = new Vector3(fArray[k], fArray[k+1], fArray[k+2]);
			}
			return vArray;
		}
	}
}