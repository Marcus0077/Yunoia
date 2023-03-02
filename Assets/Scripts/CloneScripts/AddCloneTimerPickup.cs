using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddCloneTimerPickup : MonoBehaviour
{
    // Float that stores this pickup's time value.
    public float timeToAdd;
    
    // Initialize pickup's time value when it is initialised.
    void Awake()
    {
        timeToAdd = 5f;
    }

    // If the clone pickup this object, destroy this object and 
    // add 'timeToAdd' amount of time to the clone's timer.
    public void OnTriggerEnter(Collider other)
    {
        if (other.GameObject().CompareTag("Clone"))
        {
            Destroy(this.GameObject());
            other.GameObject().GetComponent<ExitClone>().cloneActiveTimer += timeToAdd;
        }
    }
}
