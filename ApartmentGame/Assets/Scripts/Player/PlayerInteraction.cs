using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
	public float itemPickupDistance = 4f;
	public Transform rightHand;
	public Transform leftHand;
	private bool pickedUp = false;
	public GameObject item1;
	public float force = 10f;
	
	float cooldown = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//pickedUp = false;
		if(pickedUp && item1!=null){
			item1.transform.position = rightHand.position;
			item1.transform.localPosition = Vector3.zero;
		}
		if(cooldown>0){
			cooldown-=Time.deltaTime;
		}
		if(cooldown<0){
			cooldown = 0;
		}
		
		if(Input.GetButtonDown("Fire3") && item1!=null){
			Debug.Log("Thrown");
			item1.transform.Translate(transform.forward * 4f);
			//item1.GetComponent<Rigidbody>().AddForce(-1 *transform.forward * force, 
				//ForceMode.Acceleration);
			
			item1.transform.parent = null;
			
			item1.transform.GetChild(0).
				gameObject.GetComponent<Collider>().enabled = true;
			
			item1 = null;
			pickedUp = false;
			cooldown = 0.5f;
		}
	}
	void OnTriggerStay(Collider other) {
		
		if(Input.GetButtonDown("Fire2") && !pickedUp && cooldown<0.01f){
			if(other.GetComponent<tmpItem>()!=null){
				if(other.GetComponent<tmpItem>().grabbable){
					if(item1==null){
						item1 = other.gameObject;
					}
					Debug.Log("Grabbed!");
					
					item1.transform.GetChild(0).
						gameObject.GetComponent<Collider>().enabled = false;
					item1.transform.position = rightHand.position;
					item1.transform.parent = rightHand;
					item1.transform.localPosition = Vector3.zero;
					
					item1.GetComponent<Rigidbody>().velocity = Vector3.zero;
					item1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				}
				pickedUp = true;
				cooldown = 0.5f;
			}		
		}
		
		if(Input.GetButtonDown("Fire2") && pickedUp && cooldown<0.01f){
			Debug.Log("Dropped.");
			
			item1.transform.GetChild(0).
						gameObject.GetComponent<Collider>().enabled = true;
						
			item1.transform.parent = null;
			item1 = null;
			pickedUp = false;
			cooldown = 0.5f;
		}
	}



}
