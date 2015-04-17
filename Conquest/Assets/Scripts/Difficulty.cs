using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour {
    private GameObject GameController;
    public Text diffText;
  
	// Use this for initialization
	void Start () {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        GameController.SendMessage("SetDiffText", diffText);
        GameController.SendMessage("SetDiffButton", this.GetComponent<Button>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
