using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    [SerializeField]
    private float explosionRadius;

    [SerializeField]
    private ParticleSystem particles;

    private Vector3 target = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector.Substract(transform.position, target).magnitude < 0.5f)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up * 3f, explosionRadius, Vector3.down, 4f, LayerMask.GetMask("Enemy"));
            Debug.Log($"Raycast hit {hits.Length}");
            foreach(RaycastHit hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out EnemyMovement enemy))
                {
                    enemy.SetStunned();
                }
            }

            particles.transform.parent = null;
            particles.Play();
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }
}
