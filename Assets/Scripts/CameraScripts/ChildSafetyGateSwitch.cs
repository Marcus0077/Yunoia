using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSafetyGateSwitch : MonoBehaviour
{
    // Array of invisible Game Objects that will prevent the 
    // player, or the clone, from walking out of camera bounds.
    public GameObject[] childSafetyGates;

    // At the beginning of the game, we can set the gates to be inactive, 
    // as we have not reached the area(s) where the player or clone need to be contained yet.
    private void Start()
    {
        foreach (var childSafetyGate in childSafetyGates)
        {
            childSafetyGate.SetActive(false);   
        }
    }

    // If we do reach the area(s) where the player or clone need to be contained, 
    // set the gates to active, thus activating the physical camera bounds.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var childSafetyGate in childSafetyGates)
            {
                childSafetyGate.SetActive(true);   
            }      
        }
    }
}
