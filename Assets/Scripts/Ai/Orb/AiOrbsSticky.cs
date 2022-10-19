using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiOrbsSticky : Pushable
{
    [SerializeField]
    GameObject orb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Pushed(Vector3 force, int chargeLevel, int totalCharges)
    {
        base.Pushed(force, chargeLevel, totalCharges);
        transform.parent = null;
        StartCoroutine(SwapOrb());
        //Destroy(gameObject);
    }

    private IEnumerator SwapOrb()
    {
        yield return new WaitForSeconds(1);
        Instantiate(orb, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = rb.velocity / 1.005f;
    }
}
