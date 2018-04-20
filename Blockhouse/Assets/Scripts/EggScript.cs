using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : ObjectScript {

    public Vector3 camPosition;

    public int maxHealth = 3;
    public int currentHealth;

    // Force needed to take one damage
    private int OneDamageForce = 10;

	// Use this for initialization
	public override void Start ()
    {
        currentHealth = maxHealth;

        objectMaterial = GetComponent<Renderer>().material;
        
        // turn off isKinematic and freeze rotation and position to allow for collision detection
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
        GetComponent<Rigidbody>().isKinematic = false;

        camPosition = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
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

        float force = mass * velocity;
        if (collisionObjTeam != team && force >= OneDamageForce) {
            currentHealth--;

            if (currentHealth <= 0) {
                DestroyGameManagerScript.instance.EggDied(team);
            }
        }

        DestroyGameManagerScript.instance.UpdateHealth();
    }
}
