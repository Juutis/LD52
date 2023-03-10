using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHitReceiver : MonoBehaviour, ParentCollider
{
    [SerializeField]
    private ChildCollider childCollider;
    [SerializeField]
    private EntityHealth entityHealth;

    [SerializeField]
    private Transform enemyAimTarget;
    public Transform EnemyAimTarget { get { return enemyAimTarget; } }

    public void Kill()
    {
        //Destroy(gameObject);
    }

    public void Initialize(ChildCollider targetChild)
    {
        targetChild.Initialize(this);
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[EntityHitReceiver]: TriggerEnter '{other.name}'");
        if (other.gameObject.tag == "Unlock")
        {
            UnlockPickup pickup = other.gameObject.GetComponentInParent<UnlockPickup>();
            CastableSpellManager.main.UnlockSpell(pickup.Spell);
            pickup.Kill();
        }
        if (other.gameObject.tag == "Checkpoint")
        {
            Checkpoint checkpoint = other.gameObject.GetComponent<Checkpoint>();
            checkpoint.Disable();
            CheckpointManager.main.SetCheckpoint(checkpoint);
            Debug.Log($"New checkpoint: {checkpoint.SpawnPoint}");
        }
    }
    public void OnCollisionEnter(Collision other)
    {
        Debug.Log($"[EntityHitReceiver]: CollisionEnter '{other.gameObject.name}'");
        if (other.gameObject.tag == "Projectile")
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();

            entityHealth.Modify(-projectile.Damage);
            UIManager.main.ShowPoppingText(transform.position, $"-{projectile.Damage}", Color.red);
            SoundManager.main.PlaySound(GameSoundType.ArrowHitZarguuf);
        }
    }
}
