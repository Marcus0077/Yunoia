using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{
    protected Rigidbody rb;
    protected float velocity = 1;
    protected float reqChargeLevel = 1;
    float queuedVelocity;
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

    public virtual void Activate()
    {
        
    }

    public virtual void CapSpeed()
    {
        if(data != null && data.maxSpeed != 0)
        {
            if (Mathf.Abs(rb.velocity.magnitude) <= data.maxSpeed)
            {
                if (queuedVelocity > 0 && rb.velocity.magnitude != data.maxSpeed)
                {
                    float usedVelocity = data.maxSpeed - Mathf.Abs(rb.velocity.magnitude);
                    queuedVelocity -= data.maxSpeed - Mathf.Abs(rb.velocity.magnitude);
                    rb.velocity = rb.velocity.normalized * data.maxSpeed;
                }
            }
            else
            {
                queuedVelocity = Mathf.Abs(rb.velocity.magnitude) - Mathf.Min(data.maxSpeed, Mathf.Abs(rb.velocity.magnitude));
                rb.velocity = rb.velocity.normalized * Mathf.Min(data.maxSpeed, Mathf.Abs(rb.velocity.magnitude));
            }
        }
    }

    void FixedUpdate()
    {
        CapSpeed();
    }
}
