using UnityEngine;
using System.Collections;

public class HomePlanet : MonoBehaviour {
	private Transform target;
	//private Transform nullTarget;
	public GameObject ship;
	public Transform shipSpawn;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Fire1") && target){
			(Instantiate(ship, shipSpawn.position, shipSpawn.rotation) as GameObject).SendMessage ("changeTarget", target);
			//ship.SendMessage("changeTarget",target);
		}
	}
	
	void changeTarget (Transform tar){
		target = tar;
	}
	void removeTarget (){
		target = null;
	}
}
