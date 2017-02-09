using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("Node")]
public class Node{
	
	[XmlElement("_ID")]
	public int _ID;
	
	[XmlElement("_text")]
	public string _text;
	
	[XmlElement("_name")]
	public string _name;
	
	[XmlArray("_options")]
	[XmlArrayItem("dialogueOption")]
	public List<dialogueOption> _options;
	
	//for serialization
	public Node() {
		_options = new List<dialogueOption>();
	}
	
	public Node(string text){
		_text = text;
		_options = new List<dialogueOption>();
	}
}
