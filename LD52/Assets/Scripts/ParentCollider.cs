using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ParentCollider
{
    void Initialize(ChildCollider targetChild);
    void OnTriggerEnter(Collider other);

    void OnCollisionEnter(Collision other);
    void Kill();
}
