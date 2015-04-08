using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectAll : MonoBehaviour {
    private GameObject GameController;


	void Start () {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        transform.position = new Vector3(50,20);
	}

    public void selectAll()
    {
        GameController.SendMessage("SelectAll");
    }
}
