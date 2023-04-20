using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    public CanvasGroup text;
    private void Awake()
    {
        FadeToTransparent(5f);
    }

     public void FadeToBlack(float waitTime)
    {
        text.LeanAlpha(1, waitTime).setEaseInOutQuint();
    }
    
    public void FadeToTransparent(float waitTime)
    {
        text.LeanAlpha(0, waitTime);
    } 
}
