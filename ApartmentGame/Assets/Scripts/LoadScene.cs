using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour {
	
	public Camera transition;
	//public npcDialogue dummy;
	public string scene;
	
	void OnTriggerEnter(Collider col){
		Debug.Log(col.tag);
		if(col.tag == "Player" && Input.GetButtonDown("Pickup")){
			npcDialogue.saveState();
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
