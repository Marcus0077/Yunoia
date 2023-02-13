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
    public int activeIndex;
    protected int amount;
    protected Vector2 cursorMovement;
    protected Vector3 mousePosition;
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
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            if (!GameManager.Instance.GetMouseCursor())
            {
                menuMove = playerControls.Menu.Move;
                menuMove.performed += ctx => StartMove(ctx);
                menuMove.canceled += ctx => CancelMove();
                //menuAltMove = playerControls.Menu.AltMove;
                //menuAltMove.performed += ctx => StartMove(ctx);
                //menuAltMove.canceled += ctx => CancelMove();
            }
            else
            {
                menuAltMove = playerControls.Menu.AltMove;
                Debug.Log(menuAltMove);
                menuAltMove.started += ctx => StartMoveCursor();
                menuAltMove.performed += ctx => cursorMovement = ctx.ReadValue<Vector2>();
                menuAltMove.canceled += ctx => cursorMovement = Vector2.zero;
                //menuAltMove.performed += ctx => StartMoveCursor(ctx);
            }
            menuPress = playerControls.Menu.Press;
            menuPress.performed += ctx => PressMenu();
            menuCancel = playerControls.Menu.Back;
            menuCancel.performed += ctx => Back();
        }
    }

    void Awake()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
            if (!GameManager.Instance.GetMouseCursor())
            {
                menuMove = playerControls.Menu.Move;
                menuMove.performed += ctx => StartMove(ctx);
                menuMove.canceled += ctx => CancelMove();
                //menuAltMove = playerControls.Menu.AltMove;
                //menuAltMove.performed += ctx => StartMove(ctx);
                //menuAltMove.canceled += ctx => CancelMove();
            }
            else
            {
                menuAltMove = playerControls.Menu.AltMove;
                Debug.Log(menuAltMove);
                menuAltMove.started += ctx => StartMoveCursor();
                menuAltMove.performed += ctx => cursorMovement = ctx.ReadValue<Vector2>();
                menuAltMove.canceled += ctx => cursorMovement = Vector2.zero;
                //menuAltMove.performed += ctx => StartMoveCursor(ctx);
            }
            menuPress = playerControls.Menu.Press;
            menuPress.performed += ctx => PressMenu();
            menuCancel = playerControls.Menu.Back;
            menuCancel.performed += ctx => Back();
        }
    }

    public virtual void Back()
    {
        if(backIndex >= 0)
            buttons[backIndex].Press();
    }

    public void OnEnable()
    {
        GameManager.Instance.SetColor();
        if (!GameManager.Instance.GetMouseCursor())
        {
            menuMove.Enable();
            //menuAltMove.Enable();
        }
        else
        {
            menuAltMove.Enable();
        }
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
        if (!GameManager.Instance.GetMouseCursor())
        {
            menuMove.Disable();
            //menuAltMove.Disable();   
        }
        else
        {
            menuAltMove.Disable();
        }
        menuPress.Disable();
        menuCancel.Disable();
        ExitAll();
        stop = true;
        moving = null;
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

    public void StartMoveCursor()
    {
        mousePosition = new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0);
        //cursorMovement = context.ReadValue<Vector2>();
        //Mouse.current.WarpCursorPosition(Input.mousePosition + new Vector3(cursorMovement.x, cursorMovement.y,0) * GameManager.Instance.sensitivity);
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


public interface MenuStop
{
    bool Stop { get; set; }
}