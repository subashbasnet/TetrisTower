using UnityEngine;
using System.Collections;

public class gameoverscript : MonoBehaviour {

	public GameObject scoreText;
	// Use this for initialization
	void Start () {
		//Show final Score
		scoreText.guiText.text="" + GameScript.scoreObject;
	}
	
	void Update () {
		//Switch to game-screen if clicked
		if (Input.GetMouseButtonDown (0)) {
			Invoke ("startgame", 0.5f);
		}
	}
	//Switch to game-screen
	void startgame() {
		Application.LoadLevel("gamescene");
	}
}
