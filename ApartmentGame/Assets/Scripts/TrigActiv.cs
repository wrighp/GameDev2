using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigActiv : MonoBehaviour {

	public GameObject target;
	
	void OnTriggerEnter(Collider col)
	{
		if ( col.tag == "Player" )
		{
			target.SetActive(true);
			Destroy(gameObject);
		}
	}
}
