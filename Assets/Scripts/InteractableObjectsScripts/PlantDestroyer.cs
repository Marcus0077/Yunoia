using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDestroyer : MonoBehaviour
{
    public GameObject plantToDestroy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Faceless"))
        {
            Destroy(plantToDestroy);
        }
    }
}
