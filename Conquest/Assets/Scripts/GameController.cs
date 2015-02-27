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
	
	// Update is called once per frame
	void Update () {
        //Sets count to 0.
        count = 0;
        //Checks to see how many player planets there are.
        foreach (GameObject i in planets)
        {
            if (i.tag == "Player")
            {
                count++;
            }
        }
        //If there are no player planets, starts the lose script.
        if (count == 0)
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
        if (count == planets.Count)
        {
            //Enables and unhides the button, changes the button listener to NextLevel().
            levelButton.enabled = true;
            levelButton.GetComponent<CanvasRenderer>().SetAlpha(1);
            levelText.text = "Next Level";
            levelButton.onClick.RemoveAllListeners();
            levelButton.onClick.AddListener(() => { NextLevel(); });
            EndGame("You Win!");
        }
   		CalculateAITurn ();


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
        foreach (GameObject i in planets)
        {
            if (i.tag == "Enemy")
            {
                i.SendMessage("SendShips", GetAITarget());
            }
        }
    }

    Transform GetAITarget()
    {
        foreach (GameObject i in planets)
        {
            if (i.tag != "Enemy")
            {
                return i.transform;
            }
        }
        return null;
    }

}
