using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatHandler : MonoBehaviour
{
    // Health UI Text
    public TextMeshProUGUI healthText;
    
    // Active Timer Text UI
    public TextMeshProUGUI activeTimerText;

    public int cloneHP;

    private bool isRunning;
    public bool inCombat;

    public Color changeAIColor;
    public Color changeCloneColor;
    
    void Awake()
    {
        healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();
        activeTimerText = GameObject.FindGameObjectWithTag("Active Timer").GetComponent<TextMeshProUGUI>();
        healthText.text = "";
        activeTimerText.text = "";
        

        changeCloneColor = new Color(1f, .8f, .3f, .7f);
        changeAIColor = Color.black;
        
        isRunning = false;
        inCombat = false;
    }

    private void FixedUpdate()
    {
        if (inCombat && !isRunning)
        {
            StartCoroutine(TakeCloneHP());
        }
    }

    IEnumerator TakeCloneHP()
    {
        isRunning = true;
        
        changeAIColor = Color.blue;
        yield return new WaitForSeconds(0.5f);
            
        if (inCombat)
        {
            changeAIColor = Color.red;
            changeCloneColor = Color.red;
            cloneHP -= 1;
            healthText.text = "Clone Health: " + cloneHP + "/3";
            yield return new WaitForSeconds(0.25f);
        }

            changeAIColor = Color.black;
        changeCloneColor = new Color(1f, .8f, .3f, .7f);
        yield return new WaitForSeconds(1f);

        isRunning = false;
    }
}
