using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCombat : MonoBehaviour
{
    private CombatHandler combatHandler;
    
    void Awake()
    {
        combatHandler = FindObjectOfType<CombatHandler>();
    }
    
    void FixedUpdate()
    {
        this.GetComponent<Renderer>().material.color = combatHandler.changeAIColor;
    }
}
