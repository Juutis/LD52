using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour
{

    private Collider ownCollider;
    ParentCollider parentCollider;
    public ParentCollider Parent { get { return parentCollider; } }
    public void Initialize(ParentCollider parent)
    {
        parentCollider = parent;
        ownCollider = GetComponent<Collider>();
    }

    public void Disable()
    {
        ownCollider.enabled = false;
    }
    public void Kill()
    {
        if (parentCollider == null)
        {
            return;
        }
        parentCollider.Kill();
    }

    void OnTriggerEnter(Collider other)
    {
        if (parentCollider == null)
        {
            return;
        }
        parentCollider.OnTriggerEnter(other);
    }


    void OnCollisionEnter(Collision other)
    {
        if (parentCollider == null)
        {
            return;
        }
        parentCollider.OnCollisionEnter(other);
    }

}

