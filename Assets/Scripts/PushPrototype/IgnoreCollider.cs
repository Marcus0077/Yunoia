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
        Collider[] colliders = transform.GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            Physics.IgnoreCollision(other.GetComponent<Collider>(), col);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
