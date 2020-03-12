using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shape : MonoBehaviour
{
	
	public List<GameObject> cubes = new List<GameObject> ();
	public static readonly int MaxShapeSize = 4;
	GameObject grid;
	public bool IsMoving = true;
	public bool IsStop = false;
	public bool IsRightEmpty = true;
	public bool IsLeftEmpty = true;
	public bool IsForwardEmpty = true;
	public bool IsBackwardEmpty = true;
	public float speed;
	public Vector3 rotation = new Vector3 (0, 0, 0);
	public int rotCount = 0;
	Vector3 center;
	public static int SENSITIVITY = 30;
//    string vector;
	List<Vector2> touches;
	int tapCount;
	
	// Use this for initialization
	void Start ()
	{
		grid = GameObject.Find ("Grid");
		
		
		speed = 0.8f;
		touches = new List<Vector2> ();
		tapCount = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
//		GameObject grid = GameObject.Find ("Grid");
	var gridScript = grid.GetComponent<Grid> ();
//		
		if (rotCount != 0) {
			if (rotCount == 5)
				center = new Vector3 ((GetMaxXCord () + GetMinXCord ()) / 2, (GetMaxYCord () + GetMinYCord ()) / 2, (GetMaxZCord () + GetMinZCord ()) / 2);
			rotCount--;
			Rotate (rotation);
		}
		
		if (!gridScript.pause && !gridScript.gameOver) {
			if (!IsStop) {
				if (GetMinYCord () <= -(Grid.DEPTH - 1)) {
					Stop ();
				}
			
				if (IsMoving) {
					StartCoroutine (AutoMove ());
				}
			}		
			if (!IsStop && !(gridScript.comPlay)) {
				KeyHandler ();
			}
		}
	}
	
	void KeyHandler ()
	{
		var touch = Input.GetTouch (0);

		if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) {
			touches.Add (new Vector2 (touch.position.x, touch.position.y));
		} else if (touch.phase == TouchPhase.Ended) {
//            vector = "" + touches.Count;

			if (touches.Count <= 5)
				tapInput ();
			else if (touches.Count > 5)
				swipeInput ();
			touches = new List<Vector2> ();

		}

	}

	void tapInput ()
	{
		if (touches [0].y > Screen.height * 0.75) {
			if (GetMaxZCord () < 0)
				MoveForward ();
		} else if (touches [0].y < Screen.height * 0.25) {
			if (GetMinZCord () > ((-Grid.HEIGHT) + 1))
				MoveBackward ();
		} else if (touches [0].x > 2 * Screen.width / 3) {
			if (GetMaxXCord () < Grid.WIDTH - 1)
				MoveRight ();
		} else if (touches [0].x < Screen.width / 3) {
			if (GetMinXCord () > 0)
				MoveLeft ();
		} else if (tapCount == 0)
			tapCount = 5;
		else if (tapCount > 0)
			speed = 0.01f;
	}

	void swipeInput ()
	{
		Vector2 refVec = touches [touches.Count - 1] - touches [0];
		Vector2 midVec = (touches [touches.Count - 1] + touches [0]) / 2;
		if (Mathf.Abs (refVec.x) > Screen.width / 10) {
			if (Mathf.Abs (touches [touches.Count / 2].y - midVec.y) > SENSITIVITY)
				circularSwipeX ();
			else
				straightSwipeX ();

		} else if (Mathf.Abs (refVec.y) > Screen.height / 3) {
			if (Mathf.Abs (touches [touches.Count / 2].x - midVec.x) > SENSITIVITY)
				circularSwipeY ();
			else
				straightSwipeY ();
		}
	}

	void circularSwipeX ()
	{
		if (touches [touches.Count - 1].x > touches [0].x) {
			if (touches [touches.Count / 2].y > touches [0].y)
				rotation = new Vector3 (0, 1, 0);
			else
				rotation = new Vector3 (0, -1, 0);
		} else {
			if (touches [touches.Count / 2].y > touches [0].y)
				rotation = new Vector3 (0, -1, 0);
			else
				rotation = new Vector3 (0, 1, 0);
		}
		rotCount = 5;		
	}

	void circularSwipeY ()
	{
		if (touches [touches.Count - 1].y > touches [0].y) {
			if (touches [touches.Count / 2].x > touches [0].x)
				rotation = new Vector3 (0, -1, 0);
			else
				rotation = new Vector3 (0, 1, 0);
		} else {
			if (touches [touches.Count / 2].x > touches [0].x)
				rotation = new Vector3 (0, 1, 0);
			else
				rotation = new Vector3 (0, -1, 0);
		}
		rotCount = 5;
	}

	void straightSwipeY ()
	{
		if (touches [touches.Count - 1].y > touches [0].y)
			rotation = new Vector3 (1, 0, 0);
		else
			rotation = new Vector3 (-1, 0, 0);
		rotCount = 5;
	}

	void straightSwipeX ()
	{
		if (touches [touches.Count - 1].x > touches [0].x)
			rotation = new Vector3 (0, 0, -1);
		else
			rotation = new Vector3 (0, 0, 1);
		rotCount = 5;
	}
		
	public void Rotate (Vector3 rotation, int rotAng = 18)// 90/5 = 18, rotating 18 degrees 5 for animation
	{
		//Vector3 center = new Vector3 ((GetMaxXCord () + GetMinXCord ()) / 2, (GetMaxYCord () + GetMinYCord ()) / 2, (GetMaxZCord () + GetMinZCord ()) / 2);

		foreach (GameObject c in cubes) {
			c.transform.RotateAround (center, rotation, rotAng);
			//c.transform.position = new Vector3 (Mathf.Round (c.transform.position.x), Mathf.Round (c.transform.position.y - 1), Mathf.Round (c.transform.position.z));
		}
		if (rotCount == 0) {
			if (GetMaxXCord () > Grid.WIDTH - 1) {
				int maxX = GetMaxXCord ();
				foreach (GameObject c in cubes) {
					c.transform.position = new Vector3 (c.transform.position.x - (maxX - (Grid.WIDTH - 1)), c.transform.position.y, c.transform.position.z);
				}
			}
			if (GetMinXCord () < 0) {
				int minX = -GetMinXCord ();
				foreach (GameObject c in cubes) {
					c.transform.position = new Vector3 (c.transform.position.x + minX, c.transform.position.y, c.transform.position.z);
				}
			}
			if (GetMaxZCord () > 0) {
				int maxZ = GetMaxZCord ();



				foreach (GameObject c in cubes) {
					c.transform.position = new Vector3 (c.transform.position.x, c.transform.position.y, c.transform.position.z - maxZ);
				}
			}
			if (GetMinZCord () < (-Grid.HEIGHT + 1)) {
				int minZ = GetMinZCord ();



				foreach (GameObject c in cubes) {
					c.transform.position = new Vector3 (c.transform.position.x, c.transform.position.y, c.transform.position.z - (minZ - (-Grid.HEIGHT + 1)));
				}
			}
//		foreach (GameObject c in cubes) {
//			c.transform.position = new Vector3 (c.transform.position.x, c.transform.position.y + 1, c.transform.position.z);
//		}




			foreach (GameObject c in cubes) {
				//c.transform.RotateAround (center, rotation, 90/5);
				c.transform.position = new Vector3 (Mathf.Round (c.transform.position.x), Mathf.Round (c.transform.position.y), Mathf.Round (c.transform.position.z));
			}
		}
	}
	
	public void Stop ()
	{
		if (!IsStop) {
			IsStop = true;
			IsMoving = false;
			
			GameObject grid = GameObject.Find ("Grid");
			var gridScript = grid.GetComponent<Grid> ();
			gridScript.EnterShapToGrid (this.gameObject);
			gridScript.ChackRowsComplete ();
			
			foreach (GameObject c in cubes) {
				Destroy (c.particleSystem);
				//Destroy (c.collider);
			}
			if (!gridScript.gameOver) {

				GameObject shapeMaker = GameObject.Find ("ShapeMaker");
				var shapeMakerScript = shapeMaker.GetComponent<ShapeMakerScript> ();
				shapeMakerScript.CreateRandomShape ();
			}

			//var pathFinderScript = grid.GetComponent<PathFinder> ();
		
			//pathFinderScript.comPlayed = false;
		}
	}
	
	public int GetMinYCord ()
	{
		int min = (int)cubes [0].transform.position.y;
		foreach (GameObject c in cubes) {
			min = (min < (int)c.transform.position.y) ? min : (int)c.transform.position.y;
		}
		return min;
	}

	public int GetMaxYCord ()
	{
		int max = (int)cubes [0].transform.position.y;
		foreach (GameObject c in cubes) {
			max = (max > (int)c.transform.position.y) ? max : (int)c.transform.position.y;
		}
		return max;
	}

	public int GetMinXCord ()
	{
		int min = (int)cubes [0].transform.position.x;
		foreach (GameObject c in cubes) {
			min = (min < (int)c.transform.position.x) ? min : (int)c.transform.position.x;
		}
		return min;
	}

	public int GetMaxXCord ()
	{
		int max = (int)cubes [0].transform.position.x;
		foreach (GameObject c in cubes) {
			max = (max > (int)c.transform.position.x) ? max : (int)c.transform.position.x;
		}
		return max;
	}
	
	public int GetMinZCord ()
	{
		int min = (int)cubes [0].transform.position.z;
		foreach (GameObject c in cubes) {
			min = (min < (int)c.transform.position.z) ? min : (int)c.transform.position.z;
		}
		return min;
	}

	public int GetMaxZCord ()
	{
		int max = (int)cubes [0].transform.position.z;
		foreach (GameObject c in cubes) {
			max = (max > (int)c.transform.position.z) ? max : (int)c.transform.position.z;
		}
		return max;
	}
	
	IEnumerator AutoMove ()
	{
		IsMoving = false;
		MoveDown ();
		yield return new WaitForSeconds(speed);
		IsMoving = true;
	}
	
	bool MoveForward ()
	{
		if (IsForwardEmpty && upWallCheck ()) {
			foreach (GameObject c in cubes) {
				c.transform.position = new Vector3 (c.transform.position.x, c.transform.position.y, c.transform.position.z + 1);
			}
			return true;
		}
		return false;
	}
	
	bool MoveBackward ()
	{
		if (IsBackwardEmpty && downWallCheck ()) {
			foreach (GameObject c in cubes) {
				c.transform.position = new Vector3 (c.transform.position.x, c.transform.position.y, c.transform.position.z - 1);
			}
			return true;
		}
		return false;
	}

	bool MoveRight ()
	{
		if (IsRightEmpty && rightWallCheck ()) {
			foreach (GameObject c in cubes) {
				c.transform.position = new Vector3 (c.transform.position.x + 1, c.transform.position.y, c.transform.position.z);
			}
			return true;
		}
		return false;
	}

	bool MoveLeft ()
	{
		if (IsLeftEmpty && leftWallCheck ()) {
			foreach (GameObject c in cubes) {
				c.transform.position = new Vector3 (c.transform.position.x - 1, c.transform.position.y, c.transform.position.z);
			}
			return true;
		}
		return false;
	}
	
	void MoveDown ()
	{
		foreach (GameObject c in cubes) {
			c.transform.position = new Vector3 (c.transform.position.x, c.transform.position.y - 1, c.transform.position.z);
		}
	}

	public void moveToPos (Vector3 newPos)
	{
		//Vector3 center = new Vector3 ((GetMaxXCord () + GetMinXCord ()) / 2, (GetMaxYCord () + GetMinYCord ()) / 2, (GetMaxZCord () + GetMinZCord ()) / 2);
		Vector3 centerOfLeftUp = new Vector3 (GetMinXCord (), (GetMaxYCord () + GetMinYCord ()) / 2, GetMaxZCord ());
		
		while (centerOfLeftUp.x < newPos.x) {	
			if (!MoveRight ())
				break;
			//	foreach (GameObject c in cubes) {
			//		c.transform.position = new Vector3 (c.transform.position.x + 1, c.transform.position.y, c.transform.position.z);
			
			//center = new Vector3 ((GetMaxXCord () + GetMinXCord ()) / 2, (GetMaxYCord () + GetMinYCord ()) / 2, (GetMaxZCord () + GetMinZCord ()) / 2);
			centerOfLeftUp = new Vector3 (GetMinXCord (), (GetMaxYCord () + GetMinYCord ()) / 2, GetMaxZCord ());
		}
		
		while (centerOfLeftUp.x > newPos.x) {	
			if (!MoveLeft ())
				break;
			//foreach (GameObject c in cubes) {
			//	c.transform.position = new Vector3 (c.transform.position.x - 1, c.transform.position.y, c.transform.position.z);
			//}
			
			//center = new Vector3 ((GetMaxXCord () + GetMinXCord ()) / 2, (GetMaxYCord () + GetMinYCord ()) / 2, (GetMaxZCord () + GetMinZCord ()) / 2);
			centerOfLeftUp = new Vector3 (GetMinXCord (), (GetMaxYCord () + GetMinYCord ()) / 2, GetMaxZCord ());
		}
		
		
		while (centerOfLeftUp.z < newPos.z) {	
			if (!MoveForward ())
				break;
			//foreach (GameObject c in cubes) {
			//	c.transform.position = new Vector3 (c.transform.position.x, c.transform.position.y, c.transform.position.z + 1);
			//}
			//center = new Vector3 ((GetMaxXCord () + GetMinXCord ()) / 2, (GetMaxYCord () + GetMinYCord ()) / 2, (GetMaxZCord () + GetMinZCord ()) / 2);
			centerOfLeftUp = new Vector3 (GetMinXCord (), (GetMaxYCord () + GetMinYCord ()) / 2, GetMaxZCord ());
		}
		
		while (centerOfLeftUp.z > newPos.z) {
			if (!MoveBackward ())
				break;
			//foreach (GameObject c in cubes) {
			//	c.transform.position = new Vector3 (c.transform.position.x, c.transform.position.y, c.transform.position.z - 1);
			//}
			//center = new Vector3 ((GetMaxXCord () + GetMinXCord ()) / 2, (GetMaxYCord () + GetMinYCord ()) / 2, (GetMaxZCord () + GetMinZCord ()) / 2);
			centerOfLeftUp = new Vector3 (GetMinXCord (), (GetMaxYCord () + GetMinYCord ()) / 2, GetMaxZCord ());
			;
		}
	}
	
	bool rightWallCheck ()
	{
		foreach (GameObject c in cubes) {
			if ((c.transform.position.x + 1) >= Grid.WIDTH)
				return false;
		}
		return true;
	}

	bool leftWallCheck ()
	{
		foreach (GameObject c in cubes) {
			if ((c.transform.position.x - 1) < 0)
				return false;
		}
		return true;
	}

	bool upWallCheck ()
	{
		foreach (GameObject c in cubes) {
			if ((-(c.transform.position.z + 1)) >= Grid.HEIGHT)
				return false;
		}
		return true;
	}
	
	bool downWallCheck ()
	{
		foreach (GameObject c in cubes) {
			if ((-(c.transform.position.z - 1)) < 0)
				return false;
		}
		return true;
	}










































































































}
