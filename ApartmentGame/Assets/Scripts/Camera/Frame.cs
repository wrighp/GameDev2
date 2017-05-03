using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour {
	
	public Transform player;
	public Transform npc;
	public Transform startPos;
	
	Vector3 target;
	
	public float angle;
	public float distance;
	public float yOffset =  0;
	public bool left;
	public float time = 3f;
	
	//player to npc distance
	float P2N;
	//player to camera distance
	float P2C;
	
	float hyp;
	float opp;
	float adj;

	// Use this for initialization
	void Start () {
		//startPos = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnEnable()
	{
		player = GameObject.Find("Player").transform;
		
		hyp = Vector3.Distance(player.position, npc.position);
		opp = hyp * Mathf.Sin(angle * Mathf.Deg2Rad);
		adj = hyp * Mathf.Cos(Mathf.Deg2Rad*angle);
		
		if(left)
		{
			target = new Vector3(player.position.x + (adj*distance),
			transform.position.y + yOffset, 
			npc.position.z - (opp*distance));
		}
		
		else
		{
			target = new Vector3(npc.position.x - (adj*distance),
			transform.position.y + yOffset, 
			player.position.z + (opp*distance));
		}
		
		
		StartCoroutine(Adjust(target));
		
	}
	
	//lerp the camera to the appropriate position from the startPos
	IEnumerator Adjust(Vector3 t)
	{
		float curr =  Time.deltaTime;
		
		float distance = Vector3.Distance(transform.position, t);
		
		while(distance>0.1f)
		{
			curr += Time.deltaTime;
			
			transform.position = Vector3.Lerp(startPos.position, t, curr/time);
			transform.LookAt(npc);
			
			distance = Vector3.Distance(transform.position, t);
			
			yield return null;
		}
		yield return null;
	}
}
