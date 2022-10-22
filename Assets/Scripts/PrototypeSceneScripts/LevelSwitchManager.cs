using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LevelSwitchManager : MonoBehaviour
{

    public Rigidbody Player;

    public List<Transform> LevelStarts;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MutedGrappleEnd"))
        {
            this.GetComponent<Grapple>().enabled = false;
            this.GetComponent<AbilityPush>().enabled = true;
            
            Player.transform.position = LevelStarts[0].position;
        }
        else if (other.CompareTag("MutedPushEnd"))
        {
            this.GetComponent<Grapple>().enabled = true;
            this.GetComponent<AbilityPush>().enabled = true;
            this.GetComponent<SummonClone>().enabled = true;
            
            Player.transform.position = LevelStarts[1].position;
        }
        if (other.CompareTag("MutedCloneEnd"))
        {
            Player.transform.position = LevelStarts[2].position;
        }
    }
}
