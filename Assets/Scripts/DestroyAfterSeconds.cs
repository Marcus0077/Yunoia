using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DestroyThis());
        }
    }

    private IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this);
    }
}
