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
	//private Image indicatorImage;
	private WorldToScreenUI worldUI;
	// Use this for initialization
	void Start () {
		//Transform canvas = GameObject.FindObjectOfType<Canvas> ().transform;
		//indicator = (GameObject)GameObject.Instantiate (indicatorPrefab, canvas);
		indicator = Instantiate (indicatorPrefab, transform);
		indicator.GetComponent<LookAtCamera>().setIcon(sourceImage);
		indicator.GetComponent<LookAtCamera>().target = transform;
		indicator.GetComponent<LookAtCamera>().yOffset = yOffset;
		//worldUI = indicator.GetComponent<WorldToScreenUI> ();
		indicator.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(display || alwaysDisplay){
			//worldUI.image = sourceImage;
			indicator.SetActive(true);
			//worldUI.offset.y = yOffset;
			Transform t = followTransform;
			if(t == null){
				t = transform;
			}
			//worldUI.followTransform = t;
		}
		else{
			indicator.SetActive(false);
		}
		
	}
	void OnDisable(){
		if(indicator){
			indicator.SetActive(false);
		}
	}
	void OnDestroy(){
		Destroy (indicator);
	}
}
