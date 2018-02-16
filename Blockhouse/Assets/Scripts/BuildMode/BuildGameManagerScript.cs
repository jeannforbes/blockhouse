﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGameManagerScript : MonoBehaviour
{

    // singleton-type GameManager
    public static BuildGameManagerScript instance = null;
    
    public Canvas canvas;

    private Camera mainCamera;
    public GameObject allCubes;

    public GameObject selectedCube;
    private GameObject displayCube;
    private GameObject tempCube;

    public GameObject[] cubes;

    public float shotForce = 1000.0f;
    private Vector3 fireDirection;

    private float dragSpeed = 600f;
    private Vector3 dragOrigin;

    public GameObject boundingFloor;
    public bool cannotPlaceCube;

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
        
        DontDestroyOnLoad(allCubes);
    }

    // Use this for initialization
    void Start()
    {
        selectedCube = null;
        tempCube = null;
        displayCube = null;

        mainCamera = Camera.main;

        cubes = GameObject.FindGameObjectsWithTag("Cube");

        SetSurroundingHitboxActive(false);

        for (int i = 0; i < cubes.Length; i++)
        {
            //cubes[i].GetComponent<Rigidbody>().useGravity = false;
            cubes[i].GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {


        // Select Cube
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();

            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit && hitInfo.transform.gameObject.tag == "Cube")
            {
                //Debug.Log("CUBE");
                tempCube = hitInfo.transform.gameObject;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit && hitInfo.transform.gameObject.tag == "Cube" && hitInfo.transform.gameObject == tempCube)
            {
                if (selectedCube == null)
                {
                    SelectCube(hitInfo.transform.gameObject);

                    displayCube = Instantiate(selectedCube);
                    //Destroy(displayCube.GetComponent<BoxCollider>());
                    //Destroy(displayCube.GetComponent<Rigidbody>());
                    displayCube.transform.GetChild(0).gameObject.SetActive(false);
                    displayCube.GetComponent<Renderer>().material.color = new Color(1, 1, .5f, 0.5f);
                    // turn off isKinematic and freeze rotation and position to allow for collision detection
                    displayCube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
                    displayCube.GetComponent<Rigidbody>().isKinematic = false;

                    //SetSurroundingHitboxActive(true);
                }

                else
                {
                    //selectedCube.transform.position = displayCube.transform.position;
                    //DeselectCube();
                }
            }

            tempCube = null;
        }

        // Move the display cube
        if (displayCube != null)
        {
            // Move the cube to distance in front of camera
            Vector3 cubePos = mainCamera.transform.position + (mainCamera.transform.forward * 5);
            
            Renderer rend = boundingFloor.GetComponent<Renderer>();
            //Debug.Log(rend.bounds.min + " , " + rend.bounds.max);

            // if in bounds, move the cube
            if (cubePos.x > rend.bounds.min.x &&
                cubePos.x < rend.bounds.max.x &&
                cubePos.z > rend.bounds.min.z &&
                cubePos.z < rend.bounds.max.z &&
                cubePos.y > 0)
            {

                // move cube and check for collisions;
                displayCube.transform.position = cubePos;

                // If colliding with object, don't move
                if (displayCube.GetComponent<CubeScript>().collidingObjects.Count > 0) {
                    cannotPlaceCube = true;
                    displayCube.GetComponent<Renderer>().material.color = new Color(1, 0, 0f, 0.5f);
                }
                else {
                    cannotPlaceCube = false;
                    displayCube.GetComponent<Renderer>().material.color = new Color(1, 1, .5f, 0.5f);
                }
            }
            
            // Rotate the cube
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(0)) return;
            float rotX = Input.GetAxis("Mouse X") * dragSpeed * Mathf.Deg2Rad;
            float rotY = Input.GetAxis("Mouse Y") * dragSpeed * Mathf.Deg2Rad;

            Vector3 relativeUp = mainCamera.transform.TransformDirection(Vector3.up);
            Vector3 relativeRight = mainCamera.transform.TransformDirection(Vector3.right);

            //Turns relativeUp vector from world to objects local space
            Vector3 objectRelativeUp = displayCube.transform.InverseTransformDirection(relativeUp);
            //Turns relativeRight vector from world to object local space
            Vector3 objectRelaviveRight = displayCube.transform.InverseTransformDirection(relativeRight);

            Quaternion rotateBy = Quaternion.AngleAxis(-rotX / gameObject.transform.localScale.x, objectRelativeUp)
                * Quaternion.AngleAxis(rotY / gameObject.transform.localScale.x, objectRelaviveRight);

            displayCube.transform.localRotation *= rotateBy;

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

    public void PlaceCube()
    {
        selectedCube.transform.position = displayCube.transform.position;
        selectedCube.transform.rotation = displayCube.transform.rotation;
        DeselectCube();
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

    public void StartDestroyMode()
    {
        DeselectCube();

        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].GetComponent<Rigidbody>().isKinematic = false;
            cubes[i].transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    private Vector2 GetForceFrom(Vector3 fromPos, Vector3 toPos)
    {
        return (new Vector2(toPos.x, toPos.y) - new Vector2(fromPos.x, fromPos.y)) * 2.0f;// shotForce;
    }

    private void SetSurroundingHitboxActive(bool value)
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].transform.GetChild(0).gameObject.SetActive(value);
        }
    }
}