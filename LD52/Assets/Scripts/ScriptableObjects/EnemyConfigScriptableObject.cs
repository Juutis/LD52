using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
public class EnemyConfigScriptableObject : ScriptableObject
{
    public float VisionRange;
    public float ConeOfVision;
    public float HearingRange;
    public float AttackRange;
    public float MinAttackRange;
    public float MovementSpeed;
    public float TurnRate;
    public float MoveAngle;
    public float SearchDuration;
    public float SearchMaxWait;
    public float SearchMinWait;
    public float SearchRange;
}
