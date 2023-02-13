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

        // Iterates through all colliders in the radius of the player
        foreach (var hitCollider in hitColliders)
        {
            if (((LayerMask.GetMask("Grapple") & 1 << hitCollider.gameObject.layer) > 0) || ((LayerMask.GetMask("GrappleYank") & 1 << hitCollider.gameObject.layer) > 0))
            {
                // Only considers colliders found above the player
                if (hitCollider.bounds.center.y > playerRB.worldCenterOfMass.y)
                {
                    if (closestHook == null)
                    {
                        closestHook = hitCollider;
                    }

                    // Sets a new closestHook if a 'Grapple' or 'GrappleYank' point is closer than the current closestHook
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
