using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsate : MonoBehaviour {
	
	SpriteRenderer pain;
	float phase;
	float alphaClamp;
	
	// Use this for initialization
	void Start () {
		pain = gameObject.GetComponent<SpriteRenderer>();
		alphaClamp = pain.color.a;
		phase = Random.Range(0,5);
	}
	
	// Update is called once per frame
	void Update () {
		pain.color = new Color(pain.color.r, pain.color.g, pain.color.b, 
			Mathf.Clamp(pain.color.a + Mathf.Sin(Time.time + phase) + 
			Mathf.Cos(Time.time + phase), 0, alphaClamp));
	}
}
