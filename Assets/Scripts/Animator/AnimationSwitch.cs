using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSwitch : MonoBehaviour
{
    [SerializeField] Animator animator;
    
    public bool GetIdle()
    {
        return animator.Idle;
    }

    public void SetIdle(bool input)
    {
        animator.Idle = input;
    }

    public bool GetRun()
    {
        return animator.Run;
    }

    public void SetRun(bool input)
    {
        animator.Run = input;
    }

    public bool GetLeap()
    {
        return animator.Leap;
    }

    public bool SetLeap(bool input)
    {
        animator.Leap = input;
    }
}
