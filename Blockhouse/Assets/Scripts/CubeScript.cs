using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : ObjectScript
{
    public bool isSelected = false;
    
    public float connectAngle = 505.5f;
    public float jointBreakForce = 500.0f;
    public float jointBreakTorque = 500.0f;

    //private float checkRadius = 4.0f;

    public List<GameObject> collidingObjects;
    public List<GameObject> connectedObjects;

    // Use this for initialization
    public override void Start()
    {
        objectMaterial = GetComponent<Renderer>().material;

        collidingObjects = new List<GameObject>();
        connectedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    public override void Update()
    {
    }

    void OnCollisionEnter(Collision collisionObj)
    {
        // Add to colliding objects
        collidingObjects.Add(collisionObj.gameObject);

        // Check if Cube
        if (collisionObj.gameObject.tag != "Cube")
            return;

        // get fin the team of the object
        int objTeam = collisionObj.gameObject.GetComponent<ObjectScript>().team;

        // if on the same team, create joint when touching
        if (objTeam == team)
        {
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
    }
    // Do the same thing in CollisionStay as CollisionEnter because one sometimes misses what the other catches.
    void OnCollisionStay(Collision collisionObj)
    {
        if (collisionObj.gameObject.tag != "Cube")
            return;

        // get fin the team of the object
        int objTeam = collisionObj.gameObject.GetComponent<ObjectScript>().team;

        // if on the same team, create joint when touching
        if (objTeam == team)
        {
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
    }
    public void OnCollisionExit(Collision collisionObj)
    {
        collidingObjects.Remove(collisionObj.gameObject);
    }

    public void AddFixedJoint(GameObject collisionObj) {
        // Check if already connected
        if (connectedObjects.Contains(collisionObj)) {
            //Debug.Log("ALREADY CONNECTED");
            return;
        }

        connectedObjects.Add(collisionObj);

        var joint = gameObject.AddComponent<FixedJoint>();
        joint.breakForce = jointBreakForce;
        joint.breakTorque = jointBreakTorque;
        joint.connectedBody = collisionObj.GetComponent<Rigidbody>();

        collisionObj.GetComponent<CubeScript>().AddFixedJoint(gameObject);
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

            if (isSelected)
            {
                objectMaterial.color = Color.white;
            }
            else
            {
                objectMaterial.color = teamColor;
            }
        }
    }
}
