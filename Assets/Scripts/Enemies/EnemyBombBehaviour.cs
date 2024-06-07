using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBombBehaviour : MonoBehaviour
{
    private PlayerController player;
    private NavMeshAgent navMeshAgent;
    [SerializeField] float distanceToDetectPlayer;
    [SerializeField] float distanceToExplode = 1.3f;
    [SerializeField] GameObject explosionParticles;
    EnemyState currentState;

    [SerializeField] List<Transform> patrolPoints;
    private int currentPoint = 0;

    private float idleTimer;
    [SerializeField] float timeOnIdle;
    [SerializeField] Animator enemyAnimator;

    [SerializeField] StudioEventEmitter bombMovingSound;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        //player = GameController.GetGameController().GetPlayer();
        //player = GameController.GetGameController().GetPlayer();
        navMeshAgent = GetComponent<NavMeshAgent>();
        EnterState(EnemyState.PATROL);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:
                enemyAnimator.SetTrigger("Idle");
                OnState_Idle();
                break;
            case EnemyState.PATROL:
                enemyAnimator.SetTrigger("Walk");
                OnState_Patrol();
                break;
            case EnemyState.PURSUE:
                enemyAnimator.SetTrigger("Walk");
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
        GameObject particles = GameObject.Instantiate(explosionParticles, transform.position + (Vector3.up * (navMeshAgent.height/2)), transform.rotation);
        particles.transform.localScale *= 1.5f;

        bombMovingSound.Stop(); bombMovingSound.PlayEvent = EmitterGameEvent.None;
        FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().bombAttackDeathSound, transform.position);

        if(Random.Range(1, 4) == 1)
            FindObjectOfType<AudioManager>().SetPlaySfx(FindObjectOfType<AudioManager>().circusMasterHeySound, transform.position);
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
