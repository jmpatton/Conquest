using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Resume : MonoBehaviour {
    public Text resume;

	void Start () {
        if (PlayerPrefs.GetInt("Current") > 1)
        {
            resume.text = "Resume";
        }
        else
        {
            gameObject.GetComponent<Button>().enabled = false;
            gameObject.GetComponent<CanvasRenderer>().SetAlpha(0);
            resume.text = "";
        }
	}

    public void ResumeGame()
    {
        Application.LoadLevel(PlayerPrefs.GetInt("Current"));
    }
}
