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

    // Get references and initialize variables when pressure plates spawn.
    private void Awake()
    {
        isPlayer = false;
        isClone = false;
    }

    // Determine whether player or clone is on this pressure plate and activate it.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isClone)
        {
            HideWall();

            isPlayer = true;
        }
        else if (other.CompareTag("Clone") && !isPlayer)
        {
            HideWall();

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
