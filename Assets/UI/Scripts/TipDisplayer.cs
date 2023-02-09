using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipDisplayer : MonoBehaviour
{
    public GameObject message;
    public TextMeshProUGUI messageText;
    public float displayTime;
    public TipController tc;

    void Awake()
    {
        tc = GetComponent<TipController>();
    }
    
    void OnTriggerEnter()
    {
        message.SetActive(true);
        tc.OnEnable();
    }

    void OnTriggerExit()
    {
        tc.CloseDialog();
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
