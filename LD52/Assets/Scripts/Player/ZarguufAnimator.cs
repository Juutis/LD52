using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZarguufAnimator : MonoBehaviour
{
    public static ZarguufAnimator main;

    public void Awake()
    {
        main = this;
    }

    private Animator anim;

    [SerializeField]
    private bool debugCast = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (debugCast)
        {
            Cast();
            debugCast = false;
        }
    }

    public void Cast()
    {
        anim.SetBool("Cast", true);
    }

    public void CastCallback()
    {
        anim.SetBool("Cast", false);
    }
}
