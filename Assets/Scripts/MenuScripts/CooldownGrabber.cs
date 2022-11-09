using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownGrabber : MonoBehaviour
{
    AbilityPush pushFromPlayer;
    SummonClone cloneFromPlayer;
    Grapple grappleFromPlayer;
    BasicMovement dashFromPlayer;
    // Start is called before the first frame update
    void Start()
    {
        pushFromPlayer = GetComponent<AbilityPush>();
        cloneFromPlayer = GetComponent<SummonClone>();
        grappleFromPlayer = GetComponent<Grapple>();
        dashFromPlayer = GetComponent<BasicMovement>();
    }

    public float GetCD(string ability)
    {
        if(ability == "push")
        {
            if(cloneFromPlayer.cloneSummoned && cloneFromPlayer.clone.GetComponent<BasicMovement>().canMove)
            {
                return cloneFromPlayer.clone.GetComponent<AbilityPush>().CooldownRemaining();
            }
            else
            {
                return pushFromPlayer.CooldownRemaining();
            }
        }
        else if(ability == "clone")
        {
            return cloneFromPlayer.CooldownRemaining();
        }
        else if(ability == "grapple")
        {
            if (cloneFromPlayer.cloneSummoned && cloneFromPlayer.clone.GetComponent<BasicMovement>().canMove)
            {
                return cloneFromPlayer.clone.GetComponent<Grapple>().CooldownRemaining();
            }
            else
            {
                return grappleFromPlayer.CooldownRemaining();
            }
        }
        else if(ability == "dash")
        {
            if (cloneFromPlayer.cloneSummoned && cloneFromPlayer.clone.GetComponent<BasicMovement>().canMove)
            {
                return cloneFromPlayer.clone.GetComponent<BasicMovement>().CooldownRemaining();
            }
            else
            {
                return dashFromPlayer.CooldownRemaining();
            }
        }
        else
        {
            return -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
