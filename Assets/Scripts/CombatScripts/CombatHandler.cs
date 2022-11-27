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
        

        changeCloneColor = new Color(1f, 1f, 1f, .3f);
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
        ExitClone clone = FindObjectOfType<ExitClone>();
        isRunning = true;
        
        changeAIColor = Color.blue;
        yield return new WaitForSeconds(0.5f);
            
        if (inCombat)
        {
            changeAIColor = Color.red;
            changeCloneColor = new Color(1f, 0f, 0f, .3f);
            //cloneHP -= 1;
            //healthText.text = "Clone Health: " + cloneHP + "/3";
            clone.Timer -= 5f;
            yield return new WaitForSeconds(0.25f);
        }

        changeAIColor = Color.black;
        changeCloneColor = new Color(1f, 1f, 1f, .3f);
        yield return new WaitForSeconds(1f);

        isRunning = false;
    }
}
