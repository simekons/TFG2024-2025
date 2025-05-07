using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab; // Prefab del enemigo
    public Transform[] spawnPoints; // Puntos de spawn
    public float spawnInterval = 3f; // Tiempo entre spawns
    public int maxEnemies = 8; // Máximo de enemigos en escena

    private float spawnTimer = 0f;
    private int enemyCount = 0;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && enemyCount < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // Selecciona un punto aleatorio de spawn
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instancia el enemigo en el punto seleccionado
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        enemyCount++;
    }

    public void EnemyDied()
    {
        enemyCount--; // Llamar a esto cuando un enemigo muera
    }

    
}
