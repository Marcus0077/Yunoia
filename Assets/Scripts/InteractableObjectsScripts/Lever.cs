using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    // Script references.
    private CloneInteractions cloneInteractions;
    private PlayerInteractions playerInteractions;
    private GameManager gameManager;
    
    // Game object references.
    public GameObject Counterpart;
    public GameObject Door;

    // Bool Variables.
    public bool isActivated;
    public bool isPlayer;
    public bool isClone;
    public bool Complete;

    // Lever countdown timer.
    private float leverTimer;

    // Lever UI references.
    public Image activateText;

    // Lever animation reference.
    public Animator animator;

    // Audio variables.
    public AudioSource audioSource;
    public AudioClip leverSound;
    private bool audioPlayed;

    // Lever-type toggles and array of AI GameObjects if 
    // AI lever is toggled.
    public bool isSingleLever;
    public bool isAiLever;
    public GameObject[] aiCounterparts;
    
    // Integer that determines which camera we will switch to on 
    // puzzle completion.
    public int puzzleNum;
    
    // Animation Variables
    public Animator cageAnimator;

    // Get references and initialize variables when levers spawn.
    void Start()
    {
        //activateText.enabled = false;
        
        gameManager = FindObjectOfType<GameManager>();

        isActivated = false;
        isPlayer = false;
        isClone = false;
        Complete = false;
        audioPlayed = false;

        leverTimer = 1.5f;
    }

    // Called each frame.
    void Update()
    {
        CheckForLeverActivation();
    }

    // If lever is activated, begin lever activation sequence.
    void CheckForLeverActivation()
    {
        if (isActivated)
        {
            PlayAudioAndAnimation();

            if (!isSingleLever)
            {
                SubtractFromLeverTimer();

                if (leverTimer > 0)
                {
                    if (Counterpart != null && Counterpart.GetComponent<Lever>().isActivated && !Complete)
                    {
                        StartCoroutine(CompletePuzzle());
                    }
                }
                else
                {
                    EndLeverSequence();
                }
            }
            else
            {
                if (!Complete)
                {
                    StartCoroutine(CompletePuzzle());
                }
            }
        }
    }

    // Plays lever activation sound and animation.
    void PlayAudioAndAnimation()
    {
        animator.SetBool("PullLever", true);

        if (!audioPlayed)
        {
            audioSource.PlayOneShot(leverSound);

            audioPlayed = true;
        }
    }

    // Subtracts from lever countdown timer as long as the lever sequence is
    // not complete and the countdown timer is greater than 1;
    void SubtractFromLeverTimer()
    {
        if ((Counterpart == null || !Counterpart.GetComponent<Lever>().isActivated) && !Complete && leverTimer > 0)
        {
            leverTimer -= Time.deltaTime;
        }
    }
    
    private IEnumerator CompletePuzzle()
    {
        Complete = true;

        if (!isSingleLever)
        {
            Counterpart.GetComponent<Lever>().Complete = true;
        }

        float waitTime = 2.5f;
        
        gameManager.ShowPuzzleWrapper(puzzleNum, waitTime);

        yield return new WaitForSeconds(waitTime);
        
        CompleteLeverSequence();
    }

    // Opens specified door if both levers have been activated before time runs out.
    void CompleteLeverSequence()
    {
        if (Counterpart != null)
        {
            //Counterpart.GetComponent<Lever>().activateText.enabled = false;
        }

        if (!isAiLever)
        {
            if (Door.CompareTag("Door"))
            {
                Door.GetComponent<Door>().Open();
            }
            else
            {
                Destroy(Door);
            }
        }
        else
        {
            if (aiCounterparts != null)
            {
                foreach (var aiCounterpart in aiCounterparts)
                {
                    aiCounterpart.GetComponent<AiMovement>().canAiMove = true;
                    cageAnimator.SetBool("openedCage", true);
                }
            }
        }

        //activateText.enabled = false;
    }

    // Resets lever sequence if both levers were not activated before time runs out.
    void EndLeverSequence()
    {
        animator.SetBool("PullLever", false);

        if(Counterpart != null)
            Counterpart.GetComponent<Lever>().isActivated = false;

        audioPlayed = false;
        isActivated = false;
        leverTimer = 1.5f;
    }

    // Checks whether clone or player is currently within the lever trigger,
    // and grants interaction to whichever one is in the trigger.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Complete)
        {
            //activateText.enabled = true;
            
            playerInteractions = other.GetComponent<PlayerInteractions>();
            playerInteractions.canPressLever = true;

            isPlayer = true;
        }
        else if (other.CompareTag("Clone") && !Complete)
        {
            //activateText.enabled = true;
            
            cloneInteractions = other.GetComponent<CloneInteractions>();
            cloneInteractions.canPressLever = true;

            isClone = true;
        }
    }

    // Checks whether clone or player is currently exiting the lever trigger,
    // and disables interaction for whichever one is exiting the trigger.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayer)
        {
            //activateText.enabled = false;
            
            playerInteractions.canPressLever = false;
            
            isPlayer = false;
        }
        else if (other.CompareTag("Clone") && isClone)
        {
            //activateText.enabled = false;
            
            cloneInteractions.canPressLever = false;
            
            isClone = false;
        }
    }
}
