using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    // Actions to take place upon touching the collider
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
        else if (other.CompareTag("BoxScale"))
        {
            BoxRespawner box = other.gameObject.GetComponent<BoxRespawner>();
            if (box != null)
            {
                 box.Reset();
            }
        }
    }

    public void FadetoBlack()
    {
        StartCoroutine(FadeThenDie());
    }

    private IEnumerator FadeThenDie()
    {
        GameManager.Instance.dying = true;
        if (GameObject.FindObjectOfType<FadeBlack>() != null)
        {
            GameObject.FindObjectOfType<FadeBlack>().FadeToBlack(1.5f);
        }

        yield return new WaitForSeconds(1.5f);
        GameObject.FindWithTag("MainCanvas").transform.Find("Lose Screen Object").gameObject.SetActive(true);
        GameManager.Instance.dying = false;
    }
}
