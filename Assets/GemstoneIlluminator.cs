using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GemstoneIlluminator : MonoBehaviour
{

    public GameObject crystalPlatform;
    private bool activated = false;

    private void Start()
    {
        this.GameObject().GetComponent<Light>().intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (crystalPlatform.GetComponent<PlantDestroyer>().thisCrystalComplete == 1 && !activated)
        {
            activated = true;
            this.GameObject().GetComponent<Light>().intensity = 10;
        }
    }
}
