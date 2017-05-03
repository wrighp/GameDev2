using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour {
	
	Camera camera;
	public Transform target;
	bool draw = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//if the player is close enough, draw a sprite over the thing
		//but under player
		if(draw)
		{
			
		}
	}
	
	void OnTriggerStay(Collider col)
	{
		if(col.tag!="Player")
			return;
		draw = true;
		camera = Camera.main;
		//position ui over head
	}
	
	void OnTriggerExit(Collider col)
	{
		if(col.tag!="Player")
			return;
		draw = false;
	}
}
