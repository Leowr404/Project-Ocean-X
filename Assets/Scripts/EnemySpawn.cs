using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField]private TextMeshProUGUI pontosTxt;
    [SerializeField] private TextMeshProUGUI pontosUI;

    private float timeSinceLastSpawn;
    public int currentPoints = 0;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        pontosTxt.color = Color.green;
        SpawnerOn = true;
    }
    void Update()
    {
        spawnRate = Random.Range(0.5f,1.5f);
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
        pontosTxt.text = currentPoints.ToString("00");
        pontosUI.text = currentPoints.ToString("00");
    }

    public void AddPoints(int points)
    {
        currentPoints += points;
    }
}
