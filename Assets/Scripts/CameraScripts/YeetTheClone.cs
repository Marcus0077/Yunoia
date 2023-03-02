using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YeetTheClone : MonoBehaviour
{
    // Literally just despawns the clone if the clone walks through this trigger.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Clone"))
        {
            other.GetComponent<ExitClone>().despawnClone = true;
        }
    }
}
