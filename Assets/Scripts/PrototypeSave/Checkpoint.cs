using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            GameObject hit = contact.otherCollider.gameObject;
            if (hit.tag == "Player")
            {
                PlayerPrefs.SetInt("CheckpointSaved", 1);
                PlayerPrefs.SetFloat("TempX", transform.position.x);
                PlayerPrefs.SetFloat("TempY", transform.position.y+GetComponent<Collider>().bounds.size.y/2+hit.GetComponent<Collider>().bounds.size.y / 2);
                PlayerPrefs.SetFloat("TempZ", transform.position.z);
                Debug.Log(transform.position.y + GetComponent<Collider>().bounds.size.y / 2);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}