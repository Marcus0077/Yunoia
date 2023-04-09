using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBlack : MonoBehaviour
{
    private void Awake()
    {
        FadeToTransparent(3f);
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
