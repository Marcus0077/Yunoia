using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuTraverseWithBars : MenuTraverse
{
    public bool barEnter;

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
        if (activeIndex < 0)
            return;
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
            buttons[backIndex].Press();
        }
    }

    public void ExitAll()
    {
        base.ExitAll();
        barEnter = false;
    }

    void Update()
    {
        if(barEnter)
        {
            buttons[activeIndex].GetComponent<OnBarHover>().Move((int)Math.Round(cursorMovement.x, 1, MidpointRounding.AwayFromZero));
            return;
        }
        if (GameManager.Instance.GetMouseCursor() && cursorMovement != Vector2.zero)
        {
            //int scalar = 5;
            //if ((cursorMovement.x < 0 || cursorMovement.y > 0) && !(cursorMovement.x > 0 || cursorMovement.y < 0))
            //    scalar = 4;
            mousePosition += new Vector3(cursorMovement.x, cursorMovement.y, 0) * 50 * GameManager.Instance.GetFloat(Settings.SENSE);
            //Debug.Log(currentPosition);
            Mouse.current.WarpCursorPosition(mousePosition);
        }
    }
}
