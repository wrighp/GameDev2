using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPump : MonoBehaviour {

	private Animator animator;
	public GameObject WaterBalloon;
	public Transform spawnPoint;
	public float spawn_time = 2f;
	bool spawning = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	void OnTriggerStay (Collider col)
	{
		if (col.tag == "Player" && Input.GetButton ("Fire1") && !spawning) 
		{
			spawning = true;
			animator.SetTrigger ("Pump");
			StartCoroutine(spawnballoon ());
		}
	}
	IEnumerator spawnballoon(){
		yield return new WaitForSeconds (spawn_time);
		GameObject instance = Instantiate (WaterBalloon, spawnPoint.position, spawnPoint.rotation);
		spawning = false;
	}
}

