using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuTraverse : MonoBehaviour, MenuStop
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
        menuMove.Enable();
        menuPress.Enable();
        stop = false;
        activeIndex = 0;
        if (prevMenu != null)
            prevMenu.OnDisable();
    }

    public void OnDisable()
    {
        menuMove.Disable();
        menuPress.Disable();
        stop = true;
        ExitAll();
        if (prevMenu != null)
            prevMenu.OnEnable();
    }

    public void PressMenu()
    {
        if (activeIndex >= 0)
        {
            buttons[activeIndex].Press();
        }
    }

    public void MoveMenu(InputAction.CallbackContext context)
    {
        if(!stop)
        {
            int amount = Math.Min(1, (int)Mathf.Round(context.ReadValue<Vector2>().x) + -1 * (int)Mathf.Round(context.ReadValue<Vector2>().y));
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

    public void ExitAll()
    {
        foreach (OnButtonHover button in buttons)
            button.OnHoverExit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public interface MenuStop
{
    bool Stop { get; set; }
}