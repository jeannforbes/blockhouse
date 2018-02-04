using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour {

    public GameObject buildMode;
    public GameObject destroyMode;

    private GameManagerScript gameManager;

	// Use this for initialization
	void Start () {
        buildMode = transform.GetChild(0).gameObject;
        destroyMode = transform.GetChild(1).gameObject;

        gameManager = GameManagerScript.instance;
        if (gameManager.gameState == GameManagerScript.GameStates.Build) {
            buildMode.SetActive(true);
            destroyMode.SetActive(false);
        }
        else if (gameManager.gameState == GameManagerScript.GameStates.Destroy)
        {
            buildMode.SetActive(false);
            destroyMode.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (gameManager.gameState == GameManagerScript.GameStates.Build) {
            buildMode.SetActive(true);
            destroyMode.SetActive(false);

            // show the unselect button
            if (gameManager.selectedCube != null)
            {
                buildMode.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                buildMode.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else if (gameManager.gameState == GameManagerScript.GameStates.Destroy)
        {
            buildMode.SetActive(false);
            destroyMode.SetActive(true);
        }
    }
}
