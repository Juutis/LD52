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

    }
    public void OnCollisionEnter(Collision other)
    {
        Debug.Log($"[EntityHitReceiver]: CollisionEnter '{other.gameObject.name}'");
        if (other.gameObject.tag == "Projectile")
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();

            entityHealth.Modify(-projectile.Damage);
        }
    }
}
