using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBreakable : PushableAnimatable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        base.Pushed(force, chargeLevel, totalCharges, pusher);
    }

    public override IEnumerator DisplayAnimation()
    {
        anim.SetBool("break", true);
        yield return WaitForAnimation("break"); // have real name
        Destroy(gameObject);
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
