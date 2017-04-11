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

	// Use this for initialization
	//deactivate the previous objects, set the chapter tasks as null to be loaded
	//by the dialogue objects
	void Start () {
		current.SetActive(false);
		current = Episodes[epNum];
		current.SetActive(true);
		
		ProgressManager.chapterTasks = null;
		ProgressManager.taskList = null;
		
	}
	
	//on load episode also set the ProgressTask variables to null
	public static void LoadEpisode()
	{
		epNum+=1;
	}
	
}
