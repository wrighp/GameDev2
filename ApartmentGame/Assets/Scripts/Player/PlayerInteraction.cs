using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
	public Collider parentCollider;
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
		if(item1 == null)
			pickedUp = false;
		
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
		
		if(Input.GetButtonDown("Fire3") && pickedUp && cooldown < 0.01f){
			Debug.Log("Thrown");
			//item1.transform.Translate(transform.forward * 4f);
			Collider childCollider = item1.transform.GetChild (0).gameObject.GetComponent<Collider> ();
			item1.GetComponent<Rigidbody>().AddForce(1f *transform.forward * force, ForceMode.Impulse);		
			StartCoroutine(EnableColliders(.5f,childCollider,parentCollider));
			ReleaseItem(item1);
			item1 = null;

		}
		else if(Input.GetButtonDown("Fire2") && pickedUp && cooldown < 0.01f){
			Debug.Log("Dropped.");
			Collider childCollider = item1.transform.GetChild (0).gameObject.GetComponent<Collider> ();
			StartCoroutine(EnableColliders(0,childCollider,parentCollider));
			ReleaseItem (item1);
			item1 = null;

		}
	}
	void OnTriggerStay(Collider other) {
		IndicatorOverlay overlay = other.GetComponent<IndicatorOverlay> ();
		if(overlay != null && other.gameObject != item1){
			overlay.display = true;
		}
		if(Input.GetButtonDown("Fire2") && !pickedUp && cooldown<0.01f){
			if(other.GetComponent<tmpItem>()!=null){
				if(other.GetComponent<tmpItem>().grabbable){
					if(item1==null){
						item1 = other.gameObject;
					}
					//Debug.Log("Grabbed!");
					Collider childCollider = item1.transform.GetChild (0).gameObject.GetComponent<Collider> ();
					//item1.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
					Physics.IgnoreCollision (childCollider, parentCollider, true);
					item1.transform.position = rightHand.position;
					item1.transform.parent = rightHand;
					item1.transform.localPosition = Vector3.zero;
					item1.GetComponent<Rigidbody>().velocity = Vector3.zero;
					item1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
					pickedUp = true;
					cooldown = 0.25f;

					if(overlay != null){
						overlay.display = false;
					}
				}
			}		
		}

	}
	void OnTriggerEnter(Collider other) {
		
	}
	void OnTriggerExit(Collider other) {
		IndicatorOverlay overlay = other.GetComponent<IndicatorOverlay> ();
		if(overlay != null){
			overlay.display = false;
		}
	}

	IEnumerator EnableColliders(float waitTime, Collider a, Collider b) {
		yield return new WaitForSeconds(waitTime);
		Physics.IgnoreCollision (a, b, false);
	}

	void ReleaseItem (GameObject item1)
	{
		item1.transform.parent = null;
		item1.GetComponent<Rigidbody>().velocity = Vector3.zero;
		item1.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		pickedUp = false;
		cooldown = 0.5f;

	}

	void ThrowGuide(){
	
	}

}
