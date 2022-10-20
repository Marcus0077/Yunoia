using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiOrbsSticky : Pushable
{
    [SerializeField]
    GameObject orb, player;
    Vector3 relativePosition;
    bool attached = false;
    [SerializeField]
    float timeToTravel = 1;
    WaitForSecondsRealtime waitForSecondsRealtime;
    // Start is called before the first frame update
    void Start()
    {
        
        player = Object.FindObjectsOfType<AbilityPush>()[0].gameObject;
        relativePosition = transform.InverseTransformPoint(player.transform.position);
    }

    public override void Pushed(Vector3 force, int chargeLevel, int totalCharges)
    {
        attached = false;
        base.Pushed(force, chargeLevel,totalCharges);
        StartCoroutine(PushTimer());
    }

    public IEnumerator PushTimer()
    {
        if (waitForSecondsRealtime == null)
        {
            waitForSecondsRealtime = new WaitForSecondsRealtime(timeToTravel);
        }
        else
        {
            waitForSecondsRealtime.waitTime = timeToTravel;
        }
        yield return waitForSecondsRealtime;
        AiOrbs newOrb = Instantiate(orb, transform.position, transform.rotation).GetComponent<AiOrbs>();
        Destroy(gameObject);
    }

    public override void Awake()
    {
        base.Awake();
        attached = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (attached)
        {
            transform.position = player.transform.position - relativePosition;
        }
        else
        {
            rb.velocity = rb.velocity / 1.005f;
        }
    }
}
