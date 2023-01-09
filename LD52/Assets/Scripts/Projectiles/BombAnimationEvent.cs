using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BombAnimationEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent animationEvent;

    public void AnimationEvent()
    {
        animationEvent.Invoke();
    }
}
