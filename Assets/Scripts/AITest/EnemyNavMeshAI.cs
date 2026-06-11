using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyNavMeshAI : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Detection")]
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float loseRange = 16f;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float patrolStoppingDistance = 0.2f;
    [SerializeField] private float chaseStoppingDistance = 1.8f;
    [SerializeField] private float waypointReachDistance = 0.4f;
    [SerializeField] private float waitTimeAtWaypoint = 1.5f;
    [SerializeField] private bool randomPatrol = false;

    [Header("Animation")]
    [SerializeField] private string speedParameter = "Speed";
    [SerializeField] private float animationDampTime = 0.1f;

    [Header("Rotation")]
    [SerializeField] private float facePlayerSpeed = 8f;

    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private EnemyState currentState;
    private bool stateInitialized;

    private int patrolIndex;
    private float waitTimer;

    private bool HasPatrolPoints
    {
        get
        {
            return patrolPoints != null && patrolPoints.Length > 0;
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Start()
    {
        if (HasPatrolPoints)
        {
            ChangeState(EnemyState.Patrol);
        }
        else
        {
            ChangeState(EnemyState.Idle);
        }
    }

    private void Update()
    {
        CheckForPlayer();

        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdle();
                break;

            case EnemyState.Patrol:
                UpdatePatrol();
                break;

            case EnemyState.Chase:
                UpdateChase();
                break;
        }

        UpdateAnimations();
    }

    private void CheckForPlayer()
    {
        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (currentState != EnemyState.Chase && distanceToPlayer <= chaseRange)
        {
            ChangeState(EnemyState.Chase);
        }
        else if (currentState == EnemyState.Chase && distanceToPlayer >= loseRange)
        {
            if (HasPatrolPoints)
            {
                ChangeState(EnemyState.Patrol);
            }
            else
            {
                ChangeState(EnemyState.Idle);
            }
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (stateInitialized && currentState == newState)
        {
            return;
        }

        stateInitialized = true;
        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
                EnterIdle();
                break;

            case EnemyState.Patrol:
                EnterPatrol();
                break;

            case EnemyState.Chase:
                EnterChase();
                break;
            case EnemyState.Attack:
                EnterAttack();
                break; 
        }
    }

    private void EnterIdle()
    {
        agent.isStopped = true;
        agent.ResetPath();
        waitTimer = 0f;
    }

    private void UpdateIdle()
    {
        // Enemy stays idle until the player enters chaseRange.
    }

    private void EnterPatrol()
    {
        if (!HasPatrolPoints)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        agent.isStopped = false;
        agent.speed = walkSpeed;
        agent.stoppingDistance = patrolStoppingDistance;

        waitTimer = 0f;

        SetCurrentPatrolDestination();
    }

    private void EnterAttack()
    {
        animator.SetTrigger("Attack");
        ChangeState(EnemyState.Chase);
    }

    private void UpdatePatrol()
    {
        if (!HasPatrolPoints)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        if (!ReachedDestination())
        {
            return;
        }

        agent.isStopped = true;
        waitTimer += Time.deltaTime;

        if (waitTimer >= waitTimeAtWaypoint)
        {
            waitTimer = 0f;

            ChooseNextPatrolPoint();

            agent.isStopped = false;
            SetCurrentPatrolDestination();
        }
    }

    private void EnterChase()
    {
        agent.isStopped = false;
        agent.speed = runSpeed;
        agent.stoppingDistance = chaseStoppingDistance;
        waitTimer = 0f;
    }

    private void UpdateChase()
    {
        if (player == null)
        {
            if (HasPatrolPoints)
            {
                ChangeState(EnemyState.Patrol);
            }
            else
            {
                ChangeState(EnemyState.Idle);
            }

            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseStoppingDistance)
        {
            agent.isStopped = true;
            agent.ResetPath();
            FaceTarget(player.position);
            animator.SetTrigger("Attack");
            ChangeState(EnemyState.Attack);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    private bool ReachedDestination()
    {
        if (agent.pathPending)
        {
            return false;
        }

        if (agent.remainingDistance == Mathf.Infinity)
        {
            return false;
        }

        float reachDistance = Mathf.Max(agent.stoppingDistance, waypointReachDistance);

        return agent.remainingDistance <= reachDistance;
    }

    private void SetCurrentPatrolDestination()
    {
        if (!HasPatrolPoints)
        {
            return;
        }

        Transform point = patrolPoints[patrolIndex];

        if (point == null)
        {
            ChooseNextPatrolPoint();
            point = patrolPoints[patrolIndex];
        }

        if (point != null)
        {
            agent.SetDestination(point.position);
        }
    }

    private void ChooseNextPatrolPoint()
    {
        if (!HasPatrolPoints)
        {
            return;
        }

        if (randomPatrol && patrolPoints.Length > 1)
        {
            int nextIndex = patrolIndex;

            while (nextIndex == patrolIndex)
            {
                nextIndex = Random.Range(0, patrolPoints.Length);
            }

            patrolIndex = nextIndex;
        }
        else
        {
            patrolIndex++;

            if (patrolIndex >= patrolPoints.Length)
            {
                patrolIndex = 0;
            }
        }
    }

    private void UpdateAnimations()
    {
        float animationSpeed = 0f;

        bool isMoving = agent.velocity.sqrMagnitude > 0.05f && !agent.isStopped;

        if (currentState == EnemyState.Patrol && isMoving)
        {
            animationSpeed = 0.5f;
        }
        else if (currentState == EnemyState.Chase && isMoving)
        {
            animationSpeed = 1f;
        }
        
        animator.SetFloat(speedParameter, animationSpeed, animationDampTime, Time.deltaTime);
    }

    private void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            facePlayerSpeed * Time.deltaTime
        );
    }

    private void OnValidate()
    {
        chaseRange = Mathf.Max(0f, chaseRange);
        loseRange = Mathf.Max(chaseRange + 0.1f, loseRange);

        walkSpeed = Mathf.Max(0f, walkSpeed);
        runSpeed = Mathf.Max(walkSpeed, runSpeed);

        patrolStoppingDistance = Mathf.Max(0f, patrolStoppingDistance);
        chaseStoppingDistance = Mathf.Max(0f, chaseStoppingDistance);
        waypointReachDistance = Mathf.Max(0.05f, waypointReachDistance);
        waitTimeAtWaypoint = Mathf.Max(0f, waitTimeAtWaypoint);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRange);
    }
}