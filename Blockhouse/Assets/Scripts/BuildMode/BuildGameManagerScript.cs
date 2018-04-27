using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGameManagerScript : MonoBehaviour
{
    // singleton-type GameManager
    public static BuildGameManagerScript instance = null;
    
    public Canvas canvas;

    private Camera mainCamera;
    public GameObject allCubes;
    public GameObject allBoundingFloors;
    public GameObject allEggs;

    public GameObject selectedCube;
    private GameObject displayCube;
    private GameObject tempCube;

    public GameObject[] cubes;
    public GameObject[] eggs;

    public float shotForce = 1000.0f;
    private Vector3 fireDirection;

    private float dragSpeed = 600f;
    private Vector3 dragOrigin;

    public int numTeams;
    public int currentTeam = 0;
    
    public GameObject[] boundingFloors;

    public bool cannotPlaceCube;

    private void Awake()
    {
        // singleton-type setup
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        // Place all objects sent to Destroy scene on DontDestroyOnLoad
        DontDestroyOnLoad(allCubes);
        DontDestroyOnLoad(allBoundingFloors);
        DontDestroyOnLoad(allEggs);
    }

    // Use this for initialization
    void Start()
    {
        selectedCube = null;
        tempCube = null;
        displayCube = null;

        mainCamera = Camera.main;


        // Set team colors
        numTeams = 2;
        Color team0Color = Color.red;
        Color team1Color = Color.blue;

        // Cubes
        cubes = GameObject.FindGameObjectsWithTag("Cube");

        //SetSurroundingHitboxActive(false);

        for (int i = 0; i < cubes.Length; i++)
        {
            //cubes[i].GetComponent<Rigidbody>().isKinematic = true;

            // set the team colors
            ObjectScript oScript = cubes[i].GetComponent<ObjectScript>();
            switch (oScript.team)
            {
                case 0:
                    oScript.teamColor = team0Color;
                    break;
                case 1:
                    oScript.teamColor = team1Color;
                    break;
            }
            oScript.objectMaterial.color = oScript.teamColor;
            oScript.SetChildrenColor();
        }

        // Eggs
        eggs = GameObject.FindGameObjectsWithTag("Egg");

        //SetSurroundingHitboxActive(false);

        for (int i = 0; i < eggs.Length; i++)
        {
            eggs[i].GetComponent<Rigidbody>().isKinematic = true;

            // set the team colors
            ObjectScript oScript = eggs[i].GetComponent<ObjectScript>();
            switch (oScript.team)
            {
                case 0:
                    oScript.teamColor = team0Color;
                    break;
                case 1:
                    oScript.teamColor = team1Color;
                    break;
            }
            oScript.objectMaterial.color = oScript.teamColor;
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

            // SELECT CUBE ON CLICK
            if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();

            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit && hitInfo.transform.gameObject.tag == "Cube" &&
                hitInfo.transform.gameObject.GetComponent<CubeScript>() != null  &&
                hitInfo.transform.gameObject.GetComponent<CubeScript>().team == currentTeam)
            {
                //Debug.Log("CUBE");
                tempCube = hitInfo.transform.gameObject;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit && hitInfo.transform.gameObject.tag == "Cube" && hitInfo.transform.gameObject == tempCube &&
                hitInfo.transform.gameObject.GetComponent<CubeScript>() != null &&
                hitInfo.transform.gameObject.GetComponent<CubeScript>().team == currentTeam)
            {
                //Debug.Log("HIT CUBE");
                if (selectedCube == null)
                {
                    SelectCube(hitInfo.transform.gameObject);

                    // create a display cube to simulate where the cube will be placed
                    displayCube = Instantiate(selectedCube);
                    //displayCube.transform.GetChild(0).gameObject.SetActive(false);
                    displayCube.GetComponent<Renderer>().material.color = new Color(1, 1, .5f, 0.5f);
                    
                    SetAllCubesKinematic(true);

                    // turn off isKinematic and freeze rotation and position to allow for collision detection
                    displayCube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
                    displayCube.GetComponent<Rigidbody>().isKinematic = false;

                    //SetSurroundingHitboxActive(true);
                }
            }

            tempCube = null;
        }

        // MOVE DISPLAY CUBE IN RELATION TO CAMERA
        if (displayCube != null)
        {
            // Move the cube to distance in front of camera
            Vector3 cubePos = mainCamera.transform.position + (mainCamera.transform.forward * 5);

            // if in bounds, move the cube
            Renderer rend = boundingFloors[currentTeam].GetComponent<Renderer>();
            
            // move cube and check for collisions;
            displayCube.transform.position = cubePos;

            // check if display cube is colliding with other objects
            // cannot place the cube if it is colliding
            if (displayCube.GetComponent<CubeScript>().collidingObjects.Count > 0 ||    // if not colliding
                    !(cubePos.x > rend.bounds.min.x &&                                  // or not in bounds
                    cubePos.x < rend.bounds.max.x &&
                    cubePos.z > rend.bounds.min.z &&                                    // cannot place
                    cubePos.z < rend.bounds.max.z)
                    ) {
                cannotPlaceCube = true;
                displayCube.GetComponent<Renderer>().material.color = new Color(1, 0, 0f, 0.5f);
            }
            else {
                cannotPlaceCube = false;
                displayCube.GetComponent<Renderer>().material.color = new Color(1, 1, .5f, 0.5f);
            }
            
            // Rotate the cube on drag
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

        SetAllCubesKinematic(false);
    }

    // All things necessary for switching scene to Destroy mode
    public void StartDestroyMode()
    {
        DeselectCube();

        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].GetComponent<Rigidbody>().isKinematic = false;
            //cubes[i].transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    public void SetAllCubesKinematic(bool isKin)
    {
        Debug.Log("KINEMATIC : " + isKin);
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].GetComponent<Rigidbody>().isKinematic = isKin;
        }
    }
        /*
        private void SetSurroundingHitboxActive(bool value)
        {
            for (int i = 0; i < cubes.Length; i++)
            {
                if (cubes[i].transform.childCount != 0)
                {
                    cubes[i].transform.GetChild(0).gameObject.SetActive(value);
                }
            }
        }
        */
}
