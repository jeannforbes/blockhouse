using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    // singleton-type GameManager
    public static GameManagerScript instance = null;

    public enum GameStates {
        Build,
        Destroy
    }

    public GameStates gameState;
    public Canvas canvas;

    public GameStates GameState {
        get {
            return gameState;
        }
        set {
            gameState = value;
            
            if (gameState == GameStates.Build) {
                canvas.GetComponent<CanvasScript>().destroyMode.SetActive(false);
                canvas.GetComponent<CanvasScript>().buildMode.SetActive(true);
            }
            else if (gameState == GameStates.Destroy)
            {
                canvas.GetComponent<CanvasScript>().buildMode.SetActive(false);
                canvas.GetComponent<CanvasScript>().destroyMode.SetActive(true);
            }
        }
    }


    private Camera mainCamera;
    private GameObject selectedCube;
    private GameObject tempCube;

    public GameObject[] cubes;
    
    public float shotForce = 1000.0f;
    private Vector3 fireDirection;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        selectedCube = null;
        tempCube = null;

        mainCamera = Camera.main;

        //GameState = GameStates.Build;
        gameState = GameStates.Destroy;

        cubes = GameObject.FindGameObjectsWithTag("Cube");
    }
	
	// Update is called once per frame
	void Update () {

        // DESTROY
        if (gameState == GameStates.Destroy)
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

                    // Remove joints
                    FixedJoint[] joints = selectedCube.GetComponents<FixedJoint>();
                    Debug.Log("WTF: " + joints.Length);
                    for (int i = 0; i < joints.Length; i++) {
                        Destroy(joints[i]);
                    }

                    Vector3 shotVector = fireDirection * shotForce;
                    selectedCube.GetComponent<Rigidbody>().AddForce(shotVector);
                }
            }
        }


        // BUILD
        if (gameState == GameStates.Build) { }

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
