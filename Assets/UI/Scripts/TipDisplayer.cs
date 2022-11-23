using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipDisplayer : MonoBehaviour
{
    public GameObject message;
    public TextMeshProUGUI messageText;

    void OnTriggerEnter()
    {
        message.SetActive(true);
    }
}
