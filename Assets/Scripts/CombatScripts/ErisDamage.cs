using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErisDamage : MonoBehaviour
{
    public ErisBoss boss;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            boss.ErisHurt();
            Destroy(gameObject);
        }
    }
}
