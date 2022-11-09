using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject Blocker;

    private bool isPlayer;
    public bool isClone;

    private void Awake()
    {
        isPlayer = false;
        isClone = false;
    }

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

    public void HideWall()
    {
        Blocker.GetComponent<Renderer>().enabled = false;
        Blocker.GetComponent<Collider>().enabled = false;
    }
    
    public void AppearWall()
    {
        Blocker.GetComponent<Renderer>().enabled = true;
        Blocker.GetComponent<Collider>().enabled = true;
    }
}
