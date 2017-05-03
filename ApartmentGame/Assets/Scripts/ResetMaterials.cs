using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For this to work whitenoise has to not have a predefined material but find the material the object has on Start
public class ResetMaterials : MonoBehaviour {
	public class TwoMats{
		public Material mat1;
		public Material mat2;
		public TwoMats(Material one, Material two){
			mat1 = one;
			mat2 = two;
		}
	}
	public static Dictionary<string,TwoMats> resetAlready = new Dictionary<string,TwoMats>();
	// Use this for initialization
	void Awake () {
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (var renderer in renderers) {
			if(renderer.material != null){
				if(renderer.material.shader.name == "Whitenoise"){
					if(resetAlready.ContainsKey(gameObject.name)){
						//Material should be normal at this point (it got lerped)
					}
					else{
						//Make copy of what the material should look like
						renderer.material = new Material(renderer.material);
						resetAlready.Add(gameObject.name, new TwoMats(renderer.material,new Material(renderer.material)));
					}
					break;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnDisable(){
		//Reset material here
		TwoMats mats;
		resetAlready.TryGetValue(gameObject.name, out mats);
		if(mats != null){
			mats.mat1.CopyPropertiesFromMaterial(mats.mat2);
		}

	}
}
