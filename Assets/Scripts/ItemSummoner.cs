using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSummoner : MonoBehaviour
{
    public GameObject[] Drops;
    private float spawnRate;
    private float timeSinceLastSpawn;
    public Transform[] spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnRate = Random.Range(4f, 7f);
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnRate)
        {
            timeSinceLastSpawn = 0;
            int randomDrop = Random.Range(0, Drops.Length);
            int randomIndex = Random.Range(0, spawnPoint.Length);
            Instantiate(Drops[randomDrop], spawnPoint[randomIndex].position, Quaternion.identity);
        }

    }
}
