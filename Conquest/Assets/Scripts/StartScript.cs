using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {
    private int difficulty;
    private const int EASY = 1;
    private const int MEDIUM = 2;
    private const int HARD = 3;
    private Text diffText;
    private Button diffButton;
	// Use this for initialization
	void Start () {
        difficulty = EASY;
        PlayerPrefs.SetInt("Difficulty", EASY);
        diffText.text = "Easy";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartGame(){
		Application.LoadLevel ("Level1");
	}

	void Instructions(){
		Application.LoadLevel ("Instructions");
	}

    void SetDiffText(Text t)
    {
        diffText = t;
    }

    public void SetDiffButton(Button but)
    {
        diffButton = but;
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
