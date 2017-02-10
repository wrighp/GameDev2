using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
/*
	Contains the dialogue, the character speaking and 
	a list of dialogueOption objects for choices
	as well as the list of methods and arguments for said methods
*/
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
	
	[XmlArray("_calls")]
	[XmlArrayItem("Call")]
	public List<Call> _calls;
	
	//for serialization
	public Node() {
		_options = new List<dialogueOption>();
	}
	
	public Node(string text){
		_text = text;
		_options = new List<dialogueOption>();
	}
}
