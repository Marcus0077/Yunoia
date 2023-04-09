using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GemstoneIlluminator : MonoBehaviour
{

    public GameObject crystalPlatform;
    public GameObject purifierVfx;
    private bool activated = false;

    private void Start()
    {
        //this.GameObject().GetComponent<Light>().intensity = 0;
        purifierVfx.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (crystalPlatform.GetComponent<PlantDestroyer>().thisCrystalComplete == 1 && !activated)
        {
            activated = true;
            //this.GameObject().GetComponent<Light>().intensity = 10;
            purifierVfx.SetActive(true);
        }
    }
}
