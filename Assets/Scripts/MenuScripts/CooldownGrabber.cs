using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownGrabber : MonoBehaviour
{
    AbilityPush pushFromPlayer;
    SummonClone cloneFromPlayer;
    Grapple grappleFromPlayer;
    // Start is called before the first frame update
    void Start()
    {
        pushFromPlayer = GetComponent<AbilityPush>();
        cloneFromPlayer = GetComponent<SummonClone>();

    }

    public float GetCD(string ability)
    {
        if(ability == "push")
        {
            if(cloneFromPlayer.cloneSummoned)
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
            // return some value some function that returns cooldown of clone
            return -1; //temporary value until function is added
        }
        else if(ability == "grapple")
        {
            if (cloneFromPlayer.cloneSummoned)
            {
                //return cloneFromPlayer.clone.GetComponent<Grapple>().FunctionToGetCooldown();
            }
            else
            {
                //return grappleFromPlayer.FunctionToGetCooldown();
            }
            return -1; //temporary value until function is added
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
