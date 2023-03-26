using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErisAttackStairs : MonoBehaviour
{
    public ErisAttackController controller;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            controller.hit = true;
            Destroy(gameObject);
        }
        else if (collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
