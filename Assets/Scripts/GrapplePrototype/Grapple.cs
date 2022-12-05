using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [SerializeField] float swingSpeed = 0.3f;
    [SerializeField] float yankSpeedStrong = 0.7f;
    [SerializeField] float yankSpeedWeak = 0.7f;
    [SerializeField] float stopDistance = 2.5f;
    [SerializeField] float taughtDistance = 5.0f;

    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform shootTransform;

    [SerializeField] AimAssist aimAssist;
    public Collider bestHook;
    public Collider playerCollider;
    public Vector3 bestHookCenter;
    public float radius = 12.0f;

    [SerializeField] Rigidbody playerRB;
    [SerializeField] BasicMovement player;
    [SerializeField] float changePerSecond;
    [SerializeField] float grappleCooldown = 1;
    float cdRemaining;

    [SerializeField] float horizontalPullSpeed = 0.5f;

    Hook hook;
    bool grappleActive;
    bool ready = true;
    public bool canYank = false;
    public bool yankReady = false;

    PlayerControls grappleControls;
    public InputAction shootHook;
    public InputAction cancelHook;
    public InputAction extendGrapple;

    [SerializeField] private AudioSource shoot;
    [SerializeField] private AudioSource yank;

    // Additions from Will :)
    private bool releasedHook = false; // input testing

    public GameObject grappleEffect;
    public GameObject grappleEffectDestroy;

    public bool grappleNeedsDeath;

    // Start is called before the first frame update
    void Start()
    {
        grappleNeedsDeath = false;
        grappleActive = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hook == null && !grappleActive)
        {
            bestHook = aimAssist.HookDetection(shootTransform.position, radius);
            bestHookCenter = bestHook.bounds.center;
            shootTransform.LookAt(bestHookCenter);
        }

        if (bestHook != null)
        {
            if (Vector3.Distance(bestHookCenter, shootTransform.position) > radius)
            {
                bestHook = null;
            }
        }

        if (grappleEffectDestroy != null)
        {
            grappleNeedsDeath = true;
        }
        
        if (bestHook != null && hook == null && shootHook.IsPressed() && ready == true)
        {
            hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
            shoot.Play();

            grappleEffectDestroy = Instantiate(grappleEffect, shootTransform.position, this.transform.rotation);

            hook.Initialize(this, shootTransform);
            StartCoroutine(DestroyHookAfterLifetime());
        }
        // Old/Updated
        else if (hook != null && (cancelHook.IsPressed() || releasedHook)) // input testing
        {
            DestroyHook(); 
            releasedHook = false; // input testing
        }
        
        if (!grappleActive || hook == null)
        {
            return;
        }
        
        // Updated - Press Shoot again to 'yank' if connected to 'GrappleYank' points
        if (canYank && yankReady)
        {
            if (grappleActive && shootHook.IsPressed() && player.isGrounded)
            {
                playerRB.AddForce((hook.transform.position - transform.position) * yankSpeedStrong, ForceMode.Impulse);
                yank.Play();
            }
        }
        /*else if (!yankHook)
        {
            if (grappleActive && shootHook.IsPressed() && player.isGrounded)
            {
                playerRB.AddForce((hook.transform.position - transform.position) * yankSpeedWeak, ForceMode.Impulse);
                yank.Play();
            }
        }*/
        
        if (Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)
        {
            DestroyHook();
        }
        /*else
        {
            rigid.AddForce((hook.transform.position - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);
        }*/

        /*
        if (Math.Abs(transform.position.x - hook.transform.position.x) <= horizontalYankDistance)
        {
            return;
        }
        else
        {
            rigid.AddRelativeForce(rigid.transform.forward);
        }*/

        // Old
        /*if (extendGrapple.IsPressed() && grappleActive == true)
        {
            playerRB.AddForce(Physics.gravity * 7.0f * playerRB.mass);
        }*/

        if (hook != null)
        {
            if (hook != null && (playerRB.position.y > (hook.transform.position.y - 1)))
            {
                DestroyHook();
            }
            
            if (Vector3.Distance(transform.position, hook.transform.position) <= taughtDistance || player.isGrounded)
            {
                return;
            }
            else
            {
                playerRB.AddForce((hook.transform.position - transform.position).normalized * swingSpeed, ForceMode.Impulse);
            }
        }
    }

    public void StartGrapple()
    {
        ready = false;
        grappleActive = true;
        yankReady = false;

        if (canYank)
        {
            StartCoroutine(YankDelay());
        }
    }

    public void DestroyHook()
    {
        if (hook == null)
        {
            return;
        }

        ready = false;
        canYank = false;
        yankReady = false;
        grappleActive = false;

        Destroy(hook.gameObject);
        hook = null;

        StopAllCoroutines();

        StartCoroutine(GrappleCooldown());
    }

    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(0.15f);

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
        yield return new WaitForSeconds(5f);

        DestroyHook();
    }

    /*private IEnumerator StartReel()
    {
        rigid.AddForce((hook.transform.position - transform.position).normalized * reelSpeed, ForceMode.Impulse);

        yield return new WaitForSeconds(1.0f);
    }*/

    private IEnumerator GrappleCooldown()
    {
        cdRemaining = grappleCooldown;
        while(cdRemaining > 0)
        {
            cdRemaining -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        ready = true;

        StopAllCoroutines();
    }

    public float CooldownRemaining()
    {
        if (cdRemaining > 0)
        {
            return cdRemaining;
        }
        else
        {
            return -1;
        }
    }

    private IEnumerator YankDelay()
    {
        yield return new WaitForSeconds(0.5f);

        yankReady = true;
    }

    // Enable input action map controls.
    private void OnEnable()
    {
        grappleControls = new PlayerControls();

        shootHook = grappleControls.Grapple.ShootHook;
        shootHook.Enable();

        cancelHook = grappleControls.Grapple.CancelHook;
        cancelHook.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        shootHook.Disable();
        cancelHook.Disable();
    }
}
