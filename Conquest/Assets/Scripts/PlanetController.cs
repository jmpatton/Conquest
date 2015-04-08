using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlanetController : MonoBehaviour {
    //Public Variables
	public float productionRate;
	public float ships;
	public string owner;
	public GameObject ship;
	public Transform shipSpawn;
	public GameController gameController;
    //Private Variables
    private GUIText text;
	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<GUIText>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		ships += Time.deltaTime * productionRate;
		text.text = ((int)ships).ToString();

		if (Input.GetButton ("Fire1") && ships >= 1) 
		{
			ships--;
			Instantiate (ship, shipSpawn.position, shipSpawn.rotation);
		}

	}

	void OnTriggerEnter(Collider other)
	{
		//ignore boundary
		if (other.tag == "Boundary") 
		{
			return;
		}
		
		if (other.tag == "Ship")
		{
			//if its your own ship
			if (owner == other.gameObject.name.ToString())
			{
				//Debug.Log(owner);
				ships++;
			}
			//decrement your ships
			else if ((int)ships >= 1)
			{
				ships--;
			}
			else
			{
				gameController.EndGame("You were defeated! Remember, the enemy\nspawns ships slower than you.");
				//ships = 1f;
				//string substring = other.gameObject.name.ToString ();
				////owner = other.gameObject.name.ToString();
				//owner = System.Text.RegularExpressions.Regex.Replace (substring, "(Clone)", "");
			}

			Destroy (other.gameObject);//destroy the ship
		}
	}//end OnTriggerEnter


}
