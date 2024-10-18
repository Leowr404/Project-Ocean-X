using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn instance; 
    public GameObject enemyType1;
    public GameObject enemyType2;
    public GameObject boss;
    public float spawnRate;
    public Transform[] spawnPoints;
    public int pointsToSpawnBoss = 100;
    public bool spawnBoss = false;
    public bool SpawnerOn = true;

    private float timeSinceLastSpawn;
    public int currentPoints = 0;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        spawnRate = Random.Range(1f,2f);
        SpawnerOn = true;
    }
    void Update()
    {
        if(currentPoints >= pointsToSpawnBoss)
        {
            spawnBoss = true;
        }

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnRate && SpawnerOn == true)
        {
            timeSinceLastSpawn = 0;

            if (currentPoints < pointsToSpawnBoss && spawnBoss == false)
            {
                // Escolhe aleatoriamente um tipo de inimigo
                int randomIndex = Random.Range(0, spawnPoints.Length);
                GameObject enemyToSpawn = Random.value < 0.5f ? enemyType1 : enemyType2;
                Instantiate(enemyToSpawn, spawnPoints[randomIndex].position, Quaternion.identity);
            }
            else if  (currentPoints > pointsToSpawnBoss && spawnBoss == true)
            {
                
                Instantiate(boss, spawnPoints[0].position, Quaternion.identity);
                SpawnerOn = false;

                
            }
        }
        
    }

    public void AddPoints(int points)
    {
        currentPoints += points;
    }
}
