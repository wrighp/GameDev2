using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour {
	
	public Camera transition;
	//public npcDialogue dummy;
	public string scene;
	public int doorID = 1;
	public static Dictionary<int, Transform> doors;
	
	//set the door ID in the hash table, there must be a door with ID 0
	void Awake()
	{
		if(doors == null)
			doors = new Dictionary<int, Transform>();
		
		doors[doorID] = this.gameObject.transform.GetChild(0);
	}
	
	void OnTriggerStay(Collider col){
		if(col.tag!="Player")
			return;

		Vector3 p4 = col.transform.TransformDirection(Vector3.forward);
		float PDotN = Vector3.Dot(p4, transform.position - col.transform.position);
		
		if(Input.GetButtonDown("Fire1")
			/*&& (PDotN>0.25|| Vector3.Distance(col.transform.position, transform.position) < 2)*/)
		{
			Debug.Log("Changing Scenes");
			npcDialogue.saveState();
			ProgressManager.doorID = doorID;
			//preserve.Instance.transitions+=1;
			//SceneManager.LoadScene(scene);
			SceneTransition.setScene(scene);
			transition.GetComponent<SceneTransition>().play = true;
		}
	}
	public void Load()
	{
		SceneTransition.setScene(scene);
		transition.GetComponent<SceneTransition>().play = true;
	}
}
