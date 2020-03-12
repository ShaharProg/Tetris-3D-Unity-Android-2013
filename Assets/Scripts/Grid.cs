using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
	public GameObject cube;
	GameObject _camera3D = null;
	public static readonly int WIDTH = 6;
	public static readonly int HEIGHT = 6;
	public static readonly int DEPTH = 20;
	public int player_score = 0;
	public GameObject[,,] grid;
	public bool gameOver = false;
	bool gameOverAnim = false;
	public bool comPlay = false;
	public bool pause = false;
	bool enterName = false;
	int gameType = 0;
	public string stringToEdit;
	
	// Use this for initialization
	void Start ()
	{
		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
		grid = new GameObject[WIDTH, DEPTH, HEIGHT];
			
		//_camera2D = GameObject.Find("D2Camera");
		_camera3D = GameObject.Find ("D3Camera");
		//_camera2D.camera.enabled = false;
		_camera3D.camera.enabled = true;
		
		
		gameType = PlayerPrefs.GetInt ("gameType");
		if (gameType == 1) {
			comPlay = true;
		} else {
			comPlay = false;
		}
		

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (gameOver) {			
			Vector3 camPos = _camera3D.transform.position;
			if (camPos.y > (-18) && !gameOverAnim)
				_camera3D.transform.position = new Vector3 (camPos.x, (float)(camPos.y - 0.3), camPos.z);
			else if (!gameOverAnim) {
				GameObject shapeMaker = GameObject.Find ("ShapeMaker");
				var shapeMakerScript = shapeMaker.GetComponent<ShapeMakerScript> ();
				Destroy (shapeMakerScript.lastShape);
				StartCoroutine (GameOverAnim ());
				gameOverAnim = true;
			}
			
		} 
		if (Input.GetKeyDown (KeyCode.Escape))
			pause = !pause;

	}
	
	void OnGUI ()
	{
		
		int menuWidth = (int)(Screen.width * 0.6);
		int menuHeight = (int)(Screen.height * 0.7);
		int xStart = Screen.width / 2 - menuWidth / 2;
		int yStart = (int)(Screen.height * 0.2);
		int xOfsset = (int)(menuWidth * 0.2);
		int yOfsset = (int)(Screen.height * 0.1);
//		GameObject grid = GameObject.Find ("Grid");
//		var gridScript = grid.GetComponent<Grid> ();
		//GUI.Box (new Rect (0, 0, menuWidth * 0.45f, yOfsset), "max:" + gridScript.maxHeight ());	
		GUI.Box (new Rect (0, 0, menuWidth * 0.45f, yOfsset), "Player Score: " + player_score);
		if (gameType == 2) {
			if (GUI.Button (new Rect (25, 50, menuWidth * 0.6f, yOfsset), comPlay ? "Computer Control" : "Player Control")) {
				comPlay = !comPlay;
				if (comPlay) {
					GameObject shapeMaker = GameObject.Find ("ShapeMaker");
					var shapeMakerScript = shapeMaker.GetComponent<ShapeMakerScript> ();
					var shape = shapeMakerScript.lastShape;
		
					var kaka = shape.AddComponent<PathFinder> ();
					kaka.cube = shapeMakerScript.cube;
				}
					
			}
		}
		
		if(!pause && enterName){
			        stringToEdit = GUI.TextField(new Rect((float)xStart + xOfsset, (float)(yStart + yOfsset * 3), menuWidth * 0.6f, yOfsset), stringToEdit, 25);
					if (GUI.Button (new Rect ((float)xStart + xOfsset, (float)(yStart + yOfsset * 4), menuWidth * 0.6f, yOfsset), "Submit Score")) {
						//GameObject HScont = GameObject.Find ("HSController");
						//var HScontScript = HScont.GetComponent<Grid> ();
						StartCoroutine(HSController.PostScores(stringToEdit,player_score));
						enterName = false;
			}
			
		}
		if (pause) {
			GUI.Box (new  Rect (xStart, yStart, menuWidth, menuHeight), "Game Paused");
			if (GUI.Button (new Rect ((float)xStart + xOfsset, (float)yStart + yOfsset, menuWidth * 0.6f, yOfsset), "Resume Game")) {
				//ResetGame();
				pause = false;
			}
			
			if (GUI.Button (new Rect ((float)xStart + xOfsset, (float)(yStart + yOfsset * 2.5), menuWidth * 0.6f, yOfsset), "Restart Game")) {
				//ResetGame();
				Application.LoadLevel ("MainScene");
			}
		
			if (GUI.Button (new Rect ((float)xStart + xOfsset, (float)(yStart + yOfsset * 4), menuWidth * 0.6f, yOfsset), "Exit to main menu")) {
				Application.LoadLevel ("MainMenuScene");
			}
			
			if (GUI.Button ( new Rect(Screen.width-105,5,100,60), "Sound: ON/OFF")) {
				GameObject sound = GameObject.Find("Korobeiniki3");
				if(sound.audio.mute)
				{
					sound.audio.mute = false;
				}
				else
				{
					sound.audio.mute = true;
				}
			}

		}
	}
	
	IEnumerator GameOverAnim ()
	{
		for (int j=DEPTH - 1; j>0; j--) {
			for (int i=0; i<WIDTH; i++) {
				for (int k=0; k<HEIGHT; k++) {
					if (grid [i, j, k] == null) {
						var c = (GameObject)Instantiate (cube, new Vector3 (i, -j, -k), transform.rotation);
						setColorByDepth (c);
					}
				}
			}
			yield return new WaitForSeconds((float)0.1);
			Vector3 camPos = _camera3D.transform.position;
			if (camPos.y < 18) {
				_camera3D.transform.position = new Vector3 (camPos.x, (float)(camPos.y + 2), camPos.z);
			}
			//yield return new WaitForSeconds((float)0.1);
		}
		StartCoroutine (GameOverMat ());
		enterName = true;
		yield return null;
	}
	
	void ResetGame ()
	{	
		for (int i=0; i<WIDTH; i++) {
			for (int j=0; j<DEPTH; j++) {
				for (int k=0; k<HEIGHT; k++) {
					Destroy (grid [i, j, k]);
					grid [i, j, k] = null;
				}
			}
		}
		grid = new GameObject[WIDTH, DEPTH, HEIGHT];
		player_score = 0;
	}
	
	public void EnterShapToGrid (GameObject shape)
	{
		var shapeScript = shape.GetComponent<Shape> ();
		
		if (shapeScript.GetMaxYCord () >= 0) {
			gameOver = true;
			return;
		}
		
		foreach (GameObject c in shapeScript.cubes) {
			int x = (int)c.transform.position.x;
			int y = (int)c.transform.position.y;
			int z = (int)c.transform.position.z;
			
			setColorByDepth (c);
			//Debug.Log("("+x+","+-y+","+-z+")");
			grid [x, -y, -z] = c;
		}
		
		player_score += 10;
	}
	
	void setColorByDepth (GameObject c)
	{
		int y = (int)c.transform.position.y;
		
		switch (-y) {
		case 19:
			c.renderer.material.color = new Color (1, 0, 0);
			break;
		case 18:
			c.renderer.material.color = new Color (0, 1, 0);
			break;
		case 17:
			c.renderer.material.color = new Color (0, 0, 1);
			break;
		case 16:
			c.renderer.material.color = new Color (1, 0, 1);
			break;
		case 15:
			c.renderer.material.color = new Color (1, 1, 0);
			break;
		case 14:
			c.renderer.material.color = new Color (0, 1, 1);
			break;
		case 13:
			c.renderer.material.color = new Color (1, 0.0784f, 0.5764f);
			break;
		case 12:
			c.renderer.material.color = new Color (1, 0, 0);
			break;
		case 11:
			c.renderer.material.color = new Color (0, 1, 0);
			break;
		case 10:
			c.renderer.material.color = new Color (0, 0, 1);
			break;
		case 9:
			c.renderer.material.color = new Color (1, 0, 0);
			break;
		case 8:
			c.renderer.material.color = new Color (0, 1, 0);
			break;
		case 7:
			c.renderer.material.color = new Color (0, 0, 1);
			break;
		case 6:
			c.renderer.material.color = new Color (1, 0, 0);
			break;
		case 5:
			c.renderer.material.color = new Color (0, 1, 0);
			break;
		case 4:
			c.renderer.material.color = new Color (0, 0, 1);
			break;
		case 3:
			c.renderer.material.color = new Color (1, 0, 0);
			break;
		case 2:
			c.renderer.material.color = new Color (0, 1, 0);
			break;
		case 1:
			c.renderer.material.color = new Color (0, 0, 1);
			break;
		case 0:
			c.renderer.material.color = new Color (1, 1, 1);
			break;
		}
	}
	
	public int maxHeight ()
	{
		int counter = 0;
		for (int i=DEPTH; i<DEPTH; i++) {
			for (int j=0; j<HEIGHT; j++) {
				for (int k=0; k<WIDTH; k++) {
					if (grid [k, i, j] != null) {
						j = HEIGHT;
						counter = 0;
						break;
					} else {
						counter++;
						if (counter == HEIGHT * WIDTH)
							return i;
					}
				}
			}
		}
		return 0;
	}

	public void ChackRowsComplete ()
	{
		List<int> completeRows = new List<int> ();
		int counter = 0;
		
		for (int i=0; i<DEPTH; i++) {
			for (int j=0; j<HEIGHT; j++) {
				for (int k=0; k<WIDTH; k++)
					if (grid [k, i, j] != null) {
						counter++;
					}
			}
			if (counter == WIDTH * HEIGHT) {
				completeRows.Add (i);
				player_score += 100;
			}
			counter = 0;
		}
		 
		foreach (int i in completeRows) {
			for (int j=0; j<WIDTH; j++) {
				for (int k=0; k<HEIGHT; k++) {
					Destroy (grid [j, i, k]);
					grid [j, i, k] = null;
				}
			}
			for (int i2=i-1; i2>0; i2--) {
				for (int j2=0; j2<WIDTH; j2++) {
					for (int k2=0; k2<HEIGHT; k2++) {
						if (grid [j2, i2, k2] != null) { //&& grid[j2,i2,k2].transform.position.y > -i)
							grid [j2, i2, k2].transform.position = new Vector3 (grid [j2, i2, k2].transform.position.x, grid [j2, i2, k2].transform.position.y - 1, grid [j2, i2, k2].transform.position.z);
							grid [j2, i2 + 1, k2] = grid [j2, i2, k2];
							grid [j2, i2, k2] = null;
							setColorByDepth (grid [j2, i2 + 1, k2]);
						}
					}
				}
			}
		}
		
	}
	//				 				 --------10---------,---------20--------,--------30---------,-40-
	IEnumerator GameOverMat ()
	{//              1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1
		int [,] mat = new int [13, 41]{{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
									  {0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,1,1,1,1,1,1,1,1,1,0},
						   			  {0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,1,0,1,0,0,0,1,0,1,0,1,0,0,0,0,0,0,0,0,0},	
						   			  {0,1,0,0,0,1,1,1,1,1,0,0,0,1,1,1,1,1,0,0,0,1,0,0,0,1,0,0,0,1,0,1,1,1,1,1,1,1,1,1,0},
						   			  {0,1,0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0},	
						   			  {0,1,1,1,1,1,1,1,0,0,0,1,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,1,0,1,1,1,1,1,1,1,1,1,0},
						   			  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
						   			  {0,0,1,1,1,1,1,1,1,0,0,1,0,0,0,0,0,0,0,1,0,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,0,0},
						   			  {0,1,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0},
						   			  {0,1,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,1,1,1,1,1,1,1,1,0,1,0,1,1,1,1,1,0,0,0},
						   			  {0,1,0,0,0,0,0,0,0,1,0,0,0,0,1,0,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0},	
						   			  {0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,1,0,0,0,0,0,0,0,1,0},
						   			  {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};
		
		Color col = new Color (Random.Range (0, 2), Random.Range (0, 2), Random.Range (0, 2));
		List<GameObject> cubes = new List<GameObject> ();
		for (int i=12; i>=0; i--)
			for (int j=40; j>=0; j--)
				if (mat [i, j] == 1) {
					var c = (GameObject)Instantiate (cube, new Vector3 ((float)(j - 17), 10, 3 - i), transform.rotation);
					c.renderer.material.color = col;
					cubes.Add (c);
				}
		
		while (cubes[cubes.Count-1].transform.position.y > 1) {
			foreach (GameObject c in cubes) {
				c.transform.position = new Vector3 (c.transform.position.x, c.transform.position.y - 1, c.transform.position.z);
			}
			yield return new WaitForSeconds((float)0.1);
		}
	

//																 *******       *     *       * ********* 
//																 *            * *    * *   * * *
//																 *   *****   *****   *   *   * *********
//																 *     *    *     *  *       * *
//																 *******   *       * *       * *********
//																  
//																  *******  *       * ********* ********
//																 *       *  *     *  *         *      *
//																 *       *   *   *   ********* * *****
//																 *       *    * *    *         *      *
//																  *******      *     ********* *       * 
									
	}
}