using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPartialLogs : MonoBehaviour {

	/// <summary>
	/// Tests GameEvents partial class as well as reflection invocation
	/// The Log method takes a string and LogBulletPoints takes a string and string array
	/// </summary>
	// Use this for initialization
	void Start () {
		GameEvents.Call ("Log", "Log Test");
		//Arguments will need to be passed in from the xml as an array
		//LogBulletPoints takes a bullet point as well as the array of strings to make a logged list
		object[] logBulletArgs = {"***",new string[]{ "Element 1", "Element 2", "Element 3" }};
		GameEvents.Call ("LogBulletPoints", logBulletArgs);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
