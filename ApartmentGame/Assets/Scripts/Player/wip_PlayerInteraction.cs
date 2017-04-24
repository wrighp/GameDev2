using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wip_PlayerInteraction : MonoBehaviour {

	public Collider parentCollider;
	
	public Transform rightHand;
	public Transform leftHand;
	
	public GameObject item1;
	public GameObject item2;
	
	public float maxAngle = 45f;
	public float force = 10f;
	public float guideTime = 2f; //Number of seconds guide simulates for
	public int guideSegments = 20; //Number of segments guide has


	const float PICKUP_DELAY = .25f; //Time until object can be picked up again after dropped to avoid spamming

	float downTime, upTime, pressTime = 0;
	float droptime = 1f;

	bool pickedUpRight = false;
	bool pickedUpLeft = false;
	
	float cooldown = 0;
	float cooldownL = 0;
	
	bool triggerDown = false; //Was Throw trigger held down last frame
	bool triggerDownLeft = false;
	
	LineRenderer line;
	
	bool aimingR = false;
	bool aimingL = false;

	// Use this for initialization
	void Start () {
		line = GetComponentInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(item1 != null && pickedUpRight){
			//Object will keep moving due to its rigidbody
			item1.transform.position = rightHand.position;
			item1.transform.parent = rightHand;
		}
		
		if(item2 != null && pickedUpLeft)
		{
			item2.transform.position = leftHand.position;
			item2.transform.parent = leftHand;
		}

		if(cooldown>0){
			cooldown -= Time.deltaTime;
		}
		if(cooldown<0){
			cooldown = 0;
		}
		if(cooldownL>0){
			cooldownL -= Time.deltaTime;
		}
		if(cooldownL<0){
			cooldownL = 0;
		}
		
		//if dialogue is running, ignore all of this
		if(!npcDialogue.running)
		{
			if(pickedUpLeft && pickedUpRight)
			{
				if(Input.GetButton("ThrowL") && Input.GetButton("Throw"))
				{
					Debug.Log("Fusion!");
					//pickedUpRight = false;
					//pickedUpLeft = false;
				}
			}
			
			if(pickedUpLeft)
			{
				if(Input.GetButton("ThrowL"))
				{	
					throwLogic("ThrowL", item2, false);
				}
				else{
					aimingL = false;
					line.enabled = false;
				}

				if(/*Input.GetButtonUp("ThrowL")*/Input.GetButtonDown("PickupL")
					&&aimingL)
				{	
					Throw(item2, false);
				}
				
				else if(Input.GetButtonDown("PickupL"))
				{	
					ReleaseItem(item2);
					item2 = null;
				}
			}
			
			if(pickedUpRight)
			{
				if(Input.GetButton("Throw"))
				{
					throwLogic("Throw", item1,true);
				}
				else
				{
					aimingR = false;
					line.enabled = false;
				}
				
				if(/*Input.GetButtonUp("Throw")*/Input.GetButtonDown("Pickup")
					&& aimingR){
					Throw(item1, true);
				}
				
				else if(Input.GetButtonDown("Pickup")){
					ReleaseItem(item1);
					item1 = null;
				}
			}
		}

		//Can only start throwing again if button used to pick up object is released
		if(item1 != null && Input.GetButtonUp("Pickup")){
			pickedUpRight = true;
		}
		if(item2 != null && Input.GetButtonUp("PickupL"))
		{
			pickedUpLeft = true;
		}
		
	}
	
	void throwLogic (string Axis, GameObject item, bool right)
	{
		//Debug.Log(Axis);
		float throwAxis = Input.GetAxis (Axis);
		bool throwReleased = Input.GetButtonUp(Axis);

		MouseOrbitImproved mo = Camera.main.GetComponent<MouseOrbitImproved> ();
		float angle = maxAngle * .5f;
		if (mo != null) {
			angle = Mathf.Deg2Rad * (maxAngle - (Camera.main.transform.eulerAngles.x - mo.yMinLimit) * (maxAngle) / (mo.yMaxLimit - mo.yMinLimit));
		}
		Vector3 throwVector = (transform.forward + new Vector3 (0, angle, 0f)).normalized;

		//Debug.Log("throwthrow");
		ThrowGuide (item, throwVector, transform.forward);
		line.enabled = true;
		aimingR = right;
		aimingL = !right;
	}
	
	void Throw(GameObject item, bool right)
	{
		Rigidbody rb = item.GetComponent<Rigidbody> ();
		
		MouseOrbitImproved mo = Camera.main.GetComponent<MouseOrbitImproved> ();
		float angle = maxAngle * .5f;
		if (mo != null) {
			angle = Mathf.Deg2Rad * (maxAngle - (Camera.main.transform.eulerAngles.x - mo.yMinLimit) * (maxAngle) / (mo.yMaxLimit - mo.yMinLimit));
		}
		Vector3 throwVector = (transform.forward + new Vector3 (0, angle, 0f)).normalized;
		
		Collider childCollider = item.transform.GetComponentInChildren<Collider> ();
		rb.AddForce (throwVector * force, ForceMode.Impulse);		
		StartCoroutine (EnableColliders (.5f, childCollider, parentCollider));
		ReleaseItem (item);
		item = null;

		if (right) {
			item1 = null;
			pickedUpRight = false;
		} else {
			item2 = null;
			pickedUpLeft = false;
		}

		Debug.Log (item);
		rb.freezeRotation = false;
		line.enabled = false;
		
		aimingR = false;
		aimingL = false;
	}


	
	void OnTriggerStay(Collider other) {
		IndicatorOverlay overlay = other.GetComponent<IndicatorOverlay> ();
		if(overlay != null && other.gameObject != item1 && other.gameObject != item2 && 
		overlay.display == false)
		{
			overlay.Enable();
		}
		
		if(other.gameObject != item2 &&
		Input.GetButtonDown("Pickup") && !pickedUpRight && cooldown<0.01f){
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
					pickedUpRight = false;
					cooldown = PICKUP_DELAY;

					if(overlay != null){
						//For displaying icon over object
						overlay.display = false;
					}
				}
			}		
		}
		if(other.gameObject!= item1 && 
		Input.GetButtonDown("PickupL") && !pickedUpLeft && cooldownL<0.01f)
		{
			if(other.GetComponent<tmpItem>()!=null){
				if(other.GetComponent<tmpItem>().grabbable){
					if(item2==null){
						item2 = other.gameObject;
					}
					//Debug.Log("Grabbed!");
					Collider childCollider = item2.transform.GetComponentInChildren<Collider> ();
					//item1.transform.GetChild(0).gameObject.GetComponent<Collider>().enabled = false;
					Physics.IgnoreCollision (childCollider, parentCollider, true);
					item2.transform.position = leftHand.position;
					item2.transform.parent = leftHand;
					item2.transform.localPosition = Vector3.zero;
					Rigidbody rb = item2.GetComponentInChildren<Rigidbody>();
					rb.freezeRotation = true;
					pickedUpLeft = false;
					cooldownL = PICKUP_DELAY;

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

	void ReleaseItem (GameObject item)
	{
		IndicatorOverlay overlay = item.GetComponent<IndicatorOverlay> ();
		if(overlay != null){
			//For displaying icon over object
			overlay.display = true;
		}
		item.transform.parent = null;
		item.GetComponent<Rigidbody>().velocity = Vector3.zero;
		item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		//pickedUpRight = false;
		//cooldown = PICKUP_DELAY;
		if(item == item1)
		{
			pickedUpRight = false;
			cooldown = PICKUP_DELAY;
		}
		else
		{
			pickedUpLeft = false;
			cooldownL = PICKUP_DELAY;
		}
		
		item = null;

	}

	void ThrowGuide(GameObject item, Vector3 throwVector, Vector3 forward){
		//F = M*a
		float angle = throwVector.y;
		float velocity = force / item.GetComponent<Rigidbody>().mass;
		float yVel = velocity * Mathf.Sin(angle);
		float xVel = velocity * Mathf.Cos(angle);
		Vector3 pos = item.transform.position;
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
		//Debug.Log("Checking item "+ itemName);
		if(!pickedUpRight || !pickedUpLeft)
			return false;
		
		if(item1.GetComponent<tmpItem>().name!=null &&
			item1.GetComponent<tmpItem>().name == itemName)
			{
				//Debug.Log("player is holding " + itemName);
				return true;
			}
		else if(item2.GetComponent<tmpItem>().name!=null &&
			item2.GetComponent<tmpItem>().name == itemName)
			{
				//Debug.Log("player is holding " + itemName);
				return true;
			}	
		else
			return false;
	}
	
	public bool IsHolding(string[] items)
	{
		if(!pickedUpRight && !pickedUpLeft)
			return false;
		
		if(pickedUpRight && item1.GetComponent<tmpItem>().name == null)
			return false;
		else
		{
			for(int i=0; i<items.Length; i++)
			{
			if(item1.GetComponent<tmpItem>().name == items[i])
				return true;
			}
		}
		if(pickedUpLeft && item2.GetComponent<tmpItem>().name == null)
			return false;
		else
		{
			for(int i=0; i<items.Length; i++)
			{
			if(item1.GetComponent<tmpItem>().name == items[i])
				return true;
			}
		}
		return false;
	}

}
