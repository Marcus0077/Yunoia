using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collision) 
    {
        // foreach (ContactPoint contact in collision.contacts)
        // {
            // GameObject hit = contact.otherCollider.gameObject;

            GameObject hit = collision.GameObject();
            
            if (hit.tag == "Player")
            {
                DataManager.gameData.checkpointed = true;
                DataManager.gameData.level = GameObject.FindWithTag("StateDrivenCam").GetComponent<Animator>().GetInteger("roomNum");
                Debug.Log(DataManager.gameData.level);
                DataManager.gameData.position = transform.position + Vector3.right; //(Vector3.right * (GetComponent<Collider>().bounds.size.y / 2 + hit.GetComponent<Collider>().bounds.size.y / 2));
                GameObject.FindWithTag("Save Icon").GetComponent<Animator>().SetTrigger("Saving");
                
                Destroy(this);
            }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}