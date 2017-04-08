using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPan : MonoBehaviour {

	public Material material;
	public Vector2 panAmount;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 offset = material.GetTextureOffset("_MainTex");
		offset += Time.deltaTime * panAmount;
		offset.x = offset.x % 1f;
		offset.y = offset.y % 1f;
		material.SetTextureOffset("_MainTex",offset);
	}
}
