using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour, GameRhythmSubscriber
{
    [SerializeField]
    private GoblinAnimator goblinAnimator;
    [SerializeField]
    private Transform shootPos;
    [SerializeField]
    private EnemyMovement enemyMovement;
    private int notesBetweenShots = 8;

    private EntityHitReceiver playerHitReceiver;

    private void Start()
    {
        playerHitReceiver = PlayerMovement.main.GetComponent<EntityHitReceiver>();
        GameRhythm.main.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Shoot()
    {
        goblinAnimator.Shoot();
        ProjectileLauncher.main.Launch(playerHitReceiver.EnemyAimTarget.position, shootPos.position);
    }

    public void RhythmUpdate(int note)
    {
        if (note % notesBetweenShots == 0 && enemyMovement.IsInAttackRange)
        {
            Shoot();
        }
    }
}
