using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtCamera : MonoBehaviour {
	
	public Camera camera;
	public Transform target;
	public float yOffset = 0;
	
	// Use this for initialization
	void Start () {
		transform.SetParent(target);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
		
		transform.position = new Vector3(target.position.x, target.position.y+yOffset, 
			target.position.z);
	}
	
	public void setIcon(Sprite newImage)
	{
		transform.Find("Image").gameObject.GetComponent<Image>().sprite = newImage;
	}
}
