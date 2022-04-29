using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public float spawnTime;
    public GameObject rock;
    public float timeSinceSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
           timeSinceSpawn += Time.deltaTime; 

        if(timeSinceSpawn>spawnTime)
        {
            Instantiate(rock, this.transform.position, this.transform.rotation);
            timeSinceSpawn = 0;
        }
    }
}
