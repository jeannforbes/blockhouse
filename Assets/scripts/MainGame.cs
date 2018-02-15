using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour {

    // PUBLIC
    public GameObject foundation;
    public List<GameObject> blocks;
    public int numBlocksToSpawn = 15;

    // PRIVATE
    float xMax, zMax;

	// Use this for initialization
	void Start () {
        xMax = foundation.transform.lossyScale.x * 0.5f;
        zMax = foundation.transform.lossyScale.z * 0.5f;

        Invoke("SpawnBlocks", 5);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnBlocks()
    {
        if (blocks.Count <= 0 || !foundation) return;

        GameObject block;
        for(int i=0; i<numBlocksToSpawn; i++)
        {
            block = Instantiate(blocks[Random.Range(0, blocks.Count)]);
            block.transform.Translate(new Vector3(Random.Range(-xMax, xMax), 1f, Random.Range(-zMax, zMax)));
            block.transform.parent = GameObject.FindGameObjectWithTag("GroundPlaneStage").transform;
        }
    }
}
