using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player movement, direction based off of camera rotation
/// </summary>
[RequireComponent (typeof (Rigidbody))]
public class PlayerMovement : MonoBehaviour {

	public Camera camera;
	public float acceleration;
	public float maxSpeed; //Max horizontal speed (not vertical)
	public ForceMode forceMode;
	public Vector3 dir;
	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate(){
		Vector3 camAngle = new Vector3(0, camera.transform.eulerAngles.y, 0);
		Quaternion quat = Quaternion.Euler(camAngle);
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0 ,Input.GetAxisRaw("Vertical")).normalized;
		//Input axis is now corrected for camera rotation
		input = quat * input;

		//Apply as force or direct acceleration change
		rb.AddForce(input * acceleration, forceMode);

		//Cap max horizontal speed;
		Vector3 hVel = rb.velocity;
		hVel.y = 0;
		hVel = hVel.sqrMagnitude > maxSpeed * maxSpeed ? hVel.normalized * maxSpeed : hVel;
		hVel.y =  rb.velocity.y;
		rb.velocity = hVel;

	}
}
