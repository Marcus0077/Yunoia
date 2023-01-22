using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceMod : MonoBehaviour
{
    [SerializeField]
    PhysicMaterial bounceMat;
    [SerializeField]
    float startBounce;
    [SerializeField]
    int maxMinions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (maxMinions == 0)
        {
            return;
        }
        BasicMovement player = collider.gameObject.GetComponent<BasicMovement>();
        if (player != null)
        {
            bounceMat.bounciness = startBounce + ((1 - startBounce) / maxMinions * player.attachedMinionCount);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
