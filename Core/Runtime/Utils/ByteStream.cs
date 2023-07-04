using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class ByteStream
	{
		public byte[] bytes { get; private set; }
		public int size { get; private set; }
		public int index { get; set; }

		public ByteStream(int size)
		{
			bytes = new byte[size];
			this.size = size;
			this.index = 0;
		}

		public ByteStream(byte[] bytes)
		{
			this.bytes = bytes;
			this.size = bytes.Length;
			this.index = 0;
		}

		public void SetByteArray(byte[] bytes)
		{
			this.bytes = bytes;
			this.size = bytes.Length;
			this.index = 0;
		}

		public void Clear() 
		{
			bytes = new byte[size];
		}

		public void ReadBlank()
		{
			index += 1;
		}

		public bool ReadBool()
		{
			bool value = bytes[index] > 0 ? true : false;
			index += 1;
			return value;
		}

		public float ReadFloat()
		{
			float value = System.BitConverter.ToSingle(bytes, index);
			index += 4;
			if (float.IsNaN(value)) return 0;
			return value;
		}

		public double ReadDouble()
		{
			double value = System.BitConverter.ToDouble(bytes, index);
			index += 8;
			if (double.IsNaN(value)) return 0;
			return value;
		}

		public byte ReadByte()
		{
			byte value = bytes[index];
			index += 1;
			return value;
		}

		public short ReadInt16()
		{
			short value = System.BitConverter.ToInt16(bytes, index);
			index += 2;
			return value;
		}

		public int ReadInt32()
		{
			int value = System.BitConverter.ToInt32(bytes, index);
			index += 4;
			return value;
		}

		public long ReadInt64()
		{
			long value = System.BitConverter.ToInt64(bytes, index);
			index += 8;
			return value;
		}

		public ushort ReadUInt16()
		{
			ushort value = System.BitConverter.ToUInt16(bytes, index);
			index += 2;
			return value;
		}

		public uint ReadUInt32()
		{
			uint value = System.BitConverter.ToUInt32(bytes, index);
			index += 4;
			return value;
		}

		public ulong ReadUInt64()
		{
			ulong value = System.BitConverter.ToUInt64(bytes, index);
			index += 8;
			return value;
		}

		public void WriteBlank()
		{
			bytes[index] = 0;
			index += 1;
		}

		public void WriteBool(bool value)
		{
			bytes[index] = (byte)(value ? 1 : 0);
			index += 1;
		}
		
		public void WriteFloat(float value)
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

		public void WriteDouble(double value)
		{
			if (double.IsNaN(value)) value = 0;
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

		public void WriteByte(byte value)
		{
			bytes[index] = value;
			index += 1;
		}

		public void WriteInt16(short value)
		{
			byte[] num = System.BitConverter.GetBytes(value);
			int n = index;
			bytes[n + 0] = num[0];
			bytes[n + 1] = num[1];
			index += 2;
		}

		public void WriteInt32(int value)
		{
			byte[] num = System.BitConverter.GetBytes(value);
			int n = index;
			bytes[n + 0] = num[0];
			bytes[n + 1] = num[1];
			bytes[n + 2] = num[2];
			bytes[n + 3] = num[3];
			index += 4;
		}

		public void WriteInt64(long value)
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

		public void WriteUInt16(ushort value)
		{
			byte[] num = System.BitConverter.GetBytes(value);
			int n = index;
			bytes[n + 0] = num[0];
			bytes[n + 1] = num[1];
			index += 2;
		}

		public void WriteUInt32(uint value)
		{
			byte[] num = System.BitConverter.GetBytes(value);
			int n = index;
			bytes[n + 0] = num[0];
			bytes[n + 1] = num[1];
			bytes[n + 2] = num[2];
			bytes[n + 3] = num[3];
			index += 4;
		}

		public void WriteUInt64(ulong value)
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

		//============================================================================================

		public void WriteBytes(byte[] value)
		{
			int len = value.Length;
			for(int i=0; i<len; i++)
			{
				bytes[index++] = value[i];
			}
		}

		public byte[] ReadBytes(int len)
		{
			byte[] res = new byte[len];
			for(int i=0; i<len; i++)
			{
				res[i] = bytes[index++];
			}
			return res;
		}

		//============================================================================================

		public void WriteVector2(Vector2 value)
		{
			WriteFloat(value.x);
			WriteFloat(value.y);
		}

		public void WriteVector3(Vector3 value)
		{
			WriteFloat(value.x);
			WriteFloat(value.y);
			WriteFloat(value.z);
		}

		public void WriteVector4(Vector4 value)
		{
			WriteFloat(value.x);
			WriteFloat(value.y);
			WriteFloat(value.z);
			WriteFloat(value.w);
		}

		public void WriteColor(Color value)
		{
			WriteFloat(value.r);
			WriteFloat(value.g);
			WriteFloat(value.b);
			WriteFloat(value.a);
		}

		public void WriteColor32(Color32 value)
		{
			WriteByte(value.r);
			WriteByte(value.g);
			WriteByte(value.b);
			WriteByte(value.a);
		}

		public void WriteQuaternion(Quaternion value)
		{
			WriteFloat(value.x);
			WriteFloat(value.y);
			WriteFloat(value.z);
			WriteFloat(value.w);
		}

		public void WriteMatrix4x4(Matrix4x4 value)
		{
			WriteFloat(value[0]);
			WriteFloat(value[1]);
			WriteFloat(value[2]);
			WriteFloat(value[3]);
			WriteFloat(value[4]);
			WriteFloat(value[5]);
			WriteFloat(value[6]);
			WriteFloat(value[7]);
			WriteFloat(value[8]);
			WriteFloat(value[9]);
			WriteFloat(value[10]);
			WriteFloat(value[11]);
			WriteFloat(value[12]);
			WriteFloat(value[13]);
			WriteFloat(value[14]);
			WriteFloat(value[15]);
		}

		public Vector2 ReadVector2()
		{
			return new Vector2( ReadFloat(), ReadFloat() );
		}

		public Vector3 ReadVector3()
		{
			return new Vector3( ReadFloat(), ReadFloat(), ReadFloat() );
		}

		public Vector4 ReadVector4()
		{
			return new Vector4( ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat() );
		}

		public Color ReadColor()
		{
			return new Color( ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat() );
		}

		public Color32 ReadColor32()
		{
			return new Color32( ReadByte(), ReadByte(), ReadByte(), ReadByte() );
		}

		public Quaternion ReadQuaternion()
		{
			return new Quaternion( ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat() );
		}

		public Matrix4x4 ReadMatrix4x4()
		{
			return new Matrix4x4( 
				new Vector4( ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat() ),
				new Vector4( ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat() ),
				new Vector4( ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat() ),
				new Vector4( ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat() )
			);
		}
	}
}