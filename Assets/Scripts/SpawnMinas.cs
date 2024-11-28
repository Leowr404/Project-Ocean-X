using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnMinas : MonoBehaviour
{
    public GameObject mina;
    public float spawnRate;
    public Transform[] spawnPoints;
    public bool SpawnerOn = true;

    private float timeSinceLastSpawn;
    public int currentPoints = 0;
    private void Start()
    {
        SpawnerOn = true;
    }
    void Update()
    {
        spawnRate = Random.Range(1f, 3f);
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnRate && SpawnerOn == true)
        {
            timeSinceLastSpawn = 0;
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(mina, spawnPoints[randomIndex].position, Quaternion.identity);
        }
       
    }

}
