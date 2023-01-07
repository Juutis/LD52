using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour, GameRhythmSubscriber
{
    [SerializeField]
    private GoblinAnimator goblinAnimator;
    [SerializeField]
    private EnemyMovement enemyMovement;
    private int notesBetweenShots = 4;

    private void Start() {
        GameRhythm.main.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Shoot()
    {
        goblinAnimator.Shoot();
        ProjectileLauncher.main.Launch(transform.position, enemyMovement.PlayerPosition);
    }

    public void RhythmUpdate(int note)
    {
        if (note % notesBetweenShots == 0 && enemyMovement.IsInAttackRange)
        {
            Shoot();
        }
    }
}
