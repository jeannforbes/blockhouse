using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode_CanvasScript : MonoBehaviour {

    BuildGameManagerScript gameManager;

    public GameObject unselectButton;

	// Use this for initialization
	void Start () {
        gameManager = BuildGameManagerScript.instance;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameManager.selectedCube == null)
        {
            unselectButton.SetActive(false);
        }
        else {
            unselectButton.SetActive(true);
        }
	}
}
