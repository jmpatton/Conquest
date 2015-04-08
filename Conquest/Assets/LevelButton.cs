using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    public Text text;
    public Text endText;
    private GameObject gameController;
    // Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameController.SendMessage("SetGameButton", this.GetComponent<Button>());
        gameController.SendMessage("SetGameButtonText", text);
        gameController.SendMessage("SetEndText", endText);
	}

}
