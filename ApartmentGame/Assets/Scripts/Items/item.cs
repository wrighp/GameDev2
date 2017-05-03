using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class Item{
	
	//outer tag
	[XmlAttribute("_name")]
	public string name;
	
	//inner elements of the attribute
	[XmlElement("_attributes")]
	public List<string> _attributes;
	
	//[XmlElement("Durability")]
	//public float durability;
	
	Item(){}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	} 
	
	public bool hasAttribute(string attribute){
		return _attributes.Contains(attribute);
	}
	
	public bool hasAttribute(string[] attributes){
		foreach(string item in attributes){
			if(!hasAttribute(item))
				return false;
		}
		return true;
	}
}
