using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffect : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    void Start()
    {
    }

    public void Play()
    {
        animator.SetTrigger("Play");
    }

    public void Finished()
    {
    }
}
