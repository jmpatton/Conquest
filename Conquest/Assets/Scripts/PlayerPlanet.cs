using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerPlanet : MonoBehaviour {
	//Gives target information that it can then send to the ships.
	private Transform target;
	//Used to send the status of the planet (for now just the ship count) to a gui text.
	public Text status;
	//The number of ships currently in possession.
	public float ships;
	//Allows you to set a ship object.
	public GameObject ship;
	//Sets the location to spawn the ships.
	public Transform shipSpawn;
	//Tells the planet whether it is targeted, or selected, or not.
	public bool isTargeted = false;
	//Tells whether the mouse is over the planet or not.  Used to change the isTargeted variable.
	private bool mouseOn = true;
	public GameObject hightlight;

	void Start () {
		status.text = "";
	}

	void Update () {
		if (ships < 50)
			ships += Time.deltaTime;
		//Checks to see if the planet is targeted.
		if (isTargeted) {
						//If the planet is targeted, outputs the status of the planet to a GUI text.
						status.text = ((int)ships).ToString ();

						//display the highlight circle around planet
						hightlight.active = true;
		} else {
			hightlight.active = false;
		}
		/* Checks whether the planet should send ships or not, based on A. if the mouse is clicked,
		 * B. if the planet has a target (AKA an enemy planet), 
		 * C. if the ship itself is "targeted" by the player, or selected, and
		 * D. if it has any ships to send. */
		if (Input.GetButton ("Fire1") && target && isTargeted && ships > 1 ){

			//int send = ships * ;
			while (ships > 1){
				// This very long line essentially spawns a ship and tells it where to go.
				(Instantiate(ship, shipSpawn.position, shipSpawn.rotation) as GameObject).SendMessage ("changeTarget", target);
				ships--;
			}
		}
		else {
			/* Checks whether to "deselect" or untarget the planet.  
			 * It does this is you click off of the planet and not
			 * on an enemy planet. */
			if (Input.GetButton ("Fire1") && !target && !mouseOn){
				isTargeted = false;
				status.text = "";
			}
		}
	}
	//Allows other scripts to change the target variable.
	void ChangeTarget (Transform tar){
		target = tar;
	}
	//Allows other scripts to set the target to null.
	void RemoveTarget (){
		target = null;
	}

	void OnMouseOver (){
		//Determines whether to set it as the targeted(selected) planet.
		if (Input.GetButton ("Fire1")){
			isTargeted = true;
			mouseOn = true;
		}
	}
	//Changes the mouseOn variable once the mouse leaves the area.  Needed to remove it as the target or selection.
	void OnMouseExit (){
		mouseOn = false;
	}
}
