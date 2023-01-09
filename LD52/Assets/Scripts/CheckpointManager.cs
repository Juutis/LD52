using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private Checkpoint mostRecentCheckpoint;
    public Checkpoint MostRecentCheckpoint { get { return mostRecentCheckpoint; } }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        mostRecentCheckpoint = checkpoint;
    }
}
