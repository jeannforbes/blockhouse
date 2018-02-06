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

    public GameObject selectedCube;
    private GameObject displayCube;
    private GameObject tempCube;

    public GameObject[] cubes;

    public float shotForce = 1000.0f;
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
                    Destroy(displayCube.GetComponent<BoxCollider>());
                    Destroy(displayCube.GetComponent<Rigidbody>());
                    displayCube.transform.GetChild(0).gameObject.SetActive(false);
                    displayCube.GetComponent<Renderer>().material.color = new Color(1, 1, .5f, 0.5f);

                    SetSurroundingHitboxActive(true);
                }

                else
                {
                    selectedCube.transform.position = displayCube.transform.position;

                    //selectedCube.GetComponent<CubeScript>().IsSelected = false;
                    //selectedCube = null;
                    //Destroy(displayCube);
                    //displayCube = null;
                    DeselectCube();


                }
            }

            tempCube = null;
        }

        // Place the cube
        if (displayCube != null)
        {
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hitInfo;
            if (Physics.Raycast(castPoint, out hitInfo))
            {
                //Debug.Log(hitInfo.transform.gameObject.name);

                Vector3 cubePos = hitInfo.point - (hitInfo.normal / 2);
                if (hitInfo.transform.gameObject.tag == "Floor")
                {
                    cubePos = hitInfo.point + (hitInfo.normal / 2);
                }
                cubePos.x = Mathf.Round(cubePos.x);
                cubePos.y = Mathf.Round(cubePos.y);
                cubePos.z = Mathf.Round(cubePos.z);
                displayCube.transform.position = cubePos;
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
