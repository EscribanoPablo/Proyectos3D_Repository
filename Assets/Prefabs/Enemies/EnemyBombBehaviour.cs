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
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:
                break;
            case EnemyState.PATROL:
                break;
            case EnemyState.PURSUE:
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
        }
    }

    private void OnState_Patrol()
    {

    }

    private void OnState_Pursue()
    {

        if ((DistanceToPlayer() > distanceToDetectPlayer))
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        else
        {
            Explode();
        }
    }

    private void EnterState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.IDLE:
                break;
            case EnemyState.PATROL:
                NextPatrolPoint();
                break;
            case EnemyState.PURSUE:
                break;
            default:
                break;
        }
        currentState = state;
    }

    private void NextPatrolPoint()
    {
        ++currentPoint;
        if (currentPoint >= patrolPoints.Count)
            currentPoint = 0;
        navMeshAgent.SetDestination(patrolPoints[currentPoint].position);
    }

    private void Explode()
    {

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
