using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDestroyer : MonoBehaviour
{
    [SerializeField]
    GameObject linkedGO;

    void OnDestroy()
    {
        Destroy(linkedGO);
    }
}
