using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("dialogueOption")]
public class dialogueOption{
	
	[XmlElement("_text")]
	public string _text;
	
	[XmlElement("_dest")]
	public int _dest;
	
	dialogueOption() {}
	
	public dialogueOption(string text, int dest){
		_text = text;
		_dest = dest;
	}
}
