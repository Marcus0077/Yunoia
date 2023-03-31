using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject player, terrain, ravine;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter (Collider collider)
    {
        player.transform.position = teleportTarget.transform.position;

        if (ravine != null && terrain != null)
        {
            ravine.SetActive(false);
            terrain.SetActive(true);
        }
    }
}
