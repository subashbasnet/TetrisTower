using UnityEngine;
using System.Collections;

public class startScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Switch to game-screen after 2 seconds
		Invoke ("startgame", 2f);
	}
	
	// Update is called once per frame
	void Update () {
		//Switch to game-screen at click
		if (Input.GetMouseButtonDown (0)) {
						Invoke ("startgame", 0.5f);
				}
	}
	//Switch to game-screen
	void startgame() {
				Application.LoadLevel("gamescene");
		}
}
