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
    private int notesBetweenShots = 4;

    private EntityHitReceiver playerHitReceiver;

    private float attackPoint = 0.7f;
    private float attackDuration = 3.0f;
    private bool readyToAttack = true;

    private void Start()
    {
        playerHitReceiver = PlayerMovement.main.GetComponent<EntityHitReceiver>();
        GameRhythm.main.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartShootAnimation() 
    {
        goblinAnimator.Shoot();
    }

    public void Shoot()
    {
        ProjectileLauncher.main.Launch(playerHitReceiver.EnemyAimTarget.position, shootPos.position);
    }

    public void ResetAttack()
    {
        readyToAttack = true;
    }

    public void RhythmUpdate(int note)
    {
        if (!readyToAttack)
        {
            return;
        }
        int attackPointInNotes = Mathf.CeilToInt(attackPoint / GameRhythm.main.NoteLength);
        if (note % notesBetweenShots == notesBetweenShots - attackPointInNotes && enemyMovement.IsInAttackRange)
        {
            float animationDelay = attackPoint - (attackPointInNotes * GameRhythm.main.NoteLength);
            Invoke("StartShootAnimation", animationDelay);
            Invoke("Shoot", animationDelay + attackPoint);
            Invoke("ResetAttack", attackDuration);
            readyToAttack = false;
        }
    }
}
