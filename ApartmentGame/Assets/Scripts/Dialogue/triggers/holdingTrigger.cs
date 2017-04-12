using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdingTrigger : MonoBehaviour {
	
	public string thing;
	public npcDialogue speaker;
	public string tableVal;
	public int targetNode;
	public int defaultNode;
	public GameObject target;
	
	void onTriggerStay(Collider col)
	{
		if(col.tag!="Player")
			return;
		Debug.Log(Vector3.Distance(target.transform.position, transform.position));
		//set the dialogue node to the target node
		if(/*col.GetComponent<PlayerInteraction>().IsHolding(thing)*/ 
		Vector3.Distance(target.transform.position, transform.position) < 5&& !npcDialogue.tasks[tableVal])
		{
			speaker.setReset(targetNode);
		}
		else if (!npcDialogue.tasks[tableVal] && Vector3.Distance(target.transform.position, transform.position) > 5)
		{
			speaker.setReset(defaultNode);
		}
	}
}
