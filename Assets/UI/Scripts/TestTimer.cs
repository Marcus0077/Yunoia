using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTimer : MonoBehaviour
{
    public Image timerBar;
    float timeLeft;
    public float maxTime;
    public Animator anim;
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

        if(timeLeft < 5)
        {
            anim.SetTrigger("shake");
        }
    }
}
