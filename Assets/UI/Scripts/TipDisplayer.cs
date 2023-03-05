using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipDisplayer : MonoBehaviour
{
    public GameObject message, prompt;
    public TextMeshProUGUI messageText;
    public float displayTime;
    public TipController tc;
    private bool isOpen;

    void Awake()
    {
        tc = GetComponent<TipController>();
        isOpen = false;
    }
    
    void OnTriggerStay()
    {
        if(!isOpen)
        {
            prompt.SetActive(true);
        
            if(Input.GetKey(KeyCode.E) && !isOpen)
            {
                message.SetActive(true);
                tc.OnEnable();
                isOpen = true;
            }
        }

        else if(isOpen && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(CloseMessage());
        }
    }

    void OnTriggerExit()
    {
        tc.CloseDialog();
        isOpen = false;
        prompt.SetActive(false);
    }

    private IEnumerator CloseMessage()
    {
        prompt.SetActive(true);
        tc.CloseDialog();

        yield return new WaitForSeconds(1f);

        isOpen = false;
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
