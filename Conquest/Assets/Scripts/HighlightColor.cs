using UnityEngine;
using System.Collections;

public class HighlightColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetColor(string tag)
    {
        if (tag == "Player")
        {
            renderer.material.color = Color.blue;
        }
        else
        {
            if (tag == "Enemy")
            {
                renderer.material.color = Color.red;
            }
            else
            {
                if (tag == "Neutral")
                {
                    renderer.material.color = Color.gray;
                }
                else
                {
                    if (tag == "Highlight")
                    {
                        renderer.material.color = Color.yellow;
                    }
                }
            }
        }
    }
}
