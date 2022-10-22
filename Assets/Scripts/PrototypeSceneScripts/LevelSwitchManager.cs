using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LevelSwitchManager : MonoBehaviour
{

    public Rigidbody Player;

    public List<Transform> LevelStarts;

    public AbilityPush playerAbilityPush;

    public bool pushRestored;

    private void Start()
    {
        this.GetComponent<SummonClone>().enabled = false;
        this.GetComponent<AbilityPush>().enabled = false;

        playerAbilityPush.restored = false;
        pushRestored = false;
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
            this.GetComponent<SummonClone>().enabled = true;
            
            Player.transform.position = LevelStarts[1].position;
        }
        else if (other.CompareTag("MutedCloneEnd"))
        {
            this.GetComponent<Grapple>().enabled = false;
            this.GetComponent<SummonClone>().enabled = false;
            
            playerAbilityPush.restored = true;
            pushRestored = true;
            
            Player.transform.position = LevelStarts[2].position;
        }
        else if (other.CompareTag("RestoredPushEnd"))
        {
            this.GetComponent<Grapple>().enabled = true;
            this.GetComponent<SummonClone>().enabled = true;
            
            Player.transform.position = LevelStarts[3].position;
        }
    }
}
