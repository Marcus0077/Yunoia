using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] float pullSpeed = 0.75f;
    //[SerializeField] float horizontalPullSpeed = 5.0f;
    [SerializeField] float stopDistance = 2.5f;
    //[SerializeField] float taughtDistance = 5.0f;
    //[SerializeField] float horizontalYankDistance = 3.0f;
    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform shootTransform;
    [SerializeField] float hookLife;
    [SerializeField] float maxHookLife;
    [SerializeField] Rigidbody playerRB;
    [SerializeField] float changePerSecond;

    Hook hook;
    bool grappleActive;
    bool ready = true;
    Rigidbody rigid;

    PlayerControls grappleControls;
    public InputAction shootHook;
    public InputAction cancelHook;
    public InputAction extendGrapple;
    
    // Additions from Will :)
    private bool releasedHook = false; // input testing

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        grappleActive = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hook == null && shootHook.IsPressed() && ready == true)
        {
            hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
            hook.Initialize(this, shootTransform);
            StartCoroutine(DestroyHookAfterLifetime());
        }
        // Old
        else if (hook != null && (cancelHook.IsPressed() || releasedHook)) // input testing
        {
            DestroyHook(); 
            releasedHook = false; // input testing
        }

        if (!grappleActive || hook == null)
        {
            return;
        }

        // Old
        if (Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)
        {
            DestroyHook();
        }
        else
        {
            rigid.AddForce((hook.transform.position - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);
        }

        // Updated
        /*if (Vector3.Distance(transform.position, hook.transform.position) <= taughtDistance)
        {
            return;
        }
        else
        {
            rigid.AddForce((hook.transform.position - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);
        }*/

        /*if (Math.Abs(transform.position.x - hook.transform.position.x) <= horizontalYankDistance)
        {
            return;
        }
        else
        {
            rigid.AddRelativeForce(rigid.transform.forward);
        }*/

        // Old
        if (extendGrapple.IsPressed() && grappleActive == true)
        {
            playerRB.AddForce(Physics.gravity * 7.0f * playerRB.mass);
        }

        if (hook != null && (playerRB.position.y > hook.transform.position.y))
        {
            DestroyHook();
        }

        if (!shootHook.IsPressed() && hook != null)
        {
            StopAllCoroutines();
            DestroyHook();
        }
    }

    public void StartGrapple()
    {
        ready = false;
        grappleActive = true;
    }

    public void DestroyHook()
    {
        if (hook == null)
        {
            return;
        }

        ready = false;
        grappleActive = false;
        Destroy(hook.gameObject);
        hook = null;
        StartCoroutine(GrappleCooldown());
    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(0.4f);

        if (grappleActive == true)
        {
            StartCoroutine(ExtendLifetime());
        }
        else
        {
            DestroyHook();
        }
    }

    private IEnumerator ExtendLifetime()
    {
        yield return new WaitForSeconds(4f);

        DestroyHook();
    }

    public void StartPull()
    {
        
    }

    private IEnumerator GrappleCooldown()
    {
        yield return new WaitForSeconds(1.0f);

        ready = true;
    }

    // Enable input action map controls.
    private void OnEnable()
    {
        grappleControls = new PlayerControls();

        shootHook = grappleControls.Grapple.ShootHook;
        shootHook.Enable();

        cancelHook = grappleControls.Grapple.CancelHook;
        cancelHook.Enable();

        extendGrapple = grappleControls.Grapple.ExtendGrapple;
        extendGrapple.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        shootHook.Disable();
        cancelHook.Disable();
        extendGrapple.Disable();
    }
}
