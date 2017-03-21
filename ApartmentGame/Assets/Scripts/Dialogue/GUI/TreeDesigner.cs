using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//anything including unityEditor won't be included in the final build
public class TreeDesigner : EditorWindow {
	
	//==========================NODE COMPONENTS===========================
	Dialogue dialogue;
	List<Node> nodes;
	List<dialogueOption> options;
	List<Call> calls;
	
	Node currentNode;
	
	
	//==========================GUI COMPONENTS============================
	//colours and textures
	Texture2D headerSectionTex;
	Texture2D mainSectionTex;
	Texture2D buttonSectionTex;
	
	Color headerSectionCol = new Color(0, 0, 0, 1);
	Color mainSectionCol = new Color(50f/255f, 150f/255f, 200f/255f, 1);
	Color buttonSectionCol = new Color(100f/255f, 10f/255f, 200f/255f, 1);
	
	//sections
	Rect headerSection;
	Rect mainSection;
	Rect buttonSection;
	
	//styles
	static GUIStyle textStyle;
	
	//this describes where the user will be able to access this from
	[MenuItem("Window/Dialogue Tree Designer")]
	static void OpenWindow()
	{
		//textStyle = GUIStyle.none;
		//textStyle.normal.textColor = Color.white;
		
		
		TreeDesigner window = (TreeDesigner)GetWindow(typeof(TreeDesigner));
		//restraining size of window
		window.minSize = new Vector2(300, 600);
		//can also set maxSize
		window.Show();
	}
	
	
	//======================Event Functions (OnEnable, OnGUI) ============
	//similar to start/awake function
	void OnEnable()
	{
		InitTex();
	}
	
	//similar to update(), called 1+ times per interaction, not once per frame
	void OnGUI()
	{
		DrawHeader();
		DrawLayouts();
		DrawButtons();
		DrawBody();
	}
	
	//===================================================
	//initialize texture 2Ds
	void InitTex()
	{
		//method 1 of defining textures
		headerSectionTex = new Texture2D(1,1);
		headerSectionTex.SetPixel(0, 0, headerSectionCol);
		headerSectionTex.Apply();
		
		mainSectionTex = new Texture2D(1, 1);
		mainSectionTex.SetPixel(0, 0, mainSectionCol);
		mainSectionTex.Apply();
		
		buttonSectionTex = new Texture2D(1, 1);
		buttonSectionTex.SetPixel(0, 0, buttonSectionCol);
		buttonSectionTex.Apply();
		
		//other method is making a sprite outside of this (sprite) 
		/*
		texname = Resources.Load<Texture2D>("path")
		then do gui.draw
		*/
	}
	
	//define the rect values and draw based on those rects
	void DrawLayouts()
	{
		//x, y, width, height
		headerSection.x = 0; /* in top left at all times*/
		headerSection.y = 0;
		headerSection.width = Screen.width;
		headerSection.height = 50;
		
		//button section
		buttonSection.x = 0;
		buttonSection.y = 50;
		buttonSection.width = Screen.width/4;
		buttonSection.height = Screen.height-50;
		
		//main section => where all the nodes will be shown
		mainSection.x = Screen.width/4;
		mainSection.y = 50;
		mainSection.width = Screen.width;
		mainSection.height = Screen.height-50;
		
		//pass it to GUI
		GUI.DrawTexture(headerSection, headerSectionTex);
		GUI.DrawTexture(mainSection, mainSectionTex);
		GUI.DrawTexture(buttonSection, buttonSectionTex);
	}
	
	void DrawHeader()
	{
		//like open braces
		GUILayout.BeginArea(headerSection);
		
		GUILayout.Label("Dialogue Tree Designer"/*, textStyle*/);
		
		//like close braces
		GUILayout.EndArea();
	}
	
	void DrawBody()
	{
		GUILayout.BeginArea(mainSection);
		
		
		GUILayout.EndArea();
	}
	
	//handles all of the buttons
	void DrawButtons()
	{
		GUILayout.BeginArea(buttonSection);
		
		
		if(GUILayout.Button("Create Node", GUILayout.Height(40)))
		{
			NodeSettings.OpenWindow();
		}
		
		if(GUILayout.Button("Edit Node", GUILayout.Height(40)))
		{
			NodeSettings.OpenWindow();
		}
		
		if(GUILayout.Button("Delete Node", GUILayout.Height(40)))
		{
			NodeSettings.OpenWindow();
		}
		
		if(GUILayout.Button("Load Tree", GUILayout.Height(40)))
		{
			TreeManager.OpenWindow(false, dialogue);
		}
		
		if(GUILayout.Button("Save Tree", GUILayout.Height(40)))
		{
			TreeManager.OpenWindow(true, dialogue);
		}
		
		GUILayout.EndArea();
	}
	
	//=======================More Node Components======================
	//put load here
	
	//update the state of the tree
	//clear everything that's been drawn and redraw the tree
	static public void UpdateTree()
	{
		
		
		
	}
	
}

//class for node editor
public class NodeSettings : EditorWindow
{
	static NodeSettings window;
	
	//for opening the window
	static public void OpenWindow()
	{
		window = (NodeSettings)GetWindow(typeof(NodeSettings));
		window.minSize = new Vector2(300, 300);
		window.Show();
		
	}
	
	void OnGUI()
	{

	}
}

//class for Tree loading and saving
public class TreeManager : EditorWindow
{
	static TreeManager window;
	static Dialogue dialogue;
	static bool saving;
	static string path;
	
	//for opening the window
	static public void OpenWindow(bool s, Dialogue d = null)
	{
		saving = s;
		dialogue = d;
		window = (TreeManager)GetWindow(typeof(TreeManager));
		window.minSize = new Vector2(300, 300);
		window.Show();
		
	}
	
	static public Dialogue Load(string p)
	{
		Debug.Log(p);
		
		string path = "tmp";
		return (new Dialogue());
		//return Dialogue.Load(path);
	}
	
	static public void Save(string p)
	{
		Debug.Log(p);
		//dialogue.Save(path);
	}
	
	void OnGUI()
	{
		if(saving)
			DrawSave();
		else
			DrawLoad();
	}
	
	void DrawSave()
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Save Path");
		EditorGUILayout.EndHorizontal();
		//enter path
		path = EditorGUILayout.TextField(path);
		
		if(GUILayout.Button("Save", GUILayout.Height(40)))
		{
			Save(path);
			//Debug.Log("Saved to " +path);
		}
	}
	void DrawLoad()
	{
		//Debug.Log("We're Loading");
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Load Path");
		EditorGUILayout.EndHorizontal();
		//enter path
		path = EditorGUILayout.TextField(path);
		
		if(GUILayout.Button("Load", GUILayout.Height(40)))
		{
			dialogue = Load(path);
			//Debug.Log("Loaded " + path);
			TreeDesigner.UpdateTree();
		}
	}
}





