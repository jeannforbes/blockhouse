using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode_UnselectButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Unselect()
    {
        BuildGameManagerScript.instance.DeselectCube();
    }
}
