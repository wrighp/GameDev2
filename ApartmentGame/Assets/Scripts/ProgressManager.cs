using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Use this to manage the tasks that the player has achieved
*/

public class ProgressManager : MonoBehaviour {
	
	public static ProgressManager Instance;
	
	
	//dialogue's tasks
	public static Dictionary<string, bool> tasks;
	public static Dictionary<string, bool> chapterTasks;
	//each character's reset node
	public static Dictionary<string, int> resetNodes;
	
	public static List<string> taskList;
	
	public static int doorID;
	
	void Awake(){
		if(Instance == null){
			DontDestroyOnLoad (gameObject);
			Instance = this;
		}
		else if(Instance!=this){
			Destroy(gameObject);
		}
	}
	
	public static void setPlayerLocation(GameObject player)
	{
		player.transform.position = LoadScene.doors[doorID].position;
	}
}
