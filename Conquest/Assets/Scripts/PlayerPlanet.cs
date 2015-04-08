using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerPlanet : MonoBehaviour {
    //Public Variables
    public List<GameObject> planets = new List<GameObject>();//Creates a list for all the planets.
    public GameObject player;//Sets a target for enemy
    public float ships;//The number of ships currently in possession.
    public float initialShips;
    public GameObject ship;//Allows you to set a ship object.
    public GameObject enemyShip;//Enemy ship object.
    public Transform shipSpawn;//Sets the location to spawn the ships.
    public bool isTargeted = false;//Tells the planet whether it is targeted, or selected, or not.
    public int productionRate;//determines the how many ships produced per second
    public int shipCapacity;//ship production halts if this limit is reached.
    public GameObject color;
    public AudioClip playerSelectAudio;// Player planet select sound
    public AudioClip enemyNeutralSelectAudio;// The select audio for enemy or neutral planets
    public GameObject highlight;//used to highlight planet
    //Private Variables
    private Transform target;//Gives target information that it can then send to the ships. 
    private GUIText shipCountText;//used to display shipcount
    private bool mouseOn = false;//Tells whether the mouse is over the planet or not.  Used to change the isTargeted variable.
    private AudioSource source;
    private int amount = 4;

	

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

	void Awake () {
		source = GetComponent<AudioSource> ();
	}
	

	void Update () {
      
		//update text on shipcount label
		shipCountText.text = ((int)ships).ToString ();
		ProduceShips ();
		//Checks to see if the planet is targeted.
        if (gameObject.tag == "Player")
        {

            /* Checks whether the planet should send ships or not, based on
             * A. if the mouse is clicked,
             * B. if the planet has a target (AKA an enemy planet), 
             * C. if the ship itself is "targeted" by the player, or selected, and
             * D. if it has any ships to send. */
            if (Input.GetMouseButtonDown(0) && target && isTargeted && ships > 1)
            {

                double send = ships - (ships * (.25 * amount));
                Debug.Log(send);
                while (ships > send)
                {
                    // This very long line essentially spawns a ship and tells it where to go.
                    if (gameObject.tag == "Player")
                    {
                        (Instantiate(ship, shipSpawn.position, shipSpawn.rotation) as GameObject).SendMessage("changeTarget", target);
                        ships--;
                    }
                }
                RemoveTarget();
            }
            else
            {
                /* Checks whether to "deselect" or untarget the planet.  
                 * It does this is you click off of the planet and not
                 * on an enemy planet or a player planet while holding down shift. */
                if (Input.GetMouseButtonDown(0) && !target && !mouseOn)
                {
                    if (!Input.GetButton("Fire3"))
                    {
                        isTargeted = false;
                        color.SendMessage("SetColor", gameObject.tag);
                    }
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
            if (Input.GetMouseButtonDown(0))
            {
                mouseOn = true;
                Target();
            }
        }
        else
        {
            if (gameObject.tag == "Enemy" || gameObject.tag == "Neutral")
            {
                /*Checks to see if the player is clicking on the planet. If they are, 
		 * sends the transform information to the player. */
                if (Input.GetMouseButtonDown(0))
                {
                    foreach (GameObject i in planets)
                    {
                        if (i.GetComponent<PlayerPlanet>().isTargeted && i.tag == "Player")
                        {
							source.PlayOneShot (enemyNeutralSelectAudio, 1F);
                            i.SendMessage("ChangeTarget", transform);
                        }
                    }
                }
            }
        }
	}

    public void Target()
    {
        isTargeted = true;
        source.PlayOneShot(playerSelectAudio, 1F);
        color.SendMessage("SetColor", "Highlight");
    }
	//Changes the mouseOn variable once the mouse leaves the area.  Needed to remove it as the target or selection.
	void OnMouseExit (){
        if (tag == "Player")
        {
		    mouseOn = false;
        }
        else {
            foreach (GameObject i in planets)
            {
                if (i.GetComponent<PlayerPlanet>().isTargeted && i.tag == "Player")
                {
                    i.SendMessage("RemoveTarget");
                }
            }
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
                       // productionRate = 3;
                        color.SendMessage("SetColor", gameObject.tag);
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
                           // productionRate = 2;
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
		if (ships < shipCapacity && gameObject.tag != "Neutral") {
			ships += Time.deltaTime * (float)productionRate;
		}
	}

    public void SendAmount(int send)
    {
        amount = send;
    }
}
