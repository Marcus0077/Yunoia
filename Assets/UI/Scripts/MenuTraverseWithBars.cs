using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuTraverseWithBars : MenuTraverse
{
    bool barEnter;

    public override void MoveMenu()
    {
        if (!barEnter)
        {
            if (amount != 0)
            {
                buttons[activeIndex].OnHoverExit();
                activeIndex += amount;
            }
            activeIndex = (activeIndex % buttons.Length + buttons.Length) % buttons.Length;
            EnterCurrent(activeIndex);
        }
        else
        {
            buttons[activeIndex].GetComponent<OnBarHover>().Move(amount);
        }
    }

    public override void PressMenu()
    {
        if (buttons[activeIndex].GetComponent<OnBarHover>() != null)
        {
            barEnter = true;
            return;
        }
        buttons[activeIndex].Press();
    }

    public override void Back()
    {
        if (barEnter)
        {
            barEnter = false;
            return;
        }
        if (backIndex >= 0)
        {
            Debug.Log("a");
            buttons[backIndex].Press();
        }
    }

    public void ExitAll()
    {
        base.ExitAll();
        barEnter = false;
    }
}
