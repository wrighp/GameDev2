using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class State : MonoBehaviour {
	
	public GameObject sparking;
	public GameObject flaming;
	
	private bool onFire = false;
	private bool zapped = false;
	private bool wet = false;
	private bool insulated = false;
	
	float zapDeathTimer = 3f;
	float zapTimer = 0f;
	
	float fireDeathTimer = 5f;
	float fireTimer = 0f;
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//do the things
		//if the player is on fire for too long, game over
		if(onFire){
			Debug.Log("ON FIRE");
			fireTimer+= Time.deltaTime;
		}
		else{
			fireTimer-= Time.deltaTime;
		}
		
		if(zapped){
			Debug.Log("GETTING ZAPPED");
			zapTimer+= Time.deltaTime;
		}
		else{
			zapTimer-= Time.deltaTime;
		}
		
		if(zapTimer>zapDeathTimer){
			Debug.Log("DEAD");
			SceneManager.LoadScene("Lose");
		}
		if(fireTimer>fireDeathTimer){
			Debug.Log("DEAD");
			SceneManager.LoadScene("Lose");
		}
		if(fireTimer<0){
			fireTimer = 0f;
		}
		if(zapTimer<0){
			zapTimer = 0f;
		}
		
	}
	
	public void burning(bool x){
		onFire = x;
		flaming.SetActive(x);
	}
	
	public void shocked(bool x){
		zapped = x;
		sparking.SetActive(x);
	}
	
	//take away attributes
	void OnTriggerExit(Collider col){
		if(col.gameObject.GetComponent<tmpItem>() == null)
			return;
		
		string affliction = col.gameObject.GetComponent<tmpItem>().getAttribute();
		
		if(affliction == "Wet"){
			wet = false;
			Debug.Log("NOT WET");
		}
		if(affliction == "Rubber"){
			insulated = false;
			Debug.Log("NOT INSULATED");
		}
		
		//take fire damage
		if(affliction == "Fire"){
			burning(false);
			Debug.Log("NOT ON FIRE");
		}
		
		if(affliction == "Sparks"){
			shocked(false);
			Debug.Log("NOT ZAPPED");
		}
	}
	
	void OnTriggerStay(Collider col){
		if(col.gameObject.GetComponent<tmpItem>() == null)
			return;
		
		string affliction = col.gameObject.GetComponent<tmpItem>().getAttribute();
		
		if(affliction == "Wet"){
			burning(false);
			Debug.Log("NOT ON FIRE");
		}
		if(affliction == "Rubber"){
			shocked(false);
			Debug.Log("NOT SHOCKED");
		}
		
		//take fire damage
		if(affliction == "Fire" && !wet){
			burning(true);
		}
		
		if(affliction == "Sparks" && !insulated){
			shocked(true);
		}
	}
}
