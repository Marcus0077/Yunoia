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
    public int thisCrystalComplete;
    public AudioSource audioSource;
    public AudioClip purifierSound;

    private void Awake()
    {
        numCrystals = coCrystals.Length + 1;

        thisCrystalComplete = 0;
        crystalsComplete = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Faceless"))
        {
            this.GetComponent<SphereCollider>().enabled = !this.GetComponent<SphereCollider>().enabled;
            
            if (!isMultipleCrystals)
            {
                foreach (var plantDestroyer in plantToDestroy)
                {
                    Destroy(plantDestroyer);
                }
            }
            else
            {
                thisCrystalComplete = 1;
                crystalsComplete = 1;

                foreach (var coCrystal in coCrystals)
                {
                    if (coCrystal.GetComponent<PlantDestroyer>().thisCrystalComplete == 1)
                    {
                        crystalsComplete =
                            crystalsComplete + coCrystal.GetComponent<PlantDestroyer>().thisCrystalComplete;
                            audioSource.PlayOneShot(purifierSound);
                    }
                }
                
                Debug.Log(crystalsComplete);

                if (crystalsComplete == numCrystals)
                {
                    foreach (var plant in plantToDestroy)
                    {
                        if (plant != null)
                        {
                            Destroy(plant);
                        }
                    }
                }
            }
        }
    }
}
