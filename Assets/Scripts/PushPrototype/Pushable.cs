using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{
    protected Rigidbody rb;
    protected float velocity = 1;
    protected float reqChargeLevel = 1;
    [SerializeField]
    protected PushableFunction data;

    public float pushSpeed
    {
        get { return velocity; } private set { velocity = value; } 
    }
    void Start()
    {
        
    }

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(data != null)
        {
            velocity = data.pushSpeed;
            reqChargeLevel = data.reqChargeLevel;
        }
    }

    public virtual bool Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        if(chargeLevel >= reqChargeLevel)
        {
            float chargeMultiplier = chargeLevel / (float)totalCharges;
            rb.AddForce(force * velocity * chargeMultiplier, ForceMode.VelocityChange);
            if (data != null)
            {
                data.OnPush(gameObject);
                return true;
            }
            else
            {
                Debug.Log("Missing scriptable object");
                return false;
            }
        } else
        {
            return false;
        }
    }
}
