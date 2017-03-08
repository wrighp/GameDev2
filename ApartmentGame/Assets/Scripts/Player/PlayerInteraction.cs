using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
	public float itemPickupDistance = 4f;
	public Transform rightHand;
	public Transform leftHand;
	private bool pickedUp = false;
	GameObject item1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		pickedUp = false;
	}
	void OnTriggerStay(Collider other) {
		if (Input.GetButtonDown ("Fire2") && !pickedUp) {
			var itemScript = other.GetComponentInChildren<tmpItem> ();
			if (itemScript && itemScript.grabbable) {
				if (Vector3.Distance (other.transform.position, transform.position) <= itemPickupDistance) {
					if (item1 != null) {
						item1.transform.parent = null;
						var rb = item1.GetComponent<Rigidbody> ();
						if (rb) {
							rb.detectCollisions = true;
							rb.WakeUp ();
							rb.useGravity = true;
						} else {
							item1.AddComponent<Rigidbody> ();
						}
					}
					Debug.Log ("grabbed " + other.gameObject.name);
					item1 = itemScript.transform.parent.gameObject;

					//item1.transform.position = rightHand.position;
					item1.transform.parent = rightHand;
					item1.transform.localPosition = Vector3.zero;
					var rb2 = item1.GetComponent<Rigidbody>();
					if (rb2) {
						
						rb2.detectCollisions = false;
						rb2.useGravity = false;
						rb2.Sleep ();
						Destroy (rb2);
					}
					pickedUp = true;
				}
			}
		}
	}



}
