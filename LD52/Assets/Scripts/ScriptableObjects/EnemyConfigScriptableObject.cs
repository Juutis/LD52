using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
public class EnemyConfigScriptableObject : ScriptableObject
{
    public float visionRange;
    public float attackRange;
    public float movementSpeed;
    public float turnRate;
    public float moveAngle;
    public float aggroTimeout;
}
