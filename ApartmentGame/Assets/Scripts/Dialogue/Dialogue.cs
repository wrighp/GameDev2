using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Dialogue")]
public class Dialogue{
	
	[XmlArray("_nodes")]
	[XmlArrayItem("Node")]
	public List<Node> _nodes;

	public Dialogue(){
		_nodes = new List<Node>();
	}
	
	public void addNode(Node node){
		if (node == null)	return;
		
		_nodes.Add(node);
		node._ID = _nodes.IndexOf(node);
	}
	
	public void addOption(string text, Node start, Node end){
		if(!_nodes.Contains(start))
			addNode(start);
		if(!_nodes.Contains(end))
			addNode(end);
		
		dialogueOption option;
		
		if(end == null)
			option = new dialogueOption(text, -1);
		else
			option = new dialogueOption(text, end._ID);
		
		start._options.Add(option);
		
	}
	
	public static Dialogue Load(string path){
		
		TextAsset _xml = Resources.Load<TextAsset>(path);
		
		XmlSerializer serial = new XmlSerializer(typeof(Dialogue));
		StringReader reader = new StringReader(_xml.text);
		
		Dialogue dialogue = (Dialogue) serial.Deserialize(reader);
		
		reader.Close();
		return dialogue;
	}
}
