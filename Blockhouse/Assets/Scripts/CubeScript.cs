using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour {
    
    public bool isSelected = false;
    protected Material cubeMaterial;

    // Use this for initialization
    void Start () {
        cubeMaterial = GetComponent<Renderer>().material;

    }
	
	// Update is called once per frame
	void Update () {

        //selectedCube.GetComponent<Renderer>().material = selectedCube.GetComponent<Renderer>().materials[1];

        //Debug.Log(GetComponent<Renderer>().material);
    }

    public bool IsSelected {
        get {
            return isSelected;
        }
        set {
            this.isSelected = value;

            if (isSelected) {
                //Debug.Log("true");
                //GetComponent<Renderer>().material = selectedMaterial;
                cubeMaterial.color = Color.red;
            } else {
                // Debug.Log(GetComponent<Renderer>().materials[0]);
                //GetComponent<Renderer>().material = mainMaterial;
                cubeMaterial.color = Color.white;
            }
        }
    }
}
