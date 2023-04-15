using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToBossRoom : MonoBehaviour
{
    public ErisAttackController controller;

    public void OnTriggerEnter (Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            controller.inBossRoom = true;
        }
    }
}
