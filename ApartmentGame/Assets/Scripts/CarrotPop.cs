using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotPop : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	void OnTriggerStay (Collider col)
	{

		if (col.tag != "Player")
			//spawnballoon ();

		if (Input.GetButton ("Fire1")) {
			
			print ("popthatshit");
			animator.SetTrigger ("Pop");

		}
	}
}