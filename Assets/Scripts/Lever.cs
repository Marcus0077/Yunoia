using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    public GameObject Counterpart;
    public GameObject Door;

    public bool isActivated;
    private bool isPlayer;
    public bool isClone;
    public bool Complete;

    private CloneInteractions cloneInteractions;
    private PlayerInteractions playerInteractions;

    public float leverTimer;

    public TextMeshProUGUI activateText;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        activateText.enabled = false;
        
        isActivated = false;
        isPlayer = false;
        isClone = false;
        Complete = false;

        leverTimer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            animator.SetBool("PullLever", true);

            if (!Counterpart.GetComponent<Lever>().isActivated && !Complete)
            {
                leverTimer -= Time.deltaTime;
            }

            if (Counterpart.GetComponent<Lever>().isActivated && leverTimer > 0)
            {
                Complete = true;
                Counterpart.GetComponent<Lever>().Complete = true;

                Door.GetComponent<Door>().Open();
                
                //Door.GetComponent<Collider>().enabled = false;
                //Door.GetComponent<Renderer>().enabled = false;
                
                activateText.enabled = false;
            }
            else if (leverTimer <= 0)
            {
                animator.SetBool("PullLever", false);
                
                Counterpart.GetComponent<Lever>().isActivated = false;
                isActivated = false;
                leverTimer = 1f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isClone)
        {
            activateText.enabled = true;
            
            playerInteractions = other.GetComponent<PlayerInteractions>();
            playerInteractions.canPress = true;

            isPlayer = true;
        }
        else if (other.CompareTag("Clone") && !isPlayer)
        {
            activateText.enabled = true;
            
            cloneInteractions = other.GetComponent<CloneInteractions>();
            cloneInteractions.canPress = true;

            isClone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayer)
        {
            Debug.Log("works");
            
            activateText.enabled = false;
            
            playerInteractions.canPress = false;
            
            isPlayer = false;
        }
        else if (other.CompareTag("Clone") && isClone)
        {
            activateText.enabled = false;
            
            cloneInteractions.canPress = false;
            
            isClone = false;
        }
    }
}
