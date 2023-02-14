using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddCloneTimerPickup : MonoBehaviour
{
    public float timeToAdd;
    // Start is called before the first frame update
    void Awake()
    {
        timeToAdd = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GameObject().CompareTag("Clone"))
        {
            Destroy(this.GameObject());
            other.GameObject().GetComponent<ExitClone>().cloneActiveTimer += timeToAdd;
        }
    }
}
