using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    public CanvasGroup text;
    private static bool levelStarted = false;
    private void Awake()
    {
        if(!levelStarted)
        {
            StartCoroutine(FadeToTransparent(5f));
        }
        else if (levelStarted)
        {
            text.LeanAlpha(0, 0);
        }
    }

     public void FadeToBlack(float waitTime)
    {
        text.LeanAlpha(1, waitTime).setEaseInOutQuint();
    }
    
    IEnumerator FadeToTransparent(float waitTime)
    {
        text.LeanAlpha(0, waitTime);

        yield return new WaitForSeconds(5f);

        levelStarted = true;
        Debug.Log(levelStarted);
    } 

    public void LevelFinished()
    {
        levelStarted = false;
        Debug.Log(levelStarted);
    }
}
