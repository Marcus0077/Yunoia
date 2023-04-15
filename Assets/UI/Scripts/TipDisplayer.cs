using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TipDisplayer : MonoBehaviour
{
    public GameObject prompt, player;
    public float displayTime;
    public TipController tc;
    public InputActionAsset controls;
    public InputAction interact;
    public bool isOpen, closing;
    public HintTrigger trigger;
    public HintManager hm;

    void Awake()
    {
        controls = GameManager.Instance.GetComponent<PlayerInput>().actions;
        controls.Enable();
        interact = controls.FindActionMap("Interaction")["Press"];
        tc = GetComponent<TipController>();
        isOpen = false;
        closing = false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            prompt.SetActive(true);
            player = other.gameObject;
            interact.performed += ToggleMessage;
            trigger.DisplayHint();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(player !=null && other.gameObject.name == player.name)
        {
            interact.performed -= ToggleMessage;
            player = null;
            if (isOpen)
                tc.CloseDialog();
            isOpen = false;
            prompt.SetActive(false);
        }
    }

    void ToggleMessage(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        if (!isOpen)
        {
            prompt.SetActive(false);
            tc.OnEnable();
            isOpen = true;
            trigger.DisplayHint();
        }
        else if(!closing)
        {
            hm.NextHint();

            if(hm.NoHints == true)
            {
                closing = true;
                tc.CloseDialog();
                prompt.SetActive(true);
            }
        }
    }

    void OnDestroy()
    {
        interact.performed -= ToggleMessage;
    }

    /*void Awake()
    {
        StartCoroutine(DisplayMessage());
    }

    IEnumerator DisplayMessage()
    {
        message.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        message.SetActive(false);
    }*/
}
