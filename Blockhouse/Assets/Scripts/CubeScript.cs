using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : ObjectScript
{
    public bool isSelected = false;
    
    public float connectAngle = 5.5f;
    public float jointBreakForce = 50.0f;
    public float jointBreakTorque = 50.0f;

    //private float checkRadius = 4.0f;

    public List<GameObject> collidingObjects;
    public List<GameObject> connectedObjects;

    public bool shatterable;

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

        /*   SHATTER CODE
         *   Commented out for now
        //Debug.Log(collisionObj.relativeVelocity.magnitude);
        if (shatterable && collisionObj.relativeVelocity.magnitude > 50) {
            // Shatter
            Debug.Log("SHATTER");
            
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;

                //transform.GetChild(i).parent = null;
                child.tag = "Cube";
                child.SetActive(true);
                child.GetComponent<CubeScript>().team = team;
                child.GetComponent<CubeScript>().shatterable = false;
                child.GetComponent<BoxCollider>().enabled = true;
                child.GetComponent<CubeScript>().enabled = true;
                child.AddComponent<Rigidbody>();
                
            }
            transform.DetachChildren();
            Destroy(gameObject);
            DestroyGameManagerScript.instance.cubes = GameObject.FindGameObjectsWithTag("Cube");
        }
        */
        HandleCollisions(collisionObj);
    }
    // Do the same thing in CollisionStay as CollisionEnter because one sometimes misses what the other catches.
    void OnCollisionStay(Collision collisionObj)
    {
        HandleCollisions(collisionObj);
    }
    public void OnCollisionExit(Collision collisionObj)
    {
        collidingObjects.Remove(collisionObj.gameObject);
    }

    private void HandleCollisions(Collision collisionObj) {
        // Check if Cube
        if (collisionObj.gameObject.tag != "Cube" && collisionObj.gameObject.GetComponent<ObjectScript>() == null)
            return;


        if (collisionObj.gameObject.GetComponent<ObjectScript>() == null)
        {
            //Debug.Log("AAA");
            //Debug.Log(collisionObj.gameObject);
            //Debug.Log(collisionObj.gameObject.GetComponent<ObjectScript>().team);
        }
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
                SetChildrenColor();
            }
            else
            {
                objectMaterial.color = teamColor;
                SetChildrenColor();
            }
        }
    }
}
