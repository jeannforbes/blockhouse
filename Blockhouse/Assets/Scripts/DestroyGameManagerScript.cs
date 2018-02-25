﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameManagerScript : MonoBehaviour
{

    // singleton-type GameManager
    public static DestroyGameManagerScript instance = null;
    
    public Canvas canvas;

    public GameObject allBoundingFloors;

    private Camera mainCamera;
    public GameObject allCubes;

    public GameObject selectedCube;
    private GameObject displayCube;
    private GameObject tempCube;

    public GameObject[] cubes;

    public GameObject[] boundingFloors;

    public int numTeams;
    public int currentTeam = 0;

    public float shotForce = 100000.0f;
    private Vector3 fireDirection;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(allCubes);
    }

    // Use this for initialization
    void Start()
    {
        selectedCube = null;
        tempCube = null;
        displayCube = null;

        mainCamera = Camera.main;

        allCubes = GameObject.FindGameObjectWithTag("AllCubes");

        allBoundingFloors = GameObject.FindGameObjectWithTag("AllBoundingFloors");

        numTeams = allBoundingFloors.transform.childCount;

        boundingFloors = new GameObject[numTeams];
        for (int i = 0; i < boundingFloors.Length; i++) {
            boundingFloors[i] = allBoundingFloors.transform.GetChild(i).gameObject;
        }

        cubes = GameObject.FindGameObjectsWithTag("Cube");

        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // SWITCH TEAMS BY PRESSING Q
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (selectedCube != null)
                DeselectCube();

            currentTeam++;
            currentTeam = currentTeam % numTeams;
        }

        // Select Cube
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit && hitInfo.transform.gameObject.tag == "Cube")
            {
                Vector3 cubePos = hitInfo.transform.position;

                // if in bounds, move the cube
                Renderer rend = boundingFloors[currentTeam].GetComponent<Renderer>();

                if (cubePos.x > rend.bounds.min.x &&
                    cubePos.x < rend.bounds.max.x &&
                    cubePos.z > rend.bounds.min.z &&
                    cubePos.z < rend.bounds.max.z &&
                    cubePos.y > 0)
                {
                    //Debug.Log("CUBE");
                    tempCube = hitInfo.transform.gameObject;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit && hitInfo.transform.gameObject.tag == "Cube" && hitInfo.transform.gameObject == tempCube)
            {
                Vector3 cubePos = hitInfo.transform.position;

                // if in bounds, move the cube
                Renderer rend = boundingFloors[currentTeam].GetComponent<Renderer>();

                if (cubePos.x > rend.bounds.min.x &&
                    cubePos.x < rend.bounds.max.x &&
                    cubePos.z > rend.bounds.min.z &&
                    cubePos.z < rend.bounds.max.z &&
                    cubePos.y > 0)
                {
                    SelectCube(hitInfo.transform.gameObject);
                }
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

                // Remove joints
                FixedJoint[] joints = selectedCube.GetComponents<FixedJoint>();
                for (int i = 0; i < joints.Length; i++)
                {

                    // Destroy the joints of the cubes connected to selected Cube
                    CubeScript cScript = joints[i].connectedBody.gameObject.GetComponent<CubeScript>();

                    if (cScript.connectedObjects.Contains(selectedCube))
                    {
                        FixedJoint[] fixedJoints = cScript.GetComponents<FixedJoint>();

                        for (int j = 0; j < fixedJoints.Length; j++)
                        {
                            if (fixedJoints[j].connectedBody.gameObject == selectedCube)
                            {
                                Destroy(fixedJoints[j]);
                            }
                        }
                    }

                    Destroy(joints[i]);
                }

                Vector3 shotVector = fireDirection * shotForce;
                selectedCube.GetComponent<Rigidbody>().AddForce(shotVector);

                DeselectCube();
            }
        }
    }

    public void SelectCube(GameObject cube)
    {
        if (cube == null)
        {
            selectedCube = null;
            return;
        }

        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].GetComponent<CubeScript>().IsSelected = false;
        }

        selectedCube = cube;
        cube.GetComponent<CubeScript>().IsSelected = true;
        //Debug.Log(selectedCube.name);
    }

    public void DeselectCube()
    {
        if (selectedCube != null)
        {
            selectedCube.GetComponent<CubeScript>().IsSelected = false;
            selectedCube = null;
        }

        if (displayCube != null)
        {
            Destroy(displayCube);
            displayCube = null;
        }
    }

    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * 2.0f;// shotForce;
    }
}
