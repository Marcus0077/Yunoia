using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDestroyer : MonoBehaviour
{
    public GameObject[] plantToDestroy;
    public GameObject[] coCrystals;

    public bool isMultipleCrystals;
    public int numCrystals;
    public int crystalsComplete;

    private void Awake()
    {
        numCrystals = coCrystals.Length;
        Debug.Log(numCrystals);

        crystalsComplete = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Faceless"))
        {
            if (!isMultipleCrystals)
            {
                foreach (var plantDestroyer in plantToDestroy)
                {
                    Destroy(plantDestroyer);
                }
            }
            else
            {
                crystalsComplete += 1;

                foreach (var coCrystal in coCrystals)
                {
                    crystalsComplete = crystalsComplete + coCrystal.GetComponent<PlantDestroyer>().crystalsComplete;
                    Debug.Log(crystalsComplete);
                }

                if (crystalsComplete == numCrystals)
                {
                    foreach (var plantDestroyer in plantToDestroy)
                    {
                        if (plantDestroyer != null)
                        {
                            Destroy(plantDestroyer);
                        }
                    }
                }
            }
        }
    }
}
