using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
	
	public string scene;
	//public npcDialogue dummy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider col){
		Debug.Log(col.tag);
		if(col.tag == "Player"){
			//dummy.saveState();
			//preserve.Instance.transitions+=1;
			SceneManager.LoadScene(scene);
		}
	}
}
