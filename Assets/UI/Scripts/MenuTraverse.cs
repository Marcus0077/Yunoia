using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuTraverse : MonoBehaviour
{
    [SerializeField]
    protected OnButtonHover[] buttons;
    [SerializeField]
    protected MenuTraverse prevMenu;
    protected int activeIndex, amount;
    protected PlayerControls playerControls;
    protected InputAction menuMove, menuAltMove, menuPress, menuCancel;
    protected bool stop, move;
    protected float time = .2f;
    protected WaitForSecondsRealtime waitForSecondsRealtime;
    protected Coroutine moving;
    [SerializeField]
    protected int backIndex = -1;
    public bool Move
    {
        get { return move; }
        set { move = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        playerControls = new PlayerControls();
        menuMove = playerControls.Menu.Move;
        menuMove.performed += ctx => StartMove(ctx);
        menuMove.canceled += ctx => CancelMove();
        //menuAltMove = playerControls.Menu.AltMove;
        //menuAltMove.performed += ctx => StartMove(ctx);
        //menuAltMove.canceled += ctx => CancelMove();
        menuPress = playerControls.Menu.Press;
        menuPress.performed += ctx => PressMenu();
        menuCancel = playerControls.Menu.Back;
        menuCancel.performed += ctx => Back();
    }

    public virtual void Back()
    {
        if(backIndex >= 0)
            buttons[backIndex].Press();
    }

    public void OnEnable()
    {
        menuMove.Enable();
        //menuAltMove.Enable();
        menuPress.Enable();
        menuCancel.Enable();
        stop = false;
        activeIndex = 0;
        buttons[activeIndex].OnHoverEnter();
        if (prevMenu != null)
            prevMenu.OnDisable();
    }

    public void OnDisable()
    {
        menuMove.Disable();
        //menuAltMove.Disable();
        menuPress.Disable();
        menuCancel.Disable();
        stop = true;
        moving = null;
        ExitAll();
        if (prevMenu != null)
            prevMenu.OnEnable();
    }

    public virtual void PressMenu()
    {
        Debug.Log(activeIndex);
        buttons[activeIndex].Press();
    }

    public virtual void MoveMenu()
    {
        if (amount != 0)
        {
            buttons[activeIndex].OnHoverExit();
            activeIndex += amount;
        }
        activeIndex = (activeIndex % buttons.Length + buttons.Length) % buttons.Length;
        EnterCurrent(activeIndex);
    }

    public void StartMove(InputAction.CallbackContext context)
    {
        if (!move)
        {
            amount = (int)context.ReadValue<float>();
            if (moving == null)
            {
                move = true;
                moving = StartCoroutine(SlowMoveMenu());
            }
            else
            {
                MoveMenu();
            }
        }
    }

    protected void CancelMove()
    {
        move = false;
    }

    protected IEnumerator SlowMoveMenu()
    {
        yield return null;
        Coroutine self = moving;
        while (move)
        {
            MoveMenu();
            if (waitForSecondsRealtime == null)
            {
                waitForSecondsRealtime = new WaitForSecondsRealtime(time);
            }
            else
            {
                waitForSecondsRealtime.waitTime = time;
            }
            yield return waitForSecondsRealtime;
        }
        if (moving == self)
        {
            moving = null;
        }
         
    }

    public void EnterCurrent(int index)
    {
        buttons[index].OnHoverEnter();
        activeIndex = index;
    }

    public void ExitCurrent()
    {
        if(activeIndex >= 0)
            buttons[activeIndex].OnHoverExit();
    }

    public virtual void ExitAll()
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