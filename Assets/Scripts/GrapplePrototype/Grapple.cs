using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    float pullSpeed = 0.27f;
    [SerializeField] float yankSpeedStrong = 0.7f;
    [SerializeField] float yankSpeedWeak = 0.7f;
    float stopDistanceClose = 2.5f;
    float stopDistanceFar = 13.0f;
    float taughtDistance = 3.0f;
    [SerializeField] float maxSwingHeight = float.MaxValue;
    [SerializeField] Vector3 lastPlayerPos;
    public bool canCheckPos = true;

    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform shootTransform;

    [SerializeField] AimAssist aimAssist;
    [SerializeField] public Collider hookBase;
    [SerializeField] public Collider bestHook;
    public Collider playerCollider;
    public Vector3 bestHookCenter;
    public float radius = 12.0f;

    [SerializeField] Rigidbody playerRB;
    [SerializeField] BasicMovement player;
    [SerializeField] float changePerSecond;
    [SerializeField] float grappleCooldown = 1;
    float maxVelocity;
    float sqrMaxVelocity;
    [SerializeField] bool canReverseSwing = true;
    [SerializeField] bool canApplyForce = true;
    [SerializeField] bool forwardSwing = true;
    float cdRemaining;

    Hook hook;
    bool grappleActive;
    bool ready = true;
    public bool canYank = false;
    public bool yankReady = false;

    public PlayerControls grappleControls;
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

        SetMaxVelocity(15.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Caps player player velocity according to SetMaxVelocity() on awake
        if (playerRB.velocity.sqrMagnitude > sqrMaxVelocity)
        {
            playerRB.velocity = playerRB.velocity.normalized * maxVelocity;
        }

        if (hook == null && !grappleActive)
        {
            bestHook = aimAssist.HookDetection(shootTransform.position, radius);

            if (bestHook != null)
            {
                bestHookCenter = bestHook.bounds.center;
                shootTransform.LookAt(bestHookCenter);
            }
        }

        if (bestHook != hookBase)
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
        
        if (Vector3.Distance(transform.position, hook.transform.position) <= stopDistanceClose)
        {
            DestroyHook();
        }
        // Break Active Grapple if the player is too far from the grapple point
        else if (Vector3.Distance(transform.position, hook.transform.position) >= stopDistanceFar)
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

        if (grappleActive)
        {
            GrappleForce();
        }
    }

    public void StartGrapple()
    {
        ready = false;
        grappleActive = true;
        yankReady = false;
        forwardSwing = true;

        stopDistanceFar = Vector3.Distance(transform.position, hook.transform.position) + 1.5f;

        if (!canYank)
        {
            maxSwingHeight = playerRB.position.y + 1.5f;
        }
        else if (canYank)
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
        canCheckPos = true;
        canApplyForce = true;
        canReverseSwing = true;

        maxSwingHeight = float.MaxValue;

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

    private void GrappleForce()
    {
        if (canCheckPos)
        {
            StartCoroutine(SetLastPosition());
        }

        if (playerRB.position.y > (hook.transform.position.y - 1))
        {
            DestroyHook();
        }

        if ((playerRB.position.y > maxSwingHeight) && canApplyForce)
        {
            StartCoroutine(ReverseSwing());
        }

        if (!player.isGrounded && canApplyForce)
        {
            HorizontalSwing();
        }

        if (player.isGrounded)
        {
            return;
        }
        /*if (Vector3.Distance(transform.position, hook.transform.position) <= taughtDistance || (playerRB.position.y > maxSwingHeight))
        {
            playerRB.AddRelativeForce(Vector3.down * 1.0f, ForceMode.VelocityChange);
        }*/
        else if (canApplyForce)
        {
            playerRB.AddRelativeForce((hook.transform.position - transform.position).normalized * pullSpeed, ForceMode.Impulse);
        }

        return;
    }

    private void HorizontalSwing()
    {
        if (forwardSwing)
        {
            playerRB.AddRelativeForce(7.0f, 0.0f, 0.0f, ForceMode.VelocityChange);
        }
        else
        {
            playerRB.AddRelativeForce(-7.0f, 0.0f, 0.0f, ForceMode.VelocityChange);
        }
    }

    private IEnumerator SetLastPosition()
    {
        canCheckPos = false;
        lastPlayerPos = transform.position;

        yield return new WaitForSeconds(0.5f);

        canCheckPos = true;
    }

    public void SetMaxVelocity(float maxVelocity)
    {
        this.maxVelocity = maxVelocity;
        sqrMaxVelocity = maxVelocity * maxVelocity;
    }

    private IEnumerator ReverseSwing()
    {
        if (canReverseSwing)
        {
            forwardSwing = !forwardSwing;
            canReverseSwing = false;
            canApplyForce = false;
            StartCoroutine(RemoveForce());
        }

        yield return new WaitForSeconds(1.5f);

        canReverseSwing = true;
    }

    private IEnumerator RemoveForce()
    {
        yield return new WaitForSeconds(0.5f);

        canApplyForce = true;
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
