using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HideThis : MonoBehaviour
{
    private Animator cameraAnimator;
    
    public int numToCompare;

    public float waitBeforeHiding;
    public float waitBeforeAppearing;
    
    private bool isRunning = false;

    private void Start()
    {
        if (GameObject.FindObjectOfType<CinemachineStateDrivenCamera>().GetComponent<Animator>() != null)
        {
            cameraAnimator = GameObject.FindObjectOfType<CinemachineStateDrivenCamera>().GetComponent<Animator>();
        }
    }
    
    void Update()
    {
        if (cameraAnimator.GetInteger("roomNum") == numToCompare && !isRunning)
        {
            StartCoroutine(HideObject());
        }
    }
    
    private IEnumerator HideObject()
    {
        isRunning = true;
        
        yield return new WaitForSeconds(waitBeforeHiding);
        
        this.GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(waitBeforeAppearing);
        
        this.GetComponent<MeshRenderer>().enabled = true;

        isRunning = false;
    }
}


