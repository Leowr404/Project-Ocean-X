using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSpawner : MonoBehaviour
{
    //Nao esta bem otimizado
    public Transform[] spawnPoints;  // Array de pontos de spawn
    public GameObject[] scenarioItems;  // Array de itens de cenário
    public float spawnInterval = 4f;   // Intervalo entre spawns
    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnScenarioItems(); // Modificado para múltiplos spawns
            spawnTimer = 0f;
        }
    }

    void SpawnScenarioItems()
    {
        // Escolher um número aleatório de pontos de spawn, entre 1 e o número máximo (6)
        int numberOfSpawns = Random.Range(1, spawnPoints.Length + 1);

        // Criar uma lista de pontos já escolhidos para não repetir
        bool[] usedSpawnPoints = new bool[spawnPoints.Length];

        for (int i = 0; i < numberOfSpawns; i++)
        {
           
            int spawnIndex;
            do
            {
                spawnIndex = Random.Range(0, spawnPoints.Length);
            } while (usedSpawnPoints[spawnIndex]); 
            usedSpawnPoints[spawnIndex] = true;
            // Escolher um item de cenário aleatório
            int itemIndex = Random.Range(0, scenarioItems.Length);
            Quaternion uprightRotation = Quaternion.Euler(-90f, 0f, 0f);
            Instantiate(scenarioItems[itemIndex], spawnPoints[spawnIndex].position, uprightRotation);
        }
    }
}
