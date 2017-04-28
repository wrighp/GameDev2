using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Episode manager for each scene
	set the game object group active for the correponding chapters
	This should be alright
	could combine into Progress Manager
*/

public class EpisodeManager : MonoBehaviour {
	
	public GameObject[] Episodes;
	public static int epNum = 0;
	GameObject current;

	public Canvas pause_screen;
	// Use this for initialization
	//deactivate the previous objects, set the chapter tasks as null to be loaded
	//by the dialogue objects
	void Awake () {
		//Debug.Log("LOADING EPISODE");
		if(current == null)
			current = Episodes[0];
		
		if(current == null)
			return;
		
		current.SetActive(false);
		current = Episodes[epNum];
		current.SetActive(true);
	}
	
	//on load episode also set the ProgressTask variables to null
	public static void LoadEpisode()
	{
		epNum+=1;
		//don't reset the overall tasks
		ProgressManager.chapterTasks = null;
		ProgressManager.taskList = null;
		ProgressManager.resetNodes = null;
	}
	
	public static void SetPlayerLocation()
	{
		
		
		return;
	}
	void Update (){
		if (Input.GetButtonDown("Submit")){
			Debug.Log ("submit is working");
			if (pause_screen.gameObject.activeInHierarchy == true) {
				pause_screen.gameObject.SetActive(false);
				Time.timeScale = 1;
			}else{
				pause_screen.gameObject.SetActive(true);
				Time.timeScale = 0;
			}
		}
	}
}
