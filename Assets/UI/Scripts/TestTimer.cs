using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTimer : MonoBehaviour
{
    public Image timerBar;
    float timeLeft;
    public float maxTime, phase1, phase2, phase3, finalPhase;
    public Animator anim;

    public GameObject flame1, flame2, flame3, finalFlame;
    // Start is called before the first frame update
    void Start()
    {
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLeft > 0)
        {
            timeLeft-= Time.deltaTime;
            timerBar.fillAmount = timeLeft/maxTime;
        }
        else
        {
            Destroy(timerBar);
        }

        if(timeLeft < phase1)
        {
            flame1.SetActive(true);
        }

        if(timeLeft < phase2)
        {
            flame2.SetActive(true);
        }

        if(timeLeft < phase3)
        {
            flame3.SetActive(true);
        }

        if(timeLeft < finalPhase)
        {
            finalFlame.SetActive(true);
        }
    }
}
