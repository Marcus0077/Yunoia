using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    public TextMeshProUGUI winText;

    public GameObject playAgain;
    public GameObject loadMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        winText.enabled = false;
        playAgain.SetActive(false);
        loadMenu.SetActive(false);
    }
    
    public void ReloadVSDenial()
    {
        SceneManager.LoadScene("VSDenial");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winText.enabled = true;
            playAgain.SetActive(true);
            loadMenu.SetActive(true);
            
            other.GetComponent<Grapple>().enabled = false;
            other.GetComponent<AbilityPush>().enabled = false;
            other.GetComponent<SummonClone>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winText.enabled = false;
            playAgain.SetActive(false);
            loadMenu.SetActive(false);
            
            other.GetComponent<Grapple>().enabled = true;
            other.GetComponent<AbilityPush>().enabled = true;
            other.GetComponent<SummonClone>().enabled = true;
        }
    }
}
