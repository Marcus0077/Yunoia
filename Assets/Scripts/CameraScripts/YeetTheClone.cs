using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YeetTheClone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Clone"))
        {
            other.GetComponent<ExitClone>().despawnClone = true;
        }
    }
}
