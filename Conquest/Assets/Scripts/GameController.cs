using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public List<GameObject> planets = new List<GameObject>();
	public Text gameOverText;
    public Button restartButton;
    public Text restartText;
    private int count;
	// Use this for initialization
	void Start () {
        planets.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        planets.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        planets.AddRange(GameObject.FindGameObjectsWithTag("Neutral"));
        gameOverText.text = "";
        restartButton.enabled = false;
        restartButton.GetComponent<CanvasRenderer>().SetAlpha(0);
        restartText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        count = 0;
        foreach (GameObject i in planets)
        {
            if (i.tag == "Player")
            {
                count++;
            }
        }
        if (count == 0)
        {
            EndGame("You lost...");
            restartButton.enabled = true;
            restartButton.GetComponent<CanvasRenderer>().SetAlpha(1);
            restartText.text = "Restart Level";
        }
        if (count == planets.Count)
        {
            restartButton.enabled = true;
            restartButton.GetComponent<CanvasRenderer>().SetAlpha(1);
            restartText.text = "Next Level";
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() => { NextLevel(); });
            EndGame("You Win!");
        }

		CalculateAITurn ();

	}

	public void EndGame(string message)
	{
		gameOverText.text = message;
	}

    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

	void CalculateAITurn()
	{
		foreach (GameObject i in planets)
		{
			if (i.tag == "Enemy")
			{
				i.SendMessage ("SendShips",GetAITarget());
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

    void NextLevel()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
