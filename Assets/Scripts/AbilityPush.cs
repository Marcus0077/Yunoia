using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPush : MonoBehaviour
{
    [SerializeField]
    int maxChargeLevel = 1, minPush = 1;
    [SerializeField]
    float chargeSpeed = 1, rangeMod = 1, cooldown = 1;
    float chargeTime, timer = -1;
    bool ableToPush;
    // Start is called before the first frame update
    void Start()
    {

    }

    void PushTargets(float range)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            Pushable pushedObj = hitCollider.GetComponent<Pushable>();
            if (pushedObj != null)
            {
                Vector3 direction = pushedObj.transform.position - transform.position;
                direction = new Vector3(direction.x, 0, direction.z).normalized;
                float distance = Vector3.Distance(pushedObj.transform.position, transform.position);
                float proximityMultiplier = range / distance;
                //float chargeMultiplier = (range + 1 - minPush) / (float)(maxChargeLevel + 1);
                pushedObj.Pushed(proximityMultiplier * direction, (int)range + 1 - minPush, maxChargeLevel + 1);
            }
                
        }
        RenderVolume(range*2);
    }

    void RenderVolume(float radius)
    {
        Transform shape;
        shape = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        Destroy(shape.GetComponent<Collider>());
        Vector3 scale = new Vector3();
        scale.x = radius;
        scale.y = radius;
        scale.z = radius;
        shape.localScale = scale;
        shape.position = transform.position;
        shape.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse"); 
        shape.GetComponent<Renderer>().material.color = new Color(1, 1, 1, .5f);
        shape.GetComponent<Renderer>().enabled = true;
        StartCoroutine(EraseRender(shape)); // erase after 1 second
    }

    private IEnumerator EraseRender(Transform shape)
    {
        yield return new WaitForSeconds(1);
        Destroy(shape.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if(Time.time - timer < cooldown)
            {
                ableToPush = false;
            } else
            {
                timer = Time.time;
                ableToPush = true;
            }
            //start charging with animation
        }
        if (ableToPush && Input.GetKeyUp("space"))
        {
            chargeTime = (Time.time - timer)*chargeSpeed+minPush;
            //start animation
            PushTargets(Mathf.Clamp((int)chargeTime, minPush, maxChargeLevel+minPush) * rangeMod);
        }
    }
}
