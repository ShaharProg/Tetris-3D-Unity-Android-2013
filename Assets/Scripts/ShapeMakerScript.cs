using UnityEngine;
using System.Collections;

public class ShapeMakerScript : MonoBehaviour {
	
	public GameObject cube;
	public GameObject lastShape;
	// Use this for initialization
	void Start () {
		CreateRandomShape();

	}
	
	public void CreateRandomShape()
	{
		int rnd =  Random.Range(0,7);
		
		var nextShape = (GameObject)Instantiate(cube,new Vector3(0,10,0),transform.rotation );
		
		nextShape.renderer.material.color = new Color(1f, 1f, 1f, 0.5f);
		nextShape.transform.localScale = new Vector3(0,0,0);
		
		var nextShapeScript = nextShape.gameObject.AddComponent<Shape>();
		
		switch(rnd)
		{
		case 0: // square
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(1,1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(1,0,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,0,-2),transform.rotation ));
			break;
		case 1: // line
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,0,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,-1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,-2,-2),transform.rotation ));
			break;
		case 2: // z
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(1,1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,0,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(3,0,-2),transform.rotation ));
			break;
		case 3: // mirror - z
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(3,1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,0,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(1,0,-2),transform.rotation ));
			break;
		case 4: // L
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,0,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,-1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(3,-1,-2),transform.rotation ));
			break;
		case 5: // mirror - L
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,0,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,-1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(1,-1,-2),transform.rotation ));
			break;
		case 6: // pluse
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,1,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(1,0,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(2,0,-2),transform.rotation ));
			nextShapeScript.cubes.Add( (GameObject)Instantiate(cube,new Vector3(3,0,-2),transform.rotation ));
			break;
		}
		
		
		foreach(GameObject c in nextShapeScript.cubes)
		{
			c.renderer.material.color = nextShape.renderer.material.color;
			c.gameObject.tag = "cube";
			var cubeScript = c.AddComponent<Cube>();
			cubeScript.super = nextShape;
		}
		
		lastShape = nextShape;
		GameObject grid = GameObject.Find ("Grid");
		var gridScript = grid.GetComponent<Grid> ();
		
		if(gridScript.comPlay){
		var kaka = lastShape.AddComponent<PathFinder>();
		kaka.cube = cube;
		}
	}
}
