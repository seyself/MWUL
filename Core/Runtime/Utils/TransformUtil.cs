using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class TransformUtil
	{
		public TransformUtil()
		{
			
		}
		public enum ForwardAngle {
			X,
			Y,
			Z,
		}
		
		public static Quaternion TwoPointAngleLookAt(Vector3 basePosition, Vector3 lookAtPosition)
		{
			return TwoPointAngleLookAt(basePosition, lookAtPosition, ForwardAngle.Y);
		}

		public static Quaternion TwoPointAngleLookAt(Vector3 basePosition, Vector3 lookAtPosition, ForwardAngle forwardAngle)
		{
			Vector3 a = lookAtPosition;
			Vector3 b = basePosition;
			Vector3 c = a - b;
			if (c.x == 0 && c.y == 0 && c.z == 0) return Quaternion.identity;
			switch(forwardAngle)
			{
				case ForwardAngle.X : return Quaternion.LookRotation(c, c) * Quaternion.Euler(0, -90, 0);
				case ForwardAngle.Y : return Quaternion.LookRotation(c, c) * Quaternion.Euler(90, 0, 0);
				// default /* ForwardAngle.Z */ : 
				// 	return Quaternion.LookRotation(c, c);
			}
			return Quaternion.LookRotation(c, c);
		}

		/// <summary>
		/// Extract translation from transform matrix.
		/// </summary>
		/// <param name="matrix">Transform matrix. This parameter is passed by reference
		/// to improve performance; no changes will be made to it.</param>
		/// <returns>
		/// Translation offset.
		/// </returns>
		public static Vector3 ExtractTranslationFromMatrix(ref Matrix4x4 matrix) 
		{
			// Vector3 translate;
			// translate.x = matrix.m03;
			// translate.y = matrix.m13;
			// translate.z = matrix.m23;
			// return translate;
			return matrix.GetColumn(3);
		}
		
		/// <summary>
		/// Extract rotation quaternion from transform matrix.
		/// </summary>
		/// <param name="matrix">Transform matrix. This parameter is passed by reference
		/// to improve performance; no changes will be made to it.</param>
		/// <returns>
		/// Quaternion representation of rotation transform.
		/// </returns>
		public static Quaternion ExtractRotationFromMatrix(ref Matrix4x4 matrix)
		{
			// Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
            Quaternion q = new Quaternion();
            q.w = Mathf.Sqrt(Mathf.Max(0, 1 + matrix[0, 0] + matrix[1, 1] + matrix[2, 2])) / 2;
            q.x = Mathf.Sqrt(Mathf.Max(0, 1 + matrix[0, 0] - matrix[1, 1] - matrix[2, 2])) / 2;
            q.y = Mathf.Sqrt(Mathf.Max(0, 1 - matrix[0, 0] + matrix[1, 1] - matrix[2, 2])) / 2;
            q.z = Mathf.Sqrt(Mathf.Max(0, 1 - matrix[0, 0] - matrix[1, 1] + matrix[2, 2])) / 2;
            q.x *= Mathf.Sign(q.x * (matrix[2, 1] - matrix[1, 2]));
            q.y *= Mathf.Sign(q.y * (matrix[0, 2] - matrix[2, 0]));
            q.z *= Mathf.Sign(q.z * (matrix[1, 0] - matrix[0, 1]));
            return q;
		}

		public static Quaternion ExtractLookRotationFromMatrix(ref Matrix4x4 matrix)
		{
			Vector3 forward;
			forward.x = matrix.m02;
			forward.y = matrix.m12;
			forward.z = matrix.m22;
		
			Vector3 upwards;
			upwards.x = matrix.m01;
			upwards.y = matrix.m11;
			upwards.z = matrix.m21;
		
			return Quaternion.LookRotation(forward, upwards);
		}
		
		/// <summary>
		/// Extract scale from transform matrix.
		/// </summary>
		/// <param name="matrix">Transform matrix. This parameter is passed by reference
		/// to improve performance; no changes will be made to it.</param>
		/// <returns>
		/// Scale vector.
		/// </returns>
		public static Vector3 ExtractScaleFromMatrix(ref Matrix4x4 matrix) 
		{
			// Vector3 scale;
			// scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
			// scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
			// scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
			// return scale;
			Vector3 scale = new Vector3(matrix.GetColumn(0).magnitude, matrix.GetColumn(1).magnitude, matrix.GetColumn(2).magnitude);
			if (Vector3.Cross (matrix.GetColumn (0), matrix.GetColumn (1)).normalized != (Vector3)matrix.GetColumn (2).normalized) 
			{
				scale.x *= -1;
			}
			return scale;
		}
		
		/// <summary>
		/// Extract position, rotation and scale from TRS matrix.
		/// </summary>
		/// <param name="matrix">Transform matrix. This parameter is passed by reference
		/// to improve performance; no changes will be made to it.</param>
		/// <param name="localPosition">Output position.</param>
		/// <param name="localRotation">Output rotation.</param>
		/// <param name="localScale">Output scale.</param>
		public static void DecomposeMatrix(ref Matrix4x4 matrix, out Vector3 localPosition, out Quaternion localRotation, out Vector3 localScale) 
		{
			localPosition = ExtractTranslationFromMatrix(ref matrix);
			localRotation = ExtractRotationFromMatrix(ref matrix);
			localScale = ExtractScaleFromMatrix(ref matrix);
		}
		
		/// <summary>
		/// Set transform component from TRS matrix.
		/// </summary>
		/// <param name="transform">Transform component.</param>
		/// <param name="matrix">Transform matrix. This parameter is passed by reference
		/// to improve performance; no changes will be made to it.</param>
		public static void SetTransformFromMatrix(Transform transform, ref Matrix4x4 matrix) 
		{
			transform.localPosition = ExtractTranslationFromMatrix(ref matrix);
			transform.localRotation = ExtractRotationFromMatrix(ref matrix);
			transform.localScale = ExtractScaleFromMatrix(ref matrix);
		}
		
		
		// EXTRAS!
		
		/// <summary>
		/// Identity quaternion.
		/// </summary>
		/// <remarks>
		/// <para>It is faster to access this variation than <c>Quaternion.identity</c>.</para>
		/// </remarks>
		public static readonly Quaternion IdentityQuaternion = Quaternion.identity;

		/// <summary>
		/// Identity matrix.
		/// </summary>
		/// <remarks>
		/// <para>It is faster to access this variation than <c>Matrix4x4.identity</c>.</para>
		/// </remarks>
		public static readonly Matrix4x4 IdentityMatrix = Matrix4x4.identity;
		
		/// <summary>
		/// Get translation matrix.
		/// </summary>
		/// <param name="offset">Translation offset.</param>
		/// <returns>
		/// The translation transform matrix.
		/// </returns>
		public static Matrix4x4 TranslationMatrix(Vector3 offset) 
		{
			Matrix4x4 matrix = IdentityMatrix;
			matrix.m03 = offset.x;
			matrix.m13 = offset.y;
			matrix.m23 = offset.z;
			return matrix;
		}

		public static Quaternion LookAt(Vector3 basePosition, Vector3 lookTarget)
		{
			Vector3 z = (lookTarget - basePosition).normalized;
			Vector3 x = Vector3.Cross(Vector3.up, z).normalized;
			Vector3 y = Vector3.Cross(z, x).normalized;

			Matrix4x4 m = Matrix4x4.identity;
			m[0, 0] = x.x; m[0, 1] = y.x; m[0, 2] = z.x;
			m[1, 0] = x.y; m[1, 1] = y.y; m[1, 2] = z.y;
			m[2, 0] = x.z; m[2, 1] = y.z; m[2, 2] = z.z;

			// return GetLookAtRotation(m);
			return ExtractRotationFromMatrix(ref m);
		}

		private static Quaternion GetLookAtRotation(Matrix4x4 m)
		{
			float[] elem = new float[4];
			elem[0] = m.m00 - m.m11 - m.m22 + 1.0f;
			elem[1] = -m.m00 + m.m11 - m.m22 + 1.0f;
			elem[2] = -m.m00 - m.m11 + m.m22 + 1.0f;
			elem[3] = m.m00 + m.m11 + m.m22 + 1.0f;

			int biggestIdx = 0;
			for (int i = 0; i < elem.Length; i++)
			{
				if (elem[i] > elem[biggestIdx])
				{
					biggestIdx = i;
				}
			}

			if (elem[biggestIdx] < 0)
			{
				Debug.Log("Wrong matrix.");
				return new Quaternion();
			}

			float[] q = new float[4];
			float v = Mathf.Sqrt(elem[biggestIdx]) * 0.5f;
			q[biggestIdx] = v;
			float mult = 0.25f / v;

			switch (biggestIdx)
			{
				case 0:
					q[1] = (m.m10 + m.m01) * mult;
					q[2] = (m.m02 + m.m20) * mult;
					q[3] = (m.m21 - m.m12) * mult;
					break;
				case 1:
					q[0] = (m.m10 + m.m01) * mult;
					q[2] = (m.m21 + m.m12) * mult;
					q[3] = (m.m02 - m.m20) * mult;
					break;
				case 2:
					q[0] = (m.m02 + m.m20) * mult;
					q[1] = (m.m21 + m.m12) * mult;
					q[3] = (m.m10 - m.m01) * mult;
					break;
				case 3:
					q[0] = (m.m21 - m.m12) * mult;
					q[1] = (m.m02 - m.m20) * mult;
					q[2] = (m.m10 - m.m01) * mult;
					break;
			}

			return new Quaternion(q[0], q[1], q[2], q[3]);
		}


		// https://www.geeks3d.com/20141201/how-to-rotate-a-vertex-by-a-quaternion-in-glsl/
		public static Quaternion QuaternionMultiplication(Quaternion q1, Quaternion q2)
		{
			Quaternion qr = new Quaternion();
			qr.x = (q1.w * q2.x) + (q1.x * q2.w) + (q1.y * q2.z) - (q1.z * q2.y);
			qr.y = (q1.w * q2.y) - (q1.x * q2.z) + (q1.y * q2.w) + (q1.z * q2.x);
			qr.z = (q1.w * q2.z) + (q1.x * q2.y) - (q1.y * q2.x) + (q1.z * q2.w);
			qr.w = (q1.w * q2.w) - (q1.x * q2.x) - (q1.y * q2.y) - (q1.z * q2.z);
			return qr;
		}

		public static Quaternion QuaternionFromAxisAngle(Vector3 axis, float angle)
		{
			Quaternion qr = new Quaternion();
			float half_angle = (angle * 0.5f) * 3.14159f / 180.0f;
			qr.x = axis.x * Mathf.Sin(half_angle);
			qr.y = axis.y * Mathf.Sin(half_angle);
			qr.z = axis.z * Mathf.Sin(half_angle);
			qr.w = Mathf.Cos(half_angle);
			return qr;
		}

		public static Quaternion QuaternionInverse(Quaternion q)
		{
			return new Quaternion(-q.x, -q.y, -q.z, q.w);
		}

		public static Vector3 RotateVertexPosition(Vector3 position, Vector3 axis, float angle)
		{
			Quaternion qr = QuaternionFromAxisAngle(axis, angle);
			Quaternion qr_conj = QuaternionInverse(qr);
			Quaternion q_pos = new Quaternion(position.x, position.y, position.z, 0);
			
			Quaternion q_tmp = QuaternionMultiplication(qr, q_pos);
			qr = QuaternionMultiplication(q_tmp, qr_conj);
			
			return new Vector3(qr.x, qr.y, qr.z);
		}
	}
}