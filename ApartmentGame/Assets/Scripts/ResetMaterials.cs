using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMaterials : MonoBehaviour {
	public static HashSet<string> resetAlready = new HashSet<string>();
	// Use this for initialization
	void Start () {
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		if(!resetAlready.Contains(gameObject.name)){
			foreach (var renderer in renderers) {
				if(renderer.material != null){
					if(renderer.material.shader.name == "Whitenoise"){
						//Reset values here
						//renderer.material.set
					}	
				}
			}
			resetAlready.Add(gameObject.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
