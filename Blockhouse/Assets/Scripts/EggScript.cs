using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : ObjectScript {

	// Use this for initialization
	public override void Start ()
    {
        objectMaterial = GetComponent<Renderer>().material;
        
        // turn off isKinematic and freeze rotation and position to allow for collision detection
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        GetComponent<Rigidbody>().isKinematic = false;
    }
	
	// Update is called once per frame
	public override void Update () {
		
	}

    void OnCollisionEnter(Collision collisionObj)
    {
        Vector3 velocityVector = collisionObj.relativeVelocity;
        float velocity = velocityVector.magnitude;
        float mass = collisionObj.gameObject.GetComponent<Rigidbody>().mass;
        int collisionObjTeam = collisionObj.gameObject.GetComponent<ObjectScript>().team;
        Debug.Log("EGG HIT! Vel:" + velocity + ", Mass:" + mass + ", Team:" + team);
    }
}
