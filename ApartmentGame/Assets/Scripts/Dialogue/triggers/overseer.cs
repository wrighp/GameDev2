using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is specifically for calling changing scripts when certain things
//are destroyed

public class overseer : MonoBehaviour {
	
	//targets to destroy
	public List<GameObject> targets;
	//triggers to enable
	public List<dTrigger> triggers;
	public List<GameObject> toDestroy;
	private bool success = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for(int i=0;i<targets.Count; i++){
			if(targets[i] != null){
				success = false;
				break;
			}
			success = true;
		}
		
		if(success){
			for(int i=0; i<triggers.Count; i++){
				triggers[i].enabled = true;
			}
			for(int i=0; i<toDestroy.Count; i++){
				Destroy(toDestroy[i]);
			}
			Destroy(this);
		}
	}
}
