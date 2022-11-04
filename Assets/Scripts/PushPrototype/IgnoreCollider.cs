using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollider : MonoBehaviour
{
    [SerializeField]
    GameObject other;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(other.GetComponent<Collider>(),GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
