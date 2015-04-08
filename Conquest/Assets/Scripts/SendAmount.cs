using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SendAmount : MonoBehaviour {
    private GameObject GameController;
    int send = 4;
    public Text amount;


	void Start () {
        GameController = GameObject.FindGameObjectWithTag("GameController");
        transform.position = new Vector3(130,20);
        amount.text = (25 * send).ToString();
	}

    public void sendAmount()
    {
        if (send > 1)
        {
            send--;
        }
        else
        {
            send = 4;
        }
        amount.text = (25 * send).ToString();
        GameController.SendMessage("SendAmount", send);
    }
}
