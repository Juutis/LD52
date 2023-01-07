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

    // Start is called before the first frame update
    void Start()
    {
        aggroTimeoutStarted = -config.aggroTimeout;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = config.movementSpeed;
        agent.angularSpeed = config.turnRate;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        float playerDistance = EnemyToPlayer2D().magnitude;
        if (playerDistance < config.visionRange || Time.time - aggroTimeoutStarted < config.aggroTimeout)
        {
            var targetRotation = Quaternion.LookRotation(EnemyToPlayerDir(), transform.up);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, config.turnRate);

            float angle = Vector3.Angle(EnemyToPlayerDir(), transform.forward);
            if (angle < config.moveAngle && playerDistance > 0.9f * config.attackRange)
            {
                agent.SetDestination(player.position);
            }

            if (playerDistance < config.visionRange)
            {
                aggroTimeoutStarted = Time.time;
            }
        }
    }

    private Vector2 EnemyToPlayer2D()
    {
        return new Vector2(player.position.x, player.position.z) - new Vector2(transform.position.x, transform.position.z);
    }

    private Vector3 EnemyToPlayerDir()
    {
        var v2Dir = EnemyToPlayer2D().normalized;
        return new(v2Dir.x, 0, v2Dir.y); 
    }
}
