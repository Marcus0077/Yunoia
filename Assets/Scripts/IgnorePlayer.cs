using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayer : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(player.GetComponent<Collider>(),GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
