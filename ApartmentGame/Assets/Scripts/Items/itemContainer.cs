using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//get the root xml tag to look at
[XmlRoot("ItemCollection")]
public class itemContainer{
	
	//array of items, each item in the array is of type Item
	[XmlArray("Items")]
	[XmlArrayItem("Item")]
	
	//Item list
	public List<Item> items = new List<Item>();

	public static itemContainer Load(string Path){
		
		TextAsset _xml = Resources.Load<TextAsset>(Path);
		
		XmlSerializer serializer = new XmlSerializer(typeof(itemContainer));
		StringReader reader = new StringReader(_xml.text);
		
		itemContainer items = serializer.Deserialize(reader) as itemContainer;
		reader.Close();
		return items;
	}
}
