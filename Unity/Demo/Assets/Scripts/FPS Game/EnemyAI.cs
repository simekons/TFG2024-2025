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

    private NavMeshAgent agent;
    private float wanderTimer = 0f;
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
            isChasing = true;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            isChasing = false;
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
}
