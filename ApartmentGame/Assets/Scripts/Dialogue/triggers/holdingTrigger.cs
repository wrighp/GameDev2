using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdingTrigger : MonoBehaviour {
	
	public string thing;
	public npcDialogue speaker;
	public int targetNode;
	public int defaultNode;
	
	void onTriggerStay(Collider col)
	{
		if(col.tag!="Player")
			return;
		
		//set the dialogue node to the target node
		if(col.GetComponent<PlayerInteraction>().IsHolding(thing))
		{
			speaker.setReset(targetNode);
		}
		else
		{
			speaker.setReset(defaultNode);
		}
	}
}
