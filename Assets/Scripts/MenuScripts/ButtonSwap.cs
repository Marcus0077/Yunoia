using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonSwap : MonoBehaviour
{
    [SerializeField]
    Sprite keyboard, controller;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(GameManager.Instance.GetFloat(Settings.CTRL) > 0)
        {
            if(controller != null)
                GetComponent<Image>().sprite = controller;
        }
        else
        {
            if(keyboard != null)
                GetComponent<Image>().sprite = keyboard;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
