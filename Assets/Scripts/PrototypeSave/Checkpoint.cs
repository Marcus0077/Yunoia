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
                DataManager.gameData.checkpointed = true;
                DataManager.gameData.level = GameObject.FindWithTag("StateDrivenCam").GetComponent<Animator>().GetInteger("roomNum");
                Debug.Log(DataManager.gameData.level);
                DataManager.gameData.position = transform.position + (Vector3.up * (GetComponent<Collider>().bounds.size.y / 2 + hit.GetComponent<Collider>().bounds.size.y / 2));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}