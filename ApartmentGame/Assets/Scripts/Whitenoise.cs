using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whitenoise : MonoBehaviour {
	
	public Material WhitenoiseMaterial;
	public Shader regularShader;
	
	//public float Step;
	
	float whiteNoise = 1;
	float XAmp;
	float YAmp;
	float ZAmp;
	
	float currTime = 0;
	public float fadeTime = 3.5f;

	// Use this for initialization
	void Start () {
		whiteNoise = WhitenoiseMaterial.GetFloat("_TexBlend");
		XAmp = WhitenoiseMaterial.GetFloat("_AX");
		YAmp = WhitenoiseMaterial.GetFloat("_AY");
		ZAmp = WhitenoiseMaterial.GetFloat("_AZ");
	}
	
	public void Play()
	{
		if(WhitenoiseMaterial.shader != regularShader)
			StartCoroutine(Normalize());
	}
	
	//lerp all the amplitudes and frequencies
	IEnumerator Normalize()
	{
		//Debug.Log("Normalizing!");
		currTime = Time.deltaTime;
		while(whiteNoise>=0 && XAmp>=0 && YAmp>=0 && ZAmp>=0)
		{
			currTime+=Time.deltaTime;
			//whiteNoise-=Time.deltaTime;
			whiteNoise = Mathf.Lerp(1, 0, currTime/fadeTime);
			XAmp = Mathf.Lerp(0.5f, 0, currTime/fadeTime);
			YAmp = Mathf.Lerp(0.5f, 0, currTime/fadeTime);
			ZAmp = Mathf.Lerp(0.5f, 0, currTime/fadeTime);
			
			WhitenoiseMaterial.SetFloat("_TexBlend", whiteNoise);
			WhitenoiseMaterial.SetFloat("_AX",XAmp);
			WhitenoiseMaterial.SetFloat("_AY", YAmp);
			WhitenoiseMaterial.SetFloat("_AZ", ZAmp);
			
			yield return null;
		}
		
		WhitenoiseMaterial.SetFloat("_TexBlend", 0);
		WhitenoiseMaterial.SetFloat("_AX", 0);
		WhitenoiseMaterial.SetFloat("_AY", 0);
		WhitenoiseMaterial.SetFloat("_AZ", 0);
		
		//set the shader to the default one
		WhitenoiseMaterial.shader = regularShader;
			
		yield return null;
	}
}
