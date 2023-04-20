using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonSwap : MonoBehaviour
{
    [SerializeField]
    Sprite keyboard, controller;
    [SerializeField]
    bool keyboardLong;
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
            {
                if(keyboardLong)
                    GetComponent<RectTransform>().localPosition = new Vector3(GetComponent<RectTransform>().localPosition.x-5, GetComponent<RectTransform>().localPosition.y, GetComponent<RectTransform>().localPosition.z);
                GetComponent<Image>().sprite = keyboard;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
