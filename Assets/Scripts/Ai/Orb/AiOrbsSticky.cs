using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiOrbsSticky : Pushable
{
    [SerializeField]
    GameObject orb;
    public GameObject player;
    Vector3 relativePosition;
    bool attached = false;
    [SerializeField]
    float timeToTravel = 1;
    //WaitForSecondsRealtime waitForSecondsRealtime;
    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
            player = Object.FindObjectsOfType<AbilityPush>()[0].gameObject;
        Vector3 tempOriginalScale = transform.localScale;
        transform.localScale = Vector3.one;
        relativePosition = transform.InverseTransformPoint(player.transform.position);
        transform.localScale = tempOriginalScale;
        if (player.GetComponent<BasicMovementPlayer>())
        {
            player.GetComponent<BasicMovementPlayer>().AddMinion(1);
        }
        else
        {
            player.GetComponent<BasicMovementClone>().AddMinion(1);
        }
    }

    public override void Pushed(Vector3 force, int chargeLevel, int totalCharges)
    {
        if (player.GetComponent<BasicMovementPlayer>() && attached)
        {
            player.GetComponent<BasicMovementPlayer>().AddMinion(-1);
        }
        else if(attached)
        {
            player.GetComponent<BasicMovementClone>().AddMinion(-1);
        }
        attached = false;
        base.Pushed(force, chargeLevel,totalCharges);
        StartCoroutine(PushTimer(timeToTravel));
    }

    public IEnumerator PushTimer(float time)
    {
        //if (waitForSecondsRealtime == null)
        //{
        //    waitForSecondsRealtime = new WaitForSecondsRealtime(timeToTravel);
        //}
        //else
        //{
        //    waitForSecondsRealtime.waitTime = timeToTravel;
        //}
        //yield return waitForSecondsRealtime;
        yield return new WaitForSeconds(time);
        AiOrbs newOrb = Instantiate(orb, transform.position, transform.rotation).GetComponent<AiOrbs>();
        newOrb.GetComponent<AiOrbs>().player = player;
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
            if(player == null)
            {
                attached = false;
                StartCoroutine(PushTimer(0));
            }
            else
            {
                transform.position = player.transform.position - relativePosition;
            }
        }
        else
        {
            rb.velocity = rb.velocity / 1.005f;
        }
    }
}
