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
	
	void OnTriggerStay(Collider col)
	{
		if(col.tag!="Player")
			return;
		
		if(npcDialogue.tasks == null || !npcDialogue.tasks.ContainsKey(beginVal))
		{
			Debug.Log("Don't have the task yet.");
			return;
		}
		
		if(npcDialogue.tasks.ContainsKey(tableVal))
			Destroy(this);
		
		//Debug.Log(Vector3.Distance(target.transform.position, transform.position));
		//set the dialogue node to the target node
		if(col.GetComponent<PlayerInteraction>().IsHolding(thing)
		/*Vector3.Distance(target.transform.position, transform.position) < 5*/
			&& npcDialogue.tasks[beginVal])
		{
			Debug.Log("Wa wa we wa");
			speaker.setReset(targetNode);
			npcDialogue.tasks[beginVal] = false;
		}
		else if (/*Vector3.Distance(target.transform.position, transform.position) > 5
			&&*/ !npcDialogue.tasks.ContainsKey(tableVal))
		{
			npcDialogue.tasks[beginVal] = true;
			speaker.setReset(defaultNode);
		}
	}
}
