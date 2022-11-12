using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject Counterpart;
    public GameObject Door;

    public bool isActivated;
    private bool isPlayer;
    private bool isClone;

    private CloneInteractions cloneInteractions;
    private PlayerInteractions playerInteractions;

    public float leverTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        isActivated = false;
        isPlayer = false;
        isClone = false;

        leverTimer = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            Debug.Log(leverTimer);

            if (!Counterpart.GetComponent<Lever>().isActivated)
            {
                leverTimer -= Time.deltaTime;
            }

            if (Counterpart.GetComponent<Lever>().isActivated && leverTimer > 0)
            {
                Door.GetComponent<Collider>().enabled = false;
                Door.GetComponent<Renderer>().enabled = false;
            }
            else if (leverTimer <= 0)
            {
                Counterpart.GetComponent<Lever>().isActivated = false;
                isActivated = false;
                leverTimer = 2f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInteractions = other.GetComponent<PlayerInteractions>();
            playerInteractions.canPress = true;

            isPlayer = true;
        }
        else if (other.CompareTag("Clone"))
        {
            cloneInteractions = other.GetComponent<CloneInteractions>();
            cloneInteractions.canPress = true;

            isClone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInteractions.canPress = false;
            
            isPlayer = false;
        }
        else if (other.CompareTag("Clone"))
        {
            cloneInteractions.canPress = false;
            
            isPlayer = false;
        }
    }
}
