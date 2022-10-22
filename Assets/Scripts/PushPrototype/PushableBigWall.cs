using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBigWall : Pushable
{
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Pushed(Vector3 force, int chargeLevel, int totalCharges)
    {
        if(chargeLevel>1)
        {
            base.Pushed(force, chargeLevel, totalCharges);
            //GetComponent<Renderer>().material.color = Color.blue;
            GetComponent<Renderer>().material = mat;
            GetComponent<Collider>().enabled = false;
        }
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
