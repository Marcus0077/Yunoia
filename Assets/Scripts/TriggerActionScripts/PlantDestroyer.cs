using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public Levels level;

    private void Awake()
    {
        //numCrystals = coCrystals.Length + 1;

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
                            //audioSource.PlayOneShot(purifierSound);
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

                    if (numCrystals == 4)
                    {
                        StartCoroutine(LoadHub());
                    }
                }
            }
        }
    }

    private IEnumerator LoadHub()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("HubFinal");
        DataManager.gameData.position.Set(0.92f, 47.07f, 19.93f);
        GameManager.Instance.CompleteLevel(level);
    }
}
