using UnityEngine;
using System.Collections;

public class EnemyTarget : MonoBehaviour {
	
	public GameObject player;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver (){
		if (Input.GetButton("Fire1")){
			player.SendMessage ("changeTarget", transform);
		}
	}
	void OnMouseExit (){
		player.SendMessage ("removeTarget");
	}
	void OnTriggerEnter (Collider other){
		if (other.tag == "Ship"){
			Destroy (other.gameObject);
		}
	}
}
