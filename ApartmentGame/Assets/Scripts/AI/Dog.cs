using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dog : MonoBehaviour {
	public Transform player;
	private NavMeshAgent nav;

	public float followResetTime = 6f;
	private float timer = 0;

	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Have dog run to random positions around the player every so often
		timer += Time.deltaTime;
		Vector2 direction = Random.insideUnitCircle;
		Vector3 moveDirection = new Vector3 (direction.x, 0, direction.y);
		float distance = Vector3.Distance (player.position, transform.position);
		float navDistance = Vector3.Distance (nav.destination, transform.position);

		if (distance >= 10f) {
			nav.speed = 7f;
			nav.angularSpeed = 240f;
		}
		else{
			nav.speed = Mathf.Lerp(.75f, 7f,  distance / 10f);
			nav.angularSpeed = Mathf.Lerp(120f, 240f,  distance / 10f);
		}

		if (player != null && (timer >= followResetTime || distance > 10f || navDistance <= .75f) ) {
			timer = Random.Range (0, followResetTime);
			nav.destination = player.position + moveDirection * Random.Range(2f, 8f);
		}

		//nav.destination = nav.destination + moveDirection * Time.deltaTime * 2.5f;
	}
}
