using UnityEngine;
using System.Collections;

public class Help : MonoBehaviour {
	
	public bool pause = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			pause = !pause;
	}
	
	
	void OnGUI ()
	{
		if (GUI.Button (new Rect (5,5,100,60), "Back")) {
				Application.LoadLevel ("MainMenuScene");
		}
		
		
		if (pause) {
				Application.LoadLevel ("MainMenuScene");
		}
	}
	
}
