using UnityEngine;
using System.Collections;

//Information about a single Tetris piece
public class PieceScript : MonoBehaviour {


	//isKinematic: Is the piece still in its default-start-position and fixed?
	//hasBeenMoved: Has the piece been moved?
	public bool isKinematic,hasBeenMoved;
	public void Start() {
		isKinematic = true;
		hasBeenMoved = false;
	}
}
