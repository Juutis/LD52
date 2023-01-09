using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePullPlum : MonoBehaviour
{
    private static float unscaledAnimationDuration = 2.042f;
    private static float steps = 5;
    private static float durationPerStep = 1.0f;
    private static float animationScale = unscaledAnimationDuration / (steps * durationPerStep);

    private bool pulling = false;

    private Animator anim;
    private int curStep = 0;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (pulling)
        {

        }
    }

    public void PullAction()
    {
        if (!pulling)
        {
            if (curStep < steps)
            {
                pulling = true;
                Invoke("StopAnimation", durationPerStep);
                anim.speed = animationScale;
                curStep++;
            }
            else
            {
                Debug.Log("WIN!");
                GameManager.main.NextLevelScreen();
            }
        }
    }

    public void StopAnimation()
    {
        anim.speed = 0;
        pulling = false;
    }
}
