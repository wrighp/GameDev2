using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLookAt : MonoBehaviour {
	
	public Transform lookTarget;
	public Transform moveTarget;
	
	public float speed = 3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, step);
		
		transform.LookAt(lookTarget);
	}
}
