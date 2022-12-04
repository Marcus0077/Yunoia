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

    // Start is called before the first frame update
    public void Initialize(Grapple grapple, Transform shootTransform)
    {
        this.grapple = grapple;
        this.shootTransform = shootTransform;

        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.enabled = false;
        rigid.AddForce(shootTransform.forward * hookForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] positions = new Vector3[]
            {
                transform.position,
                grapple.transform.position
            };
        
        lineRenderer.SetPositions(positions);
    }

    private void OnTriggerEnter(Collider other)
    {
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

        if ((LayerMask.GetMask("Ground") & 1 << other.gameObject.layer) > 0)
        {
            grapple.DestroyHook();
        }

        if ((LayerMask.GetMask("Wall") & 1 << other.gameObject.layer) > 0)
        {
            grapple.DestroyHook();
        }
    }
}
