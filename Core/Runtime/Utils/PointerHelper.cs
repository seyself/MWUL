using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App 
{
	public class PointerHelper
	{
		public Camera targetCamera;
		public Collider hitCollider { get; private set; }
		public GameObject hitObject { get; private set; }
		public Vector3 hitPoint { get; private set; }
		public Vector3 localHitPoint { get; private set; }

		bool _pressed = false;
		public bool pressed { get { return _pressed; } }

		public PointerHelper()
		{
			targetCamera = Camera.main;
		}

		public PointerHelper(Camera camera)
		{
			targetCamera = camera;
		}

		public bool GetTouchBegin()
		{
			if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
			{
				_pressed = true;
				return true;
			}
			return false;
		}

		public bool GetTouchEnd()
		{
			if (Input.GetMouseButtonUp(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) 
			{
				_pressed = false;
				return true;
			}
			return false;
		}

		public bool GetTouchMove()
		{
			if (Input.GetMouseButton(0) || (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary))) 
			{
				return true;
			}
			return false;
		}

		public Vector2 GetPosition()
		{
			if (Input.touchCount > 0) return Input.GetTouch(0).position;
			if (Input.GetMouseButton(0)) return Input.mousePosition;
			return Vector2.zero;
		}

		public bool HitTest()
		{
			Ray ray = targetCamera.ScreenPointToRay(GetPosition());
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(ray, out hit)) 
			{
				// Debug.Log("is Hit");
				hitCollider = hit.collider;
				hitObject = hit.collider.gameObject;
				hitPoint = hit.point;
				localHitPoint = hitObject.transform.InverseTransformPoint(hitPoint);
				return true;
			}
			// Debug.Log("not Hit");
			return false;
		}

		public bool HitTestPlane(Vector3 inNormal, Vector3 inPoint)
		{
			return HitTestPlane(new Plane(inNormal, inPoint));
		}

		public bool HitTestPlane(Plane plane)
		{
			Ray ray = targetCamera.ScreenPointToRay(GetPosition());
			// var plane = new Plane(new Vector3(0f, 1f, 0f), 0f);
			float distance;
			if (plane.Raycast(ray, out distance)) 
			{
				Vector3 hit = ray.GetPoint(distance);
				// var point = new Vector2(hit_pos.x, hit_pos.z);
				// hitCollider = hit.collider;
				// hitObject = hit.collider.gameObject;
				hitPoint = hit;
				localHitPoint = hit;
				return true;
				// WaterSurface.Instance.makeBump(ref point, 0.02f /* value */, 1f /* size */);
			}
			return false;
		}

		public Vector3 ToLocal(Transform target)
		{
			return target.InverseTransformPoint(hitPoint);
		}
	}
}
