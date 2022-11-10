using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownGrabber : MonoBehaviour
{
    AbilityPush pushFromPlayer;
    AbilityPush pushFromClone;
    SummonClone cloneFromPlayer;
    Grapple grappleFromPlayer;
    Grapple grappleFromClone;
    BasicMovement dashFromPlayer;
    BasicMovement dashFromClone;
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
            if(cloneFromPlayer.cloneSummoned && dashFromClone.canMove)
            {
                return pushFromClone.CooldownRemaining();
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
            if (cloneFromPlayer.cloneSummoned && dashFromClone.canMove)
            {
                return grappleFromClone.CooldownRemaining();
            }
            else
            {
                return grappleFromPlayer.CooldownRemaining();
            }
        }
        else if(ability == "dash")
        {
            if (cloneFromPlayer.cloneSummoned && dashFromClone.canMove)
            {
                return dashFromClone.CooldownRemaining();
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
    void FixedUpdate()
    {
        if (cloneFromPlayer.cloneSummoned && dashFromClone == null)
        {
            dashFromClone = cloneFromPlayer.clone.GetComponent<BasicMovement>();
            pushFromClone = cloneFromPlayer.clone.GetComponent<AbilityPush>();
            grappleFromClone = cloneFromPlayer.clone.GetComponent<Grapple>();
        }
        else if (!cloneFromPlayer.cloneSummoned && dashFromClone != null)
        {
            dashFromClone = null;
            pushFromClone = null;
            grappleFromClone = null;
        }
    }
}
