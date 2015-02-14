using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour {
	private Transform target;
	private NavMeshAgent navComponent;
	// Use this for initialization
	void Start () {
		navComponent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		if (target){
			navComponent.SetDestination(target.position);
		}
	}

	void changeTarget (Transform tar){
		target = tar;
	}
}
