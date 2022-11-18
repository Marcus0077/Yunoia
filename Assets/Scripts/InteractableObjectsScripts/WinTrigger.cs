using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    // Win UI text.
    public TextMeshProUGUI winText;

    // Win UI button references.
    public GameObject playAgain;
    public GameObject loadMenu;
    
    // Get references and initialize variables when win trigger spawn.
    void Awake()
    {
        winText.enabled = false;
        playAgain.SetActive(false);
        loadMenu.SetActive(false);
    }
    
    // Reloads the scene for the Sprint 3 denial level.
    public void ReloadVSDenial()
    {
        SceneManager.LoadScene("VSDenial");
    }

    // Loads the main menu scene.
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // Determines if player is in the win trigger. If so, deactivate all abilities
    // and load win-state UI.
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

    // Determines if player is exiting the win trigger. If so, activate all abilities
    // and remove win-state UI.
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
