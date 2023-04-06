using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.ProBuilder;

public class RenderFaceless : MonoBehaviour
{
    // Array of Faceless AI Game Objects that do not need
    // to be rendered yet.
    public GameObject[] Facelesses;
    
    public int thisRoom;
    
    private Animator cameraAnimator;
    
    // At the beginning of the game, disable the mesh renderer for 
    // all Faceless AI that are in future rooms.
    void Start()
    {
        if (GameObject.FindObjectOfType<CinemachineStateDrivenCamera>().GetComponent<Animator>() != null)
        {
            cameraAnimator = GameObject.FindObjectOfType<CinemachineStateDrivenCamera>().GetComponent<Animator>();
        }
        
        if (cameraAnimator.GetInteger("roomNum") != thisRoom)
        {
            foreach (var Faceless in Facelesses)
            {
                Faceless.GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
        }
    }

    // If we have reached a room that contains un-rendered Faceless AIs, 
    // turn their renderer back on so that we can see them.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var Faceless in Facelesses)
            {
                Faceless.GetComponent<SkinnedMeshRenderer>().enabled = true;
            }
        }
    }
}
