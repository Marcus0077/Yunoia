using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] float pullSpeed = 0.5f;
    [SerializeField] float stopDistance = 2.5f;
    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform shootTransform;
    [SerializeField] float hookLife;
    [SerializeField] float maxHookLife;
    [SerializeField] Rigidbody playerRB;
    [SerializeField] float changePerSecond;

    Hook hook;
    bool pulling;
    Rigidbody rigid;

    PlayerControls grappleControls;
    private InputAction shootHook;
    private InputAction cancelHook;
    private InputAction extendGrapple;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        pulling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hook == null && shootHook.IsPressed())
        {
            StopAllCoroutines();
            pulling = false;
            hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
            hook.Initialize(this, shootTransform);
            StartCoroutine(DestroyHookAfterLifetime());
        }
        else if (hook != null && cancelHook.IsPressed())
        {
            DestroyHook();
        }

        if (!pulling || hook == null)
        {
            return;
        }

        if (Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)
        {
            DestroyHook();
        }
        else
        {
            rigid.AddForce((hook.transform.position - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);
        }

        if (extendGrapple.IsPressed() && pulling == true)
        {
            playerRB.AddForce(Physics.gravity * 1.2f * playerRB.mass);
        }

        if (playerRB.position.y > hook.transform.position.y)
        {
            DestroyHook();
        }
    }

    public void StartPull()
    {
        pulling = true;
    }

    public void DestroyHook()
    {
        if (hook == null)
        {
            return;
        }

        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(0.4f);

        if (pulling == true)
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
