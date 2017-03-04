using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpItem : MonoBehaviour {
	
	//this objects attribute(s)
	public string attribute;
	
	//particle system or objects to instantiate when certain
	//incoming attributes collide
	public GameObject[] effects;
	public string[] incomingAttribute;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void onTriggerEnter(Transform col){
		
	}
	
	void onTriggerExit(Transform col){
		
	}
	
	void onTriggerStay(Transform col){
		
	}
}
