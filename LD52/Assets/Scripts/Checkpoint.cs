using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPosition;
    public Vector3 SpawnPoint { get { return spawnPosition.position; } }

    [SerializeField]
    private Collider checkpointCollider;

    public void Disable()
    {
        checkpointCollider.enabled = false;
    }

}
