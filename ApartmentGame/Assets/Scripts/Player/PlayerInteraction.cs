using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
	public Collider parentCollider;
	public Transform rightHand;
	public Transform leftHand;
	public GameObject item1;
	public float maxAngle = 45f;
	public float force = 10f;
	public float guideTime = 2f; //Number of seconds guide simulates for
	public int guideSegments = 20; //Number of segments guide has

	const float PICKUP_DELAY = .25f; //Time until object can be picked up again after dropped to avoid spamming
	bool pickedUp = false;
	float cooldown = 0;
	bool triggerDown = false; //Was Throw trigger held down last frame
	LineRenderer line;
	// Use this for initialization
	void Start () {
		line = GetComponentInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(item1 != null){
			//Object will keep moving due to its rigidbody
			item1.transform.position = rightHand.position;
			item1.transform.parent = rightHand;
		}

		if(cooldown>0){
			cooldown -= Time.deltaTime;
		}
		if(cooldown<0){
			cooldown = 0;
		}
			
		if(pickedUp){
			
			float throwAxis = Input.GetAxis("Throw");
			bool throwReleased = triggerDown &&  throwAxis < .1f;
			triggerDown = throwAxis >= .1f; //Reset to false after thrown

			Rigidbody rb = item1.GetComponent<Rigidbody>();
			//10 and 15 hardcoded in currently for camera min and max angles
			MouseOrbitImproved mo = Camera.main.GetComponent<MouseOrbitImproved>();
			float angle = maxAngle * .5f;
			if(mo != null){
				angle = Mathf.Deg2Rad * (maxAngle - (Camera.main.transform.eulerAngles.x - mo.yMinLimit) * (maxAngle) / (mo.yMaxLimit - mo.yMinLimit));
			}
			Vector3 throwVector = (transform.forward + new Vector3(0,angle,0f)).normalized;

			if(Input.GetButton("Fire1") || triggerDown){
				ThrowGuide(throwVector, transform.forward);
				line.enabled = true;
			}
			else{
				line.enabled = false;
			}
			if(Input.GetButtonUp("Throw") || throwReleased){
				throwReleased = false;
				//Debug.Log("Thrown");
				//Map 10-15 to 45-0
				Collider childCollider = item1.transform.GetComponentInChildren<Collider> ();
				rb.AddForce(throwVector * force, ForceMode.Impulse);		
				StartCoroutine(EnableColliders(.3f,childCollider,parentCollider));
				ReleaseItem(item1);
				item1 = null;
				rb.freezeRotation = false;

			}
			else if(Input.GetButtonDown("Drop")){
				//Debug.Log("Dropped");
				Collider childCollider = item1.transform.GetComponentInChildren<Collider> ();
				StartCoroutine(EnableColliders(0,childCollider,parentCollider));
				ReleaseItem (item1);
				item1 = null;
				rb.freezeRotation = false;
				rb.velocity = GetComponentInParent<Rigidbody>().velocity;
			}
		}
		//Can only start throwing again if button used to pick up object is released
		if(item1 != null && Input.GetButtonUp("Pickup")){
			pickedUp = true;
		}
			
	}
	void OnTriggerStay(Collider other) {
		IndicatorOverlay overlay = other.GetComponent<IndicatorOverlay> ();
		if(overlay != null && other.gameObject != item1 && overlay.display == false){
			overlay.Enable();
		}
		if(Input.GetButtonDown("Pickup") && !pickedUp && cooldown<0.01f){
			if(other.GetComponent<tmpItem>()!=null){
				if(other.GetComponent<tmpItem>().grabbable){
					if(item1==null){
						item1 = other.gameObject;
					}
					//Debug.Log("Grabbed!");
					Collider childCollider = item1.transform.GetComponentInChildren<Collider> ();
					//item1.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
					Physics.IgnoreCollision (childCollider, parentCollider, true);
					item1.transform.position = rightHand.position;
					item1.transform.parent = rightHand;
					item1.transform.localPosition = Vector3.zero;
					Rigidbody rb = item1.GetComponentInChildren<Rigidbody>();
					rb.freezeRotation = true;
					pickedUp = false;
					cooldown = PICKUP_DELAY;

					if(overlay != null){
						//For displaying icon over object
						overlay.display = false;
					}
				}
			}		
		}

	}
	void OnTriggerEnter(Collider other) {
		IndicatorOverlay overlay = other.GetComponent<IndicatorOverlay> ();
		if(overlay != null){
			overlay.Enable();
		}
	}
	void OnTriggerExit(Collider other) {
		IndicatorOverlay overlay = other.GetComponent<IndicatorOverlay> ();
		if(overlay != null){
			overlay.Disable();
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
		cooldown = PICKUP_DELAY;

	}

	void ThrowGuide(Vector3 throwVector, Vector3 forward){
		//F = M*a
		float angle = throwVector.y;
		float velocity = force / item1.GetComponent<Rigidbody>().mass;
		float yVel = velocity * Mathf.Sin(angle);
		float xVel = velocity * Mathf.Cos(angle);
		Vector3 pos = item1.transform.position;
		float segTime = guideTime / (guideSegments - 1);
		float time = 0;
		Vector3[] positions = new Vector3[guideSegments];
		line.numPositions = guideSegments;
		for(int i = 0; i < guideSegments; i++){
			positions[i] = pos + forward * (xVel * time) + new Vector3(0, yVel * time + Physics.gravity.y * .5f * time * time, 0);
			time += segTime;
		}
		line.SetPositions(positions);
	}
	
	//check to see if the player is holding a certain item
	public bool IsHolding(string itemName)
	{
		if(!pickedUp)
			return false;
		
		if(item1.GetComponent<tmpItem>().name!=null &&
			item1.GetComponent<tmpItem>().name == itemName)
			return true;
		else
			return false;
	}
	
	public bool IsHolding(string[] items)
	{
		if(!pickedUp)
			return false;
		
		if(item1.GetComponent<tmpItem>().name == null)
			return false;
		
		for(int i=0; i<items.Length; i++)
		{
			if(item1.GetComponent<tmpItem>().name == items[i])
				return true;
		}
		return false;
	}

}
