using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour {

    public int team;
    public Color teamColor;
    public Material objectMaterial;

    // Use this for initialization
    public virtual void Start () {
		
	}

    // Update is called once per frame
    public virtual void Update () {
		
	}

    public void SetChildrenColor()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            if (t.gameObject.GetComponent<Renderer>() != null)
            {
                t.gameObject.GetComponent<Renderer>().material.color = objectMaterial.color;
            }
        }
    }
}
