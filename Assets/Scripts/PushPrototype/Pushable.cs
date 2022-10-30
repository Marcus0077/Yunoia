using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{
    protected Rigidbody rb;
    protected float velocity = 1;
    protected float reqChargeLevel = 1;
    public float queuedVelocity;
    public float totalVelocity = 0;
    public bool canReturn;
    bool pushed, skip;
    Vector3 signs, pushDirection;
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
        rb.isKinematic = true;
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
            Constraints();
            totalVelocity = 0;
            rb.isKinematic = false;
            //skip = 
            //rb.AddForce(force * velocity * chargeMultiplier, ForceMode.VelocityChange);
            if (!pushed || data.canStack)
            {
                pushed = true;
                float chargeMultiplier = chargeLevel / (float)totalCharges;
                rb.velocity = force * velocity * chargeMultiplier;
            }
            else
            {
                return false;
            }
            if (data != null && data.maxSpeed != 0)
            {
                canReturn = data.returning;
            }
            //CapSpeed();
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

    public virtual void Constraints()
    {
        if(data != null)
        {
            Vector3 constraints = data.constraints;
            if(constraints.x == 1)
            {
                rb.constraints |= RigidbodyConstraints.FreezePositionX;
            }
            if (constraints.y == 1)
            {
                rb.constraints |= RigidbodyConstraints.FreezePositionY;
            }
            if (constraints.z == 1)
            {
                rb.constraints |= RigidbodyConstraints.FreezePositionZ;
            }
        }
    }

    public virtual void Activate()
    {
        
    }

    public virtual void CapSpeed()
    {
        if (data != null && data.maxSpeed != 0 && data.drag != 0)
        {
            if (totalVelocity == 0)
            {
                Debug.Log("start");
                signs = new Vector3(Mathf.Sign(rb.velocity.x), Mathf.Sign(rb.velocity.y), Mathf.Sign(rb.velocity.z));
                pushDirection = rb.velocity.normalized;
                if (canReturn)
                {
                    if (rb.velocity.magnitude > data.maxSpeed)
                    {
                        Debug.Log("fast");
                        totalVelocity = rb.velocity.magnitude;
                        queuedVelocity = rb.velocity.magnitude - data.maxSpeed;
                    }
                    else
                    {
                        Debug.Log("slow");
                        totalVelocity = rb.velocity.magnitude;
                    }

                }
                else if (!canReturn)
                {
                    Debug.Log("no return");
                    totalVelocity = -1;
                }
            }
            if (queuedVelocity <= 0)
            {
                if (rb.velocity.magnitude > 0)
                {
                    Debug.Log("slowing");
                    rb.velocity = new Vector3(
                            signs.x * Mathf.Max(Mathf.Abs(rb.velocity.x) - data.maxSpeed * data.drag * Time.deltaTime,0),
                            signs.y * Mathf.Max(Mathf.Abs(rb.velocity.y) - data.maxSpeed * data.drag * Time.deltaTime,0),
                            signs.z * Mathf.Max(Mathf.Abs(rb.velocity.z) - data.maxSpeed * data.drag * Time.deltaTime,0)
                        );
                }
                else
                {
                    if (canReturn)
                    {
                        Debug.Log("pivot");
                        queuedVelocity = Mathf.Max(totalVelocity - data.maxSpeed,0);
                        if(queuedVelocity <= 0)
                        {
                            rb.velocity = pushDirection * -totalVelocity;
                        }
                        else
                        {
                            rb.velocity = pushDirection * -data.maxSpeed;
                        }
                        pushDirection = rb.velocity.normalized;
                        signs = new Vector3(Mathf.Sign(rb.velocity.x), Mathf.Sign(rb.velocity.y), Mathf.Sign(rb.velocity.z));
                        canReturn = false;
                    }
                    else
                    {
                        Debug.Log("stopping");
                        pushed = false;
                        rb.isKinematic = true;
                    }
                }
            }
            else
            {
                Debug.Log("moving");
                queuedVelocity -= data.drag * Time.deltaTime;
                rb.velocity = pushDirection * data.maxSpeed;
            }
            //if (rb.velocity.magnitude <= data.maxSpeed)
            //{
            //    if (queuedVelocity > 0 && rb.velocity.magnitude != data.maxSpeed)
            //    {
            //        float usedVelocity = data.maxSpeed - Mathf.Abs(rb.velocity.magnitude);
            //        queuedVelocity += rb.velocity.magnitude - data.maxSpeed;
            //        rb.velocity = rb.velocity.normalized * data.maxSpeed;
            //    }
            //    else if(queuedVelocity <= 0 && canReturn)
            //    {
            //        queuedVelocity = totalVelocity;
            //        rb.velocity *= -1;
            //        canReturn = false;
            //    }
            //}
            //else
            //{
            //    totalVelocity = queuedVelocity = rb.velocity.magnitude - data.maxSpeed;
            //    rb.velocity = rb.velocity.normalized * data.maxSpeed;
            //}
        }

    }

    void FixedUpdate()
    {
        if(pushed)
        {
            CapSpeed();
            //if(skip)
            //{
            //    skip = false;
            //}
            //else
            //{
            //    CapSpeed();
            //}
        }
    }
}
