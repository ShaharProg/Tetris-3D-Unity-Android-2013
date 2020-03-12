using UnityEngine;
using System.Collections;
 
public class HSController : MonoBehaviour
{
	private static string secretKey = "itsTheBachMan"; // Edit this value and make sure it's the same as the one stored on the server
	public static string addScoreURL = "http://tetris3d.webatu.com/Tetris/addscore.php?"; //be sure to add a ? to your url
	public static string highscoreURL = "http://tetris3d.webatu.com/Tetris/display.php";
	
	public string textToDes = "";
 
	void Start ()
	{
		StartCoroutine (GetScores ());
		gameObject.guiText.text = "";
	}
 
	// remember to use StartCoroutine when calling this function!
	public static IEnumerator PostScores (string name, int score)
	{
		//This connects to a server side php script that will add the name and score to a MySQL DB.
		// Supply it with a string representing the players name and the players score.
		string hash = Md5Sum (name + score + secretKey);
 
		string post_url = addScoreURL + "name=" + WWW.EscapeURL (name) + "&score=" + score + "&hash=" + hash;
 
		// Post the URL to the site and create a download object to get the result.
		WWW hs_post = new WWW (post_url);
		yield return hs_post; // Wait until the download is done
 
		if (hs_post.error != null) {
			print ("There was an error posting the high score: " + hs_post.error);
		}
	}
 
	// Get the scores from the MySQL DB to display in a GUIText.
	// remember to use StartCoroutine when calling this function!
	IEnumerator GetScores ()
	{
		//gameObject.guiText.text = "Loading ScoresSt";
		textToDes = "Loading ScoresSt";
		WWW hs_get = new WWW (highscoreURL);
		yield return hs_get;
 
		if (hs_get.error != null) {
			print ("There was an error getting the high score: " + hs_get.error);
		} else {
			string s = hs_get.text;
			//char[] a= "<".ToCharArray();
			s = s.Split('<')[0];
			//gameObject.guiText.text = hs_get.text; // this is a GUIText that will display the scores in game.
			textToDes = s; // this is a GUIText that will display the scores in game.
		}
	}
	
	
	void OnGUI ()
	{
		
		int menuWidth = (int)(Screen.width * 0.6);
		int menuHeight = (int)(Screen.height * 0.7);
		
		GUI.Box (new Rect ((int)(Screen.width * 0.2), (int)(Screen.height * 0.15),menuWidth, menuHeight), "High Scores:\n\n" + textToDes);
	}
	
	
	
	public static string Md5Sum (string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();
		byte[] bytes = ue.GetBytes (strToEncrypt);
 
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
		byte[] hashBytes = md5.ComputeHash (bytes);
 
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
 
		for (int i = 0; i < hashBytes.Length; i++) {
			hashString += System.Convert.ToString (hashBytes [i], 16).PadLeft (2, '0');
		}
 
		return hashString.PadLeft (32, '0');
	}
 
}