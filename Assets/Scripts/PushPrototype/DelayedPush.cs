using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedPush : MonoBehaviour
{
    public GameObject pusher;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnParticleSystemStopped()
    {
        Debug.Log("Works");
        pusher.GetComponent<AbilityPush>().PushTargets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
