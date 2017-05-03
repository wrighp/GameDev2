using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;

/// <summary>
/// GameEvents is a collection of methods to be invoked through reflection
/// To add functions, create a new .cs file with a class header of:
/// 	public partial class GameEvents{
/// </summary>
public partial class GameEvents{
	public static GameEvents Instance { get; private set;}

	static GameEvents(){
		Instance = new GameEvents ();
	}
		
	public static void Call(string methodName, params object[] args){
		MethodInfo methodInfo = typeof(GameEvents).GetMethod (methodName);
		if (methodInfo == null) {
			Debug.LogErrorFormat ("Method {0} does not exist!", methodName);
			return;
		}
		try{
			methodInfo.Invoke (Instance, args);
		}catch (Exception ex){
			if (ex is TargetParameterCountException) {
				Debug.LogErrorFormat ("Method {0} improper argument count. Had {1} needed {2}", methodName, args.Length, methodInfo.GetParameters().Length);
			}
			else if(ex is ArgumentException){
				Debug.LogErrorFormat ("Method {0} could not convert parameters. Make sure parameter types match (string or array of strings)", methodName);
					
			}
			Debug.LogException (ex);
		}
	}

}
