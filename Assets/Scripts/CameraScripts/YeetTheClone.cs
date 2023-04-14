using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YeetTheClone : MonoBehaviour
{
    private bool passedRoom;

    private void Awake()
    {
        passedRoom = false;
    }

    // Literally just despawns the clone if the clone walks through this trigger.
    private void OnTriggerEnter(Collider other)
    {
        if (!passedRoom)
        {
            if (other.CompareTag("Clone"))
            {
                other.GetComponent<ExitClone>().despawnClone = true;
            }
            else if (other.CompareTag("Player"))
            {
                if (GameObject.FindWithTag("Clone") != null)
                {
                    if ((this.GetComponent<TeleToBeaver>() == null && this.GetComponent<Teleport>() == null))
                    {
                        GameObject.FindWithTag("Clone").GetComponent<ExitClone>().despawnClone = true;
                    }
                }

                passedRoom = true;
            }
        }
    }
}
