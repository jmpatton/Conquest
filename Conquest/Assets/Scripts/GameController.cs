using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    //Creates a list for all the planets.
    public List<GameObject> planets = new List<GameObject>();
	//Text to put the game end status in.
    public Text gameEndText;
    //Button to either restart the level or go to the next level.
    public Button levelButton;
    //Text for the level button.
    public Text levelText;
    //used to compare player planets to total planets.
    private int count;
	// Use this for initialization
	public AudioClip gameOverAudio;
	private const float AITIMER = 1.8f;//determines how often the AI will make a turn
	private const float AIRESENDTIMER = 4.0f;//determines how long the AI waits before resending to a planet
	private Transform LastAITarget;//keeps the AI from sending to the same planet twice in a row
	private float timer = 0f;
	private float resendTimer = 0f;
	private bool AIRetarget = false;

	private AudioSource source;

	void Start () {
        //Next three lines add all the planets to the planets list.
        planets.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        planets.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        planets.AddRange(GameObject.FindGameObjectsWithTag("Neutral"));
        //Makes the text and buttons invisible and unclickable.
        gameEndText.text = "";
        levelButton.enabled = false;
        levelButton.GetComponent<CanvasRenderer>().SetAlpha(0);
        levelText.text = "";
	}

	void Awake () {

		source = GetComponent <AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Checks to see if there are any player planets. If not, checks for player ships.
		if (!GameObject.FindWithTag("Player"))
		{
			//Checks to see if the player has any ships out.  If not, starts the game over code.
			if (!GameObject.FindWithTag("PlayerShip"))
			{
				//sets gameEndText to the losing script.
				EndGame("You lost...");
				//Enables and unhides the button.
				levelButton.enabled = true;
				levelButton.GetComponent<CanvasRenderer>().SetAlpha(1);
				levelText.text = "Restart Level";
				levelButton.onClick.RemoveAllListeners();
				levelButton.onClick.AddListener(() => { RestartLevel(); });
			}
		}
		//Checks to see if there are any enemy planets.  If not, checks for enemy ships.
		else if (!GameObject.FindWithTag("Enemy"))
		{
			//Checks to see if the enemy has ships out. If not, starts the win code.
			if (!GameObject.FindWithTag("EnemyShip"))
			{
				//Enables and unhides the button, changes the button listener to NextLevel().
				levelButton.enabled = true;
				levelButton.GetComponent<CanvasRenderer>().SetAlpha(1);
				levelText.text = "Next Level";
				levelButton.onClick.RemoveAllListeners();
				levelButton.onClick.AddListener(() => { NextLevel(); });
				EndGame("You Win!");
			}
		}
		else {//if the AI hasn't won yet
			CalculateAITurn();
		}
	}

	public void EndGame(string message)
	{
		gameEndText.text = message;
	}

    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    void NextLevel()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    void CalculateAITurn()
    {
		timer += Time.deltaTime;
		if (timer >= AITIMER)
		{
			timer = 0f;//reset the timer
			float AIships = GetShipCount ("Enemy");
			float playerShips = GetShipCount ("Player");
			
			//tell each AI to pick a target
			CalculateAITarget (AIships, playerShips);
		}
    }

	//iterates though all planets and returns the number of ships the player has
	float GetShipCount(string playerName)
	{
		float count = 0;
		foreach (GameObject i in planets) 
		{
			if (i.tag == playerName)
			{
				count += i.GetComponent<PlayerPlanet>().ships;
			}
		}
		return count;
	}


	void CalculateAITarget(float AIships, float playerShips)
	{
		float weakestPlanetShipCount = 500000;
		GameObject tempObject = new GameObject ();
		Transform newTarget = tempObject.transform;
		Destroy (tempObject);

		foreach (GameObject i in planets)
		{
			if (i.tag != "Enemy")
			{
				if (weakestPlanetShipCount > i.GetComponent<PlayerPlanet>().ships)
				{
					if(AllowedToRetarget(GetTransform(i)))
					{
						weakestPlanetShipCount = i.GetComponent<PlayerPlanet>().ships;
						newTarget = GetTransform (i);
						LastAITarget = newTarget;
					}
				}
			}
		}
		if (weakestPlanetShipCount < AIships)
		{
			AIAttackPlanet(newTarget, AIships, weakestPlanetShipCount);
		}
	}

	bool AllowedToRetarget(Transform newTarget)
	{
		//if its the old target
		if (newTarget == LastAITarget && resendTimer <= AIRESENDTIMER) 
		{
			resendTimer += AITIMER;
			return false;//don't send ships, it hasn't been long enough
		}
		else if (newTarget == LastAITarget && resendTimer > AIRESENDTIMER)
		{
			resendTimer = 0f;
			return true;//let them send this time, its been long enough
		}
		else 
		{
			return true;//it wasn't the last target so let them send
		}
	}

	void AIAttackPlanet(Transform targetPlanet, float AIships, float targetShipCount)
	{
		float shipsSent = 0f;
		float extraShips = 5;
		foreach(GameObject i in planets)
		{
			if (shipsSent < targetShipCount + extraShips)//check to see if its sent enough ships already
			{
				if (i.tag == "Enemy")
				{
					shipsSent += i.GetComponent<PlayerPlanet>().ships;
					i.SendMessage("SendShips", targetPlanet);
				}
			}
			else
			{
				break;
			}
		}
	}

	Transform GetTransform(GameObject planet)
	{
		return planet.transform;
	}

    Transform GetAITarget(GameObject currentPlanet)
    {
		Transform currentLocation = currentPlanet.transform;
		GameObject tempObject = new GameObject ();
		Transform newTarget = tempObject.transform;
		Destroy (tempObject);
		float distance = 10000;
        foreach (GameObject i in planets)
        {
			//find the closest non-AI planet
			if (i.tag != "Enemy" && Vector3.Distance (i.transform.position, currentLocation.position) < distance)
            {
				distance = Vector3.Distance (i.transform.position, currentLocation.position);
                newTarget = i.transform;
            }
        }
        return newTarget;
    }

}
