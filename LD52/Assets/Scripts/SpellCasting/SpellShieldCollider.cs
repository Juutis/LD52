using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShieldCollider : MonoBehaviour, ParentCollider
{
    [SerializeField]
    private ChildCollider childCollider;

    void Start()
    {
        Initialize(childCollider);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(ChildCollider child)
    {
        child.Initialize(this);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[SpellShieldCollider]: Trigger detected -> {other.name}");
        if (other.tag == "Projectile")
        {
            other.GetComponent<ChildCollider>().Kill();
            SoundManager.main.PlaySound(GameSoundType.ArrowHitShield);
            UIManager.main.ShowPoppingText(transform.position, $"*blocked*", Color.yellow, 6);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log($"[SpellShieldCollider]: CollisionEnter detected -> {other.gameObject.name}");
    }

    public void Kill()
    {
        // no kill
    }
}
