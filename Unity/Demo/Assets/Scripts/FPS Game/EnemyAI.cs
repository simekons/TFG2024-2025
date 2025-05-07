using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public float wanderRadius = 10f; // Radio de movimiento aleatorio
    public float wanderInterval = 3f; // Tiempo entre cambios de dirección
    public float detectionRange = 3f;
    public float wanderSpeed = 2f;
    public float chaseSpeed = 1f;
    public float attackCooldown = 1.5f; // Tiempo entre ataques

    private NavMeshAgent agent;
    private float wanderTimer = 0f;
    private float lastAttackTime = -Mathf.Infinity; // Última vez que atacó
    private bool isChasing = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player"); // Buscar por nombre en la escena
        SetNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            if(!isChasing)
            {
                isChasing = true;
                agent.speed = chaseSpeed;
            }
            agent.SetDestination(player.transform.position);
        }
        else
        {
            if(isChasing)
            {
                isChasing = false;
                agent.speed = wanderSpeed;
                wanderTimer = 0f;
            }

            wanderTimer += Time.deltaTime;

            if (wanderTimer >= wanderInterval)
            {
                SetNewDestination();
                wanderTimer = 0f;
            }
        }
    }

    void SetNewDestination()
    {
        if (isChasing) return;

        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(Time.time >= lastAttackTime + attackCooldown)
            {
                PlayerHealth health = other.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(25); // Le baja 10 puntos
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    private void OnDestroy()
    {
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.EnemyDied();
        }
    }
}
