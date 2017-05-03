using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

/// <summary>
/// Support for logging output to the console
/// </summary>
public partial class GameEvents{
	
	public void Log(string s){
		Debug.Log(s);
	}

	public void LogWarning(string s){
		Debug.LogWarning(s);
	}

	public void LogError(string s){
		Debug.LogError(s);
	}


	/// <summary>
	/// Does a log of all the strings separated by newlines and starting with bullet string
	/// Primarily used for testing
	/// </summary>
	public void LogBulletPoints(string bullet, string[] lines){
		StringBuilder sb = new StringBuilder ();
		for (int i = 0; i < lines.Length; i++) {
			sb.AppendLine (bullet + " " + lines[i]);
		}
		Debug.Log (sb);
	}
}
