using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private EntityHitReceiver playerHitReceiver;

    [SerializeField]
    private EnemyConfigScriptableObject config;

    private NavMeshAgent agent;
    private float aggroTimeoutStarted;

    private float raycastWidth = 1f;
    private float attackDistanceCoef = 0.8f;
    private bool isInAttackRange = false;
    public bool IsInAttackRange { get { return isInAttackRange; } }
    public Vector3 PlayerPosition { get { return playerHitReceiver.transform.position; } }
    private GoblinAnimator goblinAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerHitReceiver = player.GetComponentInChildren<EntityHitReceiver>();
        aggroTimeoutStarted = -config.AggroTimeout;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = config.MovementSpeed;
        agent.angularSpeed = config.TurnRate;
        agent.acceleration = 100.0f;
        goblinAnimator = GetComponentInChildren<GoblinAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.main.Paused)
        {
            return;
        }
        isInAttackRange = false;
        float playerDistance = EnemyToPlayer2D().magnitude;

        // if player is within vision range or enemy is still aggroed
        if (playerDistance <= config.VisionRange || Time.time - aggroTimeoutStarted < config.AggroTimeout)
        {
            bool obstacleHitRight = Physics.Raycast(transform.position, GetPlayerRaycastDir(transform.right), playerDistance, LayerMask.GetMask("Obstacles"));
            bool obstacleHitLeft = Physics.Raycast(transform.position, GetPlayerRaycastDir(-transform.right), playerDistance, LayerMask.GetMask("Obstacles"));

            // if vision wasn't obstructed by obstacles
            if (!obstacleHitRight || !obstacleHitLeft)
            {
                // rotate towards player
                var targetRotation = Quaternion.LookRotation(EnemyToPlayerDir(), transform.up);
                var deltaAngle = Quaternion.Angle(transform.localRotation, targetRotation);
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, Mathf.Min(deltaAngle, config.TurnRate * Time.deltaTime));

                float angle = Vector3.Angle(EnemyToPlayerDir(), transform.forward);

                if (playerDistance <= config.AttackRange)
                {
                    agent.isStopped = true;
                    isInAttackRange = true;
                    goblinAnimator.SetWalking(false);
                }
                else if (angle < config.MoveAngle && playerDistance > config.AttackRange && isReadyToMove())
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                    isInAttackRange = false;
                    goblinAnimator.SetWalking(true);
                }

                if (playerDistance < config.VisionRange)
                {
                    aggroTimeoutStarted = Time.time;
                }
            }
        }
    }

    private void FixedUpdate()
    {
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
