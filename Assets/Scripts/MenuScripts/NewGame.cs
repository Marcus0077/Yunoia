using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewGame : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if (GameManager.Instance.GetFloat(Settings.FIRSTPLAY) >= 0)
        {
            text.text = "New Game";
        }
        else
        {
            text.text = "Start";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
