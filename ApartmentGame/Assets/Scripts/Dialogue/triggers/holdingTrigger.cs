using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdingTrigger : MonoBehaviour {
	
	public string thing;
	public npcDialogue speaker;
	public string tableVal;
	public string beginVal;
	public int targetNode;
	public int defaultNode;
	
	void OnTriggerStay(Collider col)
	{
		if(col.tag!="Player")
			return;
		
		if(npcDialogue.tasks == null || !npcDialogue.tasks.ContainsKey(beginVal))
		{
			return;
		}
		
		if(npcDialogue.tasks.ContainsKey(tableVal))
			Destroy(this);
		
		//set the dialogue node to the target node
		if(col.GetComponentsInChildren<PlayerInteraction>()[0].IsHolding(thing))
		{
			speaker.setReset(targetNode);
			npcDialogue.tasks[beginVal] = false;
		}
		else if (!npcDialogue.tasks.ContainsKey(tableVal))
		{
			npcDialogue.tasks[beginVal] = true;
			speaker.setReset(defaultNode);
		}
	}
}
