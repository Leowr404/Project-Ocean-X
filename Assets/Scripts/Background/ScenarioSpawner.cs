using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioSpawner : MonoBehaviour
{
    //Nao esta bem otimizado
    public Transform[] spawnPoints;  // Array de pontos de spawn
    public GameObject[] scenarioItems;  // Array de itens de cen�rio
    public float spawnInterval = 4f;   // Intervalo entre spawns
    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnScenarioItems(); // Modificado para m�ltiplos spawns
            spawnTimer = 0f;
        }
    }

    void SpawnScenarioItems()
    {
        // Escolher um n�mero aleat�rio de pontos de spawn, entre 1 e o n�mero m�ximo (6)
        int numberOfSpawns = Random.Range(1, spawnPoints.Length + 1);

        // Criar uma lista de pontos j� escolhidos para n�o repetir
        bool[] usedSpawnPoints = new bool[spawnPoints.Length];

        for (int i = 0; i < numberOfSpawns; i++)
        {
           
            int spawnIndex;
            do
            {
                spawnIndex = Random.Range(0, spawnPoints.Length);
            } while (usedSpawnPoints[spawnIndex]); 
            usedSpawnPoints[spawnIndex] = true;
            // Escolher um item de cen�rio aleat�rio
            int itemIndex = Random.Range(0, scenarioItems.Length);
            Quaternion uprightRotation = Quaternion.Euler(-90f, 0f, 0f);
            Instantiate(scenarioItems[itemIndex], spawnPoints[spawnIndex].position, uprightRotation);
        }
    }
}
