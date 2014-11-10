using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/* GameScript manage creation of new Tetris-pieces, 
 * moving down the Platform, 
 * keeping track of Score/Life
 */
public class GameScript : MonoBehaviour {

	//t1-t6: Prefabs of Tetris-Pieces
	public GameObject t1,t2,t3,t4,t5,t6;
	public GameObject lifesText;
	public GameObject scoreText;
	public GameObject platfrom;
	public GameObject background;
	public AudioClip pickupSound;
	//Counter of the Score (Tetris-Pieces on Screen)
	public static int scoreObject = 0;
	//Counter of Lifes
	public int lifes=3;
	//Game Speed
	public float gameSpeed= -0.009f;
	//List of all Tetris-Pieces in game
	private List<GameObject> pieceList;
	//Array of all prefabs
	private GameObject[] prefabs;
	//Deadline (if the pieces fall below, they will be destroyed, and a life will be subtracted)
	private float deadline;
	//Counter for speed delay
	private int speedDelay=0;
	//Moving background to left or right?	
	private bool backgroundInc=true;
	//Starthight for new Tetris-Pieces
	private float startheight = 12f;
	void Start () {
		//Initialize some variables
		deadline = -5f;
		lifes = 3;
		prefabs= new GameObject[]{t1,t2,t3,t4,t5,t6};
		pieceList = new List<GameObject> ();
		//Create first Piece
		GameObject piece = (GameObject) Instantiate(prefabs[(int) Random.Range(0f,6f)], new Vector3(0,startheight,-1), transform.rotation);
		pieceList.Add (piece);
		scoreObject=pieceList.Count-1;
	}
	
	// Update is called once per frame
	void Update () {
		//Move Backgroud according to backgroundInc
		if(backgroundInc) {
			background.transform.Translate(new Vector3 (0.005f, 0, 0), Space.World);
			if(background.transform.position.x>5) {
				backgroundInc=false;
			}
		} else  {
			background.transform.Translate(new Vector3 (-0.005f, 0, 0), Space.World);
			if(background.transform.position.x<-5) {
				backgroundInc=true;
			}
		}

		//Move platform down, so game gets difficult
		if (speedDelay++ > 10) {

						platfrom.transform.Translate (new Vector3 (0, gameSpeed, 0), Space.World);
						deadline += gameSpeed;
			speedDelay=0;
				}

		bool oneAboveLine = false;
		List<GameObject> objectsToDestroy = new List<GameObject> ();

		//Check if all Tetris-pieces are below 11, so new Tetris-piece can spawn
		foreach (GameObject item in pieceList) {
	
			if(item.transform.position.y > 11f) {
				oneAboveLine=true;
			}
			//check for Tetris-pieces below deadline and destroy them later
			if(item.transform.position.y < deadline) {
				objectsToDestroy.Add(item);
			}
		}
		//Destroy all Tetris-pieces below deadline and adjust scoreText and lifeText and switch to gameover, if life=0
		foreach (GameObject item in objectsToDestroy) {
			pieceList.Remove(item);
			Destroy(item);
			lifes--;
			lifesText.guiText.text = "Life: " + lifes;
			scoreObject=pieceList.Count-1;
			scoreText.guiText.text = "Score: " + scoreObject;
			if(lifes<=0) {
			Invoke("gameover",0.0f);
			}
		}

		//If no Tetris-piece above 11, then spawn new random Tetris-piece
		if (!oneAboveLine) {
			AudioSource.PlayClipAtPoint (pickupSound, transform.position);
			GameObject piece = (GameObject) Instantiate(prefabs[(int) Random.Range(0f,6f)], new Vector3(0,startheight,-1), transform.rotation);
			pieceList.Add (piece);
			scoreObject=pieceList.Count-1;
			scoreText.guiText.text = "Score: " + scoreObject;
		}
	}

	//Switch to game-over screen
	void gameover() {
		Application.LoadLevel ("gameoverscene");
		}
}
