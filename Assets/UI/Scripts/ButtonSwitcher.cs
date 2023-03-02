using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitcher : MonoBehaviour
{
    public GameObject button1, button2, button3, button4;

    [SerializeField] bool button1Active, button2Active, button3Active, button4Active;

    // Start is called before the first frame update
    void Start()
    {
        button1Active = true;

        button2Active = false;

        button3Active = false;

        button4Active = false;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.D))
       {
            NextButton();
       } 
    }

    public void NextButton()
    {
        if(button1Active)
        {
            button1.SetActive(false);
            button2.SetActive(true);

            button1Active = false;
            button2Active = true;
        }

        if(button2Active)
        {
            button2.SetActive(false);
            button3.SetActive(true);

            button2Active = false;
            button3Active = true;
        }

        if(button3Active)
        {
            button3.SetActive(false);
            button4.SetActive(true);

            button3Active = false;
            button4Active = true;
        }

        if(button4Active)
        {
            button4.SetActive(false);
            button1.SetActive(true);

            button4Active = false;
            button1Active = true;
        }
    }
}

