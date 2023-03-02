using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class HideObstructions : MonoBehaviour
{
    // Array of Transforms that are obstructing the view of the camera.
    public Transform[] Obstructions;

    // If the player or clone enters an area where there are things obstructing the camera,
    // set these objects to be invisible and only cast shadows. If they have a collider, 
    // turn the collider off.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            foreach (var obstruction in Obstructions)
            {
                obstruction.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;

                if (obstruction.GetComponent<MeshCollider>())
                {
                    obstruction.GetComponent<MeshCollider>().enabled = false;
                }
            }
        }
    }
}
