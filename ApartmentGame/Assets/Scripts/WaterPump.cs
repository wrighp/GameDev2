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
		
		if (col.tag != "Player")
			//spawnballoon ();

		if (Input.GetButton ("Fire1") && !spawning) {
			spawning = true;
			StartCoroutine(spawnballoon ());
			animator.SetTrigger ("Pump");
		//	for (var f = 1.0; f >= 0; f -= 0.1) {
		//	}

			//GameObject instance = Instantiate (WaterBalloon, transform.position, transform.rotation);
		}
	}
	IEnumerator spawnballoon(){
		print ("spawning statrt");
		print (Time.time);
		yield return new WaitForSeconds (spawn_time);
		print ("spawning complete");
		print (Time.time);
		GameObject instance = Instantiate (WaterBalloon, spawnPoint.position, spawnPoint.rotation);
		//instance.transform.position = new Vector3(-0.082f, 0.154f, -0.978f);
		spawning = false;
	}
}

