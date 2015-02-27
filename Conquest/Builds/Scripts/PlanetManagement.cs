using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlanetManagement : MonoBehaviour {

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
	//used to highlight planet
	public GameObject hightlight;
	//used to display shipcount
	private GUIText shipCountText;
	public int productionRate;//determines the how many ships produced per second
	public int shipCapacity;//ship production halts if this limit is reached.

	public Material playerMaterial;
	public Material neutralMaterial;
	public Material enemyMaterial;

	//EnemyTarget
	public GameObject player;
	private GameObject [] players;
	// private GameObject[] planets;
	// Use this for initialization

	// Use this for initialization
	void Start () {
		status.text = "";
		shipCountText = GetComponentInChildren<GUIText> ();//finds the GUIText for the label
		ships = 10;

		if (this.gameObject.tag == "Player")
		{
			this.gameObject.renderer.material = playerMaterial;
		}

		if (this.gameObject.tag == "Neutral")
		{
			this.gameObject.renderer.material = neutralMaterial;
		}

		if (this.gameObject.tag == "Enemy")
		{
			this.gameObject.renderer.material = enemyMaterial;
		}



		// Enemy Target
		players = GameObject.FindGameObjectsWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		// Enemy script
		if (this.gameObject.tag == "Enemy")
		{
			//update text on shipcount label
			shipCountText.text = ((int)ships).ToString ();
			ProduceShips ();
		}
		
		// Neutral script
		if (this.gameObject.tag == "Neutral")
		{
			//update text on shipcount label
			shipCountText.text = ((int)ships).ToString ();
			this.hightlight.SetActive (false);
		}

		// Player script
		if (this.gameObject.tag == "Player")
		{
			//update text on shipcount label
			shipCountText.text = ((int)ships).ToString ();
			ProduceShips ();
			//Checks to see if the planet is targeted.
			if (isTargeted) {
				//If the planet is targeted, outputs the status of the planet to a GUI text.
				status.text = ((int)ships).ToString ();
				
				//display the highlight circle around planet
				hightlight.SetActive(true);
			} else {
				hightlight.SetActive(false);
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
	}
	//Allows other scripts to change the target variable.
	void ChangeTarget (Transform tar){
		//if (this.gameObject.tag == "Player")
		{
			target = tar;
		}
	}
	//Allows other scripts to set the target to null.
	void RemoveTarget (){
		//if (this.gameObject.tag == "Player")
		{
			target = null;
		}
	}
	
	private void ProduceShips ()
	{
		if (ships < shipCapacity) {
			ships += Time.deltaTime * (float)productionRate;
		}
	}



	// Sharred
	void OnMouseOver (){
		//Determines whether to set it as the targeted(selected) planet.
		if (Input.GetButton ("Fire1")&& this.gameObject.tag == "Player"){
			isTargeted = true;
			mouseOn = true;
		}
		/*Checks to see if the player is clicking on the planet. If they are, 
		 * sends the transform information to the player. */
		if (Input.GetButton("Fire1") && this.gameObject.tag != "Player"){
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
		if (this.gameObject.tag != "Player")
		{
			// Calls the player function RemoveTarget which changes the target to null so it no longer targets the planet.
			player.SendMessage ("RemoveTarget");
		}
		
		if (this.gameObject.tag == "Player")
		{
			mouseOn = false;
		}
	}



	//Enemy Target

	void OnTriggerEnter (Collider other){
		// Destroys any ship object it encounters.
		if (other.tag == "Ship" && (other.GetComponent<PlayerShip>().target == transform) && this.gameObject.tag == "Enemy"){

			ships--;
			Destroy (other.gameObject);
			if (ships <= 1)
			{
				this.gameObject.tag = "Player";
				
				this.gameObject.renderer.material = playerMaterial;
				
			}

		}
		
		if (other.tag == "Ship" && (other.GetComponent<PlayerShip>().target == transform) && this.gameObject.tag == "Neutral")
		{
			Destroy (other.gameObject);
			ships--;
			if (ships == 1)
			{
				this.gameObject.tag = "Player";

				this.gameObject.renderer.material = playerMaterial;
			}
		}

		if (other.tag == "Ship" && (other.GetComponent<PlayerShip>().target == transform) && this.gameObject.tag == "Player")
		{
			ships++;
			Destroy (other.gameObject);
		}
	}
}
