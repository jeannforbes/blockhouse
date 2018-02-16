using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMode_CanvasScript : MonoBehaviour {

    BuildGameManagerScript gameManager;

    public GameObject unselectButton;
    public GameObject placeButton;
    public GameObject messageText;

    private float messageTimer;

	// Use this for initialization
	void Start () {
        gameManager = BuildGameManagerScript.instance;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameManager.selectedCube == null)
        {
            unselectButton.SetActive(false);
            placeButton.SetActive(false);
        }
        else {
            unselectButton.SetActive(true);
            placeButton.SetActive(true);
        }

        if (messageTimer > 0)
        {
            messageTimer -= Time.deltaTime;
        }
        else if (messageText.activeSelf && messageTimer <= 0) {
            messageText.SetActive(false);
        }
	}

    public void ShowMessage(string msg, float time) {
        messageText.GetComponent<Text>().text = msg;
        messageTimer = time;
        messageText.SetActive(true);
    }
}
