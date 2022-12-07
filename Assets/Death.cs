using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public GameObject deathScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            deathScreen.SetActive(true);
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
        
        SceneManager.LoadScene("VSDenial");
    }
}
