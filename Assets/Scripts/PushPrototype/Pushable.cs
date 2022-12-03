using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    Coroutine returning;

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

    IEnumerator EnableNav()
    {
        yield return new WaitForSeconds(.5f);
        rb.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        //GetComponent<NavMeshAgent>().enabled = true;
    }

    public virtual bool Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        if(chargeLevel >= reqChargeLevel)
        {
            if(GetComponent<NavMeshAgent>() != null)
            {
                //GetComponent<NavMeshAgent>().enabled = false;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionX & ~RigidbodyConstraints.FreezePositionY & ~RigidbodyConstraints.FreezePositionZ;
                StartCoroutine(EnableNav());
            }
            Constraints();
            totalVelocity = 0;
            if(velocity > 0)
            {
                rb.isKinematic = false;
            }
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
                signs = new Vector3(Mathf.Sign(rb.velocity.x), Mathf.Sign(rb.velocity.y), Mathf.Sign(rb.velocity.z));
                pushDirection = rb.velocity.normalized;
                if (canReturn)
                {
                    if (rb.velocity.magnitude > data.maxSpeed)
                    {
                        totalVelocity = rb.velocity.magnitude;
                        queuedVelocity = rb.velocity.magnitude - data.maxSpeed;
                    }
                    else
                    {
                        totalVelocity = rb.velocity.magnitude;
                    }

                }
                else if (!canReturn)
                {
                    totalVelocity = -1;
                }
            }
            if (queuedVelocity <= 0)
            {
                if (rb.velocity.magnitude > 0.05f)
                {
                    rb.velocity = new Vector3(
                            signs.x * Mathf.Max(Mathf.Abs(rb.velocity.x) - Mathf.Abs(rb.velocity.x) * data.drag * Time.deltaTime,0),
                            signs.y * Mathf.Max(Mathf.Abs(rb.velocity.y) - Mathf.Abs(rb.velocity.y) * data.drag * Time.deltaTime,0),
                            signs.z * Mathf.Max(Mathf.Abs(rb.velocity.z) - Mathf.Abs(rb.velocity.z) * data.drag * Time.deltaTime,0)
                        );
                }
                else
                {
                    if (canReturn)
                    {
                        queuedVelocity = Mathf.Max(totalVelocity - data.maxSpeed,0);
                        if(returning == null)
                            returning = StartCoroutine(DelayReturn());
                    }
                    else
                    {
                        pushed = false;
                        rb.isKinematic = true;
                    }
                }
            }
            else
            {
                queuedVelocity -= data.maxSpeed * data.drag * Time.deltaTime;
                rb.velocity = pushDirection * data.maxSpeed;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!(data != null && data.maxSpeed != 0 && data.drag != 0))
        {
            return;
        }
        foreach (ContactPoint contact in collision.contacts)
        {
            if(contact.otherCollider.gameObject.tag != "Player")
            {
                totalVelocity -= (Mathf.Max(0, queuedVelocity) + collision.relativeVelocity.magnitude);
                queuedVelocity = 0;
                rb.velocity = Vector3.zero;
                rb.Sleep();
                //signs = new Vector3(Mathf.Sign(rb.velocity.x), Mathf.Sign(rb.velocity.y), Mathf.Sign(rb.velocity.z));
                //pushDirection = rb.velocity.normalized;
                //canReturn = false;
                return;
            }
        }
    }

    IEnumerator DelayReturn()
    {
        yield return new WaitForSeconds(data.delay);
        if (queuedVelocity <= 0)
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
        returning = null;
    }

    void FixedUpdate()
    {
        if(pushed)
        {
            CapSpeed();
        }
    }
}
