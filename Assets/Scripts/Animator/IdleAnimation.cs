using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    bool irisIdle;

    // Use this for initialization
    void Start () 
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () 
    {
        if (irisIdle == true)
        {
            animator.SetBool("Idle", true);
        } 
    }
}
