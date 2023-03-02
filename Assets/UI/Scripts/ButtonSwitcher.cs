using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSwitcher : MonoBehaviour
{
    public Transform button1, button2; 
    public Button startButton, settingsButton;
    public int currentButton;

    // Start is called before the first frame update
    void Start()
    {
        currentButton = 0;
        settingsButton.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {
            NextButton();
        }

        if(Input.GetKey(KeyCode.A))
        {
            PrevButton();
        }
    }

    public void NextButton()
    {
        button1.LeanMoveLocalX(-400, 0.5f).setEaseOutExpo();

        button2.LeanMoveLocalX(0, 0.5f).setEaseOutExpo();

        startButton.enabled = false;

        settingsButton.enabled = true;
    }

    public void PrevButton()
    {
        button1.LeanMoveLocalX(0, 0.5f).setEaseOutExpo();

        button2.LeanMoveLocalX(400, 0.5f).setEaseOutExpo();

        startButton.enabled = true;

        settingsButton.enabled = false;
    }
}

