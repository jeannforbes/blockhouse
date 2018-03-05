using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Block : MonoBehaviour {

    // PUBLIC
    public bool selected, throwable;
    
    // PRIVATE
    private Vector3 posToCamera;
    private GameObject parentFoundation;

    // Use this for initialization
    void Start () {
        selected = false;
        parentFoundation = this.transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        // Destroy this object if it drops off the foundation
		//if(this.transform.position.y < -10) Destroy(gameObject);
    }

    void OnMouseDown()
    {
        if (!selected) SelectForMovement();
        else if (throwable) ThrowBlock();
        else DropBlock();
    }

    void SelectForMovement()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].GetComponent<Block>().selected = false;
            blocks[i].transform.SetParent(blocks[i].GetComponent<Block>().parentFoundation.transform);
            blocks[i].transform.GetComponent<Rigidbody>().isKinematic = false;
        }
        selected = true;
        this.transform.SetParent(GameObject.FindGameObjectWithTag("MainCamera").transform);
        this.transform.GetComponent<Rigidbody>().isKinematic = true;
    }

    void ThrowBlock()
    {
        this.GetComponent<Rigidbody>().AddForce(posToCamera);
    }

    void DropBlock()
    {
        this.selected = false;
        this.transform.GetComponent<Rigidbody>().isKinematic = false;
    }
}
