using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFramerateSync : MonoBehaviour {
	public int targetFrameRate = -1;
	public bool autoSetMaxFramerate = true;
	public VSyncModes vsync;

	public enum VSyncModes {None, Half, Full};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (autoSetMaxFramerate) {
			targetFrameRate = Screen.currentResolution.refreshRate;
		}
		//Don't do assignment unless needed, as Unity implementation is unclear
		//Repeatedly setting vsync may be bad for performance
		if (Application.targetFrameRate != targetFrameRate) {
			Application.targetFrameRate = targetFrameRate;
			Debug.Log ("Target Frame Rate set to: " + targetFrameRate);
		}
		if (QualitySettings.vSyncCount != (int)vsync) {
			QualitySettings.vSyncCount = (int)vsync;
			Debug.Log ("VSync set to: " + vsync);
		}
	}
}
