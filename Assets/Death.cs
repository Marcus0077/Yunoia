using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeThenDie());
        }
        else if (other.CompareTag("Clone"))
        {
            GameObject.FindObjectOfType<ExitClone>().despawnClone = true;
        }
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
