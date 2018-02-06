using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BuildMode_StartDestroyMode : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartDestroyMode(int sceneIndex) {
        Debug.Log("HAHAHHAHA");
        BuildGameManagerScript.instance.StartDestroyMode();

        SceneManager.LoadScene(sceneIndex);
    }
}
