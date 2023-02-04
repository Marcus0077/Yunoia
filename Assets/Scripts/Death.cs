using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public GameObject deathScreen, continueButton, menuButton;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeThenDie());
            GameObject.FindObjectOfType<PauseMenu>().DisableInput();
        }
        else if (other.CompareTag("Clone"))
        {
            GameObject.FindObjectOfType<ExitClone>().despawnClone = true;
        }
    }

    public void FadetoBlack()
    {
        StartCoroutine(FadeThenDie());
    }

    private IEnumerator FadeThenDie()
    {
        if (GameObject.FindObjectOfType<FadeBlack>() != null)
        {
            GameObject.FindObjectOfType<FadeBlack>().FadeToBlack();
        }

        yield return new WaitForSeconds(1.5f);
        
        deathScreen.SetActive(true);
        continueButton.SetActive(true);
        menuButton.SetActive(true);
    }
}