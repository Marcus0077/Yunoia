using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErisAttack : MonoBehaviour
{
    public bool secondaryEffect;
    public ErisAttackController controller;
    public GameObject collisionParticles;

    public void OnTriggerEnter(Collider collider)
    {
        if(!secondaryEffect)
        {
            if (collider.CompareTag("Player"))
            {
                controller.hit = true;
                Destroy(gameObject);
            }
            else if (collider.CompareTag("Ground") || collider.gameObject.layer == LayerMask.NameToLayer("Scale") || collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                //Destroy(gameObject);
                //Start explosion effect:
                if(collisionParticles != null)
                {
                    GameObject go = Instantiate(collisionParticles, gameObject.transform.position, gameObject.transform.rotation);
                    go.GetComponent<ErisAttack>().controller = controller;
                    controller.attackSpawned = go.GetComponent<ErisAttack>();
                }
                Destroy(gameObject);
            }
        }
        else
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("hit");
                controller.hit = true;
                Destroy(gameObject);
            }
        }
    }
}
