using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebug : MonoBehaviour
{
    private Transform player;

    [SerializeField]
    private LineRenderer visionRange;

    [SerializeField]
    private LineRenderer shootRange;

    [SerializeField]
    private LineRenderer outOfRange;

    [SerializeField]
    private EnemyConfigScriptableObject config;


    // Start is called before the first frame update
    void Start()
    {
        player = PlayerMovement.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        var dir = EnemyToPlayerDir();
        float visionWithoutAttack = config.VisionRange - config.AttackRange;

        shootRange.SetPosition(0, transform.position);
        shootRange.SetPosition(1, transform.position + dir * config.AttackRange);
        visionRange.SetPosition(0, transform.position + dir * config.AttackRange);
        visionRange.SetPosition(1, transform.position + dir * config.AttackRange + dir * visionWithoutAttack);
        outOfRange.SetPosition(0, transform.position + dir * config.AttackRange + dir * visionWithoutAttack);
        outOfRange.SetPosition(1, player.position);
    }

    private Vector3 EnemyToPlayerDir()
    {
        var v2Dir = (new Vector2(player.position.x, player.position.z) - new Vector2(transform.position.x, transform.position.z)).normalized;
        return new (v2Dir.x, 0, v2Dir.y);
    }
}
