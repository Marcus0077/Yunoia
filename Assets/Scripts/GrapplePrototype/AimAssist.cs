using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    [SerializeField] BasicMovement player;
    [SerializeField] Transform shootTransform;
    [SerializeField] Rigidbody playerRB;

    Collider closestHook;

    public Collider HookDetection(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        
        foreach (var hitCollider in hitColliders)
        {
            if (((LayerMask.GetMask("Grapple") & 1 << hitCollider.gameObject.layer) > 0) || ((LayerMask.GetMask("GrappleYank") & 1 << hitCollider.gameObject.layer) > 0))
            {
                if (hitCollider.bounds.center.y > playerRB.worldCenterOfMass.y)
                {
                    if (closestHook == null)
                    {
                        closestHook = hitCollider;
                    }

                    if (Vector3.Distance(center, hitCollider.transform.position) <= Vector3.Distance(center, closestHook.transform.position))
                    {
                        closestHook = hitCollider;
                    }
                    else if (playerRB.worldCenterOfMass.y > closestHook.bounds.center.y)
                    {
                        closestHook = hitCollider;
                    }
                }
            }
        }
        
        return closestHook;
    }
}
