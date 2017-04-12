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
	public GameObject target;
	
	void onTriggerStay(Collider col)
	{
		if(col.tag!="Player")
			return;
		
		if(!npcDialogue.tasks[beginVal])
			return;
		
		
		Debug.Log(Vector3.Distance(target.transform.position, transform.position));
		//set the dialogue node to the target node
		if(col.GetComponent<PlayerInteraction>().IsHolding(thing)
			&& !npcDialogue.tasks[tableVal])
		{
			speaker.setReset(targetNode);
			npcDialogue.tasks[beginVal] = false;
		}
		else if (!npcDialogue.tasks[tableVal])
		{
			speaker.setReset(defaultNode);
		}
	}
}
