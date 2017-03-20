using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//anything including unityEditor won't be included in the final build
public class TreeDesigner : EditorWindow {
	
	//colours and textures
	Texture2D headerSectionTex;
	Texture2D mainSectionTex;
	
	Color headerSectionCol = new Color(0, 0, 0, 1);
	Color mainSectionCol = new Color(50f/255f, 150f/255f, 200f/255f, 1);
	
	Rect headerSection;
	Rect mainSection;
	
	//this describes where the user will be able to access this from
	[MenuItem("Window/Dialogue Tree Designer")]
	static void OpenWindow()
	{
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
		
		//other method is making a sprite outside of this (sprite) 
		/*
		texname = Resources.Load<Texture2D>("path")
		then do gui.draw
		*/
	}
	
	void DrawHeader()
	{
		//like open braces
		GUILayout.BeginArea(headerSection);
		
		GUILayout.Label("Dialogue Tree Designer");
		
		//like close braces
		GUILayout.EndArea();
	}
	
	//define the rect values and draw based on those rects
	void DrawLayouts()
	{
		//x, y, width, height
		headerSection.x = 0; /* in top left at all times*/
		headerSection.y = 0;
		headerSection.width = Screen.width;
		headerSection.height = 50;
		
		mainSection.x = 0;
		mainSection.y = 50;
		mainSection.width = Screen.width;
		mainSection.height = Screen.height-50;
		
		//pass it to GUI
		GUI.DrawTexture(headerSection, headerSectionTex);
		GUI.DrawTexture(mainSection, mainSectionTex);
	}
	
	void DrawBody()
	{
		GUILayout.BeginArea(mainSection);
		
		
		GUILayout.EndArea();
	}
	
}
