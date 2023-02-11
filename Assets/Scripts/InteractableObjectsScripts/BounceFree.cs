using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adds upwards force to a bounce pad ignoring gravity physics
public class BounceFree : MonoBehaviour
{
    [SerializeField]
    float additiveForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        BasicMovement player = col.gameObject.GetComponent<BasicMovement>(); // not actually only player, includes clone
        Debug.Log(player);
        if (player != null)
        {
            player.playerRB.AddForce(transform.up * additiveForce);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
