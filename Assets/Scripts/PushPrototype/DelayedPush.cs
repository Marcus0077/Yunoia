using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedPush : MonoBehaviour
{
    public GameObject pusher;
    public float delayTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedByTime());
    }

    IEnumerator DelayedByTime()
    {
        yield return new WaitForSeconds(delayTime);
        pusher.GetComponent<AbilityPush>().PushTargets();
    }

    //public void OnParticleSystemStopped()
    //{
    //    Debug.Log("Works");
    //    pusher.GetComponent<AbilityPush>().PushTargets();
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
