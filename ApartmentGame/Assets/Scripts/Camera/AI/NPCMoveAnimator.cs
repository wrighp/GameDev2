using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMoveAnimator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		NavMeshAgent navAgent = GetComponentInChildren<NavMeshAgent> ();
		if (navAgent != null) {
			Animator anim = GetComponentInChildren<Animator> ();
			anim.SetFloat("MoveSpeed",navAgent.velocity.magnitude / navAgent.speed);
		}
	}
}
