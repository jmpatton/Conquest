using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HomePlanet : MonoBehaviour {
	private Transform target;
	//private Transform nullTarget;
	public Text status;
	public float ships;
	public GameObject ship;
	public Transform shipSpawn;
	private bool isTargeted = false;
	private bool mouseOn = true;
	// Use this for initialization
	void Start () {
		status.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		ships += Time.deltaTime;
		if (isTargeted){
			status.text = ((int)ships).ToString ();
		}
		if (Input.GetButton ("Fire1") && target && isTargeted && ships > 1 ){
			(Instantiate(ship, shipSpawn.position, shipSpawn.rotation) as GameObject).SendMessage ("changeTarget", target);
			//ship.SendMessage("changeTarget",target);
			ships--;
		}
		else {
			if (Input.GetButton ("Fire1") && !target && !mouseOn){
				isTargeted = false;
				status.text = "";
			}
		}
	}
	
	void ChangeTarget (Transform tar){
		target = tar;
	}
	void RemoveTarget (){
		target = null;
	}

	void OnMouseOver (){
		if (Input.GetButton ("Fire1")){
			isTargeted = true;
			mouseOn = true;
		}
	}
	void OnMouseExit (){
		mouseOn = false;
	}
}
