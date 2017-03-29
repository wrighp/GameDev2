using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player movement, direction based off of camera rotation
/// </summary>
[RequireComponent (typeof (Rigidbody))]
public class PlayerMovement : MonoBehaviour {

	public Camera cam;
	public float acceleration;
	public float maxSpeed; //Max horizontal speed (not vertical)
	public ForceMode forceMode;
	public LayerMask jumpMask;
	public float jumpForce = 8f;
	private Animator animator;
	private Rigidbody rb;
	private float horizontalSpeed;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		cam = cam ?? Camera.main;
		animator = GetComponentInChildren<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		//Animations here
		animator.SetFloat("MoveSpeed", horizontalSpeed/maxSpeed);
	}

	void FixedUpdate(){
		Vector3 camAngle = new Vector3(0, cam.transform.eulerAngles.y, 0);
		Quaternion quat = Quaternion.Euler(camAngle);
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0 ,Input.GetAxisRaw("Vertical")).normalized;
		//Input axis is now corrected for camera rotation
		input = quat * input;

		//Apply as force or direct acceleration change
		rb.AddForce(input * acceleration, forceMode);

		//Cap max horizontal speed;
		Vector3 hVel = rb.velocity;
		hVel.y = 0;
		horizontalSpeed = hVel.magnitude;
		hVel = horizontalSpeed > maxSpeed ? hVel.normalized * maxSpeed : hVel;
		//Jumping
		hVel.y =  rb.velocity.y;
		rb.velocity = hVel;

		if (Input.GetButtonDown ("Jump") && Physics.Raycast (transform.position, Vector3.down, 1.1f, jumpMask)) {
			rb.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
		}

	}
}
