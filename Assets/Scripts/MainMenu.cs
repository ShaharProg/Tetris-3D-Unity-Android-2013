using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	
	void OnGUI()
	{
		GUI.skin.button.fontSize =  Screen.height / 23;
		GUI.skin.box.fontSize = Screen.height / 23 + 5;
		
		int menuWidth = (int)(Screen.width*0.6);
		int menuHeight = (int)(Screen.height*0.7);
		int xStart = Screen.width/2 - menuWidth/2;
		int yStart = (int)(Screen.height*0.2);
		int xOfsset = (int)(menuWidth*0.2);
		int yOfsset = (int)(Screen.height*0.1);
		
		if (GUI.Button (new Rect (5,Screen.height-130,100,60), "Show High Score")) {
			Application.LoadLevel ("HighScore");
		}
		
		if (GUI.Button (new Rect (5,Screen.height-65,100,60), "Help")) {
				Application.LoadLevel ("HelpScene");
		}
		if (GUI.Button (new Rect (Screen.width-105,Screen.height-65,100,60), "About")) {
				Application.LoadLevel ("AboutScene");
		}
		
		GUI.Box(new Rect(xStart, yStart,menuWidth, menuHeight), "Main Menu:");
		
		
		
		if(GUI.Button(new Rect((float)xStart+xOfsset, (float)yStart+yOfsset, menuWidth*0.6f, yOfsset), "Start Player Game!"))
		{
			PlayerPrefs.SetInt("gameType",0);
			Application.LoadLevel("MainScene");
		}
		
		if(GUI.Button(new Rect((float)xStart+xOfsset, (float)(yStart+yOfsset*2.5), menuWidth*0.6f, yOfsset), "Start Comp Game!"))
		{
			PlayerPrefs.SetInt("gameType",1);
			Application.LoadLevel("MainScene");
		}
		
		if(GUI.Button(new Rect((float)xStart+xOfsset, (float)(yStart+yOfsset*4), menuWidth*0.6f, yOfsset), "Start Player/Comp Game!"))
		{
			PlayerPrefs.SetInt("gameType",2);
			Application.LoadLevel("MainScene");
		}
		
		if(GUI.Button(new Rect((float)xStart+xOfsset, (float)(yStart+yOfsset*5.5), menuWidth*0.6f, yOfsset), "Exit"))
		{
			Application.Quit();
		}
	}
}
