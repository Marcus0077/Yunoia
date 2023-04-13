using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private GameManager gameManager;
    
    // Blocker that pressure plate moves.
    public GameObject Blocker;

    // Bool variables.
    public bool isPlayer;
    public bool isClone;
    private bool inCooldown;
    
    private AudioSource audioSource;
    public AudioClip ppSound;
    
    public float cooldownTime = 5f;

    public int puzzleNum;
    
    private Animator pPlateAnimator;
    public Animator doorAnimator;

    // Get references and initialize variables when pressure plates spawn.
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        pPlateAnimator = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();

        if (Blocker != null)
        {
            doorAnimator = Blocker.GetComponent<Animator>();
        }
        
        isPlayer = false;
        isClone = false;
    }

    // Set the cooldown flag to true, wait for the cooldown time to pass, then turn the flag to false.
    private IEnumerator Cooldown()
    {
        inCooldown = true;
     
        yield return new WaitForSeconds(cooldownTime);
     
        inCooldown = false;
    }

    // Determine whether player or clone is on this pressure plate and activate it.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isClone)
            {
                StartCoroutine(CompletePuzzle());
            }

            isPlayer = true;
            
            // if (!inCooldown)
            // {
            //     StartCoroutine(Cooldown());
            // }
        }
        else if (other.CompareTag("Clone"))
        {
            if (!isPlayer)
            {
                StartCoroutine(CompletePuzzle());
            }

            isClone = true;
        }
    }
    
    private IEnumerator CompletePuzzle()
    {
        float waitTime = 2.5f;
        
        pPlateAnimator.SetBool("plateDown", true);
        audioSource.PlayOneShot(ppSound);
        
        gameManager.ShowPuzzleWrapper(puzzleNum, waitTime);

        yield return new WaitForSeconds(waitTime);
        
        HideWall();
    }

    // Determine whether player or clone is exiting this pressure plate and deactivate it.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isClone)
            {
                AppearWall();
            }

            isPlayer = false;
        }
        else if (other.CompareTag("Clone"))
        {
            if (!isPlayer)
            {
                AppearWall();
            }

            isClone = false;
        }
    }

    // Removes blocker when pressure plate is activated.
    public void HideWall()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("DoorOpen", true);
        }
        else
        {
            Blocker.GetComponent<Renderer>().enabled = false;
            Blocker.GetComponent<Collider>().enabled = false;
        }
    }
    
    // Adds blocker back when pressure plate is deactivated.
    public void AppearWall()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("DoorOpen", false);
        }
        else
        {
            Blocker.GetComponent<Renderer>().enabled = true;
            Blocker.GetComponent<Collider>().enabled = true;
        }

        pPlateAnimator.SetBool("plateDown", false);
    }
}
