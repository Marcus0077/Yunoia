using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Blocker that pressure plate moves.
    public GameObject Blocker;

    // Bool variables.
    private bool isPlayer;
    public bool isClone;
    public AudioSource audioSource;
    public AudioClip ppSound;
    public float cooldownTime = 5f;
    private bool inCooldown;
    

    // Get references and initialize variables when pressure plates spawn.
    private void Awake()
    {
        isPlayer = false;
        isClone = false;
    }

    private IEnumerator Cooldown()
    {
     //Set the cooldown flag to true, wait for the cooldown time to pass, then turn the flag to false
     inCooldown = true;
     yield return new WaitForSeconds(cooldownTime);
     inCooldown = false;
    }

    // Determine whether player or clone is on this pressure plate and activate it.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isClone)
        {
            HideWall();
            audioSource.PlayOneShot(ppSound);
            
            isPlayer = true;
             if(!inCooldown)
         StartCoroutine(Cooldown());
    
        }
        else if (other.CompareTag("Clone") && !isPlayer)
        {
            HideWall();
            audioSource.PlayOneShot(ppSound);

            isClone = true;
        }

        
    }

    // Determine whether player or clone is exiting this pressure plate and deactivate it.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isClone)
        {
            AppearWall();

            isPlayer = false;
        }
        else if (other.CompareTag("Clone") && !isPlayer)
        {
            AppearWall();

            isClone = false;
        }
    }

    public void CloneOnPlate()
    {
        HideWall();

        isClone = true;
    }

    // Removes blocker when pressure plate is activated.
    public void HideWall()
    {
        Blocker.GetComponent<Renderer>().enabled = false;
        Blocker.GetComponent<Collider>().enabled = false;
    }
    
    // Adds blocker back when pressure plate is deactivated.
    public void AppearWall()
    {
        Blocker.GetComponent<Renderer>().enabled = true;
        Blocker.GetComponent<Collider>().enabled = true;
    }
}
