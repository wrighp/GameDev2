using UnityEngine;
using System.Collections;

//load up the items into the list
public class itemLoader : MonoBehaviour {
	//in resources folder, check Load in itemContainer
	public const string path = "items";
	// Use this for initialization
	void Start () {
		itemContainer ic = itemContainer.Load(path);
		
		foreach ( Item item in ic.items){
			print(item.name);
		}
	}
}
