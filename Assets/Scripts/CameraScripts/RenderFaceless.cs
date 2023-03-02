using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class RenderFaceless : MonoBehaviour
{
    // Array of Faceless AI Game Objects that do not need
    // to be rendered yet.
    public GameObject[] Facelesses;
    
    // At the beginning of the game, disable the mesh renderer for 
    // all Faceless AI that are in future rooms.
    void Start()
    {
        foreach (var Faceless in Facelesses)
        {
            Faceless.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    // If we have reached a room that contains un-rendered Faceless AIs, 
    // turn their renderer back on so that we can see them.
    private void OnTriggerEnter(Collider other)
    {
        foreach (var Faceless in Facelesses)
        {
            Faceless.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
