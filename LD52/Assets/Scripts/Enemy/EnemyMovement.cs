using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    private EntityHitReceiver playerHitReceiver;

    [SerializeField]
    private EnemyConfigScriptableObject config;
    [SerializeField]
    private ParticleSystem stunParticles;
    [SerializeField]
    private List<Transform> patrolPoints;
    [SerializeField]
    private bool patrolPingPong;
    [SerializeField]
    private bool startPatrol;
    private Transform? currentPatrolTarget;
    private int patrolDirection = 1;

    private NavMeshAgent agent;
    private float playerLastSeen;

    private float raycastWidth = 1f;
    private float attackDistanceCoef = 0.8f;
    private bool isInAttackRange = false;
    public bool IsInAttackRange { get { return isInAttackRange; } }
    public Vector3 PlayerPosition { get { return playerHitReceiver.transform.position; } }
    private GoblinAnimator goblinAnimator;

    private int obstaclesMask;

    public Vector3 LastPlayerPosition { get; private set; }
    private EnemyState state = EnemyState.IDLE;

    private float searchTimer;
    private float searchWaitTimer;
    private EnemyState startingState;
    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private float stunnedStarted = -1;
    private float stunDuration = 7.5f;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.main.transform;
        playerHitReceiver = player.GetComponentInChildren<EntityHitReceiver>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = config.MovementSpeed;
        agent.angularSpeed = config.TurnRate;
        agent.acceleration = 100.0f;
        goblinAnimator = GetComponentInChildren<GoblinAnimator>();
        obstaclesMask = LayerMask.GetMask("Obstacles") | LayerMask.GetMask("NonBlinkPassableDynamicObstacle");
        startingState = startPatrol ? EnemyState.PATROL : state;
        state = startingState;
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.main.Paused)
        {
            return;
        }

        switch (state)
        {
            case EnemyState.IDLE:
                handleIdle();
                break;
            case EnemyState.PATROL:
                handlePatrol();
                break;
            case EnemyState.ATTACK:
                handleAttack();
                break;
            case EnemyState.CHASE:
                handleChase();
                break;
            case EnemyState.SEARCH:
                handleSearch();
                break;
            case EnemyState.STUNNED:
                handleStunned();
                break;
        }
    }

    public void SetStunned()
    {
        goblinAnimator.SetWalking(false);
        isInAttackRange = false;
        agent.isStopped = true;
        state = EnemyState.STUNNED;
        stunnedStarted = Time.time;
        stunParticles.gameObject.SetActive(true);
        stunParticles.Play();
    }

    private void handleIdle()
    {
        isInAttackRange = false;
        if (canSeePlayer())
        {
            state = EnemyState.ATTACK;
        }

        if (Vector3.Distance(transform.position, startingPosition) > 0.1f)
        {
            agent.SetDestination(startingPosition);
            agent.isStopped = false;
            goblinAnimator.SetWalking(true);
        }
        else
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            goblinAnimator.SetWalking(false);

            var deltaAngle = Quaternion.Angle(transform.rotation, startingRotation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startingRotation, Mathf.Min(deltaAngle, config.TurnRate * Time.deltaTime));
        }
    }

    private void handleStunned()
    {
        goblinAnimator.Stunned(true);
        if (Time.time - stunnedStarted > stunDuration)
        {
            state = EnemyState.SEARCH;
            searchTimer = Time.time;
            stunParticles.Stop();
            stunParticles.gameObject.SetActive(false);
            goblinAnimator.Stunned(false);
        }
    }

    int patrolIndex = 0;

    private void handlePatrol()
    {
        isInAttackRange = false;
        if (canSeePlayer())
        {
            state = EnemyState.ATTACK;
            return;
        }


        if (patrolPoints == null || patrolPoints.Count <= 1)
        {
            state = EnemyState.IDLE;
            return;
        }

        if (currentPatrolTarget == null)
        {
            currentPatrolTarget = patrolPoints.First();
        }

        if (Vector3.Distance(transform.position, currentPatrolTarget.position) > 0.1f)
        {
            agent.SetDestination(currentPatrolTarget.position);
            agent.isStopped = false;
            goblinAnimator.SetWalking(true);
        }
        else
        {
            if (patrolPingPong)
            {
                if (patrolDirection == 1 && patrolIndex == patrolPoints.Count - 1)
                {
                    patrolDirection = -1;
                }
                else if (patrolDirection == -1 && patrolIndex == 0)
                {
                    patrolDirection = 1;
                }

                patrolIndex += patrolDirection;
                currentPatrolTarget = patrolPoints[patrolIndex];
            }
            else
            {
                patrolIndex++;

                if (patrolIndex == patrolPoints.Count)
                {
                    patrolIndex = 0;
                }

                currentPatrolTarget = patrolPoints[patrolIndex];
            }
        }
    }

    private void handleAttack()
    {
        isInAttackRange = false;

        if (canSeePlayer())
        {
            float playerDistance = EnemyToPlayer2D().magnitude;

            // rotate towards player
            var targetRotation = Quaternion.LookRotation(EnemyToPlayerDir(), transform.up);
            var deltaAngle = Quaternion.Angle(transform.rotation, targetRotation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Mathf.Min(deltaAngle, config.TurnRate * Time.deltaTime));

            float angle = Vector3.Angle(EnemyToPlayerDir(), transform.forward);

            if (playerDistance <= config.AttackRange)
            {
                isInAttackRange = true;
            }
            else
            {
                isInAttackRange = false;
            }

            if (isReadyToMove() && angle < config.MoveAngle && playerDistance > config.MinAttackRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                goblinAnimator.SetWalking(true);
            }
            else
            {
                agent.isStopped = true;
                goblinAnimator.SetWalking(false);
            }

            playerLastSeen = Time.time;
            LastPlayerPosition = player.position;
        }
        else
        {
            if (state != EnemyState.CHASE)
            {
                SoundManager.main.PlaySound(GameSoundType.GobboAnnoyed);
            }
            state = EnemyState.CHASE;
        }
    }

    private void handleChase()
    {
        isInAttackRange = false;
        if (canSeePlayer())
        {
            state = EnemyState.ATTACK;
        }

        var targetDir = agent.velocity;
        if (targetDir.magnitude > 0.01f)
        {
            var targetRotation = Quaternion.LookRotation(targetDir.normalized, transform.up);
            var deltaAngle = Quaternion.Angle(transform.rotation, targetRotation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Mathf.Min(deltaAngle, config.TurnRate * Time.deltaTime));
        }

        if (isReadyToMove())
        {
            agent.isStopped = false;
            agent.SetDestination(LastPlayerPosition);
            goblinAnimator.SetWalking(true);
        }
        else
        {
            agent.isStopped = true;
            goblinAnimator.SetWalking(false);
        }

        if (Vector3.Distance(transform.position, LastPlayerPosition) < 0.25f)
        {
            state = EnemyState.SEARCH;
            searchTimer = Time.time;
            agent.isStopped = true;
            agent.SetDestination(transform.position);
            goblinAnimator.SetWalking(false);
        }
    }

    private void handleSearch()
    {
        isInAttackRange = false;
        if (canSeePlayer())
        {
            state = EnemyState.ATTACK;
        }

        if (searchWaitTimer < Time.time)
        {
            var dir = Random.Range(0, 360f);
            var dist = Random.Range(config.SearchRange / 2.0f, config.SearchRange);
            var rotation = Quaternion.AngleAxis(dir, Vector3.up);
            var targetPosition = transform.position + rotation * transform.forward * dist;
            agent.SetDestination(targetPosition);

            searchWaitTimer = Time.time + Random.Range(config.SearchMinWait, config.SearchMaxWait);
        }

        if (Vector3.Distance(agent.destination, transform.position) < 0.1f)
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
            goblinAnimator.SetWalking(false);
        }
        else
        {
            agent.isStopped = false;
            goblinAnimator.SetWalking(true);
        }

        if (Time.time - searchTimer > config.SearchDuration)
        {
            state = startingState;
        }
    }

    private void FixedUpdate()
    {
    }

    private bool canSeePlayer()
    {
        float playerDistance = EnemyToPlayer2D().magnitude;
        if (playerDistance > config.VisionRange)
        {
            return false;
        }

        var playerDirection = GetPlayerRaycastDir(Vector3.zero);
        var playerDirDiff = Vector3.Angle(playerDirection, transform.forward);
        if (playerDistance > config.HearingRange && playerDirDiff > config.ConeOfVision / 2.0f)
        {
            return false;
        }

        float playerRadius = 0.25f;
        var playerDirPerpendicular = Vector3.Cross(playerDirection, Vector3.up).normalized;

        bool obstacleHitStraight = Physics.Raycast(transform.position, playerDirection, playerDistance, obstaclesMask);
        bool obstacleHitRight = Physics.Raycast(transform.position, GetPlayerRaycastDir(playerDirPerpendicular * playerRadius), playerDistance, obstaclesMask);
        bool obstacleHitLeft = Physics.Raycast(transform.position, GetPlayerRaycastDir(-playerDirPerpendicular * playerRadius), playerDistance, obstaclesMask);

        if (obstacleHitStraight && obstacleHitRight && obstacleHitLeft)
        {
            return false;
        }

        return true;
    }

    private bool isReadyToMove()
    {
        return goblinAnimator.ReadyToMove;
    }

    private Vector3 GetPlayerRaycastDir(Vector3 offset)
    {
        var playerPos2 = new Vector2(player.position.x, player.position.z);
        var ownPos2 = new Vector2(transform.position.x, transform.position.z);
        var offset2 = new Vector2(offset.x, offset.z) * raycastWidth;
        var result2 = playerPos2 + offset2 - ownPos2;

        return new Vector3(result2.x, 0, result2.y);
    }

    private Vector2 EnemyToPlayer2D()
    {
        var playerPos2 = new Vector2(player.position.x, player.position.z);
        var ownPos2 = new Vector2(transform.position.x, transform.position.z);
        return playerPos2 - ownPos2;
    }

    private Vector3 EnemyToPlayerDir()
    {
        var v2Dir = EnemyToPlayer2D().normalized;
        return new(v2Dir.x, 0, v2Dir.y);
    }
}

enum EnemyState
{
    IDLE,
    PATROL,
    ATTACK,
    CHASE,
    SEARCH,
    STUNNED
}
