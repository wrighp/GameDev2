using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//run dialogue tree when player exits trigger space
public class exit : MonoBehaviour {
	
	public int index;
	public npcDialogue target;
	public bool selfDestruct = true;
	public bool blockMovement = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//on trigger exit, run a thing
	void OnTriggerExit(Collider col){
		if(col.tag!="Player")
			return;
		
		target.setNext(index);
		GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = !blockMovement;
		target.runDialogue();
		
		if(selfDestruct)
			Destroy(this);
	}
	
}
