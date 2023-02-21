using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class HideObstructions : MonoBehaviour
{
    public Transform[] Obstructions;

    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            foreach (var obstruction in Obstructions)
            {
                obstruction.GetComponent<MeshRenderer>().enabled = !obstruction.GetComponent<MeshRenderer>().enabled;
            }
        }
    }
}
