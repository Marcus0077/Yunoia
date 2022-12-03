using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedPush : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnParticleSystemStopped()
    {
        Debug.Log("Works");
        GameObject.FindWithTag("Player").GetComponent<AbilityPush>().PushTargets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
