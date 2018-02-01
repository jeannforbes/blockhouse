using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour {

    public GameObject buildMode;
    public GameObject destroyMode;

	// Use this for initialization
	void Start () {
        buildMode = transform.GetChild(0).gameObject;
        destroyMode = transform.GetChild(1).gameObject;

        if (GameManagerScript.instance.GameState == GameManagerScript.GameStates.Build) {
            buildMode.SetActive(true);
            destroyMode.SetActive(false);
        }
        else if (GameManagerScript.instance.GameState == GameManagerScript.GameStates.Destroy)
        {
            buildMode.SetActive(false);
            destroyMode.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
	}
}
