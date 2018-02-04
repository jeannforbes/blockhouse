using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class ObjectPuller : MonoBehaviour {

}
*/
public class ObjectPuller : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    
    public float pullRadius = 5;
    public float pullForce = 1000f;

    //public void FixedUpdate()
    public void Update()
    {
        foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius)) {
            if (collider.tag != "Cube")
            {
                Debug.Log(collider.gameObject);
                return;
            }

            Vector3 forceDirection = transform.position - collider.transform.position;

            // apply force on target towards me
            GetComponent<Rigidbody>().AddForce(forceDirection.normalized * pullForce * Time.fixedDeltaTime);

        }
    }
}