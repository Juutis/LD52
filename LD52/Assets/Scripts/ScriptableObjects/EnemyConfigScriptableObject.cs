using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
public class EnemyConfigScriptableObject : ScriptableObject
{
    public float VisionRange;
    public float AttackRange;
    public float MovementSpeed;
    public float TurnRate;
    public float MoveAngle;
    public float AggroTimeout;
    public float RateOfFire;
}
