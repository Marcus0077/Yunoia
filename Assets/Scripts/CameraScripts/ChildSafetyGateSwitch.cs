using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSafetyGateSwitch : MonoBehaviour
{
    public GameObject[] childSafetyGates;

    private void Start()
    {
        foreach (var childSafetyGate in childSafetyGates)
        {
            childSafetyGate.SetActive(false);   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            foreach (var childSafetyGate in childSafetyGates)
            {
                childSafetyGate.SetActive(true);   
            }      
        }
    }
}
