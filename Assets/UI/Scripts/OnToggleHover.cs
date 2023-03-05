using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnToggleHover : OnButtonHover
{
    [SerializeField]
    protected RawImage on, off;
    [SerializeField]
    protected Settings prefName;

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
        on.enabled = true;
        off.enabled = false;
        GameManager.Instance.SetFloat(prefName, 1);
    }

    void ToggleOff()
    {
        on.enabled = false;
        off.enabled = true;
        GameManager.Instance.SetFloat(prefName, 0);
    }

    void Toggled()
    {
        if(on.enabled)
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
