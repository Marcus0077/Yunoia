using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneCombat : MonoBehaviour
{
    private CombatHandler combatHandler;
    private ExitClone exitClone;
    
    void Awake()
    {
        combatHandler = FindObjectOfType<CombatHandler>();
        exitClone = FindObjectOfType<ExitClone>();

        combatHandler.cloneHP = 3;
    }
    
    void FixedUpdate()
    {
        this.GetComponent<Renderer>().material.color = combatHandler.changeCloneColor;

        if (combatHandler.cloneHP <= 0)
        {
            exitClone.despawnClone = true;
        }
    }
}
