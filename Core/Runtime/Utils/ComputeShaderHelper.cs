using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace App 
{
	public class ComputeShaderHelper
	{
		public ComputeShader computeShader { get; }
		public Texture inputTexture;
		public RenderTexture outputTexture;

		int textureWidth;
		int textureHeight;
		int bufferSize = 1024 * 1024;

		int _threadSize;
		int _threadGroupX;
		int _threadGroupY;
		int _threadGroupZ;

		int _kernelIndex;
		

		public ComputeShaderHelper(ComputeShader computeShader, string kernelName="CSMain")
		{
			this.computeShader = computeShader;
			_kernelIndex = computeShader.FindKernel( kernelName );
		}

		public void Setup(Texture texture) 
		{
			SetInputTexture(texture);
			CreateOutputTexture();
		}

		public void CreateOutputTexture()
		{
			outputTexture = CreateRenderTexture(textureWidth, textureHeight);
			SetTexture("_OutputTexture", outputTexture);
		}

		public void SetOutputTexture(RenderTexture renderTexture)
		{
			outputTexture = renderTexture;
			SetTexture("_OutputTexture", outputTexture);
		}

		public void SetInputTexture(Texture texture)
		{
			inputTexture = texture;
			textureWidth = inputTexture.width;
			textureHeight = inputTexture.height;
			bufferSize = textureWidth * textureHeight;
			SetThreadSizeFromTexture();
			SetTexture("_InputTexture", inputTexture);
			SetInt("_BufferSize", bufferSize);
			SetInt("_Width", textureWidth);
			SetInt("_Height", textureHeight);
			// Debug.Log($"texSize: {textureWidth} / {textureHeight}");
		}

		public void SetThreadSizeFromTexture()
		{
			_threadSize = Mathf.CeilToInt((float)bufferSize / 1024f);

			uint groupX, groupY, groupZ;
			computeShader.GetKernelThreadGroupSizes(_kernelIndex, out groupX, out groupY, out groupZ);
			_threadGroupX = Mathf.CeilToInt((float)textureWidth / (float)groupX);
			_threadGroupY = Mathf.CeilToInt((float)textureHeight / (float)groupY);
			_threadGroupZ = 1;
			computeShader.SetInt("_ThreadGroupX", _threadGroupX);
			computeShader.SetInt("_ThreadGroupY", _threadGroupY);
			computeShader.SetInt("_ThreadGroupZ", _threadGroupZ);
			// Debug.Log($"ThreadGroup: {_threadGroupX} / {_threadGroupY} / {_threadGroupZ}");
		}

		public void SetThreadSize(int bufferSize)
		{
			this.bufferSize = bufferSize;
			_threadSize = Mathf.CeilToInt((float)bufferSize / 1024f);
			int groups = Mathf.CeilToInt(Mathf.Sqrt(_threadSize));
			_threadGroupX = groups;
			_threadGroupY = groups;
			_threadGroupZ = 1;
			computeShader.SetInt("_ThreadGroupX", _threadGroupX);
			computeShader.SetInt("_ThreadGroupY", _threadGroupY);
			computeShader.SetInt("_ThreadGroupZ", _threadGroupZ);
		}

		public ComputeBuffer CreateBuffer<T>(string name, T[] buffer)
		{
			ComputeBuffer computeBuffer = new ComputeBuffer( buffer.Length, Marshal.SizeOf(typeof(T)) );
			computeBuffer.SetData(buffer);
			computeShader.SetBuffer(_kernelIndex, name, computeBuffer);
			return computeBuffer;
		}

		public ComputeBuffer CreateVec3Buffer(string name, Vector3[] buffer)
		{
			ComputeBuffer computeBuffer = new ComputeBuffer( buffer.Length, Marshal.SizeOf(typeof(Vector3)) );
			computeBuffer.SetData(buffer);
			computeShader.SetBuffer(_kernelIndex, name, computeBuffer);
			return computeBuffer;
		}

		public void DisposeBuffer(ref ComputeBuffer computeBuffer)
		{
			if (computeBuffer != null)
			{
				computeBuffer.Dispose();
				computeBuffer = null;
			}
		}

		public void SetBool(string name, bool val) { computeShader.SetBool(name, val); }
		public void SetBool(int nameID, bool val) { computeShader.SetBool(nameID, val); }
		public void SetFloat(string name, float val) { computeShader.SetFloat(name, val); }
		public void SetFloat(int nameID, float val) { computeShader.SetFloat(nameID, val); }
		public void SetFloats(string name, params float[] values) { computeShader.SetFloats(name, values); }
		public void SetFloats(int nameID, params float[] values) { computeShader.SetFloats(nameID, values); }
		public void SetInt(string name, int val) { computeShader.SetInt(name, val); }
		public void SetInt(int nameID, int val) { computeShader.SetInt(nameID, val); }
		public void SetInts(string name, params int[] values) { computeShader.SetInts(name, values); }
		public void SetInts(int nameID, params int[] values) { computeShader.SetInts(nameID, values); }
		public void SetMatrix(string name, Matrix4x4 val) { computeShader.SetMatrix(name, val); }
		public void SetMatrix(int nameID, Matrix4x4 val) { computeShader.SetMatrix(nameID, val); }
		public void SetMatrixArray(string name, Matrix4x4[] values) { computeShader.SetMatrixArray(name, values); }
		public void SetMatrixArray(int nameID, Matrix4x4[] values) { computeShader.SetMatrixArray(nameID, values); }
		public void SetVector(string name, Vector4 val) { computeShader.SetVector(name, val); }
		public void SetVector(int nameID, Vector4 val) { computeShader.SetVector(nameID, val); }
		public void SetVectorArray(string name, Vector4[] values) { computeShader.SetVectorArray(name, values); }
		public void SetVectorArray(int nameID, Vector4[] values) { computeShader.SetVectorArray(nameID, values); }
        public void SetTexture(string name, Texture texture)
		{
			computeShader.SetTexture(_kernelIndex, name, texture);
		}

		public void SetTexture(int nameID, Texture texture)
		{
			computeShader.SetTexture(_kernelIndex, nameID, texture);
		}

		public void SetBuffer(string name, ComputeBuffer buffer) 
		{
			computeShader.SetBuffer(_kernelIndex, name, buffer);
		}
		
		public void SetBuffer(int nameID, ComputeBuffer buffer)
		{
			computeShader.SetBuffer(_kernelIndex, nameID, buffer);
		}
        

		public void Dispatch()
		{
			computeShader.Dispatch(_kernelIndex, _threadGroupX, _threadGroupY, _threadGroupZ);
		}

		public Texture2D CreateTexture2D(int width, int height) 
		{
			// return new Texture2D(width, height, TextureFormat.ARGB32, 0, false);
			return new Texture2D(width, height, TextureFormat.ARGB32, false);
		}

		public RenderTexture CreateRenderTexture(int width, int height, bool filterPointMode=false) 
		{
			RenderTexture texture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB64);
			texture.name = "GeneratedRenderTexture " + width + "x" + height;
            texture.enableRandomWrite = true;
            texture.useMipMap = false;
            texture.mipMapBias = 0;
            if (filterPointMode)
            {
	            texture.filterMode = FilterMode.Point;
            }
            else
            {
	            texture.filterMode = FilterMode.Trilinear;
            }
            texture.Create();
			return texture;
		}
	}
}