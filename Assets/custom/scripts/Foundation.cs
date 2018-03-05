using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Foundation : MonoBehaviour {

    // PUBLIC
    public int numBlocksToSpawn = 10;
    public List<GameObject> blocks;

    // PRIVATE
    private bool blocksSpawned, throwingEnabled;

	// Use this for initialization
	void Start () {
        Debug.Log("Created foundation");
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.blocksSpawned)
        {
            Invoke("SpawnBlocks", 10f);
            this.blocksSpawned = true;
        }
        else if (!this.throwingEnabled)
        {
            Invoke("EnableThrowing", 30f);
            this.throwingEnabled = true;
        }
    }

    void SpawnBlocks()
    {
        Debug.Log("Spawning blocks");

        // Do we need to spawn blocks?
        if (blocks.Count <= 0 || numBlocksToSpawn <= 0) return;

        // Update instructions
        GameObject.FindGameObjectWithTag("Instructions").GetComponent<Text>().text = "Build a base with your blocks!";

        // Find global bounds of the foundation
        float xBound = this.transform.position.x + (Mathf.Floor(this.transform.lossyScale.x) * 0.5f);
        float zBound = this.transform.position.z + (Mathf.Floor(this.transform.lossyScale.z) * 0.5f);
        Debug.Log(xBound + ", " + zBound);

        GameObject block;
        for (int i = 0; i < numBlocksToSpawn; i++)
        {
            block = Instantiate(blocks[Random.Range(0, blocks.Count)]);
            block.transform.position = new Vector3(
                Random.Range(-xBound, xBound), 
                Random.Range(1f,2f), 
                Random.Range(-zBound, zBound));
            block.transform.parent = this.transform;
        }
    }

    void EnableThrowing()
    {
        Debug.Log("Enabling throwing");

        // Update instructions
        GameObject.FindGameObjectWithTag("Instructions").GetComponent<Text>().text = "Throw stuff at your opponent!";

        for (int i=0; i<blocks.Count; i++)
        {
            blocks[i].GetComponent<Block>().throwable = true;
        }
    }
}
