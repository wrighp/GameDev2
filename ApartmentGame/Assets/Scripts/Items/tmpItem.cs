using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	public bool grabbable = false;
	public bool destroyAfterUse = false;

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
	
	public string getAttribute(){
		return attribute;
	}
	
	void OnTriggerEnter(Collider col){
		if(col.gameObject.GetComponent<tmpItem>() == null)
			return;
		//see note above
		string inAttribute = col.gameObject.GetComponent<tmpItem>().getAttribute();
		//Debug.Log(inAttribute);
		
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
			if(attribute == "Fire"){
				GameObject particle2 = Instantiate(effects[1], 
				new Vector3(transform.position.x, 
					transform.position.y,
					transform.position.z), 
				Quaternion.Euler(-90,0,0)
			);
			particle2.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Foreground";
			Destroy(particle2, 10f);
			}
			if(attribute == "Sparks" && inAttribute=="Wet"){
				GameObject particle2 = Instantiate(effects[1], 
				new Vector3(transform.position.x, 
					transform.position.y-3,
					transform.position.z), 
				Quaternion.Euler(0,0,0)
			);
			//particle2.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Foreground";
			//SceneManager.LoadScene("Lose");
			}
			else if (attribute == "Sparks" && inAttribute == "Rubber"){
				Destroy(this.gameObject);
			}
			
			if(destroyAfterUse)
				Destroy(this.gameObject);
			Destroy(particle, 3f);
		}
	}
	
	void OnTriggerExit(Collider col){
		
	}
	
	void OnTriggerStay(Collider col){
		
	}
}
