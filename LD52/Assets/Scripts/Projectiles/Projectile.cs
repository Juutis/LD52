using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, ParentCollider
{



    [SerializeField]
    private ChildCollider childCollider;

    [SerializeField]
    private Rigidbody rb;
    private float speed = 15f;
    [SerializeField]
    private int damage = 2;
    public int Damage { get { return damage; } }
    public void Initialize(ChildCollider targetChild)
    {
        targetChild.Initialize(this);
    }
    public void Launch(Vector3 targetPosition)
    {
        Initialize(childCollider);
        Vector3 force = (targetPosition - transform.position).normalized * speed;
        rb.velocity = force;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Projectile]: TriggerEnter '{other.name}'");
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        transform.parent = other.transform;
        childCollider.Disable();
    }
    public void OnCollisionEnter(Collision other)
    {
        Debug.Log($"[Projectile]: CollisionEnter '{other.gameObject.name}'");
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        transform.parent = other.transform;
        childCollider.Disable();
    }

}
