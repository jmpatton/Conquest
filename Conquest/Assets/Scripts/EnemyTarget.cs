using UnityEngine;
using System.Collections;

public class EnemyTarget : MonoBehaviour {
	
	public GameObject player;
	private GameObject [] players;
   // private GameObject[] planets;
	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		//players = GameObject.FindGameObjectsWithTag ("Player");
	}

	void OnMouseOver (){
		/*Checks to see if the player is clicking on the planet. If they are, 
		 * sends the transform information to the player. */
		if (Input.GetButton("Fire1")){
			foreach (GameObject i in players){
				if (i.GetComponent<PlayerPlanet>().isTargeted){
					player = i;
					break;
				}
			}
			if (player){
				player.SendMessage ("ChangeTarget", transform);
				player.AddComponent("StartScript");
			}
		}
	}
	void OnMouseExit (){
		// Calls the player function RemoveTarget which changes the target to null so it no longer targets the planet.
		player.SendMessage ("RemoveTarget");
	}
	void OnTriggerEnter (Collider other){
		// Destroys any ship object it encounters.
		if (other.tag == "Ship" && (other.GetComponent<PlayerShip>().target == transform)){
			Destroy (other.gameObject);
		}
	}
}
