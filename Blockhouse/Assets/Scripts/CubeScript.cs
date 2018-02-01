using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{

    public bool isSelected = false;
    protected Material cubeMaterial;

    public float connectAngle = 5.5f;
    public float jointBreakForce = 5.0f;
    public float jointBreakTorque = 5.0f;

    private float checkRadius = 4.0f;

    public List<GameObject> connectedObjects;

    // Use this for initialization
    void Start()
    {
        cubeMaterial = GetComponent<Renderer>().material;
        connectedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collisionObj)
    {
        if (collisionObj.gameObject.tag != "Cube")
            return;

        // find collision point and normal.
        var point = collisionObj.contacts[0].point;
        var dir = -collisionObj.contacts[0].normal;
        point -= dir;
        RaycastHit hitInfo;
        if (collisionObj.collider.Raycast(new Ray(point, dir), out hitInfo, 2))
        {
            var normal = hitInfo.normal;
            var angle = Vector3.Angle(-transform.forward, normal);
            var diffAngle = angle % 90;

            //Debug.Log("ANGLE: " + angle);
            //Debug.Log(diffAngle);
            if (diffAngle < connectAngle)
            {
                AddFixedJoint(collisionObj.gameObject);
            }
        }
    }
    void OnCollisionStay(Collision collisionObj)
    {
        if (collisionObj.gameObject.tag != "Cube")
            return;

        // find collision point and normal.
        var point = collisionObj.contacts[0].point;
        var dir = -collisionObj.contacts[0].normal;
        point -= dir;
        RaycastHit hitInfo;
        if (collisionObj.collider.Raycast(new Ray(point, dir), out hitInfo, 2))
        {
            var normal = hitInfo.normal;
            var angle = Vector3.Angle(-transform.forward, normal);
            var diffAngle = angle % 90;

            //Debug.Log("ANGLE: " + angle);
            //Debug.Log(diffAngle);
            if (diffAngle < connectAngle)
            {
                AddFixedJoint(collisionObj.gameObject);
            }
        }
    }

    private void AddFixedJoint(GameObject collisionObj) {
        // Check if already connected
        if (connectedObjects.Contains(collisionObj)) {
            //Debug.Log("ALREADY CONNECTED");
            return;
        }
        Debug.Log("WTF ARE YOU DOING? " + connectedObjects.Count);
        connectedObjects.Add(collisionObj);

        var joint = gameObject.AddComponent<FixedJoint>();
        joint.breakForce = jointBreakForce;
        joint.breakTorque = jointBreakTorque;
        //collisionObj.GetComponent<Rigidbody>();
        //joint.connectedBody = collisionObj.GetComponent<Collision>().rigidbody;
        joint.connectedBody = collisionObj.GetComponent<Rigidbody>();
    }

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            this.isSelected = value;

            if (isSelected) {
                cubeMaterial.color = Color.red;
            }
            else {
                cubeMaterial.color = Color.white;
            }
        }
    }
}
