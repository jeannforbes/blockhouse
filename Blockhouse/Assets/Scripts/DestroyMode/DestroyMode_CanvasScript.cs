using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyMode_CanvasScript : MonoBehaviour {

    public GameObject textButton;
    public GameObject healthText;

    public GameObject gameOverText;
    public GameObject winningTeamText;

    // Use this for initialization
    void Start () {
        gameOverText.SetActive(false);
        winningTeamText.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeText(string txt)
    {
        textButton.GetComponent<Text>().text = txt;
    }

    public void ChangeHealthText(string txt) {
        healthText.GetComponent<Text>().text = txt;
    }

    public void GameOver(int winningTeam) {
        textButton.SetActive(false);
        healthText.SetActive(false);

        gameOverText.SetActive(true);
        winningTeamText.SetActive(true);
        winningTeamText.GetComponent<Text>().text = "Team " + winningTeam + " Wins";
    }
}
