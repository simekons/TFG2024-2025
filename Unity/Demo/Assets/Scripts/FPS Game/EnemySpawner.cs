using System.Collections;
using System.Collections.Generic;
using Telemetry;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab; // Prefab del enemigo
    public Transform[] spawnPoints; // Puntos de spawn
    public float spawnInterval = 3f; // Tiempo entre spawns
    public int maxEnemies = 8; // Máximo de enemigos en escena

    private float spawnTimer = 0f;
    private int enemyCount = 0;
    private static EnemySpawner instance;

    public static EnemySpawner Instance()
    {
        if (instance != null)
            return instance;
        else
            return instance = new EnemySpawner();
    }

    private void Start()
    {
        Tracker.getInstance().startGameFPS();
    }
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
        GameObject enemy =Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        enemy.name = enemyCount.ToString();

        print("New enemy is called . . ." + enemy.name);

        print("Enemy " + (5 - enemyCount) + " just spawned.");

        Tracker.getInstance().enemyAppearEvent(5 - enemyCount, "0");

        enemyCount++;
    }

    public void EnemyDied()
    {
        enemyCount--; // Llamar a esto cuando un enemigo muera
    }

    
}
