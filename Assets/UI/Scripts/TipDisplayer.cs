using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TipDisplayer : MonoBehaviour
{
    public GameObject message, prompt, player;
    public TextMeshProUGUI messageText;
    public float displayTime;
    public TipController tc;
    public InputActionAsset controls;
    public InputAction interact;
    public bool isOpen, closing;

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
            message.SetActive(true);
            tc.OnEnable();
            isOpen = true;
        }
        else if(!closing)
        {
            prompt.SetActive(true);
            closing = true;
            tc.CloseDialog();
        }
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
