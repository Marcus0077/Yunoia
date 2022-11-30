using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipDisplayer : MonoBehaviour
{
    public GameObject message;
    public TextMeshProUGUI messageText;
    public float displayTime;

   /* void OnTriggerEnter()
    {
        message.SetActive(true);
    }

    void OnTriggerExit()
    {
        message.SetActive(false);
    }*/

    void Awake()
    {
        StartCoroutine(DisplayMessage());
    }

    IEnumerator DisplayMessage()
    {
        message.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        message.SetActive(false);
    }
}
