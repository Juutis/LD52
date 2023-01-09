using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField]
    private bool debugOpen;

    [SerializeField]
    private BoxCollider gateCollider;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        gateCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (debugOpen)
        {
            Open();
        }
    }

    public void Open()
    {
        gateCollider.enabled = false;
        anim.SetTrigger("Open");
    }
}
