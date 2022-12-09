using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuTraverseWithBars : MenuTraverse, MenuStop
{
    //public void MoveMenu(InputAction.CallbackContext context)
    //{
    //    if(!stop)
    //    {
    //        int amount = Math.Min(1, (int)(-1 * Mathf.Round(context.ReadValue<Vector2>().y)));
    //        int change = (int)Mathf.Round(context.ReadValue<Vector2>().x);
    //        if (activeIndex >= 0 || amount > 0)
    //        {
    //            if (activeIndex != -1)
    //            {
    //                buttons[activeIndex].OnHoverExit();
    //            }
    //            //activeIndex += Math.Min(1, (int)(Math.Round((int)(context.ReadValue<Vector2>().x * 10) / 10f, MidpointRounding.AwayFromZero) + -1 * Math.Round((int)(context.ReadValue<Vector2>().y * 10) / 10f, MidpointRounding.AwayFromZero)));
    //            activeIndex += amount;
    //        }
    //        activeIndex = (activeIndex % buttons.Length + buttons.Length) % buttons.Length;
    //        if (buttons[activeIndex].GetComponent<OnBarHover>() != null)
    //        {
    //            buttons[activeIndex].GetComponent<OnBarHover>().Move(change);
    //        }
    //        buttons[activeIndex].OnHoverEnter();
    //        StartCoroutine(SlowMoveMenu());
    //    }
    //}
}
