using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private EnemyConfigScriptableObject config;

    private NavMeshAgent agent;
    private float aggroTimeoutStarted;

    private float raycastWidth = 1f;
    private float attackDistanceCoef = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        aggroTimeoutStarted = -config.AggroTimeout;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = config.MovementSpeed;
        agent.angularSpeed = config.TurnRate;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
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
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, config.TurnRate);

                float angle = Vector3.Angle(EnemyToPlayerDir(), transform.forward);
                if (angle < config.MoveAngle && playerDistance > attackDistanceCoef * config.AttackRange)
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                }
                else if (playerDistance <= attackDistanceCoef * config.AttackRange)
                {
                    agent.isStopped = true;
                }

                if (playerDistance < config.VisionRange)
                {
                    aggroTimeoutStarted = Time.time;
                }
            }
        }
    }

    private Vector3 GetPlayerRaycastDir(Vector3 offset)
    {
        var playerPos2 = new Vector2(player.position.x, player.position.z);
        var ownPos2 = new Vector2(transform.position.x, transform.position.z);
        var offset2 = new Vector2(offset.x, offset.z) * raycastWidth;
        var result2 = playerPos2 + offset2 - ownPos2;

        return new Vector3(result2.x, 0.5f, result2.y);
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
        return new(v2Dir.x, 0.5f, v2Dir.y);
    }
}
