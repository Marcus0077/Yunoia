using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Base class of all pushable objects
[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{
    protected Rigidbody rb;
    // Velocity is how hard a push affects the object, max speed is how fast the object can travel iff drag is also defined (in data)
    protected float velocity = 1, maxSpeed;
    // What stage of push is required for push to work (currently not implemented in game)
    protected float reqChargeLevel = 1;
    // How many times object was pushed, used for breakable wall demo
    public int pushCounter = 0;
    // How much force is stored
    public float queuedVelocity;
    public float totalVelocity = 0;
    public bool canReturn, canPush = true, capSpeed = true;
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
            maxSpeed = data.maxSpeed;
        }
    }

    // Used for minions when they used to be navmesh agents
    IEnumerator EnableNav()
    {
        yield return new WaitForSeconds(.5f);
        rb.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        //GetComponent<NavMeshAgent>().enabled = true;
    }

    // The push function
    public virtual bool Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        // if data determines that speed capping (maxSpeed) is not applicable anymore, turn it off
        if(!capSpeed)
        {
            //rb.isKinematic = true;
            //rb.velocity = Vector3.zero;
            //rb.isKinematic = false;
            maxSpeed = 0;
            //return false;
        }
        // only push if charge level requirement is met (not used, typically always true)
        if(chargeLevel >= reqChargeLevel)
        {
            if(GetComponent<NavMeshAgent>() != null)
            {
                //GetComponent<NavMeshAgent>().enabled = false;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionX & ~RigidbodyConstraints.FreezePositionY & ~RigidbodyConstraints.FreezePositionZ;
                StartCoroutine(EnableNav());
            }
            // Stop object from moving in any other direction besides specified direction
            Constraints();
            totalVelocity = 0;
            // Allow object to move using physics
            if(velocity > 0)
            {
                rb.isKinematic = false;
            }
            // Check if object is still in process of being pushed and pushes are not stackable
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
            // Returning is only allowed for speed capped objects
            if (data != null && data.maxSpeed != 0)
            {
                canReturn = data.returning;
            }
            // Run any activateable functions inside data
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

    // Stop object from moving in any other direction besides specified direction in data
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

    // Caps speed of pushed object using drag; using same formulas as unity's physics system but includes queued velocity (i.e. springs but capped speed)
    public virtual void CapSpeed()
    {
        if (data != null && maxSpeed != 0 && data.drag != 0)
        {
            // Change the direction of movement once acceleration should change signs if the object can return
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
            // if object has no more potential force, continue moving until (fake) friction should stop the object
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
                    // however, returns with same force used to move in order to return to same location
                    if (canReturn)
                    {
                        queuedVelocity = Mathf.Max(totalVelocity - data.maxSpeed,0);
                        if(returning == null)
                            returning = StartCoroutine(DelayReturn());
                    }
                    // unless object should not return
                    else
                    {
                        pushed = false;
                        rb.isKinematic = true;
                    }
                }
            }
            else
            {
                // reduce velocity by drag until reaching 0 velocty
                queuedVelocity -= data.maxSpeed * data.drag * Time.deltaTime;
                rb.velocity = pushDirection * data.maxSpeed;
            }
        }
    }

    // on hitting an option return with same force if it is a returning object
    void OnCollisionEnter(Collision collision)
    {
        if (!(data != null && maxSpeed != 0 && data.drag != 0))
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

    public PushableFunction Data()
    {
        return data;
    }

    public void Activate()
    {

    }

    // Keeps object stationary for data.delay seconds before returning
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

    // Cap speed while object is pushed
    void FixedUpdate()
    {
        if(pushed)
        {
            CapSpeed();
        }
    }
}
