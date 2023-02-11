using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiOrbsSticky : Pushable
{
    [SerializeField]
    GameObject orb;
    public GameObject player;
    Vector3 relativePosition;
    float relativeRot;
    bool attached = false;
    int pushedCount = 0; // Making sure the last push's timeToTravel has elapsed before allowing orb to move again (current push index is equal to last push index)
    [SerializeField] // How long a minion should be stunned
    float timeToTravel = 1;

    [SerializeField] AudioSource attachSound;
    [SerializeField] AudioSource detachSound;

    //WaitForSecondsRealtime waitForSecondsRealtime;
    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
            player = Object.FindObjectsOfType<AbilityPush>()[0].gameObject;
        // Find position the orb should be at relative to player position (attached to player model at its touching point)
        Vector3 tempOriginalScale = transform.localScale;
        transform.localScale = Vector3.one;
        //relativePosition = player.transform.InverseTransformPoint(transform.position);
        relativePosition = player.transform.position - transform.position;
        transform.localScale = tempOriginalScale;
        relativeRot = player.transform.eulerAngles.y;
        if (player.GetComponent<BasicMovement>())
        {
            player.GetComponent<BasicMovement>().AddMinion(1);
        }
        else if (player.GetComponent<BasicMovement>())
        {
            player.GetComponent<BasicMovement>().AddMinion(1);
        }
    }

    public override bool Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        if(base.Pushed(force, chargeLevel, totalCharges, pusher))
        {
            if (player.GetComponent<BasicMovement>() && attached)
            {
                player.GetComponent<BasicMovement>().AddMinion(-1);
            }
            else if (player.GetComponent<BasicMovement>() && attached)
            {
                player.GetComponent<BasicMovement>().AddMinion(-1);
            }
            attached = false;

            detachSound.Play();

            StartCoroutine(PushTimer(timeToTravel));
            return true;
        }
        else
        {
            return false;
        }
    }

    // Swap to orb object (moving orb) after specific float time has elapsed and no other pushes are underway
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
        pushedCount++;
        int pushIndex = pushedCount;
        yield return new WaitForSeconds(time);
        if (pushedCount == pushIndex)
        {
            AiOrbs newOrb = Instantiate(orb, transform.position, transform.rotation).GetComponent<AiOrbs>();
            newOrb.GetComponent<AiOrbs>().player = player;
            Destroy(gameObject);
        }
    }

    public override void Awake()
    {
        base.Awake();
        attached = true;

        attachSound.Play();
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
                transform.position = player.transform.position - Quaternion.Euler(0, player.transform.eulerAngles.y - relativeRot, 0) * relativePosition;
            }
        }
        else
        {
            rb.velocity = rb.velocity / 1.005f; // slows momentum from push gradually
        }
    }
}
