using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
	public GameObject cube;
	//int bachach = 300;
	int rand = 1;
	public bool comPlayed = false;
	//bool bach = true;
	public bool[,,] bestGrid, gridDup;
	List <GameObject> allCubes = new List<GameObject> ();
	//int debugbug = 0;
	Vector3 bestPos = new Vector3 (0, 0, 0);
	Vector3 rotation = new Vector3 (0, 0, 0);
	float penalty = 10000;
	// Use this for initialization
	void Start ()
	{
		comPlayed = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!comPlayed) {
			initPathFinder ();
			comPlayed = true;
		}
	}
	
	void initPathFinder ()
	{
		GameObject shapeMaker = GameObject.Find ("ShapeMaker");
		var shapeMakerScript = shapeMaker.GetComponent<ShapeMakerScript> ();
		var shape = shapeMakerScript.lastShape;
		
							
		var shapeScript = shape.GetComponent<Shape> ();
		//Debug.Log ("maxZ=" + shapeScript.GetMaxZCord() + ", minZ=" + shapeScript.GetMinZCord());
		//Debug.Log ("maxX=" + shapeScript.GetMaxXCord() + ", minX=" + shapeScript.GetMinXCord());
		GameObject grid = GameObject.Find ("Grid");
		var gridScript = grid.GetComponent<Grid> ();
		GameObject [,,] theGrid = gridScript.grid;		
		if (gridScript.comPlay && !comPlayed) {
			RotateAndPathFinder (theGrid, shape);
			shapeScript.moveToPos (bestPos);
			shapeScript.speed = 0.01f;
		}
	}
	
	private void RotateAndPathFinder (GameObject [,,] grid, GameObject shape)
	{
		bestPos = new Vector3 (0, 0, 0);
		penalty = 10000;
		var shapeScript = shape.GetComponent<Shape> ();
		for (int i=0; i<4; i++) {
			shapeScript.Rotate (new Vector3 (1, 0, 0), 90);
//			for (int j=0; j<4; j++) {
//				shapeScript.Rotate (new Vector3 (0, 1, 0));
			for (int k=0; k<4; k++) {
				shapeScript.Rotate (new Vector3 (0, 0, 1), 90);
				tetrisPathFinder (grid, shape, new Vector3 (i, 0, k));
			}
		}
		//printGrid (bestGrid, Grid.DEPTH);
		//Debug.Log ("bach = " + bachach);
		
		for (int i = 0; i<= rotation.x; i++)
			shapeScript.Rotate (new Vector3 (1, 0, 0), 90);
		for (int i = 0; i<= rotation.z; i++)
			shapeScript.Rotate (new Vector3 (0, 0, 1), 90);
	}
	
	private void tetrisPathFinder (GameObject [,,] grid, GameObject shape, Vector3 rotation)
	{
		//grid = giveMeTheGrid()->USonOfABitch()->IHaveThePowerOfThe.Lutra[n].attack().bite();
		var shapeScript = shape.GetComponent<Shape> ();
		int shapeXSize = shapeScript.GetMaxXCord () - shapeScript.GetMinXCord ();// + 1;
		int shapeZSize = Mathf.Abs (shapeScript.GetMinZCord ()) - Mathf.Abs (shapeScript.GetMaxZCord ());// + 1;
		//Debug.Log("shapeXSize = " + shapeXSize + ", shapeZSize = " + shapeZSize);
		int shapeYSize = Mathf.Abs(Mathf.Abs (shapeScript.GetMaxYCord ()) - Mathf.Abs (shapeScript.GetMinYCord ())) + 1;
		for (int i = 0; i< Grid.HEIGHT - shapeXSize; i++) 
			for (int k=0; k< Grid.WIDTH - shapeZSize; k++) {
				//Debug.Log ("i = " + i + ", k = " + k);
				
				bool [,,] boolGrid = gridDuplicate (grid, shapeYSize);
				int localHeight;
				bool [,,] boolShapeGrid = shapeAndGrid (boolGrid, shapeToGrid (shape, i, k), out localHeight);
			
				float imaxHeight = maxHeight (boolShapeGrid);
				float icheckHoles = checkHoles (boolShapeGrid) + checkXHoles (boolShapeGrid) + checkZHoles (boolShapeGrid) - 2 * (i + k) / 3;
				float icheckRowsComplete = chackRowsComplete (boolShapeGrid);
				float tmpPenalty = imaxHeight + localHeight + 3 * icheckHoles / 2 - 2 * icheckRowsComplete;
			
				if (tmpPenalty == penalty)
				if (Random.Range (0, ++rand) != 0)
					break;
				if (tmpPenalty <= penalty) {
					penalty = tmpPenalty;
					bestPos = new Vector3 (i, -maxHeightByPos (boolShapeGrid, i, k), -k);
					this.rotation = rotation;
					bestGrid = boolShapeGrid;
				}
			}
	}
	
	// converts the grid to boolean grid
	private bool [,,] gridDuplicate (GameObject [,,] grid, int shapeHeight)
	{
		int minH = minHeight (grid);
		bool [,,] gridDup = new bool[Grid.WIDTH, Grid.DEPTH, Grid.HEIGHT];
		for (int i = 0; i< Grid.HEIGHT; i++) {
			for (int k=0; k< Grid.WIDTH; k++) {
				//if (minH > shapeSize) {
					for (int j=Grid.DEPTH- 1 - minH; j>=0; j--) {
						//bachach++;
						gridDup [i, j+minH, k] = (grid [i, j, k] != null) ? true : false;
					
//					}
//				} else {
//					for (int j=Grid.DEPTH-1; j>=0; j--) {
//						//bachach++;
//						gridDup [i, j, k] = (grid [i, j, k] != null) ? true : false;
//					}
				}
			}
		}
		//this.gridDup = gridDup;
		return gridDup;
	}
	
	private bool [,,] shapeAndGrid (bool [,,] grid, bool [,,] shapeOnGrid, out int localHeight)// int i, int k)
	{
		List<Vector3> shapeCubes = new List<Vector3> ();
		for (int j = 0; j<Shape.MaxShapeSize; j++) 
			for (int i = 0; i< Grid.HEIGHT; i++) 
				for (int k=0; k< Grid.WIDTH; k++) {
					//bachach++;
					if (shapeOnGrid [i, j, k])
						shapeCubes.Add (new Vector3 (i, j, k));
				}
		int height = 0;
		while (height <= Grid.DEPTH - Shape.MaxShapeSize && 
			!grid[(int)shapeCubes[0].x,(int)shapeCubes[0].y + height,(int)shapeCubes[0].z] &&
			!grid[(int)shapeCubes[1].x,(int)shapeCubes[1].y + height,(int)shapeCubes[1].z] &&
			!grid[(int)shapeCubes[2].x,(int)shapeCubes[2].y + height,(int)shapeCubes[2].z] &&
			!grid[(int)shapeCubes[3].x,(int)shapeCubes[3].y + height,(int)shapeCubes[3].z]) {
			height++;
		}
		foreach (Vector3 vec in shapeCubes) {
			grid [(int)vec.x, (int)vec.y + height - 1, (int)vec.z] = true;
		}
		localHeight = 0;
		foreach (Vector3 vec in shapeCubes) {
			if ((height = maxHeightByPos (grid, (int)vec.x, (int)vec.z)) > localHeight)
				localHeight = height;
		}
		return grid;
	}
	
	private int shapeFirstImapct (bool [,,] grid, int i, int k)
	{
		for (int j = Shape.MaxShapeSize - 1; j>=0; j--) 
			if (grid [i, j, k])
				return  j;
		return 0; 
	}
	
	private int maxHeightByPos (bool [,,] grid, int i, int k)
	{
		for (int j = 0; j< Grid.DEPTH; j++) {
			//	bachach++;
			if (grid [i, j, k])
				return Grid.DEPTH - j - 1;
		}
		return 0;
	}
	
	private int maxHeightByPos (GameObject [,,] grid, int i, int k)
	{
		for (int j = 0; j< Grid.DEPTH; j++) {
			//	bachach++;
			if (grid [i, j, k] != null)
				return Grid.DEPTH - j - 1;
		}
		return 0;
	}
	
	
	// gets a shape and position, and returns a 6*4*6 bool grid (Width,depth,maxShapeSize) with the shape in position
	private bool [,,] shapeToGrid (GameObject shape, int i, int k)
	{
		int tmpOffsetX, tmpOffsetY, tmpOffsetZ, sizeY;
		tmpOffsetX = tmpOffsetY = tmpOffsetZ = sizeY = 0;
		bool[,,] shapeOnGrid = new bool[Grid.WIDTH, Shape.MaxShapeSize, Grid.HEIGHT];
		
		for (int i2 = 0; i2< Grid.WIDTH; i2++)
			for (int k2=0; k2< Grid.HEIGHT; k2++)
				for (int j2=Shape.MaxShapeSize-1; j2>0; j2--) {
					//	bachach++;
					shapeOnGrid [i2, j2, k2] = false;
				}
		
		var shapeScript = shape.GetComponent<Shape> ();
		
		sizeY = shapeScript.GetMaxYCord () - shapeScript.GetMinYCord ();
		
		foreach (GameObject c in shapeScript.cubes) {
			tmpOffsetX = (int)Mathf.Round (c.transform.position.x) - shapeScript.GetMinXCord ();
			tmpOffsetZ = (int)-Mathf.Round (c.transform.position.z) + shapeScript.GetMaxZCord ();
			tmpOffsetY = (int)-Mathf.Round (c.transform.position.y) + shapeScript.GetMaxYCord ();
			//tmpOffsetY2 = shapeScript.GetMaxYCord () - (int)Mathf.Round (c.transform.position.y);

//			if ((i + tmpOffsetX >= 0) && (i + tmpOffsetX < 6) && (k + tmpOffsetZ >= 0) && (k + tmpOffsetZ < 6) && ((Shape.MaxShapeSize - 1) - tmpOffsetY >= 0) && ((Shape.MaxShapeSize - 1) - tmpOffsetY < 4))
			shapeOnGrid [i + tmpOffsetX, (Shape.MaxShapeSize - 1 - sizeY) + tmpOffsetY, k + tmpOffsetZ] = true;
//			else
//				Debug.Log ("i + tmpOffsetX=" + (i + tmpOffsetX) + ", (Shape.MaxShapeSize - 1) - tmpOffsetY=" + ((Shape.MaxShapeSize - 1) - tmpOffsetY) + ", k + tmpOffsetZ=" + (k + tmpOffsetZ));
			//}
		}

		return shapeOnGrid;
	}
	

	
	// checks the depth above the i,j,k for a block
	private bool checkAbove (bool [,,] grid, int i, int j, int k)
	{
		for (; j>0; j--) {
			//bachach++;
			if (grid [i, j, k])
				return true;
		}
		return false;
	}
	
	private bool checkXAbove (bool [,,] grid, int i, int j, int k)
	{
		for (; i< Grid.WIDTH; i++) {
			//bachach++;
			if (grid [i, j, k])
				return true;
		}
		return false;
	}
	
	private bool checkZAbove (bool [,,] grid, int i, int j, int k)
	{
		for (; k< Grid.HEIGHT; k++) {
			//bachach++;
			if (grid [i, j, k])
				return true;
		}
		return false;
	}
	
	
	// returns the number of "holes" on the grid after placing a shape
	public int checkHoles (bool[,,] grid)
	{
		int penalty = 0;
		for (int i= 0; i<Grid.WIDTH; i++) {
			for (int j=0; j<Grid.HEIGHT; j++) {
				for (int k=Grid.DEPTH - 1; k>0; k--) {
					//	bachach++;
					if (!grid [i, k, j]) {
						if (checkAbove (grid, i, k, j)) {
							penalty++;
							break;
						}
					}	
				}
			}
		}
		return penalty;
	}
	
	public int checkXHoles (bool[,,] grid)
	{
		int penalty = 0;
		for (int k=Grid.DEPTH - 1; k>0; k--) {
			for (int j=0; j<Grid.HEIGHT; j++) {
				for (int i= 0; i<Grid.WIDTH; i++) {
					//bachach++;
					if (!grid [i, k, j]) {
						if (checkXAbove (grid, i, k, j)) {
							penalty++;
							break;
						}
					}	
				}
			}
		}
		return penalty;
	}
	
	public int checkZHoles (bool[,,] grid)
	{
		int penalty = 0;
		for (int k=Grid.DEPTH - 1; k>0; k--) {
			for (int i= 0; i<Grid.WIDTH; i++) {
				for (int j=0; j<Grid.HEIGHT; j++) {
					//bachach++;
					if (!grid [i, k, j]) {
						if (checkZAbove (grid, i, k, j)) {
							penalty++;
							break;
						}
					}	
				}
			}
		}
		return penalty;
	}
	
	public int maxHeight (bool[,,] grid)
	{
		int height = 0;
		for (int i =0; i<Grid.WIDTH; i++)
			for (int k =0; k<Grid.WIDTH; k++) {
				int tmpHeight = maxHeightByPos (grid, i, k);
				if (tmpHeight > height)
					height = tmpHeight;
			}
		return height;
	}
	
	public int minHeight (GameObject [,,] grid)
	{
		int height = 0;
		for (int i =0; i<Grid.WIDTH; i++)
			for (int k =0; k<Grid.WIDTH; k++) {
				int tmpHeight = maxHeightByPos (grid, i, k);
				if (tmpHeight < height)
					height = tmpHeight;
			}
		return height;
	}
	
	// returns the number of rows completed after placing a shape
	public int chackRowsComplete (bool [,,] grid)
	{
		int maxCount = 0;
		int counter = 0;
		
		for (int i=0; i<Grid.DEPTH; i++) {
			for (int j=0; j<Grid.HEIGHT; j++) {
				for (int k=0; k<Grid.WIDTH; k++) {
					if (grid [k, i, j]) {
						counter++;
					}
				}
			}
			if (counter > maxCount) {
				maxCount = counter;
			}
			counter = 0;
		}
		return maxCount;
	}
/// 
	private void printGrid (bool [,,] grid, int maxSize)
	{
		
		for (int i = 0; i<Grid.WIDTH; i++) {
			for (int k = 0; k<Grid.HEIGHT; k++) {
				for (int j = 0; j<maxSize - 1; j++) {
					if (grid [i, j, k]) {
						allCubes.Add (Instantiate (cube, new Vector3 (i + 10, -j, -k), transform.rotation) as GameObject);
					}
				}
			}
		}
	}
}
