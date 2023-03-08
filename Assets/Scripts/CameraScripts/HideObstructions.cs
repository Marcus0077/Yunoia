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

    // Decides whether to render at limited opacity or fully dissappear object(s).
    public bool partiallyDissappear;

    // Decides whether this trigger makes objects appear or disappear.
    public bool reAppear;

    public HideObstructions Counterpart;

    // If the player or clone enters an area where there are things obstructing the camera,
    // set these objects to be invisible and only cast shadows. If they have a collider, 
    // turn the collider off.
    private void OnTriggerEnter(Collider other)
    {
        if (!partiallyDissappear && !reAppear)
        {
            if (other.CompareTag("Player"))
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
        else if (partiallyDissappear && !reAppear)
        {
            if (other.CompareTag("Player") || other.CompareTag("Clone"))
            {
                foreach (var obstruction in Obstructions)
                {
                    Color obstructionColor = obstruction.GetComponent<MeshRenderer>().material.color;
                    obstructionColor.a = 0.25f;

                    obstruction.GetComponent<MeshRenderer>().material.color = obstructionColor;
                }
            }
        }
        else if (!partiallyDissappear && reAppear)
        {
            if (other.CompareTag("Player"))
            {
                foreach (var obstruction in Obstructions)
                {
                    obstruction.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;

                    if (obstruction.GetComponent<MeshCollider>())
                    {
                        obstruction.GetComponent<MeshCollider>().enabled = true;
                    }
                }
            }
        }
        else if (partiallyDissappear && reAppear)
        {
            if (other.CompareTag("Player") || other.CompareTag("Clone"))
            {
                foreach (var obstruction in Obstructions)
                {
                    Color obstructionColor = obstruction.GetComponent<MeshRenderer>().material.color;
                    obstructionColor.a = 1f;

                    obstruction.GetComponent<MeshRenderer>().material.color = obstructionColor;
                }
            }
        }
    }
}
