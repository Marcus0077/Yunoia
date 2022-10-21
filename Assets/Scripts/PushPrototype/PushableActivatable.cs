using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableActivatable : Pushable
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Pushed(Vector3 force, int chargeLevel, int totalCharges)
    {
        base.Pushed(force, chargeLevel, totalCharges);
        GetComponent<Renderer>().material.color = Color.blue;
        GetComponent<Collider>().enabled = false;
    }

    public override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
