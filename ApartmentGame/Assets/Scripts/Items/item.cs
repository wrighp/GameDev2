using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Item{
	
	//outer tag
	[XmlAttribute("Name")]
	public string name;
	
	//inner elements of the attribute
	[XmlElement("Damage")]
	public float damage;
	
	[XmlElement("Durability")]
	public float durability;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
