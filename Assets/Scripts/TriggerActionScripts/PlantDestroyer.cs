using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlantDestroyer : MonoBehaviour
{
    public GameObject[] plantToDestroy;
    public GameObject[] coCrystals;
    public GameObject[] facelessList;

    public bool isMultipleCrystals;
    public int numCrystals;
    public int crystalsComplete;
    public int thisCrystalComplete;
    
    public AudioSource audioSource;
    public AudioClip purifierSound;

    public Levels level;

    public GameObject coAI;

    public int puzzleNum;
    private GameManager gameManager;
    public bool hasPuzzleCam;

    private Animator pPlateAnimator;

    private string prefName;

    public GameObject aiPrefab;
    private GameObject spawnedAI;
    int indexOfSave = -1;

    private void Awake()
    {
        pPlateAnimator = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        prefName = gameObject.name + gameManager.currentLevel;
        numCrystals = coCrystals.Length + 1;
        thisCrystalComplete = 0;
        crystalsComplete = 0;
        indexOfSave = Array.FindIndex(DataManager.gameData.pressurePlates, f => f.name == prefName);
        if (indexOfSave >= 0)
        {
            hasPuzzleCam = false;
            CompletePlate();
        }
    }
    private IEnumerator CompletePuzzle()
    {
        if (hasPuzzleCam)
        {
            float waitTime = 2.5f;

            gameManager.ShowPuzzleWrapper(puzzleNum, waitTime);

            yield return new WaitForSeconds(waitTime);
        }

        if (isMultipleCrystals)
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
        else
        {
            foreach (var plantDestroyer in plantToDestroy)
            {
                Destroy(plantDestroyer);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Faceless"))
        {
            Debug.Log("FacelessEntered");
            for (int i = 0; i < facelessList.Length; i++)
            {
                if(facelessList[i] == other.gameObject)
                {
                    Debug.Log("found");
                    PressurePlateSave save = new PressurePlateSave();
                    save.name = prefName;
                    save.value = 1 + i;
                    for(int j = 0; j < DataManager.gameData.pressurePlates.Length; j++)
                    {
                        if (DataManager.gameData.pressurePlates[j].name == null || DataManager.gameData.pressurePlates[j].name == "")
                        {
                            Debug.Log("Found Empty at " + j);
                            DataManager.gameData.pressurePlates[j] = save;
                            break;
                        }
                    }
                }
            }
            this.GetComponent<SphereCollider>().enabled = !this.GetComponent<SphereCollider>().enabled;
            if (pPlateAnimator != null)
            {
                pPlateAnimator.SetBool("plateDown", true);
            }
            
            if (!isMultipleCrystals)
            {
                coAI = other.GameObject();
                
                Debug.Log("Puzzle Routine Plays."); 
                StartCoroutine(CompletePuzzle());
            }
            else if (thisCrystalComplete == 0)
            {
                coAI = other.GameObject();
                
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
                    StartCoroutine(CompletePuzzle());
                }
            }
        }
    }

    private void CompletePlate()
    {
        if (aiPrefab != null)
        {
            spawnedAI = Instantiate(aiPrefab, transform.position, Quaternion.Euler(Vector3.forward));

            spawnedAI.GetComponent<AiMovement>().canAiMove = false;
        }
        

        this.GetComponent<SphereCollider>().enabled = !this.GetComponent<SphereCollider>().enabled;
        facelessList[(int)DataManager.gameData.pressurePlates[indexOfSave].value - 1].SetActive(false);
        if (pPlateAnimator != null)
        {
            pPlateAnimator.SetBool("plateDown", true);
        }

        if (!isMultipleCrystals)
        {
            Debug.Log("Puzzle Routine Plays.");
            StartCoroutine(CompletePuzzle());
        }
        else if (thisCrystalComplete == 0)
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
                StartCoroutine(CompletePuzzle());
            }
        }
    }

    private IEnumerator LoadHub()
    {
        yield return new WaitForSeconds(1.25f);
        GameObject.FindObjectOfType<FadeBlack>().FadeToBlack(1.5f);
        //GameManager.Instance.SetCheckpoint(Levels.HUB, new Vector3(0.92f, 47.07f, 19.93f));
        DataManager.gameData.checkpointed = false;
        //DataManager.gameData.position.Set(0.92f, 47.07f, 19.93f);
        GameManager.Instance.CompleteLevel(level);
        SceneManager.LoadScene("BargainingOutro");
    }
}
