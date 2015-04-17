using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {
    public Text text;
    public Text endText;
    private GameObject gameController;
    // Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameController.SendMessage("SetMenuButton", this.GetComponent<Button>());
        gameController.SendMessage("SetMenuButtonText", text);
	}

    public void Menu()
    {
        Debug.Log("Here");
        Time.timeScale = 1.0f;
        PlayerPrefs.SetInt("Current", Application.loadedLevel);
        Application.LoadLevel("Start");
    }

}
