using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteractions : MonoBehaviour
{
    // Temp Object References
    public GameObject Blocker1;

    private void Awake()
    {
        Blocker1 = GameObject.FindWithTag("Blocker1");
    }

    // Player Specific
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlockerTrigger1"))
        {
            Blocker1.GetComponent<MeshRenderer>().enabled = false;
            Blocker1.GetComponent<Collider>().enabled = false;
        }
    }

    // Player Specific
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BlockerTrigger1"))
        {
            Blocker1.GetComponent<MeshRenderer>().enabled = true;
            Blocker1.GetComponent<Collider>().enabled = true;
        }
    }
}
