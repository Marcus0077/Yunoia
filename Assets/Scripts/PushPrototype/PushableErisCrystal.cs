using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PushableErisCrystal : Pushable
{
    public override void Activate()
    {
        GetComponent<ErisDamage>().DoDamage();
        
    }

    public override bool Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        return base.Pushed(force, chargeLevel, totalCharges, pusher);
    }
}
