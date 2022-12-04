using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    [SerializeField] BasicMovement player;

    Collider closestHook;

    public Collider HookDetection(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        
        foreach (var hitCollider in hitColliders)
        {
            if (((LayerMask.GetMask("Grapple") & 1 << hitCollider.gameObject.layer) > 0) || ((LayerMask.GetMask("GrappleYank") & 1 << hitCollider.gameObject.layer) > 0))
            {
                if (closestHook == null)
                {
                    closestHook = hitCollider;
                }

                if (Vector3.Distance(center, hitCollider.transform.position) <= Vector3.Distance(center, closestHook.transform.position))
                {
                    closestHook = hitCollider;
                }
            }
        }
        
        return closestHook;
    }
}
