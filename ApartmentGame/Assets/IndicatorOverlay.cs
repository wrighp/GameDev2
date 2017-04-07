using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorOverlay : MonoBehaviour {

	public Sprite sourceImage;
	public bool display = false;
	public bool alwaysDisplay = false;
	public float range = float.MaxValue;
	public float yOffset = 0f;
	public Transform followTransform;

	public GameObject indicatorPrefab;
	private GameObject indicator;
	private Image indicatorImage;
	private WorldToScreenUI worldUI;
	// Use this for initialization
	void Start () {
		Transform canvas = GameObject.FindObjectOfType<Canvas> ().transform;
		indicator = (GameObject)GameObject.Instantiate (indicatorPrefab, canvas);
		indicatorImage = indicator.GetComponent<Image> ();
		worldUI = indicator.GetComponent<WorldToScreenUI> ();
		indicatorImage.sprite = sourceImage;
		indicatorImage.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(display || alwaysDisplay){
			indicatorImage.enabled = true;
			worldUI.offset.y = yOffset;
			Transform t = followTransform;
			if(t == null){
				t = transform;
			}
			worldUI.followTransform = t;
		}
		else{
			indicatorImage.enabled = false;
		}
		
	}
	void OnDisable(){
		indicatorImage.enabled = false;
	}
	void OnDestroy(){
		Destroy (indicator);
	}
}
