using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    public enum EnemyState
    {
        None,
        Patrol,
        Chase,
        Dead,
        Attack,
    }
    public int health = 100;
    public float patrolRadius = 10;
    public float viewDistance = 5;
    public float attackDistance = 3;
    public GameObject player;

    private EnemyState currentState;
    private NavMeshAgent navMeshAgent;
    private Vector3 patrolDestination;

    public GameObject poisenCloud;
    private float lastAttack;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            currentState = EnemyState.Dead;
        }

        switch (currentState)
        {
            case EnemyState.Patrol: UpdatePatrolState(); break;
            case EnemyState.Chase: UpdateChaseState(); break;
            case EnemyState.Attack: UpdateAttackState(); break;
            case EnemyState.Dead: UpdateDeadState(); break;
        }
    }

    void UpdatePatrolState()
    {
        // Check if player is insight
        CheckForPlayer();


        if (navMeshAgent.remainingDistance <= 0) {
            CalculateNewDestination();
        }
    }

    void UpdateChaseState()
    {
        navMeshAgent.SetDestination(player.transform.position);
        
        if (navMeshAgent.remainingDistance <= attackDistance) {
            currentState = EnemyState.Attack;
        }
    }

    void UpdateAttackState()
    {
        if (Time.realtimeSinceStartup >= lastAttack + 10)
        {
            currentState = EnemyState.Patrol;
            lastAttack = 0;
        }

        if (lastAttack == 0)
        {
            // Attack;
            Instantiate(poisenCloud, player.transform.position, Quaternion.identity);
            lastAttack = Time.realtimeSinceStartup;
            navMeshAgent.destination = transform.position;
        }

    }

    void UpdateDeadState()
    {

    }

    void CheckForPlayer()
    {
        if(DistanceTo(player.transform.position) < viewDistance) 
        {
            currentState = EnemyState.Chase;
        }
    }

    void CalculateNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1);
        Vector3 finalPosition = hit.position;
        navMeshAgent.SetDestination(finalPosition);
        Debug.DrawRay(finalPosition, Vector3.up, Color.red, Mathf.Infinity);
    }

    float DistanceTo(Vector3 otherPoint) 
    {
        return Vector3.Distance(transform.position, otherPoint);
    }
}
