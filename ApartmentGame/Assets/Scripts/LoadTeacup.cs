using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTeacup : MonoBehaviour {
	
	public string scene;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider col){
		Debug.Log(col.tag);
		if(col.tag == "Player"){
			SceneManager.LoadScene(scene);
		}
	}
}
