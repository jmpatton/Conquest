﻿using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
}
