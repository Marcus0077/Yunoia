using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour
{
    public GameObject settings;
    WaitForSecondsRealtime waitForSecondsRealtime;
    // Start is called before the first frame update
    void Start()
    {
        settings.SetActive(true);
        StartCoroutine(CloseSettings());
    }

    IEnumerator CloseSettings()
    {
        if (waitForSecondsRealtime == null)
        {
            waitForSecondsRealtime = new WaitForSecondsRealtime(.1f);
        }
        else
        {
            waitForSecondsRealtime.waitTime = .1f;
        }
        yield return waitForSecondsRealtime;
        settings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
