using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBlack : MonoBehaviour
{
    public CanvasGroup text;
    private void Start()
    {
        GameObject.FindObjectOfType<PauseMenu>().pause.Disable();
        FadeToTransparent(3f);
        text.LeanAlpha(0, 3f).setOnComplete(() => { GameObject.FindObjectOfType<PauseMenu>().pause.Enable(); });
    }

     public void FadeToBlack(float waitTime)
    {
        LeanTween.color(GetComponent<RectTransform>(), Color.black, waitTime).setEaseInOutQuint();
    }
    
    public void FadeToTransparent(float waitTime)
    {
        LeanTween.color(GetComponent<RectTransform>(), Color.clear, waitTime);
    } 
}
