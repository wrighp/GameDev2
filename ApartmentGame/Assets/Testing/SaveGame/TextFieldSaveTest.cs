using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
/// <summary>
/// Class for testing saving text in textfield (as a test). Goes on the Text portion of an input field.
/// </summary>
[RequireComponent (typeof (InputField))]
public class TextFieldSaveTest : MonoBehaviour {
	private string filename = "Testing/SaveGame/textfieldTest.dat";

	void Start () {
	 
	}
	void Update () {
	
	}
	public void SaveData(){
		String path = Path.Combine(Application.dataPath,filename);
		SaveManager.SaveObject(path,new TextSaveGame(GetComponent<InputField>().text));
	}
	public void LoadData(){
		String path = Path.Combine(Application.dataPath,filename);
		TextSaveGame savegame = (TextSaveGame)SaveManager.LoadObject(path);
		GetComponent<InputField>().text = savegame.text;
	}

	[Serializable] //All fields not marked as NonSerialzed will attempt to be serialized
	class TextSaveGame{
		//Use [NonSerialized] on variables that should not be saved, or are not serializable
		public string text;
		public TextSaveGame(string s) {text = s;}
	}
}
