using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCamTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<BasicMovement>().canRotateCam = false;
            GameObject.FindWithTag("FreeCam").GetComponent<LimitedMovementCam>().enabled = true;
        }
    }
}
