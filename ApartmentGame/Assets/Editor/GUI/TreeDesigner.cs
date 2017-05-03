using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//anything including unityEditor won't be included in the final build
public class TreeDesigner : EditorWindow {
	
	//==========================Dialogue Tree Components===========================
	static Dialogue dialogue = new Dialogue();
	static List<Node> nodes = new List<Node>();
	static List<dialogueOption> options = new List<dialogueOption>();
	static bool redrawLinks = false;
	//List<Call> calls;
	
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
	
	//============================Node Editor Components===================
	//index'd the same way as the nodes
	static List<Rect> windows = new List<Rect>();
    List<int> windowsToAttach = new List<int>();
    List<int> attachedWindows = new List<int>();
	
	static Dictionary<int, List<int>> nodeConnections = new Dictionary<int, List<int>>();
	
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
		dialogue = new Dialogue();
		nodes = new List<Node>();
		options = new List<dialogueOption>();
		windows = new List<Rect>();
		nodeConnections = new Dictionary<int, List<int>>();
		
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
		
		//if windowstotattach is 2 and the connecting node isn't parented to that
		if (windowsToAttach.Count == 2 && 
			!(nodeConnections[windowsToAttach[0]].Contains(windowsToAttach[1]))) 
		{
            attachedWindows.Add(windowsToAttach[0]);
            attachedWindows.Add(windowsToAttach[1]);
			
			//create new dialogueOption
			//then open up dialogue window editor
			dialogueOption opt = new dialogueOption();
			OptionSettings.OpenWindow(opt, nodes[windowsToAttach[0]]);
			opt._dest = windowsToAttach[1];
			
			//add the dialogue option to the thingy thing
			nodes[windowsToAttach[0]]._options.Add(opt);
			
			nodeConnections[windowsToAttach[0]].Add(windowsToAttach[1]);
			
            windowsToAttach = new List<int>();
        }
		
		if (attachedWindows.Count >= 2) {
            for (int i = 0; i < attachedWindows.Count; i += 2) {
                DrawNodeCurve(windows[attachedWindows[i]], windows[attachedWindows[i + 1]]);
            }
        }
		
		if(redrawLinks)
		{
			Debug.Log("Redrawing");
			for(int i = 0; i<nodes.Count;i++)
			{
				//Debug.Log("Node " + i);
				for(int j=0; j<nodeConnections[i].Count; j++)
				{
					//Debug.Log("EY");
					DrawNodeCurve(windows[i], windows[nodeConnections[i][j]]);
				}
			}
			redrawLinks = false;
		}
		
		//for the node editor
		//mark beginning area for all popup windows
		BeginWindows();
		
		//iterate over the contained windows and draw them
        for (int i = 0; i < windows.Count; i++) {
            windows[i] = GUI.Window(i, windows[i], DrawNodeWindow, "Node " + i);
        }
 
        EndWindows();
		
		GUILayout.EndArea();
	}
	
	//draw the node window
	//change this to edit node instead
	void DrawNodeWindow(int id) {
		//edit the node
		if (GUILayout.Button("Edit Node")) {
            //windowsToAttach.Add(id);
			NodeSettings.OpenWindow(nodes[id]);
        }
		//attach to other nodes
        if (GUILayout.Button("Attach")) {
            windowsToAttach.Add(id);
        }
 
        GUI.DragWindow();
    }
	
	//node curve
	static void DrawNodeCurve(Rect start, Rect end) {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
 
        for (int i = 0; i < 3; i++) {// Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }
 
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
	
	//handles all of the buttons
	void DrawButtons()
	{
		GUILayout.BeginArea(buttonSection);
		
		//add a new node and window to the main area
		if(GUILayout.Button("Add Node", GUILayout.Height(40)))
		{
			Node tmp = new Node("");
			tmp._ID = nodes.Count;
			tmp._options = new List<dialogueOption>();
			tmp._precalls = new List<Call>();
			tmp._postcalls = new List<Call>();
			
			nodes.Add(tmp);
			//maybe remove?
			dialogue.addNode(tmp);
			windows.Add(new Rect(10, 10, 100, 100));
			//NodeSettings.OpenWindow();
			nodeConnections[tmp._ID] = new List<int>();
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
		
		if(GUILayout.Button("Close", GUILayout.Height(40)))
		{
			this.Close();
		}
		
		GUILayout.EndArea();
	}
	
	//=======================More Node Components======================
	//put load here
	
	//update the state of the tree
	//clear everything that's been drawn and redraw the tree
	//go through the whole list of nodes and draw the stuff from there
	static public void UpdateTree(Dialogue d)
	{
		redrawLinks = true;
		
		dialogue = d;
		//Debug.Log(dialogue._nodes.Count);
		//reset the node lists
		//this could actually take a while, optimize
		nodes = dialogue._nodes;
		options = new List<dialogueOption>();
		
		windows = new List<Rect>();
		nodeConnections = new Dictionary<int, List<int>>();
		
		//attach the windows
		for(int i=0; i<dialogue._nodes.Count; i++)
		{
			//Debug.Log("AAAA");
			windows.Add(new Rect(10, 10, 100, 100));
			nodeConnections[i] = new List<int>();
		}
		
		//iterate and draw node curves and remake the node connection array
		for(int i=0; i<dialogue._nodes.Count; i++)
		{
			for(int j=0; j<dialogue._nodes[i]._options.Count; j++)
			{
				if(nodes[i]._options[j]._dest!=-1)
				{
					//DrawNodeCurve(windows[i], windows[nodes[i]._options[j]._dest]);
					nodeConnections[i].Add(nodes[i]._options[j]._dest);
				}
			}
		}
	}
	
}

//class for node editor
public class NodeSettings : EditorWindow
{
	static NodeSettings window;
	
	//members
	static Node node;
	
	static string text;
	static string name;
	static string accomplish;
	static int reset;
	
	//list of pre and post calls
	static List<Call> precalls;
	static List<Call> postcalls;
	
	static List<dialogueOption> options;
	
	//============================ Editor Components===================
	//index'd the same way as the options
	static List<Rect> windows;
	
	Color mainSectionCol = new Color(50f/255f, 150f/255f, 200f/255f, 1);
	Color infoSectionCol = new Color(1f, 1f, 1f, 1);
	
	
	Rect mainSection;
	Rect infoSection;
	
	
	Texture2D mainSectionTex;
	Texture2D infoSectionTex;
	
	void OnEnable()
	{
		InitTex();
	}
	
	void InitTex()
	{
		mainSectionTex = new Texture2D(1, 1);
		mainSectionTex.SetPixel(0, 0, mainSectionCol);
		mainSectionTex.Apply();
		
		infoSectionTex = new Texture2D(1, 1);
		infoSectionTex.SetPixel(0, 0, infoSectionCol);
		infoSectionTex.Apply();
	}
	
	//for opening the window
	static public void OpenWindow(Node n = null)
	{
		node = n;
		
		if(node!=null)
		{
			text = node._text;
			name = node._name;
			accomplish = node._accomplish;
			reset = node._reset;
			
			precalls = node._precalls;
			postcalls = node._postcalls;
			
			options = node._options;
		}
		windows = new List<Rect>();
		for(int i=0; i<node._options.Count; i++)
		{
			windows.Add(new Rect(10, 10, 100, 100));
		}
		
		window = (NodeSettings)GetWindow(typeof(NodeSettings));
		window.minSize = new Vector2(300, 300);
		window.Show();
		
	}
	//draw shit for the node
	void OnGUI()
	{
		DrawLayouts();
		DrawNode();
		DrawBody();
	}
	
	void DrawLayouts()
	{
		//main section => where all the nodes will be shown
		mainSection.x = Screen.width/4;
		mainSection.y = 50;
		mainSection.width = Screen.width;
		mainSection.height = Screen.height-50;
		
		//button section
		infoSection.x = 0;
		infoSection.y = 50;
		infoSection.width = Screen.width/4;
		infoSection.height = Screen.height-50;
		
		//pass it to GUI
		GUI.DrawTexture(mainSection, mainSectionTex);
		GUI.DrawTexture(infoSection, infoSectionTex);
	}
	
	void DrawBody()
	{
		GUILayout.BeginArea(mainSection);
		
		//for the node editor
		//mark beginning area for all popup windows
		BeginWindows();
		
		//iterate over the contained windows and draw them
        for (int i = 0; i < windows.Count; i++) {
            windows[i] = GUI.Window(i, windows[i], DrawOptionWindow, options[i]._text);
        }
 
        EndWindows();
		
		GUILayout.EndArea();
	}
	
	void DrawNode()
	{
		GUILayout.BeginArea(infoSection);
		//a field for each of them
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Name");
		EditorGUILayout.EndHorizontal();
		//enter data
		name = EditorGUILayout.TextField(name);
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Text");
		EditorGUILayout.EndHorizontal();
		//enter data
		text = EditorGUILayout.TextField(text);
		
		//probably bringing this inside the main gui later
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Reset Node");
		EditorGUILayout.EndHorizontal();
		//enter data
		reset = EditorGUILayout.IntField(reset);
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Accomplishment");
		EditorGUILayout.EndHorizontal();
		//enter data
		accomplish = EditorGUILayout.TextField(accomplish);
		
		
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Precalls");
		EditorGUILayout.EndHorizontal();

		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Postcalls");
		EditorGUILayout.EndHorizontal();
		
		//have a section here for all of the dialogue options available
		//from that node
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Dialogue Options");
		EditorGUILayout.EndHorizontal();
		
		
		if(GUILayout.Button("Save", GUILayout.Height(40)))
		{
			node._text = text;
			node._name = name;
			node._accomplish = accomplish;
			node._reset = reset;
			node._options = options;
			//Debug.Log("Saved to " +path);
			this.Close();
		}
		
		GUILayout.EndArea();
	}
	
	//draw the node window
	//change this to edit node instead
	void DrawOptionWindow(int id) {
		//edit the node
		if (GUILayout.Button("Edit Info")) {
            //windowsToAttach.Add(id);
			OptionSettings.OpenWindow(options[id], node);
        }
 
        GUI.DragWindow();
    }
}

public class OptionSettings : EditorWindow
{
	static OptionSettings window;
	
	//members
	static dialogueOption option;
	static Node parent;
	
	static string text;
	static string req;
	static int dest;
	

	
	//for opening the window for the first time
	public static void OpenWindow(dialogueOption o = null, Node p = null)
	{
		option = o;
		parent = p;
		
		if(option!=null)
		{
			//set values
			text = option._text;
			req = option._req;
			dest = option._dest;
		}
		window = (OptionSettings)GetWindow(typeof(OptionSettings));
		window.minSize = new Vector2(300, 300);
		window.Show();
		
	}
	
	//for opening the window any other time
	public void OpenWindow()
	{
		window = (OptionSettings)GetWindow(typeof(OptionSettings));
		window.minSize = new Vector2(300, 300);
		window.Show();
	}
	//draw shit for the node
	void OnGUI()
	{
		DrawOption();
	}
	
	void DrawOption()
	{

		text = EditorGUILayout.TextField("Text: ", text);
		dest = EditorGUILayout.IntField("Destination Node: ", dest);
		req = EditorGUILayout.TextField("Requirement :", req);

		if(GUILayout.Button("Save", GUILayout.Height(40)))
		{
			option._text = text;
			option._req = req;
			this.Close();
		}
		
		if(GUILayout.Button("Remove Option", GUILayout.Height(40)))
		{
			parent._options.Remove(option);
			this.Close();
		}
		
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
		//Debug.Log(p);
		//return (new Dialogue());
		return Dialogue.Load(p);
	}
	
	static public void Save(string p)
	{
		dialogue.Save(p);
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
			dialogueOption opt;
			//make sure the nodes have an exit
			for(int i=0; i<dialogue._nodes.Count; i++)
			{
				if(dialogue._nodes[i]._options.Count == 0)
				{
					opt = new dialogueOption();
					opt._dest = -1;
					dialogue._nodes[i]._options.Add(opt);
				}
			}
			Save(path);
			this.Close();
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
			//Debug.Log(dialogue._nodes.Count);
			//Debug.Log("Loaded " + path);
			TreeDesigner.UpdateTree(dialogue);
			this.Close();
		}
	}
}





