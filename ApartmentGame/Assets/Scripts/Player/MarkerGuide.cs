using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used as a secondary marker for predicting camera movement, currently not used
public class MarkerGuide : MonoBehaviour {

	public float moveSpeed = .5f;
	public float scale = 1f;
	public Rigidbody rb;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		transform.position =  Vector3.Lerp(transform.position , rb.transform.position + rb.velocity * scale, Time.deltaTime / moveSpeed);
	}
}
