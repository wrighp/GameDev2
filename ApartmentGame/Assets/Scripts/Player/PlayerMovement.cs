using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player movement, direction based off of camera rotation
/// </summary>
[RequireComponent (typeof (Rigidbody))]
public class PlayerMovement : MonoBehaviour {
	
	public static PlayerMovement Instance;

	public Camera cam;
	public float acceleration;
	public float maxSpeed; //Max horizontal speed (not vertical)
	public ForceMode forceMode;
	public LayerMask jumpMask;
	public float jumpForce = 8f;
	public GameObject particleTrail; //spawned when you go fast
	private GameObject particleChild;
	private Animator animator;
	private Rigidbody rb;
	private float horizontalSpeed;
	
	//testing persistant stuff
	void Awake()
	{
		if(Instance == null){
			DontDestroyOnLoad (gameObject);
			Instance = this;
		}
		else if(Instance!=this){
			Destroy(gameObject);
		}
	}
	
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		cam = cam ?? Camera.main;
		animator = GetComponentInChildren<Animator> ();
	}
	
	void OnLevelWasLoaded()
	{
		ProgressManager.setPlayerLocation(this.gameObject);
	}

	// Update is called once per frame
	void Update () {
		
		if(cam == null)
			cam = Camera.main;
		
		//Animations here
		float speedAmount = horizontalSpeed/maxSpeed;
		animator.SetFloat("MoveSpeed", speedAmount);
		if(speedAmount > .9){
			animator.SetFloat("RunTime",animator.GetFloat("RunTime")+Time.deltaTime);
		}
		else{
			animator.SetFloat("RunTime",0);
			if(particleChild){
				//Will autodestruct due to trail renderer
				particleChild.transform.parent = null;
				particleChild = null;
			}
		}
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
		float max = maxSpeed;
		if(animator.GetFloat("RunTime") > 8){
			if(!particleChild){
				particleChild = (GameObject)GameObject.Instantiate(particleTrail,transform, false);
			}
			max *= 2f;
		}
		hVel = horizontalSpeed > maxSpeed ? hVel.normalized * max : hVel;

		//Jumping
		hVel.y =  rb.velocity.y;
		rb.velocity = hVel;

		if (Input.GetButtonDown ("Jump") && Physics.Raycast (transform.position, Vector3.down, 1.1f, jumpMask)) {
			rb.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
		}

	}
}
