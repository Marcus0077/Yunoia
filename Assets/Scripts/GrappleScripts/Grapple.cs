using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    // Speed and distance restrictions
    float pullSpeed = 0.27f;
    [SerializeField] float yankSpeedStrong = 0.7f;
    [SerializeField] float yankSpeedWeak = 0.7f;
    public float stopDistanceClose = 2.5f;
    public float stopDistanceFar;
    public float taughtDistance = 3.0f;
    [SerializeField] float maxSwingHeight = float.MaxValue;
    
    // Experimental player tracker while grappling
    [SerializeField] Vector3 lastPlayerPos;
    public bool canCheckPos = true;

    [SerializeField] GameObject hookPrefab;
    [SerializeField] Transform shootTransform;

    // Aim Assist Variables
    [SerializeField] AimAssist aimAssist;
    [SerializeField] public Collider hookBase;
    [SerializeField] public Collider bestHook;
    public Collider playerCollider;
    public Vector3 bestHookCenter;
    public float radius = 12.0f;

    [SerializeField] Rigidbody playerRB;
    [SerializeField] BasicMovement player;

    Hook hook;
    float changePerSecond;
    float grappleCooldown = 0.1f;
    float maxVelocity;
    float sqrMaxVelocity;

    // Conditions for grapple movement
    bool grappleActive;
    bool ready = true;
    public bool canYank = false;
    public bool yankReady = false;
    public bool canReverseSwing = true;
    public  bool canApplyForce = true;
    public bool forwardSwing = true;
    public bool needYankSwing = true;
    float cdRemaining;

    public PlayerControls grappleControls;
    public InputAction shootHook, cancelHook, extendGrapple;

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
        // Initializes maxSwingHeight for 'GrappleYank' points once in the air allowing for a reliable swing
        if (yankReady && !player.isGrounded && needYankSwing)
        {
            maxSwingHeight = playerRB.position.y + 1.5f;
            needYankSwing = false;
        }

        // Caps player player velocity according to SetMaxVelocity() on awake
        if (playerRB.velocity.sqrMagnitude > sqrMaxVelocity)
        {
            playerRB.velocity = playerRB.velocity.normalized * maxVelocity;
        }

        // Checks for the nearest grapple point while the grapple is inactive
        if (hook == null && !grappleActive)
        {
            bestHook = aimAssist.HookDetection(shootTransform.position, radius);

            if (bestHook != null)
            {
                bestHookCenter = bestHook.bounds.center;
                shootTransform.LookAt(bestHookCenter);
            }
        }

        // If bestHook is initialized as a point other than the hook itself, 
        // check if the player and bestHook are within the determined radius
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

        // Fires the grapple if the input is pressed, bestHook is initialized, the grapple is inactive, 
        // and the grapple is ready
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
        
        // Conditions for a Yank. These include being attached to a 'GrappleYank' point, 
        // and finishing the YankDelay() coroutine (makes yankReady = true)
        if (canYank && yankReady)
        {
            if (grappleActive && shootHook.IsPressed() && player.isGrounded)
            {
                yankReady = false;
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

        // Breaks active grapple if the player is too close to the grapple point
        if (Vector3.Distance(transform.position, hook.transform.position) <= stopDistanceClose)
        {
            DestroyHook();
        }
        // Breaks active grapple if the player is too far from the grapple point
        else if (Vector3.Distance(transform.position, hook.transform.position) >= stopDistanceFar)
        {
            DestroyHook();
        }
        
        /*else
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
        /*if (extendGrapple.IsPressed() && grappleActive == true)
        {
            playerRB.AddForce(Physics.gravity * 7.0f * playerRB.mass);
        }*/

        //  Runs the GrappleForce() coroutine while the grapple is active
        if (grappleActive)
        {
            GrappleForce();
        }
    }

    // Shoots grapple and sets all booleans allowing for a forward swing
    public void StartGrapple()
    {
        ready = false;
        grappleActive = true;
        yankReady = false;
        forwardSwing = true;

        stopDistanceFar = Vector3.Distance(transform.position, hook.transform.position) + 3.0f;

        if (!canYank)
        {
            maxSwingHeight = playerRB.position.y + 1.5f;
        }
        else if (canYank)
        {
            StartCoroutine(YankDelay());
        }
    }

    // Destroys an active grapple
    public void DestroyHook()
    {
        if (hook == null)
        {
            return;
        }

        // Reset all swing/force conditions
        ready = false;
        canYank = false;
        yankReady = false;
        grappleActive = false;
        canCheckPos = true;
        canApplyForce = true;
        canReverseSwing = true;
        needYankSwing = true;
        maxSwingHeight = float.MaxValue;

        Destroy(hook.gameObject);
        hook = null;

        StopAllCoroutines();

        StartCoroutine(GrappleCooldown());
    }

    // Failsafe that destroys the grapple a certain amount of time after shooting it when it
    // doesn't make contact with a grapple point
    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(0.2f);

        if (grappleActive == true)
        {
            StartCoroutine(ExtendLifetime());
        }
        else
        {
            DestroyHook();
        }
    }

    // If the grapple connects with a grapple point 
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

    // Cooldown between hook deletion and grapple being available again
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

    // Checks how much grapple cooldown is left
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

    // Delay between shooting the grapple and being able to yank
    private IEnumerator YankDelay()
    {
        yield return new WaitForSeconds(0.5f);

        yankReady = true;
    }

    // Runs while the grapple is active. Includes conditions for forces to be added
    private void GrappleForce()
    {
        // Experimental player position tracker
        if (canCheckPos)
        {
            StartCoroutine(SetLastPosition());
        }

        // Failsafe that destroys the grapple if the player gets close to the grapple point's y position
        if (playerRB.position.y > (hook.transform.position.y - 1))
        {
            DestroyHook();
        }

        // Once the player reaches maxSwingHeight mid swing, they have completed once cycle and are 
        // ready to swing backwards
        if ((playerRB.position.y > maxSwingHeight) && canApplyForce)
        {
            StartCoroutine(ReverseSwing());
        }

        // Maintains horizontal force if the player is in the air and force can be applied
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
            playerRB.AddRelativeForce((hook.transform.position - transform.position).normalized 
                * pullSpeed, ForceMode.Impulse);
        }

        return;
    }

    // Adds horizontal force for forward and backward swings relative to the player direction
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

    // Experimental coroutine that tracks the player's position while swinging
    private IEnumerator SetLastPosition()
    {
        canCheckPos = false;
        lastPlayerPos = transform.position;

        yield return new WaitForSeconds(0.5f);

        canCheckPos = true;
    }

    // Sets player's max velocity
    public void SetMaxVelocity(float maxVelocity)
    {
        this.maxVelocity = maxVelocity;
        sqrMaxVelocity = maxVelocity * maxVelocity;
    }

    // Simulate backward movement after completing a forward swing
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

    // 
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