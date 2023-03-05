using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnTextToggleHover : OnButtonHover
{
    [SerializeField]
    protected TextMeshProUGUI displayText;
    [SerializeField]
    protected Settings prefName;
    [SerializeField]
    protected Color[] colors;
    int index = 0;

    public override void OnEnable()
    {
        if (GameManager.Instance.textColor)
        {
            index = Array.FindIndex(colors, color => color == GameManager.Instance.ConvertFloatToHex(GameManager.Instance.GetFloat(prefName)));
            ToggleOn();
        }
        else
        {
            index = 0;
            ToggleOff();
        }
    }

    void ToggleOn()
    {
        displayText.text = "Text color on";
        GameManager.Instance.textColor = true;
        GameManager.Instance.SetFloat(prefName, (float)GameManager.Instance.ConvertHexToFloat(colors[index]));
        GameManager.Instance.SetColor();
    }

    void ToggleOff()
    {
        displayText.text = "Text color off";
        displayText.color = Color.white;
        GameManager.Instance.textColor = false;
        GameManager.Instance.SetFloat(prefName, -1);
        //Reload scene to fix text color I guess.
    }

    void Toggled()
    {
        index++;
        if (index >= colors.Length)
        {
            index = 0;
        }
        if (index == 0)
        {
            ToggleOff();
        }
        else
        {
            ToggleOn();
        }
    }

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
        Toggled();
    }
}
