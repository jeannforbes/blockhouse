using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode_PlaceCubeButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaceCube() {
        if (!BuildGameManagerScript.instance.cannotPlaceCube)
        {
            BuildGameManagerScript.instance.PlaceCube();
        }
        else {
            transform.GetComponentInParent<BuildMode_CanvasScript>().ShowMessage("Cannot place cube there.", 3.0f);
        }
    }
}
