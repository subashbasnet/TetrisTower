/*
 * Script from the offical Unity3d-Assets
 * 
 * 
 * 
 * 
 */


using UnityEngine;
using System.Collections;
public class dragIt : MonoBehaviour {
	
	public float distance = 0.2f;
	public float damper = 1.0f;
	public float frequency = 8.0f;
	public float drag = 10.0f;
	public float angularDrag = 30.0f;
	
	public bool attachToCenterOfMass = false;
	private HingeJoint2D springJoint;
	private Camera mainCamera;
	
	void Start() {
		mainCamera = FindCamera ();
	}
	
	void Update () {       
		
		if (!Input.GetMouseButtonDown (0)) {
			return;
		}
		
		//int layerMask = 1 << 10;
		int layerMask = 1 << 10;
		//RaycastHit2D hit = Physics2D.Raycast (mainCamera.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
		RaycastHit2D hit = Physics2D.Raycast (mainCamera.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity);
		//Debug.Log ("Layermask: " + LayerMask.LayerToName (8));
		// I have proxy collider objects (empty gameobjects with a 2D Collider) as a child of a 3D rigidbody - simulating collisions between 2D and 3D objects
		// I therefore set any 'touchable' object to layer 8 and use the layerMask above for all touchable items
		if(!hit.rigidbody) return;
		PieceScript ps = (PieceScript) hit.rigidbody.GetComponent(typeof(PieceScript));
		if(ps.hasBeenMoved)
			return;
		if (hit.collider != null && hit.rigidbody.isKinematic == true) {
			if(!ps.isKinematic) 
				return;
			else {
				ps.isKinematic=false;
				ps.hasBeenMoved=true;
				hit.rigidbody.isKinematic=false;
			}
		}
		if (hit.collider != null &&  hit.rigidbody.isKinematic == false) {
			
			if (!springJoint) {
				GameObject go = new GameObject ("Rigidbody2D Dragger");
				Rigidbody2D body = go.AddComponent ("Rigidbody2D") as Rigidbody2D;
				springJoint = go.AddComponent ("HingeJoint2D") as HingeJoint2D;
				body.isKinematic = true;
				body.mass=0.0001f;
			}
			springJoint.transform.position = hit.point;
			
			//springJoint.distance = distance; // there is no distance in SpringJoint2D
			//springJoint.dampingRatio = damper;// there is no damper in SpringJoint2D but there is a dampingRatio
			//springJoint.maxDistance = distance;  // there is no MaxDistance in the SpringJoint2D - but there is a 'distance' field
			//  see http://docs.unity3d.com/Documentation/ScriptReference/SpringJoint2D.html
			//springJoint.maxDistance = distance;
			springJoint.connectedBody = hit.rigidbody;
			springJoint.connectedAnchor = hit.transform.InverseTransformPoint (hit.point);
			// maybe check if the 'fraction' is normalised. See http://docs.unity3d.com/Documentation/ScriptReference/RaycastHit2D-fraction.html
			StartCoroutine ("DragObject", hit.fraction);
		} // end of hit true condition
	} // end of update
	
	IEnumerator DragObject (float distance) {
		float oldDrag = springJoint.connectedBody.drag;
		float oldAngularDrag = springJoint.connectedBody.angularDrag;
		
		springJoint.connectedBody.drag = drag;
		springJoint.connectedBody.angularDrag = angularDrag;
		Camera mainCamera = FindCamera ();
		
		
		
		while (Input.GetMouseButton (0)) {
			
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			springJoint.transform.position = ray.GetPoint (distance);
			
			yield return null;
		}
		
		if (springJoint.connectedBody) {
			//Debug.Log(springJoint.connectedBody.rigidbody2D.velocity.ToString());
			Vector2 power = springJoint.connectedBody.rigidbody2D.velocity;
			power = power/ (springJoint.connectedBody.rigidbody2D.mass+1);
			//Debug.Log(power.ToString());
			
			springJoint.connectedBody.drag = 0.1f;//oldDrag;
			springJoint.connectedBody.angularDrag = 0.05f;//oldAngularDrag;
			springJoint.connectedBody = null;
		}
	}
	
	Camera FindCamera () {
		if (camera)
			return camera;
		else
			return Camera.main;
	}
}
