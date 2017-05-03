using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Turns object to face a direction by moving on the y-axis
/// </summary>
public class TurnToVelocity : MonoBehaviour {

	public float turnSpeed = .5f;
	private Quaternion goal = Quaternion.identity;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 v = new Vector2(rb.velocity.x, rb.velocity.z);
		if(v.sqrMagnitude > .1f){
			float angle = Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg;
			goal = Quaternion.AngleAxis(angle, Vector3.up);

		}
		transform.rotation = Quaternion.Lerp(transform.rotation, goal, Time.fixedDeltaTime / turnSpeed);

	}
}
