using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("Call")]
public class Call{
	
	/*
		list of arguments
		list of lists
		add block=true
	*/
	
	
	[XmlAttribute]
	public string _m;
	
	[XmlElement("_arg")]
	public List<string> _arguments;
	
	[XmlArray("_lst")]
	[XmlArrayItem("Lst")]
	public List<Lst> _lst;
	
	Call() {}
	
	//execute the function
	public void execute(){
		var obs = new List<object>();
		for(int i=0;i<_arguments.Count;i++){
			obs.Add(_arguments[i]);	
		}
		
		for(int i=0;i<_lst.Count;i++){
			obs.Add(_lst[i]._s.ToArray());	
		}
		
		GameEvents.Call(_m, obs.ToArray());
	}
	
};

[XmlRoot("Lst")]
public class Lst{

		[XmlElement("_s")]
		public List<string> _s;
		
		Lst(){}
	
	
}
