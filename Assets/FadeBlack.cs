using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBlack : MonoBehaviour
{
    private void Awake()
    {
        FadeToTransparent();
    }

    public void FadeToBlack()
    {
        LeanTween.color(GetComponent<RectTransform>(), Color.black, 1.5f).setEaseInOutQuint();
    }
    
    public void FadeToTransparent()
    {
        LeanTween.color(GetComponent<RectTransform>(), Color.clear, 3f);
    }
}
