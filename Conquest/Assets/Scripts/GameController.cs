using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    //Public Variables
    public List<GameObject> planets = new List<GameObject>();//Creates a list for all the planets.
    public AudioClip gameOverAudio;//Audio clip for gamve over audio.
    //Private Variables
    private int count;//used to compare player planets to total planets.
	private const float AITIMER = 3.0f;//determines how often the AI will make a turn
	private const float AIRESENDTIMER = 6.0f;//determines how long the AI waits before resending to a planet
	private Transform LastAITarget;//keeps the AI from sending to the same planet twice in a row
	private float timer = 0f;
    private float resendTimer = 0f;
    private Button levelButton;//Button to either restart the level or go to the next level.
    private Text levelText;//Text for the level button.
    private Text gameEndText;//Text to put the game end status in.
    private Button menuButton;
    private Text menuText;
    private Button resumeButton;
    private Text resumeText;
    private int difficulty;
    private Button diffButton;
    private Text diffText;
    private const int EASY = 1;
    private const int MEDIUM = 2;
    private const int HARD = 3;
	//private bool AIRetarget = false;
	//private AudioSource source;

	void Start () {
        //Next three lines add all the planets to the planets list.
        planets.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        planets.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        planets.AddRange(GameObject.FindGameObjectsWithTag("Neutral"));
        //Makes the text and buttons invisible and unclickable.


	}

	void Awake () {
		//source = 
        GetComponent <AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
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
        Time.timeScale = 1.0f;
        Application.LoadLevel(Application.loadedLevel);
    }

    void NextLevel()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    void CalculateAITurn()
    {
		float timeChange = Time.deltaTime;
		timer += timeChange;
		resendTimer += timeChange;
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
    public void SelectAll()
    {
        foreach (GameObject i in planets)
        {
            if (i.tag == "Player")
            {
                i.SendMessage("Target");
            }
        }
    }

    public void SendAmount(int send)
    {
        foreach (GameObject i in planets)
        {
           i.SendMessage("SendAmount", send);
        }
    }

    public void SetGameButton(Button but)
    {
        levelButton = but;
        levelButton.enabled = false;
        levelButton.GetComponent<CanvasRenderer>().SetAlpha(0);
    }
    public void SetGameButtonText(Text tex)
    {
        levelText = tex;
        levelText.text = "";
    }
    public void SetMenuButton(Button but)
    {
        menuButton = but;
        menuButton.enabled = false;
        menuButton.GetComponent<CanvasRenderer>().SetAlpha(0);
    }
    public void SetMenuButtonText(Text tex)
    {
        menuText = tex;
        menuText.text = "";
    }
    public void SetResumeButton(Button but)
    {
        resumeButton = but;
        resumeButton.enabled = false;
        resumeButton.GetComponent<CanvasRenderer>().SetAlpha(0);
        resumeButton.onClick.AddListener(() => { Unpause(); });
    }
    public void SetResumeButtonText(Text tex)
    {
        resumeText = tex;
        resumeText.text = "";
    }
    public void SetEndText(Text tex)
    {
        gameEndText = tex;
        gameEndText.text = "";
    }
    void SetDiffText(Text t)
    {
        diffText = t;
        diffText.text = "";
    }
    public void SetDiffButton(Button but)
    {
        diffButton = but;
        diffButton.enabled = false;
        diffButton.GetComponent<CanvasRenderer>().SetAlpha(0);
    }

    void Pause()
    {
        Time.timeScale = 0.0f;
        diffButton.enabled = true;
        diffButton.GetComponent<CanvasRenderer>().SetAlpha(1);
        diffText.text = GetDifficulty();
        menuButton.enabled = true;
        menuButton.GetComponent<CanvasRenderer>().SetAlpha(1);
        menuText.text = "Main Menu";
        resumeButton.enabled = true;
        resumeButton.GetComponent<CanvasRenderer>().SetAlpha(1);
        resumeText.text = "Resume";
        levelButton.enabled = true;
        levelButton.GetComponent<CanvasRenderer>().SetAlpha(1);
        levelText.text = "Restart Level";
        levelButton.onClick.RemoveAllListeners();
        levelButton.onClick.AddListener(() => { RestartLevel(); });

    }

    void Unpause()
    {
        Time.timeScale = 1.0f;
        diffButton.enabled = false;
        diffButton.GetComponent<CanvasRenderer>().SetAlpha(0);
        diffText.text = "";
        menuButton.enabled = false;
        menuButton.GetComponent<CanvasRenderer>().SetAlpha(0);
        menuText.text = "";
        resumeButton.enabled = false;
        resumeButton.GetComponent<CanvasRenderer>().SetAlpha(0);
        resumeText.text = "";
        levelButton.enabled = false;
        levelButton.GetComponent<CanvasRenderer>().SetAlpha(0);
        levelText.text = "";
        levelButton.onClick.RemoveAllListeners();


    }

    string GetDifficulty()
    {
        string diff;
        switch (difficulty)
        {
            case EASY: diff = "Easy"; break;
            case MEDIUM: diff = "Medium"; break;
            case HARD: diff = "Hard"; break;
            default: diff = "Easy"; break;
        }
        return diff;
    }
    void Difficulty()
    {
        if (difficulty < HARD)
        {
            difficulty++;
        }
        else
        {
            difficulty = EASY;
        }
        switch (difficulty)
        {
            case EASY: diffText.text = "Easy"; break;
            case MEDIUM: diffText.text = "Medium"; break;
            case HARD: diffText.text = "Hard"; break;
        }
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }
}
