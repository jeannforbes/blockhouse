using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    // Use this for initialization
    void Start () {

        // Set button text
        GameObject.Find("StartButton").GetComponentInChildren<Text>().text = "Start";
        GameObject.Find("SettingsButton").GetComponentInChildren<Text>().text = "Settings (WIP)";

        GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(StartGame);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartGame()
    {
        Debug.Log("Starting Game");
        SceneManager.LoadScene("demo_stage01", LoadSceneMode.Single);
    }
}
