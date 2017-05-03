using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VolumeGizmo : MonoBehaviour {

	//TEXTURE/SPRITE MUST BE IN GIZMOS FOLDER
	public Texture2D icon;
	public Mesh mesh;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(.5f, .5f, .5f, .1f);
		string iconName;
		if(icon == null){
			iconName = "ButtonThumbstickUpSprite";
		}
		else{
			iconName = icon.name;
		}
		Gizmos.DrawIcon(transform.position, iconName,true);
		RenderGizmo(false);
	}
	void OnDrawGizmosSelected() {
		Gizmos.color = new Color(1, 1f, 0,.25f);
		RenderGizmo(false);
	}
	void RenderGizmo(bool drawWire = true){
		Matrix4x4 rotation = Matrix4x4.TRS(transform.position,transform.rotation,transform.lossyScale);
		Matrix4x4 pushed = Gizmos.matrix;
		Gizmos.matrix = rotation;
		if(drawWire){
			if(mesh == null){
				Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 1f, 1f));
			}
			else{
				Gizmos.DrawWireMesh(mesh);
			}
		}
		else{
			if(mesh == null){
				Gizmos.DrawCube(Vector3.zero, new Vector3(1f, 1f, 1f));
			}
			else{
				Gizmos.DrawMesh(mesh);
			}

		}
		Gizmos.matrix = pushed;
	}
}
