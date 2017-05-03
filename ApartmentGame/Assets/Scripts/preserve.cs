using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preserve : MonoBehaviour {
	
	public static preserve Instance;
	
	public Dictionary<string, bool> tasks;
	public string[] taskList;
	public int numTasks = 4;

	
	void awake(){
		if(Instance == null){
			DontDestroyOnLoad (gameObject);
			tasks = new Dictionary<string, bool>();
			Instance = this;
		}
		else if(Instance!=this){
			Destroy(gameObject);
		}
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
