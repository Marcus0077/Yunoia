using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableShield : MonoBehaviour
{
    [SerializeField]
    float pushStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Pushable>() != null)
        {
            //other.GetComponent<Rigidbody>().AddForce(pushStrength * (other.gameObject.transform.position - transform.position).normalized);
            other.GetComponent<Pushable>().Pushed(pushStrength * (other.gameObject.transform.position - transform.position).normalized, 1, 1, transform.parent.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
