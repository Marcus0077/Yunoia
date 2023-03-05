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
        if(GameManager.Instance.GetFloat(prefName) > 0)
        {
            ToggleOn();
        }
        else
        {
            ToggleOff();
        }
    }

    void ToggleOn()
    {
        GameManager.Instance.textColor = true;
        GameManager.Instance.SetColor();
    }

    void ToggleOff()
    {
        GameManager.Instance.textColor = false;
        //GameManager.Instance.menuTraverse
    }

    void Toggled()
    {
        if(GameManager.Instance.textColor)
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
