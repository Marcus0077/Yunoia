using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class RenderMinions : MonoBehaviour
{
    private GameObject[] Minions;

    private Animator camAnimator;
    
    public int thisRoom;

    public bool hideMinions;
    
    // Start is called before the first frame update
    void Start()
    {
        camAnimator = FindObjectOfType<CinemachineStateDrivenCamera>().GetComponent<Animator>();
        
        Minions = GameObject.FindGameObjectsWithTag("Minion");

        if (camAnimator.GetInteger("roomNum") != thisRoom)
        {
            foreach (var minion in Minions)
            {
                minion.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Minions = GameObject.FindGameObjectsWithTag("Minion");
        
        if (hideMinions)
        {
            foreach (var minion in Minions)
            {
                minion.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            foreach (var minion in Minions)
            {
                minion.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
}
