using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("dialogueOption")]
public class dialogueOption{
	
	//display text for dialogue option
	[XmlElement("_text")]
	public string _text;
	
	//the index of the destination node
	[XmlElement("_dest")]
	public int _dest;
	
	//the requirement for this node to be displayed
	[XmlElement("_req")]
	public string _req;
	
	dialogueOption() {}
	
	public dialogueOption(string text, int dest){
		_text = text;
		_dest = dest;
	}
}
