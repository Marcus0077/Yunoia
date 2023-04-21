using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWon : MonoBehaviour
{
    public FadeText ft;

    void OnTriggerEnter(Collider other)
    {
        ft.LevelFinished();
        Debug.Log("TRIGGERED");
    }
}
