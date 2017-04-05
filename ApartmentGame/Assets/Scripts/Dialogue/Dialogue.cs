using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//get the root tag with the information you want to deserialize
[XmlRoot("Dialogue")]
public class Dialogue{
	
	//list of Node objects under _nodes
	[XmlArray("_nodes")]
	[XmlArrayItem("Node")]
	public List<Node> _nodes;
	
	[XmlElement("_tasks")]
	public List<string> _tasks;
	//where to start off the conversation the next time this
	//npc is spoken to
	public int _next = 0;
	
	public Dialogue(){
		_nodes = new List<Node>();
	}
	
	public void addNode(Node node)
	{
		_nodes.Add(node);
	}
	
	//static dialogue loader for use of all instances of Dialogue
	public static Dialogue Load(string path){
		
		TextAsset _xml = Resources.Load<TextAsset>(path);
		XmlDocument xmldoc = new XmlDocument();
		xmldoc.LoadXml(_xml.text);
		
		//_xml = (TextAsset) xmldoc;
		
		XmlSerializer serial = new XmlSerializer(typeof(Dialogue));
		StringReader reader = new StringReader(_xml.text);
		
		Dialogue dialogue = (Dialogue) serial.Deserialize(reader);
		
		reader.Close();
		return dialogue;
	}
	
	//serialize the dialogue 
	public void Save(string path)
	{
		var serializer = new XmlSerializer(typeof(Dialogue));
 		using(var stream = new FileStream(path, FileMode.Create))
 		{
 			serializer.Serialize(stream, this);
 		}
	}
}
