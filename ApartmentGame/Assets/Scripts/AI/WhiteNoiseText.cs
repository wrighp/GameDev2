using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteNoiseText : MonoBehaviour {
	
	bool move = true;
	public Transform moveTarget;
	public float speed = 3f;
	public float maxT = 0.5f;
	
	public float distance;
	public float farDistance;
	
	private Text whitenoiseText;
	private float yPlane;

	// Use this for initialization
	void Start () {
		yPlane = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
		float D2T = Mathf.Clamp(Vector3.Distance(transform.position, moveTarget.position), 0, farDistance);
		
		if(transform.childCount!=0 && whitenoiseText == null)
		{
			whitenoiseText = GetComponentInChildren<Text>();
		}
		
		float step = speed * Time.deltaTime;
		
		if(D2T > distance && D2T < farDistance && move)
		{
			transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, step);
			transform.position = new Vector3(transform.position.x, yPlane, transform.position.z);
		}
		else
		{
			Vector3 orbit = new Vector3(transform.position.x + Mathf.Sin(Time.time),
				transform.position.y + Mathf.Cos(Time.time), transform.position.z + Mathf.Cos(Time.time));
			transform.position = Vector3.MoveTowards(transform.position, orbit, step);
		}
		
		if(whitenoiseText!=null)
		{
			Color tmp = new Color(whitenoiseText.color.r, whitenoiseText.color.g,
				whitenoiseText.color.b, Mathf.Lerp(0, maxT, distance/D2T));
			whitenoiseText.color = tmp;
		}
		
	}
}
