using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speed = 2.0f;
    private float X;
    private float Y;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Rotate Camera (Drag)
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0));
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);

           // Debug.Log(transform.forward);
        }

        // Movement
            // Forward
        if (Input.GetKey(KeyCode.W)) {
            Vector3 m = transform.forward * speed * Time.deltaTime;
            transform.Translate(m);
        }
            //Backward
        if (Input.GetKey(KeyCode.S)) {
            Vector3 m = -transform.forward * speed * Time.deltaTime;
            transform.Translate(m);
        }
            // Strafe Left
        if (Input.GetKey(KeyCode.A)) {
            Vector3 m = Vector3.Cross(transform.forward, transform.up) * speed * Time.deltaTime;
            transform.Translate(m);
        }
            // Strafe Right
        if (Input.GetKey(KeyCode.D)) {
            Vector3 m = -Vector3.Cross(transform.forward, transform.up) * speed * Time.deltaTime;
            transform.Translate(m);
        }
        // Strafe Up
        if (Input.GetKey(KeyCode.Space)) {
            Vector3 m = transform.up * speed * Time.deltaTime;
            transform.Translate(m);
        }
        // Strafe Down
        if (Input.GetKey(KeyCode.X)) {
            Vector3 m = -transform.up * speed * Time.deltaTime;
            transform.Translate(m);
        }
    }
}
