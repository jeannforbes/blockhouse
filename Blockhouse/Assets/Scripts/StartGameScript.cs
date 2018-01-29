using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadByIndex(int sceneIndex) {
        Debug.Log("Loading Scene: " + sceneIndex);

        SceneManager.LoadScene(sceneIndex);
    }
}
