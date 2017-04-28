using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dog : MonoBehaviour {
	public Transform player;
	public bool autoAddPlayer = true;
	private NavMeshAgent nav;
	Animator anim;

	public float followResetTime = 6f;
	private float timer = 0;
	public Transform mouth;
	public GameObject item;
	bool fetching = false;
	
	Collider parentCollider;

	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent> ();
		anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		//Have dog run to random positions around the player every so often
		if (autoAddPlayer && !player) {
			player = GameObject.FindGameObjectWithTag ("Player").transform;
		}
		
		if(item != null/* && pickedUpRight*/){
			//Object will keep moving due to its rigidbody
			item.transform.position = mouth.position;
			item.transform.parent = mouth;
		}
		
		timer += Time.deltaTime;
		if(!fetching)
		{
			Vector2 direction = Random.insideUnitCircle;
			Vector3 moveDirection = new Vector3 (direction.x, 0, direction.y);
			float distance = Vector3.Distance (player.position, transform.position);
			float navDistance = Vector3.Distance (nav.destination, transform.position);

			if (distance >= 10f) {
				nav.speed = 7f;
				nav.angularSpeed = 240f;
			}
			else{
				nav.speed = Mathf.Lerp(.75f, 20f,  distance / 10f);
				nav.angularSpeed = Mathf.Lerp(120f, 240f,  distance / 10f);
			}

			if (player != null && (timer >= followResetTime || distance > 10f || navDistance <= .75f) ) {
				timer = Random.Range (0, followResetTime);
				nav.destination = player.position + moveDirection * Random.Range(2f, 8f);
			}
		}

		//nav.destination = nav.destination + moveDirection * Time.deltaTime * 2.5f;
	}
	
	public void Fetch(GameObject ball)
	{
		if(item == null)
		{
			StartCoroutine(fetchObject(ball));
			fetching = true;
		}
	}
	
	IEnumerator fetchObject(GameObject target)
	{
		float distance = Vector3.Distance (target.transform.position, transform.position);
		
		//Debug.Log("going to the ball!");
		//move to the thing
		while(distance>1f)
		{
			//Debug.Log("Distance is..." + distance);
			moveTo(target.transform.position);
			distance = Vector3.Distance (target.transform.position, transform.position);
			yield return null;
		}
		//pick up the ball
		//play pickup animation
		//Debug.Log("Picking up the ball");
		
		Physics.IgnoreCollision (target.transform.GetComponentInChildren<Collider>(), 
			transform.GetComponentInChildren<Collider>());
		
		anim.Play("Pickup");
		
		yield return new WaitForSeconds(0.5f);
		
		item = target;
		item.transform.position = mouth.position;
		item.transform.parent = mouth;
		item.transform.localPosition = Vector3.zero;
		Rigidbody rb = item.GetComponentInChildren<Rigidbody>();
		rb.freezeRotation = true;
		
		
		//return to player
		//Debug.Log("Moving back to player");
		distance = Vector3.Distance(transform.position, player.position);
		
		//Debug.Log("Distance to player is..." + distance);
		
		while(distance>3f)
		{
			//Debug.Log("Distance to player is..." + distance);
			moveTo(player.position);
			distance = Vector3.Distance (player.position, transform.position);
			yield return null;
		}
		
		//drop the ball
		//Debug.Log("dropping the ball");
		EnableColliders(0.5f, item.transform.GetComponentInChildren<Collider>(), 
			transform.GetComponentInChildren<Collider>());
		
		item.transform.parent = null;
		item.GetComponent<Rigidbody>().velocity = Vector3.zero;
		item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		
		//Debug.Log("the deed is done.");
		item = null;
		fetching = false;
	}
	
	void moveTo(Vector3 location)
	{		

		Vector3 dir = location - transform.position;
		//Vector2 direction = curTarget.position.xy;
		Vector3 moveDirection = new Vector3 (dir.x, 0, dir.y);
		float distance = Vector3.Distance (location, transform.position);
		float navDistance = Vector3.Distance (nav.destination, transform.position);
		
		if (distance >= 10f) {
			nav.speed = 7f;
			nav.angularSpeed = 240f;
		}
		else{
			nav.speed = Mathf.Lerp(.75f, 7f,  distance / 10f);
			nav.angularSpeed = Mathf.Lerp(120f, 240f,  distance / 10f);
		}
		
		nav.destination = location + moveDirection;
		
		//set this to be wander later but for now just copy dog
	}
	
	IEnumerator EnableColliders(float waitTime, Collider a, Collider b) {
		yield return new WaitForSeconds(waitTime);
		Physics.IgnoreCollision (a, b, false);
	}
}
