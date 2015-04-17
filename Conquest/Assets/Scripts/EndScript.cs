using UnityEngine;
using System.Collections;

public class EndScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartGame(){
        PlayerPrefs.SetInt("Current",0);
		Application.LoadLevel ("Start");
	}
}
