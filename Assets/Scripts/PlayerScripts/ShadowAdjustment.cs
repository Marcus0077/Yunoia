using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowAdjustment : MonoBehaviour
{
    DecalProjector shadow;
    public float maxDistance;
    // Start is called before the first frame update
    void Start()
    {
        shadow = GetComponent<DecalProjector>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance, ~(1 << 3)))
        {
            //Debug.Log((maxDistance - hit.distance) / maxDistance);
            //shadow.pivot = new Vector3(0, 0, hit.distance);
            shadow.size = new Vector3((maxDistance - hit.distance) / maxDistance, (maxDistance - hit.distance) / maxDistance, shadow.size.z);
        }
    }
}
