using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneCombat : MonoBehaviour
{
    private CombatHandler combatHandler;
    private ExitClone exitClone;

    public GameObject cloneHead;
    
    void Awake()
    {
        combatHandler = FindObjectOfType<CombatHandler>();
        exitClone = FindObjectOfType<ExitClone>();


        if (combatHandler != null)
        {
            combatHandler.cloneHP = 3;
        }
    }
    
    void FixedUpdate()
    {
        cloneHead.GetComponent<Renderer>().material.color = combatHandler.changeCloneColor;

        if (combatHandler != null)
        {
            if (combatHandler.cloneHP <= 0)
            {
                exitClone.despawnClone = true;
            }
        }
    }
}
