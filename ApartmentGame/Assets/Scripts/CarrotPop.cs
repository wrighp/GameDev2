using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotPop : MonoBehaviour {

	Rigidbody rb;
	private Animator animator;
	private tmpItem itemscr;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		//rb.detectCollisions = false;
		rb.constraints = RigidbodyConstraints.FreezeAll;
		itemscr = GetComponent<tmpItem> ();
	}
	void OnTriggerStay (Collider col)
	{
		if (col.tag != "Player")
			return;
			//spawnballoon ();

		if (Input.GetButton ("Fire1")) {
			Debug.Log ("PULL ME OUT");
			rb.constraints = RigidbodyConstraints.None;
			//rb.detectCollisions = true;
			rb.AddForce (new Vector3 (0, 2f, 0));
			itemscr.enabled = true;
			this.enabled = false;
		}
	}
}