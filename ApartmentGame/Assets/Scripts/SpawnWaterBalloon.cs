using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaterBalloon : MonoBehaviour {

	public GameObject WaterBalloon;

	void Start () {
		GameObject instance = Instantiate(WaterBalloon, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
