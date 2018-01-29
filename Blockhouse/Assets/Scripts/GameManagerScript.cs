using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    public enum GameStates {
        Build,
        Destroy
    }

    public GameStates gameState;

    private Camera mainCamera;
    private GameObject selectedCube;
    private GameObject tempCube;

    public GameObject[] cubes;

    //Fire
    private float shotForce = 1000.0f;
    //public float moveSpeed = 10f;

    private Vector3 fireDirection;

    // Use this for initialization
    void Start () {
        selectedCube = null;
        tempCube = null;

        mainCamera = Camera.main;

        gameState = GameStates.Destroy;

        cubes = GameObject.FindGameObjectsWithTag("Cube");
    }
	
	// Update is called once per frame
	void Update () {
        // BUILD
        if (gameState == GameStates.Build) { }


        // DESTROY
        else if (gameState == GameStates.Destroy)
        {
            //Debug.Log("DESTROY");
            // Select Cube
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse is down");

                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit && hitInfo.transform.gameObject.tag == "Cube")
                {
                    Debug.Log("CUBE");
                    tempCube = hitInfo.transform.gameObject;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit && hitInfo.transform.gameObject.tag == "Cube" && hitInfo.transform.gameObject == tempCube)
                {
                    SelectCube(hitInfo.transform.gameObject);
                }

                tempCube = null;
            }

            // Throw Cube
            if (selectedCube != null)
            {

                fireDirection = selectedCube.transform.position - mainCamera.transform.position;
                fireDirection.Normalize();

                if (Input.GetKeyUp(KeyCode.E))
                {
                    Debug.Log("FIRE!");

                    Vector3 shotVector = fireDirection * shotForce;
                    selectedCube.GetComponent<Rigidbody>().AddForce(shotVector);
                }
            }
        }

    }

    private void SelectCube(GameObject cube) {
        if (cube == null) {
            selectedCube = null;
            return;
        }

        for (int i = 0; i < cubes.Length; i++) {
            cubes[i].GetComponent<CubeScript>().IsSelected = false;
        }

        selectedCube = cube;
        cube.GetComponent<CubeScript>().IsSelected = true;
        Debug.Log(selectedCube.name);
    }

    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * 2.0f;// shotForce;
    }
}
