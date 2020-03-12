using UnityEngine;
using System.Collections;

public class About : MonoBehaviour {
	
	
	public bool pause = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			pause = !pause;
	}
	
		void OnGUI()
	{
		GUI.skin.button.fontSize =  Screen.height / 23;
		GUI.skin.box.fontSize = Screen.height / 23 + 5;
		
		int menuWidth = (int)(Screen.width*0.6);
		int menuHeight = (int)(Screen.height*0.7);
		int xStart = Screen.width/2 - menuWidth/2;
		int yStart = (int)(Screen.height*0.2);
//		int xOfsset = (int)(menuWidth*0.2);
//		int yOfsset = (int)(Screen.height*0.1);
		
		if (GUI.Button (new Rect (5,5,100,60), "Back")) {
				Application.LoadLevel ("MainMenuScene");
		}
		if (pause) {
				Application.LoadLevel ("MainMenuScene");
		}
		
		
		GUI.Box(new Rect(xStart, yStart,menuWidth, menuHeight), "About:\n\n\n\n" +
			"Version: 1.0.0.0.9999999\n\n" +
			"Created by:\n Shahar Zigman & Elad Yosef\n\n");	
	}
}
