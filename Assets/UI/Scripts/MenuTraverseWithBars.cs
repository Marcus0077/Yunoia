using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuTraverseWithBars : MonoBehaviour, MenuStop
{
    [SerializeField]
    OnButtonHover[] buttons;
    [SerializeField]
    MenuTraverse prevMenu;
    int activeIndex;
    PlayerControls playerControls;
    InputAction menuMove;
    InputAction menuPress;
    bool stop;
    public bool Stop
    {
        get { return stop; }
        set { stop = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        playerControls = new PlayerControls();
        menuMove = playerControls.Movement.Move;
        menuMove.performed += ctx => MoveMenu(ctx);
        menuPress = playerControls.Interaction.Press;
        menuPress.performed += ctx => PressMenu();
    }

    public void OnEnable()
    {
        Debug.Log(gameObject.name);
        menuMove.Enable();
        menuPress.Enable();
        stop = false;
        activeIndex = 0;
        if (prevMenu != null)
            prevMenu.OnDisable();
    }

    public void OnDisable()
    {
        Debug.Log("a");
        menuMove.Disable();
        menuPress.Disable();
        if (prevMenu != null)
            prevMenu.OnEnable();
    }

    public void PressMenu()
    {
        if (activeIndex >= 0)
            buttons[activeIndex].Press();
    }

    public void MoveMenu(InputAction.CallbackContext context)
    {
        if(!stop)
        {
            int amount = Math.Min(1, (int)(-1 * Mathf.Round(context.ReadValue<Vector2>().y)));
            int change = (int)Mathf.Round(context.ReadValue<Vector2>().x);
            if (activeIndex >= 0 || amount > 0)
            {
                if (activeIndex != -1)
                {
                    buttons[activeIndex].OnHoverExit();
                }
                //activeIndex += Math.Min(1, (int)(Math.Round((int)(context.ReadValue<Vector2>().x * 10) / 10f, MidpointRounding.AwayFromZero) + -1 * Math.Round((int)(context.ReadValue<Vector2>().y * 10) / 10f, MidpointRounding.AwayFromZero)));
                activeIndex += amount;
            }
            activeIndex = (activeIndex % buttons.Length + buttons.Length) % buttons.Length;
            if (buttons[activeIndex].GetComponent<OnBarHover>() != null)
            {
                buttons[activeIndex].GetComponent<OnBarHover>().Move(change);
            }
            buttons[activeIndex].OnHoverEnter();
            StartCoroutine(SlowMoveMenu());
        }
    }

    IEnumerator SlowMoveMenu()
    {
        stop = true;
        yield return new WaitForSecondsRealtime(.25f);
        stop = false;
    }

    public void ExitCurrent()
    {
        if(activeIndex >= 0)
            buttons[activeIndex].OnHoverExit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
