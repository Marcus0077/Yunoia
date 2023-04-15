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
            else if (collider.CompareTag("Ground"))
            {
                //Destroy(gameObject);
                //Start explosion effect:
                GameObject go = Instantiate(collisionParticles, gameObject.transform.position, gameObject.transform.rotation);
                go.GetComponent<ErisAttack>().controller = controller;
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
