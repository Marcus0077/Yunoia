using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{
    protected Rigidbody rb;
    [SerializeField]
    protected float velocity = 1;
    void Start()
    {
        
    }

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Pushed(Vector3 force, int chargeLevel, int totalCharges)
    {
        float chargeMultiplier = chargeLevel / (float)totalCharges;
        rb.AddForce(force * velocity * chargeMultiplier, ForceMode.VelocityChange);
    }
}
