using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBombBehaviour : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent navMeshAgent;
    [SerializeField] float distanceToDetectPlayer;
    [SerializeField] float distanceToExplode = 1.3f;
    [SerializeField] GameObject explosionParticles;
    EnemyState currentState;

    [SerializeField] List<Transform> patrolPoints;
    private int currentPoint = 0;

    private float idleTimer;
    [SerializeField] float timeOnIdle;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        EnterState(EnemyState.PATROL);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:
                OnState_Idle();
                break;
            case EnemyState.PATROL:
                OnState_Patrol();
                break;
            case EnemyState.PURSUE:
                OnState_Pursue();
                break;
            default:
                break;
        }

    }

    private void OnState_Idle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= timeOnIdle)
        {
            EnterState(EnemyState.PATROL);
            idleTimer = 0;
        }
        if (DistanceToPlayer() <= distanceToDetectPlayer)
        {
            EnterState(EnemyState.PURSUE);
        }
    }

    private void OnState_Patrol()
    {
        if (!navMeshAgent.hasPath && navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            EnterState(EnemyState.IDLE);
        }
        if (DistanceToPlayer() <= distanceToDetectPlayer)
        {
            EnterState(EnemyState.PURSUE);
        }
    }

    private void OnState_Pursue()
    {
        navMeshAgent.SetDestination(player.transform.position);

        if (DistanceToPlayer() > distanceToDetectPlayer)
        {
            EnterState(EnemyState.IDLE);
        }
        else if (DistanceToPlayer() <= distanceToExplode)
        {
            Explode();
        }
    }

    private void EnterState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.IDLE:
                navMeshAgent.isStopped= true;
                break;
            case EnemyState.PATROL:
                navMeshAgent.isStopped = false;
                NextPatrolPoint();
                break;
            case EnemyState.PURSUE:
                navMeshAgent.isStopped = false;
                break;
            default:
                break;
        }
        currentState = state;
    }

    private void NextPatrolPoint()
    {
        navMeshAgent.SetDestination(patrolPoints[currentPoint].position);
        ++currentPoint;
        if (currentPoint >= patrolPoints.Count)
            currentPoint = 0;
    }

    private void Explode()
    {
        if (DistanceToPlayer() < distanceToExplode)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(transform.position);
        }
        GameObject.Instantiate(explosionParticles, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if bala, explode
        if(collision.gameObject.tag == "Cannonball")
        {
            Explode();
        }
    }

    private float DistanceToPlayer()
    {
        Vector3 l_PlayerPosition = player.transform.position;
        Vector3 l_EnemyPosition = transform.position;
        Vector3 l_EnemyToPlayer = l_PlayerPosition - l_EnemyPosition;
        return l_EnemyToPlayer.magnitude;
    }

}

public enum EnemyState
{
    IDLE,
    PATROL,
    PURSUE
}
