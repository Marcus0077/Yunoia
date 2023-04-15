using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleport : MonoBehaviour
{
    public Transform teleportTarget;
    public Rigidbody player;

    public ErisAttackController controller;

    public void OnTriggerEnter(Collider collider)
    {
        player.transform.position = teleportTarget.transform.position;
        controller.StopAllCoroutines();
        controller.inBossRoom = true;
        controller.canAttack = true;
    }
}
