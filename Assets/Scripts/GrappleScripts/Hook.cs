using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float hookForce = 1.0f;

    Grapple grapple;
    Transform shootTransform;

    float angle = 0.0f;
    Vector3 bestHookCenter;

    Rigidbody rigid;
    LineRenderer lineRenderer;

    public GameObject grappleAttach;

    public void Initialize(Grapple grapple, Transform shootTransform)
    {
        this.grapple = grapple;
        this.shootTransform = shootTransform;

        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.enabled = false;
        rigid.AddForce(shootTransform.forward * hookForce, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        // Movement of the grapple hook after shooting it
        if (grapple.grappleActive)
        {
            //transform.position = bestHookCenter;
        }

        Vector3[] positions = new Vector3[]
            {
                transform.position,
                grapple.transform.position
            };

        // Renders line for the grapple hook's path
        lineRenderer.SetPositions(positions);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Starts grapple activivation when hitting a grapple point on the 'Grapple' layer
        if ((LayerMask.GetMask("Grapple") & 1 << other.gameObject.layer) > 0)
        {
            Debug.Log(other);
            //lineRenderer.enabled = true;
            rigid.useGravity = false;
            rigid.isKinematic = true;

            Instantiate(grappleAttach, transform.position, Quaternion.identity);

            grapple.StartGrapple();
            //GetComponent<Collider>().enabled = false;
        }

        // Starts grappla activivation when hitting a grapple point on the
        // 'Grapple' layer with canYank = true
        if ((LayerMask.GetMask("GrappleYank") & 1 << other.gameObject.layer) > 0)
        {
            Debug.Log(2);
            //lineRenderer.enabled = true;
            rigid.useGravity = false;
            rigid.isKinematic = true;
            grapple.canYank = true;

            Instantiate(grappleAttach, transform.position, Quaternion.identity);

            grapple.StartGrapple();
            //GetComponent<Collider>().enabled = false;
        }

        // Destroys the grapple hook if it collides with an object on the 'Ground' or 'Wall' layers 
        if ((LayerMask.GetMask("Ground") & 1 << other.gameObject.layer) > 0)
        {
            grapple.DestroyHook();
        }
        else if ((LayerMask.GetMask("Wall") & 1 << other.gameObject.layer) > 0)
        {
            grapple.DestroyHook();
        }
    }
}
