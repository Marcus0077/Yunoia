using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnBarHover : OnButtonHover
{
    public void OnHoverEnter()
    {
        LeanTween.color(button.GetComponent<RectTransform>(), hoverColor, .5f).setEaseInOutQuint().setIgnoreTimeScale(true);
        transform.LeanScale(new Vector2(hoverXSize, hoverYSize), 0.125f).setEaseInOutQuint().setIgnoreTimeScale(true);
    }

    public void OnHoverExit()
    {
        LeanTween.color(button.GetComponent<RectTransform>(), nuetralColor, .5f).setEaseInOutQuint().setIgnoreTimeScale(true);
        transform.LeanScale(new Vector2(exitXSize, exitYSize), 0.125f).setEaseInOutQuint().setIgnoreTimeScale(true);
    }

    public void Press()
    {
        //GetComponent<Button>().onClick.Invoke();
    }

    public void Move(int amount)
    {
        GetComponent<Slider>().value = Mathf.Clamp(GetComponent<Slider>().value + amount / 10f, 0, 1);
        
    }
}
