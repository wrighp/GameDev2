using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// Static class for saving and loading binary objects. See TextFieldSaveTest for an example of how to save and load objects.
/// </summary>
public static class SaveManager {
	public static  SurrogateSelector Selector = new SurrogateSelector();
	static SaveManager(){
		//Custom surrogotes for non-natively serializable classes can be added here
		Selector.AddSurrogate(typeof(Color32),new StreamingContext(StreamingContextStates.All), new Color32SerializationSurrogate());
	}

	public static bool SaveObject(string path, object graph){
		BinaryFormatter formatter = new BinaryFormatter();
		using (FileStream stream = new FileStream(path, FileMode.Create))
		{
			formatter.SurrogateSelector = Selector;
			try
			{
				formatter.Serialize(stream,graph);
			}
			catch (IOException)
			{
				Debug.LogErrorFormat("Could not save file: {0}",path);
				return false;
			}
		}
		Debug.LogFormat("File saved: {0}",path);
		return true;
	}
	public static object LoadObject(string path)
	{
		if (!File.Exists(path))
		{
			Debug.LogErrorFormat("Could not find file to load: {0}",path);
			return null;
		}

		BinaryFormatter formatter = new BinaryFormatter();

		using (FileStream stream = new FileStream(path, FileMode.Open))
		{
			formatter.SurrogateSelector = Selector;
			try
			{
				Debug.LogFormat("Loading file: {0}",path);
				return formatter.Deserialize(stream);
			}
			catch (IOException)
			{
				Debug.LogErrorFormat("Could not load file: {0}",path);
				return null;
			}
		}
	}
	public static void DeleteGame(string path){
		if (File.Exists(path))
		{
			Debug.LogFormat("File deleted: {0}",path);
			File.Delete(path);
		}
		else{
			Debug.LogErrorFormat("Could not find file to delete: {0}",path);
		}
	}
}
