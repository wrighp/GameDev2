using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	Script for turning on and off background chatter
	
*/
public class ChatterScript : MonoBehaviour {
	
	public bool always = false;
	public AutoDialogue dialogue;

	// Use this for initialization
	void Start () {
		if(always && dialogue!=null)
			dialogue.runDialogue();
	}
	
	void OnTriggerEnter(Collider col)
	{
		//if the collider isn't the player, or we're always displaying, return
		if(col.tag != "Player" || always)
			return;
		
		if(dialogue!=null)
			dialogue.runDialogue();
	}
	
	void OnTriggerExit(Collider col)
	{
		//if the collider isn't the player, or we're always displaying, return
		if(col.tag != "Player" || always)
			return;
		
		if(dialogue!=null)
			dialogue.stopDialogue();
	}
}
