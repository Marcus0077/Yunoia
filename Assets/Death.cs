using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("VSDenial");
        }
        else if (other.CompareTag("Clone"))
        {
            GameObject.FindObjectOfType<ExitClone>().despawnClone = true;
        }
    }
}
