using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TouchCtrl : MonoBehaviour
{
	public Transform touchTransform;


	public float scaleMin = 1;
	public float scaleMax = 2;
	public float scaleSpeed = 0.005f;
	public float rotateSpeed = 1f;
	public float arObjDistance = 16f;

	private Vector3 offset;
	private Vector3 screenPoint;
	private bool move;
	private float lastDist;
	private float scale;

	// Use this for initialization
	void Start()
	{
		Reset();
	}

	public bool hasRayCast(out Vector3 hitPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 11000.0f))
		{
			Debug.DrawLine(ray.origin, hit.point, Color.green);
			MeshRenderer[] renders = touchTransform.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < renders.Length; i++)
			{
				if (renders[i].bounds.IntersectRay(ray))
				{
					hitPosition = renders[i].transform.position;
					return true;
				}
			}
		}
		hitPosition = Vector3.zero;
		return false;
	}


	public void ResetARObjectFar(float far)
	{

		arObjDistance = far;

		Reset();

	}











	public void Reset()
	{
		touchTransform.position = Camera.main.transform.position + Camera.main.transform.forward * arObjDistance;
		touchTransform.LookAt(Camera.main.transform);
		touchTransform.Rotate(new Vector3(Camera.main.transform.eulerAngles.x, 0, 0));
		touchTransform.localScale = Vector3.one;
 

	}
	Vector3 curPosition = Vector3.zero;
	void OnGUI()
	{




	}






	// Update is called once per frame
	void Update()
	{







		if (Input.GetMouseButtonDown(0))
		{
		
            screenPoint = Camera.main.WorldToScreenPoint(touchTransform.transform.position);
            offset = touchTransform.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
      
		}
		//&& Input.touchCount == 1
        if (Input.GetMouseButton(0)&& Input.touchCount == 2&&Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
		{
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            touchTransform.transform.position = new Vector3(curPosition.x, touchTransform.transform.position.y, curPosition.z);
            //touchTransform.transform.position = new Vector3(curPosition.x,touchTransform.transform.position.y,curPosition.y);
		}



	
			if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.touchCount == 1)
			{
				float x = Input.GetAxis("Mouse X");
				float y = Input.GetAxis("Mouse Y");

				if (Mathf.Abs(x) > Mathf.Abs(y))
				{
					if (x > 0f)
					{
						touchTransform.RotateAround(touchTransform.position, -Vector3.up, rotateSpeed);
					}
					if (x < 0)
					{

						touchTransform.RotateAround(touchTransform.position, Vector3.up, rotateSpeed);
					}

				}


			}

		

		if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
		{


			Touch touch = Input.GetTouch(0);
			Touch touch2 = Input.GetTouch(1);
			float curDist = Vector2.Distance(touch.position, touch2.position);
			if (curDist < lastDist+2)
			{
				scale = touchTransform.localScale.x;
				scale -= scaleSpeed;
			}
			if (curDist > lastDist+2)
			{
				scale = touchTransform.localScale.x;
				scale += scaleSpeed;
			}


            if (scale > scaleMax)
            {
                scale = scaleMax;
            }
            if (scale< scaleMin)
            {
                scale = scaleMin;
            }

			touchTransform.localScale = new Vector3(scale, scale, scale);
			lastDist = curDist;


		}






	}
}
