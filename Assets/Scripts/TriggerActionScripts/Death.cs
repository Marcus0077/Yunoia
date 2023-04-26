using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [SerializeField] AudioSource hurtSound;

    // Actions to take place upon touching the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hurtSound != null)
            {
                hurtSound.Play();
            }

            //StartCoroutine(FadeThenDie());
            GameObject.FindObjectOfType<PauseMenu>().DisableInput();
            FadetoBlack();
        }
        else if (other.CompareTag("Clone"))
        {
            if (hurtSound != null)
            {
                hurtSound.Play();
            }
            
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

        //StartCoroutine(FadeThenDie());
        GameManager.Instance.StartDeath();
    }

    //private IEnumerator FadeThenDie()
    //{
    //    GameManager.Instance.dying = true;
    //    if (GameObject.FindObjectOfType<FadeBlack>() != null)
    //    {
    //        GameObject.FindObjectOfType<FadeBlack>().FadeToBlack(1.5f);
    //    }

    //    yield return new WaitForSeconds(1.5f);
    //    GameObject.FindWithTag("MainCanvas").transform.Find("Lose Screen Object").gameObject.SetActive(true);
    //    GameManager.Instance.dying = false;
    //}
}
