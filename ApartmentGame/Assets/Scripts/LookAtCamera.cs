using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtCamera : MonoBehaviour {
	
	//public Camera camera;
	public Transform target;
	public float yOffset = 0;
	
	// Use this for initialization
	void Start () {
		if(target!=null)
			transform.SetParent(target);
		else
		{
			if(transform.parent!=null)
				target = transform.parent;
			else
				target = transform;
		}
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
