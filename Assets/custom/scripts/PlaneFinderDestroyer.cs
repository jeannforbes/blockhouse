using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneFinderDestroyer : MonoBehaviour {

	public bool BeTheOnlyOne = true;

	// Use this for initialization
	void Start () {
		if(BeTheOnlyOne) Invoke("DestroyPlaneFinder", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DestroyPlaneFinder(){
		Destroy(GameObject.Find("Plane Finder"));
	}
}
