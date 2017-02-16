using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Starts pulling or pushing object around if target transform is too closer or too far
/// </summary>
public class PullFollow : MonoBehaviour {

	public Transform target;
	public float minDistance = 8f;
	public float maxDistance = 10f;
	public float followSpeed = 1f;
	public float height = 5f;
	private Vector3 goal;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate(){
		if (target != null) {
			Vector3 targetFlat = new Vector3 (target.position.x, 0, target.position.z);
			Vector3 posFlat = new Vector3 (transform.position.x, 0, transform.position.z);

			float dist = Vector3.Distance (targetFlat, posFlat);

			if (dist < minDistance) {
				SetGoal (targetFlat, posFlat, minDistance);
			} else if (dist > maxDistance) {
				SetGoal (targetFlat, posFlat, maxDistance);
			}
			goal.y = target.position.y + height;
			transform.position = Vector3.Slerp (transform.position, goal, Time.deltaTime / followSpeed);
		}
	}

	private void SetGoal(Vector3 targetFlat, Vector3 posFlat, float distance){
		goal = target.position;
		Vector3 direction = (posFlat - targetFlat).normalized;
		goal += direction * distance;
	}
	void OnPreCull(){
		//For removing roll of camera
		transform.rotation = Quaternion.Euler(Vector3.Scale(transform.rotation.eulerAngles, new Vector3(1f,1f,0)));
	}
}
