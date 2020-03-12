using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {
	
	public GameObject super;
	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "cube")
		{
			var cubeScript = other.GetComponent<Cube>();
			var otherShapeScript = cubeScript.super.GetComponent<Shape>();
			var MyShapeScript = super.GetComponent<Shape>();
			if ( (otherShapeScript.IsStop) && (other.gameObject.transform.position.y < transform.position.y) && (other.gameObject.transform.position.x == transform.position.x)  && (other.gameObject.transform.position.z == transform.position.z) )
			{
				MyShapeScript.Stop();
			}
			if ( (otherShapeScript.IsStop) && (other.gameObject.transform.position.y == transform.position.y ))
			{
				if (other.gameObject.transform.position.x > transform.position.x)
				{
					MyShapeScript.IsRightEmpty = false;
				}
				if (other.gameObject.transform.position.x < transform.position.x)
				{
					MyShapeScript.IsLeftEmpty = false;
				}
				if (other.gameObject.transform.position.z > transform.position.z)
				{
					MyShapeScript.IsForwardEmpty = false;
				}
				if (other.gameObject.transform.position.z < transform.position.z)
				{
					MyShapeScript.IsBackwardEmpty = false;
				}
			}
		}
	}
	
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "cube")
		{
			var cubeScript = other.GetComponent<Cube>();
			var otherShapeScript = cubeScript.super.GetComponent<Shape>();
			var MyShapeScript = super.GetComponent<Shape>();
			if ( (otherShapeScript.IsStop) && (other.gameObject.transform.position.y == transform.position.y ))
			{
				if (other.gameObject.transform.position.x > transform.position.x)
				{
					MyShapeScript.IsRightEmpty = true;
				}
				if (other.gameObject.transform.position.x < transform.position.x)
				{
					MyShapeScript.IsLeftEmpty = true;
				}
				if (other.gameObject.transform.position.z > transform.position.z)
				{
					MyShapeScript.IsForwardEmpty = true;
				}
				if (other.gameObject.transform.position.z < transform.position.z)
				{
					MyShapeScript.IsBackwardEmpty = true;
				}
			}
		}
		
	}
}
