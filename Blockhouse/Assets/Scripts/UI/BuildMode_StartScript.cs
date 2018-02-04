using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode_StartScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartDestroyMode() {
        //Debug.Log("Destroy Mode ON");
        //Debug.Log(GameManagerScript.instance.GameState);
        GameManagerScript.instance.StartDestroyMode();
    }
}
