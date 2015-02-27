using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerPlanet : MonoBehaviour {
    //Creates a list for all the planets.
    public List<GameObject> planets = new List<GameObject>();
    //Sets a target for enemy
    public GameObject player;
	//Gives target information that it can then send to the ships.
	private Transform target;
	//The number of ships currently in possession.
	public float ships;
    public float initialShips;
	//Allows you to set a ship object.
	public GameObject ship;
    public GameObject enemyShip;
	//Sets the location to spawn the ships.
	public Transform shipSpawn;
	//Tells the planet whether it is targeted, or selected, or not.
	public bool isTargeted = false;
	//Tells whether the mouse is over the planet or not.  Used to change the isTargeted variable.
	private bool mouseOn = true;
	//used to highlight planet
	public GameObject highlight;
	//used to display shipcount
	private GUIText shipCountText;
	public int productionRate;//determines the how many ships produced per second
	public int shipCapacity;//ship production halts if this limit is reached.
    public GameObject color;

	void Start () {
		shipCountText = GetComponentInChildren<GUIText> ();//finds the GUIText for the label
        highlight.SetActive(true);
        color.SendMessage("SetColor", tag);
        ships = initialShips;

        //Next three lines add all the planets to the planets list.
        planets.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        planets.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        planets.AddRange(GameObject.FindGameObjectsWithTag("Neutral"));
	}

	void Update () {
      
		//update text on shipcount label
		shipCountText.text = ((int)ships).ToString ();
		ProduceShips ();
		//Checks to see if the planet is targeted.
        if (gameObject.tag == "Player")
        {
            if (isTargeted)
            {
                //display the highlight circle around planet
                //highlight.SetActive(true);
                color.SendMessage("SetColor", "Highlight");

            }
            else
            {
                //highlight.SetActive(false);
                color.SendMessage("SetColor", gameObject.tag);
            }

            /* Checks whether the planet should send ships or not, based on A. if the mouse is clicked,
             * B. if the planet has a target (AKA an enemy planet), 
             * C. if the ship itself is "targeted" by the player, or selected, and
             * D. if it has any ships to send. */
            if (Input.GetButton("Fire1") && target && isTargeted && ships > 1)
            {

                //int send = ships * ;
                while (ships > 1)
                {
                    // This very long line essentially spawns a ship and tells it where to go.
                    if (gameObject.tag == "Player")
                    {
                        (Instantiate(ship, shipSpawn.position, shipSpawn.rotation) as GameObject).SendMessage("changeTarget", target);
                        ships--;
                    }
                }
            }
            else
            {
                /* Checks whether to "deselect" or untarget the planet.  
                 * It does this is you click off of the planet and not
                 * on an enemy planet. */
                if (Input.GetButton("Fire1") && !target && !mouseOn)
                {
                    isTargeted = false;
                }
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
        if (gameObject.tag == "Player")
        {
            if (Input.GetButton("Fire1"))
            {
                isTargeted = true;
                mouseOn = true;
            }
        }
        else
        {
            if (gameObject.tag == "Enemy" || gameObject.tag == "Neutral")
            {
                /*Checks to see if the player is clicking on the planet. If they are, 
		 * sends the transform information to the player. */
                if (Input.GetButton("Fire1"))
                {
                    foreach (GameObject i in planets)
                    {
                        if (i.GetComponent<PlayerPlanet>().isTargeted && i.tag == "Player")
                        {
                            player = i;
                            break;
                        }
                    }
                    if (player)
                    {
                        player.SendMessage("ChangeTarget", transform);
                    }
                }
            }
        }
	}
	//Changes the mouseOn variable once the mouse leaves the area.  Needed to remove it as the target or selection.
	void OnMouseExit (){
        if (tag == "Player")
        {
            
		    mouseOn = false;
        }
        else {
		    player.SendMessage ("RemoveTarget");
        }
	}


    void OnTriggerEnter(Collider other)
    {
        // Destroys any ship object it encounters.
        if (other.tag == "PlayerShip" && (other.GetComponent<PlayerShip>().target == transform))
        {
            if (tag == "Player")
            {
                Destroy(other.gameObject);
                ships++;
            }
            else
            {
                if (gameObject.tag == "Enemy" || gameObject.tag == "Neutral")
                {
                    Destroy(other.gameObject);
                    ships--;
                    if (ships < 1)
                    {
                        gameObject.tag = "Player";
                        color.SendMessage("SetColor", tag);
                        productionRate = 3;
                    }
                }
            }
        }
        else
        {
            if (other.tag == "EnemyShip" && (other.GetComponent<PlayerShip>().target == transform))
            {
                if (gameObject.tag == "Enemy")
                {
                    Destroy(other.gameObject);
                    ships++;
                }
                else
                {
                    if (gameObject.tag == "Player" || gameObject.tag == "Neutral")
                    {
                        Destroy(other.gameObject);
                        ships--;
                        if (ships < 1)
                        {
                            gameObject.tag = "Enemy";
                            color.SendMessage("SetColor", gameObject.tag);
                            productionRate = 2;
                        }
                    }
                }
            }
        }
    }

	public void SendShips(Transform t)
	{
		//int send = ships * ;
		while (ships > 1) {
			// This very long line essentially spawns a ship and tells it where to go.
			(Instantiate (enemyShip, shipSpawn.position, shipSpawn.rotation) as GameObject).SendMessage ("changeTarget", t);
			ships--;
		}
	}

	public void IncrementShips()
	{
		++ships;
	}

	public void DecrementShips(string shipOwner)
	{
		if (ships > 1) {
			--ships;
		}
		else
		{
			this.gameObject.tag = shipOwner;
		}
	}

	private void ProduceShips ()
	{
		if (ships < shipCapacity) {
			ships += Time.deltaTime * (float)productionRate;
		}
	}
}
