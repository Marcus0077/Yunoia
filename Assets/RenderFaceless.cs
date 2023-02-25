using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class RenderFaceless : MonoBehaviour
{
    public GameObject[] Facelesses;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var Faceless in Facelesses)
        {
            Faceless.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var Faceless in Facelesses)
        {
            Faceless.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
