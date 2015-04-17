using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour {
    public Text text;
    public Text endText;
    private GameObject gameController;
    // Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameController.SendMessage("SetResumeButton", this.GetComponent<Button>());
        gameController.SendMessage("SetResumeButtonText", text);
	}

}
