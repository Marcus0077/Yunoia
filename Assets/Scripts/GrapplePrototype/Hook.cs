using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float hookForce = 10f;

    Grapple grapple;
    Rigidbody rigid;
    LineRenderer lineRenderer;

    public GameObject grappleAttach;

    // Start is called before the first frame update
    public void Initialize(Grapple grapple, Transform shootTransform)
    {
        transform.forward = shootTransform.forward;
        this.grapple = grapple;
        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        rigid.AddForce(transform.forward * hookForce, ForceMode.Impulse);
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
            rigid.useGravity = false;
            rigid.isKinematic = true;

            Instantiate(grappleAttach, transform.position, Quaternion.identity);

            grapple.StartGrapple();
        }

        if ((LayerMask.GetMask("GrapplePull") & 1 << other.gameObject.layer) > 0)
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;
            //grapple.pullable = true;
            
            Instantiate(grappleAttach, transform.position, Quaternion.identity);

            grapple.StartGrapple();
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
