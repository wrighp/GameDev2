using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpItem : MonoBehaviour {
	
	//for the time being, every item has a single attribute,
	//a list of effective attributes a particle effect to spawn once it collides with 
	//an object with said effective attributes
	
	//NOTE
	//eventually expand to something more genral like "melt" or "freeze"
	//and instantiate the corresponding effect
	
	//Also, account for the possibility that nothing may be spawned
	//for certain occurrences
	
	//this objects attribute(s)
	public string attribute;
	
	//particle system or objects to instantiate when certain
	//incoming attributes collide
	public GameObject[] effects;
	public List<string> affects;
	
	private Dictionary<string, GameObject> dictionary;

	// Use this for initialization
	void Start () {
		
		dictionary = new Dictionary<string, GameObject>();
		
		for(int i=0; i<affects.Count; i++){
			dictionary[affects[i]] = effects[i];
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	string getAttribute(){
		return attribute;
	}
	
	void OnTriggerEnter(Collider col){
		if(col.gameObject.GetComponent<tmpItem>() == null)
			return;
		//see note above
		string inAttribute = col.gameObject.GetComponent<tmpItem>().getAttribute();
		Debug.Log(inAttribute);
		
		//instead of this, IE call "melt" function which would destroy
		//bot this and the colliding object
		//or squish that would only destroy this one
		
		if(affects.Count!=0 && affects.Contains(inAttribute)){
			GameObject particle = Instantiate(dictionary[inAttribute], 
				new Vector3(transform.position.x, 
					transform.position.y,
					transform.position.z), 
				Quaternion.Euler(0,0,0)
			);
			Destroy(this.gameObject);
			Destroy(particle, 3f);
		}
	}
	
	void OnTriggerExit(Collider col){
		
	}
	
	void OnTriggerStay(Collider col){
		
	}
}
