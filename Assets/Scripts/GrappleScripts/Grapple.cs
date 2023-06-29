using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Grapple : MonoBehaviour, IAbility
{
    // Speed and distance restrictions
    float initialPullSpeed = 0.0f;
    float pullSpeed = 0.28f;
    float recoveryPullSpeed = 0.36f;

    float initalHorizSpeed = 0.0f;
    float horizSpeed = 8.5f;
    float recoveryHorizSpeed = 10.0f;

    float yankSpeedStrong = 6.0f;
    [SerializeField] float yankSpeedWeak = 0.7f;
    float stopDistanceClose = 2.5f;
    public float stopDistanceFar;
    public float taughtDistance = 3.0f;
    [SerializeField] float maxSwingHeight = float.MaxValue;
    float heightCapDistance = 1.0f;
    
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
    float radius = 11.0f;

    [SerializeField] Rigidbody playerRB;
    [SerializeField] BasicMovement player;

    Hook hook;
    float changePerSecond;
    float grappleCooldown = 0.1f;
    float maxVelocity = 30.0f;
    float sqrMaxVelocity;

    // Conditions for grapple movement
    public bool grappleActive;
    public bool swinging = false;
    public bool yanking = false;
    public bool ready = true;
    public bool canYank = false;
    public bool yankReady = false;
    public bool canReverseSwing = true;
    public bool canApplyForce = true;
    public bool forwardSwing = true;
    public bool needYankSwing = true;
    public bool recoverySwing = false;
    public bool initalSwingForce = false;
    public bool groundedSwing = false;
    float cdRemaining;

    Vector3 dirFromPlayer;
    float dotProd;
    public Transform testAim;

    public InputActionAsset grappleControls;
    public InputAction shootHook, cancelHook, extendGrapple;

    [SerializeField] private AudioSource shoot;
    [SerializeField] private AudioSource yank;

    // Additions from Will :)
    private bool releasedHook = false; // input testing

    public GameObject grappleEffect;
    public GameObject grappleEffectDestroy;

    public bool grappleNeedsDeath;

    Animator anim;

    public GameObject grappleIndicator;
    private GameObject[] grapplePoints;

    // Start is called before the first frame update
    void Start()
    {
        grappleNeedsDeath = false;
        grappleActive = false;

        SetMaxVelocity(maxVelocity);
        ResetSwingConditions();

        anim = GetComponentInChildren<Animator>();

        if (grappleIndicator != null)
        {
            int i = 0;
            grapplePoints = new GameObject[GameObject.FindGameObjectsWithTag("GrappleHookPoint").Length];
            
            while (i < GameObject.FindGameObjectsWithTag("GrappleHookPoint").Length)
            {
                grapplePoints[i] = GameObject.FindGameObjectsWithTag("GrappleHookPoint")[i];
                i++;
                
                //Debug.Log(i);
            }

            foreach (var point in grapplePoints)
            {
                GameObject particle = Instantiate(grappleIndicator, point.transform.TransformPoint( point.GetComponent<SphereCollider>().center), quaternion.identity);
                particle.transform.parent = point.transform;
                particle.transform.localScale = new Vector3(20, 20, 20);
            }
        }
    }

    void FixedUpdate()
    {
        if (playerRB.worldCenterOfMass.y > maxSwingHeight)
        {
            DestroyHook();
        }

        if (grappleActive && !player.isGrounded)
        {
            anim.SetBool("isGrapple", false);
            swinging = true;
        }

        // Initializes maxSwingHeight for 'GrappleYank' points once in the air allowing for a reliable swing
        /*if (yankReady && !player.isGrounded && needYankSwing)
        {
            maxSwingHeight = playerRB.position.y + heightCapDistance + 0.5f;
            needYankSwing = false;
        }*/

        // Caps player player velocity according to SetMaxVelocity() on awake
        if (playerRB.velocity.sqrMagnitude > sqrMaxVelocity)
        {
            playerRB.velocity = playerRB.velocity.normalized * maxVelocity;
        }

        // If bestHook is initialized as a point other than the hook itself, 
        // check if the player and bestHook are within the determined radius
        if (bestHook != null && bestHook != hookBase)
        {
            if (Vector3.Distance(bestHookCenter, shootTransform.position) > radius)
            {
                bestHook = null;
            }
            else
            {
                dirFromPlayer = (bestHookCenter - testAim.position).normalized;
                dotProd = Vector3.Dot(dirFromPlayer, testAim.forward);

                //Debug.Log(dotProd);
            }
        }

        if (grappleEffectDestroy != null)
        {
            grappleNeedsDeath = true;
        }

        // Old/Updated
        if (hook != null && (cancelHook.IsPressed() || releasedHook)) // input testing
        {
            DestroyHook(); 
            releasedHook = false; // input testing
        }
        
        if (!grappleActive || hook == null)
        {
            return;
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

        //  Runs the GrappleForce() method while the grapple is active
        if (grappleActive)
        {
            GrappleForce();
        }
    }

    void Update()
    {
        // Fires the grapple if the input is pressed, bestHook is initialized, the grapple is inactive, 
        // and the grapple is ready
        if (bestHook != null && hook == null && shootHook.WasPressedThisFrame() && ready == true)
        {
            if (dotProd > -0.05)
            {
                hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
                shoot.Play();

                anim.SetBool("isGrapple", true);
                shootTransform.LookAt(bestHookCenter);
                grappleEffectDestroy = Instantiate(grappleEffect, shootTransform.position, shootTransform.rotation);

                hook.Initialize(this, shootTransform);
                StartCoroutine(DestroyHookAfterLifetime());
            }
        }

        // Conditions for a Yank. These include being attached to a 'GrappleYank' point, 
        // and finishing the YankDelay() coroutine (makes yankReady = true)
        if (canYank && yankReady)
        {
            if (grappleActive && shootHook.WasPressedThisFrame() && player.isGrounded)
            {
                anim.SetBool("isGrapple", false);
                yankReady = false;
                yanking = true;
                yank.Play();
            }
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
    }

    // Shoots grapple and sets all booleans allowing for a forward swing
    public void StartGrapple()
    {
        anim.SetBool("isGrapple", true);
        ready = false;
        grappleActive = true;
        yankReady = false;
        forwardSwing = true;

        if (player.isGrounded)
        {
            groundedSwing = true;
        }

        stopDistanceFar = Vector3.Distance(transform.position, hook.transform.position) + 4.0f;

        setSwingHeight();

        if (canYank)
        {
            StartCoroutine(YankDelay());
        }
    }
    // Destroys an active grapple
    public void DestroyHook()
    {
        player.move.Enable();
        
        if (hook == null)
        {
            return;
        }

        anim.SetBool("isGrapple", false);

        // Reset all swing/force conditions
        ResetSwingConditions();

        Destroy(hook.gameObject);
        hook = null;

        StopAllCoroutines();

        StartCoroutine(GrappleCooldown());
    }

    public void setSwingHeight()
    {
        if (groundedSwing)
        {
            maxSwingHeight = playerRB.worldCenterOfMass.y + heightCapDistance + 0.5f;
        }
        else if (playerRB.velocity.y > 0.0f)
        {
            maxSwingHeight = playerRB.worldCenterOfMass.y + 1.0f;
        }
        else
        {
            maxSwingHeight = playerRB.worldCenterOfMass.y + heightCapDistance;
        }
    }

    // Failsafe that destroys the grapple a certain amount of time after shooting it when it
    // doesn't make contact with a grapple point
    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForSeconds(0.5f);

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
        yield return new WaitForSeconds(3.0f);

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
        yield return new WaitForSeconds(0.2f);

        yankReady = true;
    }

    // Runs while the grapple is active. Includes conditions for forces to be added
    private void GrappleForce()
    {
        if (yanking)
        {
            playerRB.AddRelativeForce((hook.transform.position - transform.position).normalized * yankSpeedStrong, ForceMode.Impulse);
        }

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

        // Once the player reaches maxSwingHeight mid swing, they have completed once cycle
        if ((playerRB.worldCenterOfMass.y > maxSwingHeight) && canApplyForce)
        {
            // Temporary Coroutine Swap
            //StartCoroutine(ReverseSwing());
            //StartCoroutine(RemoveForce());
        }

        if (player.isGrounded && !swinging)
        {
            return;
        }

        // Maintains horizontal force if the player is in the air and force can be applied
        if (canApplyForce)
        {
            HorizontalSwing();
        }

        /*if (initalSwingForce)
        {
            StartCoroutine(InitialSwingForce());
        }*/

        if (playerRB.velocity.y < -5.0f)
        {
            recoverySwing = true;
            StartCoroutine(RecoverySwing());
        }
        
        /*if (Vector3.Distance(transform.position, hook.transform.position) <= taughtDistance || (playerRB.position.y > maxSwingHeight))
        {
            playerRB.AddRelativeForce(Vector3.down * 1.0f, ForceMode.VelocityChange);
        }*/

        if (canApplyForce && !yanking)
        { 
            if (recoverySwing)
            {
                playerRB.AddRelativeForce((hook.transform.position - transform.position).normalized 
                * recoveryPullSpeed, ForceMode.Impulse);
            }
            else
            {
                playerRB.AddRelativeForce((hook.transform.position - transform.position).normalized 
                * pullSpeed, ForceMode.Impulse);
            }
            
            /*if (initalSwingForce)
            {
                playerRB.AddRelativeForce((hook.transform.position - transform.position).normalized 
                * initialPullSpeed, ForceMode.Impulse);
            }
            else
            {
                playerRB.AddRelativeForce((hook.transform.position - transform.position).normalized 
                * pullSpeed, ForceMode.Impulse);
            }*/
        }

        return;
    }

    private IEnumerator RecoverySwing()
    {
        yield return new WaitForSeconds(0.5f);

        recoverySwing = false;
    }

    // Adds horizontal force for forward and backward swings relative to the player direction
    private void HorizontalSwing()
    {
        player.move.Disable();

        if (recoverySwing)
        {
            playerRB.AddRelativeForce(recoveryHorizSpeed, 0.0f, 0.0f, ForceMode.VelocityChange);
        }
        else if (forwardSwing)
        {
            /*if (Vector3.Distance(transform.position, hook.transform.position) <= 4.0f)
            {
                playerRB.AddRelativeForce(0.0f, 0.0f, 0.0f, ForceMode.VelocityChange);
            }
            else */if (groundedSwing)
            {
                playerRB.AddRelativeForce((horizSpeed + 1.5f), 0.0f, 0.0f, ForceMode.VelocityChange);
            }
            else
            {
                playerRB.AddRelativeForce(horizSpeed, 0.0f, 0.0f, ForceMode.VelocityChange);
            }
        }

        /*else if (forwardSwing)
        {
            if (initalSwingForce)
            {
                playerRB.AddRelativeForce(initalHorizSpeed, 0.0f, 0.0f, ForceMode.VelocityChange);
            }
            else
            {
                playerRB.AddRelativeForce(horizSpeed, 0.0f, 0.0f, ForceMode.VelocityChange);
            }
        }*/
    }

    private IEnumerator InitialSwingForce()
    {
        yield return new WaitForSeconds(0.25f);

        initalSwingForce = false;
    }

    // Experimental coroutine that tracks the player's position while swinging
    private IEnumerator SetLastPosition()
    {
        canCheckPos = false;
        lastPlayerPos = transform.position;

        yield return new WaitForSeconds(0.3f);

        CheckMovement();
        canCheckPos = true;
    }

    // Compares current player's position to lastPlayerPos and destroys the hook
    // if there isn't enough change in the x and y values 
    void CheckMovement()
    {
        if (!player.isGrounded)
        {
            if (Math.Abs(transform.position.x - lastPlayerPos.x) < 0.1f)
            {
                //DestroyHook();
            }
            else if (Math.Abs(transform.position.y - lastPlayerPos.y) < 0.1f)
            {
                //DestroyHook();
            }
        }
    }

    // Sets player's max velocity
    public void SetMaxVelocity(float maxVelocity)
    {
        this.maxVelocity = maxVelocity;
        sqrMaxVelocity = maxVelocity * maxVelocity;
    }

    // Simulate backward movement after completing a forward swing
    /*private IEnumerator ReverseSwing()
    {
        if (canReverseSwing)
        {
            forwardSwing = !forwardSwing;
            canReverseSwing = false;
            StartCoroutine(RemoveForce());
        }

        yield return new WaitForSeconds(1.5f);

        canReverseSwing = true;
    }*/

    // 
    private IEnumerator ForceDelay()
    {
        canApplyForce = false;

        yield return new WaitForSeconds(0.2f);

        canApplyForce = true;
    }

    public void ResetSwingConditions()
    {
        ready = true;
        canYank = false;
        yankReady = false;
        grappleActive = false;
        canCheckPos = true;
        canApplyForce = true;
        canReverseSwing = true;
        needYankSwing = true;
        recoverySwing = false;
        swinging = false;
        yanking = false;
        groundedSwing = false;
        //initalSwingForce = false;
        maxSwingHeight = float.MaxValue;
        lastPlayerPos = Vector3.zero;
    }

    void Awake()
    {
        grappleControls = GameManager.Instance.GetComponent<PlayerInput>().actions;
        GameManager.Instance.abilities.Add(this);
        ResetRebind();
        shootHook = grappleControls["ShootHook"];
        cancelHook = grappleControls["CancelHook"];
    }

    // Enable input action map controls.
    private void OnEnable()
    {
        shootHook.Enable();
        cancelHook.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        shootHook.Disable();
        cancelHook.Disable();
    }

    public void ResetRebind()
    {
        //grappleControls.RemoveAllBindingOverrides();
        grappleControls = GameManager.Instance.GetInputs(grappleControls);
    }

    private void OnDestroy()
    {
        GameManager.Instance.abilities.Remove(this);
    }
}
