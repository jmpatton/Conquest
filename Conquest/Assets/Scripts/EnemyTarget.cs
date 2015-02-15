using UnityEngine;
using System.Collections;

public class EnemyTarget : MonoBehaviour {
	
	public GameObject player;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver (){
		/*Checks to see if the player is clicking on the planet. If they are, 
		 * sends the transform information to the player. */
		if (Input.GetButton("Fire1")){
			player.SendMessage ("ChangeTarget", transform);
		}
	}
	void OnMouseExit (){
		// Calls the player function RemoveTarget which changes the target to null so it no longer targets the planet.
		player.SendMessage ("RemoveTarget");
	}
	void OnTriggerEnter (Collider other){
		// Destroys any ship object it encounters.
		if (other.tag == "Ship"){
			Destroy (other.gameObject);
		}
	}
}
