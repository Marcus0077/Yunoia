using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityPush : MonoBehaviour
{
    [SerializeField] // maxChargeLevel: how many stages a push can charge up to, minPush: smallest value a push can be
    int maxChargeLevel = 1, minPush = 1;
    [SerializeField]
    float chargeSpeed = 1, cooldown = 1, shieldCooldown, shieldDuration;
    // chargeTime: tracks how long push was charging
    float chargeTime;
    // shield: tracks if shield action is queued, shielded: tracks if player is shielded
    bool ableToPush = true, charging = false, shield = false, shielded = false;
    [SerializeField]
    public bool restored, ableToShield;
    public PlayerControls pushControls;
    public InputAction pushAction;
    // pushedLevel: what stage a push is at (corresponding to charge levels)
    public int pushedLevel;
    // range: how large a push is
    public float range;
    public float cdRemaining, cdRemainingForShield;
    Transform shape;
    Vector3 oldPos;

    // Particle Variables
    public GameObject smallPushEffect;
    public GameObject largePushEffect;
    public GameObject chargePushEffect;
    public GameObject shieldEffect;

    public GameObject smallEffectDestroy;
    public GameObject largeEffectDestroy;
    public GameObject chargeEffectDestroy;

    public bool smallPushNeedsDeath;
    public bool largePushNeedsDeath;
    public bool chargePushNeedsDeath;

    public float chargeRadius;
    public float pushRadius;
    public float smallPushRadius;
    

    [SerializeField] private AudioSource source;
    
    // Start is called before the first frame update
    void Start()
    {
        smallPushNeedsDeath = false;
        largePushNeedsDeath = false;
        chargePushNeedsDeath = false;
    }

    // Push logic
    public void PushTargets()
    {
        pushedLevel = (int)range + 1 - minPush;
        Collider[] hitColliders = Physics.OverlapSphere(oldPos, range); // OverlapSphere returns any colliders inside sphere of range size at oldPos position
        foreach (var hitCollider in hitColliders)
        {
            Pushable pushedObj = hitCollider.GetComponent<Pushable>();
            if (pushedObj != null) // if the collider was a pushable object
            {
                Vector3 direction = (pushedObj.transform.position - oldPos).normalized;
                //direction = new Vector3(direction.x, direction.y, direction.z).normalized;
                float distance = Vector3.Distance(pushedObj.transform.position, oldPos);
                float proximityMultiplier = range / distance; // scales push strength by how close an object is to the pusher
                //float chargeMultiplier = (range + 1 - minPush) / (float)(maxChargeLevel + 1);
                pushedObj.Pushed(proximityMultiplier * direction, pushedLevel, maxChargeLevel + 1, gameObject);//casted to int because chargeLevel should be flat numbers not a float
            }
        }
    }

    void RenderVolume(float radius)
    {
        oldPos = transform.position;
        smallEffectDestroy = Instantiate(smallPushEffect, this.transform.position, Quaternion.identity);
        smallEffectDestroy.transform.localScale = new Vector3(smallPushRadius/2, smallPushRadius/2, smallPushRadius/2);
        smallEffectDestroy.transform.GetChild(0).localScale = new Vector3(smallPushRadius, smallPushRadius, smallPushRadius);
        smallEffectDestroy.transform.GetChild(1).localScale = new Vector3(smallPushRadius, smallPushRadius, smallPushRadius);
        smallEffectDestroy.transform.GetChild(2).localScale = new Vector3(smallPushRadius, smallPushRadius, smallPushRadius);
        smallEffectDestroy.transform.GetChild(3).localScale = new Vector3(smallPushRadius, smallPushRadius, smallPushRadius);
        DelayedPush a = smallEffectDestroy.AddComponent<DelayedPush>();
        a.pusher = gameObject;
        // Tell particle destroyer to destroy small push particles.
        smallPushNeedsDeath = true;

        // shape = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        // Destroy(shape.GetComponent<Collider>());
        // shape.localScale = new Vector3(radius, radius, radius);
        // shape.position = transform.position;
        // shape.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        // shape.GetComponent<Renderer>().material.color = new Color(1, .3f, .3f, .2f);
        // shape.GetComponent<Renderer>().enabled = true;
        // StartCoroutine(EraseRender(shape)); // erase after 1 second
    }

    public void DestroyShape()
    {
        if (shape != null)
        {
            Destroy(shape.gameObject);
        }
    }

    // Displays particles for push effect (rename function?)
    private IEnumerator RenderChargeVolume()
    {
        // shape = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        // Destroy(shape.GetComponent<Collider>());
        // shape.position = transform.position;
        // shape.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        // shape.GetComponent<Renderer>().material.color = new Color(1, 1, 1, .5f);
        // shape.GetComponent<Renderer>().enabled = true;
        
        float radius = minPush; // radius starts at minPush size
        float timeCharging = Time.time;

        chargeEffectDestroy = Instantiate(chargePushEffect, this.transform.position, Quaternion.identity); // create particle system for charging effect
            
        while (charging)
        {
            timeCharging = (Time.time - chargeTime) * chargeSpeed + minPush; // scale time charged by charge speed
            
            // int radiusShape = 2*Mathf.Clamp((int)chargeTime, minPush, maxChargeLevel + minPush);
            
            radius = Mathf.Clamp(timeCharging, minPush, maxChargeLevel + minPush); // make sure push size is clamped between minimum and maximum size

            // pushedLevel = (int)radius + 1 - minPush; //different effects like color change or something EXAMPLE COLOR CHANGE:
            
            // shape.GetComponent<Renderer>().material.color = new Color(1, 1f / (2 * pushedLevel), 1f / ( 2* pushedLevel), .2f + (.1f * pushedLevel));
            
            radius *= 2; // radius is used like diameter

            chargeRadius = radius / 8;
            pushRadius = radius / 3;

            chargeEffectDestroy.transform.position = this.transform.position;
            chargeEffectDestroy.transform.GetChild(0).localScale = new Vector3(chargeRadius, chargeRadius, chargeRadius);
            chargeEffectDestroy.transform.GetChild(0).GetChild(0).localScale = new Vector3(chargeRadius, chargeRadius, chargeRadius);

            // shape.localScale = new Vector3(radius, radius, radius);
            // shape.position = transform.position;
            if(timeCharging > maxChargeLevel+minPush) // automatically shield if max charge is reached
            {
                if(ableToShield)
                {
                    Destroy(chargeEffectDestroy);
                    shield = true;
                    PushRelease(); // remove this line if player should not automatically shielded at end of charging
                    yield break;
                }
                else
                {
                    charging = false;
                    PushRelease();
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Tell particle destroyer to destroy charge push particles.
        chargePushNeedsDeath = true;

        largeEffectDestroy = Instantiate(largePushEffect, this.transform.position, Quaternion.identity);
        largeEffectDestroy.transform.localScale = new Vector3(pushRadius/3, pushRadius/3, pushRadius/3);
        largeEffectDestroy.transform.GetChild(0).localScale = new Vector3(pushRadius / 3, pushRadius / 3, pushRadius / 3);
        largeEffectDestroy.transform.GetChild(1).localScale = new Vector3(pushRadius/2, pushRadius/2, pushRadius/2);
        largeEffectDestroy.transform.GetChild(2).localScale = new Vector3(pushRadius, pushRadius, pushRadius);
        largeEffectDestroy.transform.GetChild(3).localScale = new Vector3(pushRadius/2, pushRadius/2, pushRadius/2);
        largeEffectDestroy.transform.GetChild(4).localScale = new Vector3(pushRadius/4, pushRadius/4, pushRadius/4);
        largeEffectDestroy.transform.GetChild(5).localScale = new Vector3(pushRadius, pushRadius, pushRadius);
        largeEffectDestroy.transform.GetChild(6).localScale = new Vector3(pushRadius, pushRadius, pushRadius);
        largeEffectDestroy.transform.GetChild(7).localScale = new Vector3(pushRadius / 2, 1f, pushRadius / 2);
        DelayedPush a = largeEffectDestroy.transform.GetChild(5).gameObject.AddComponent<DelayedPush>();
        a.pusher = gameObject;
        // Tell particle destroyer to destroy large push particles.
        largePushNeedsDeath = true;
        oldPos = transform.position;
        yield return new WaitForSeconds(1);

        if (shape != null)
        {
            Destroy(shape.gameObject);
        }
    }

    private IEnumerator EraseRender(Transform shape)
    {
        yield return new WaitForSeconds(1);

        if (shape != null)
        {
            Destroy(shape.gameObject);
        }
    }

    // Cooldown for push
    private IEnumerator PushTimer()
    {
        source.Play(); // move this to fix sound bug with shielding
        ableToPush = false;
        cdRemaining = cooldown;
        while(cdRemaining > 0)
        {
            cdRemaining -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if(restored && charging)
        {
            chargeTime = Time.time; // held before cooldown ended, so start charging right when cooldown ends
            StartCoroutine(RenderChargeVolume());
        }
        cdRemaining = -1;
        ableToPush = true;
    }

    // Cooldown for shield
    private IEnumerator PushTimerForShield()
    {
        //source.Play(); add sound to shield gameobject particle system?
        ableToShield = false;
        ableToPush = false;
        yield return new WaitUntil(() => !shielded);
        cdRemainingForShield = shieldCooldown;
        while (cdRemainingForShield > 0)
        {
            cdRemainingForShield -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        cdRemainingForShield = -1;
        ableToShield = true;
    }

    // Cooldown grabber for ui
    public float CooldownRemaining()
    {
        if(cdRemaining > 0)
        {
            return cdRemaining;
        }
        else
        {
            return -1;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // When player presses push
    void PushPress()
    {
        charging = true;
        if (ableToPush)
        {
            //start animation
            if (!restored)
            {
                range = minPush;
                RenderVolume(range * 2);
                StartCoroutine(PushTimer());
            }
            else
            {
                chargeTime = Time.time;
                StartCoroutine(RenderChargeVolume());
            }
        }
    }

    // When player releases push (or called automatically for shield)
    void PushRelease()
    {
        charging = false;
        
        if (ableToPush)
        {
            //start animation
            if (restored && !shield)
            {
                chargeTime = (Time.time - chargeTime) * chargeSpeed + minPush;
                range = Mathf.Clamp(chargeTime, minPush, maxChargeLevel + minPush);
                StartCoroutine(PushTimer());
                chargeTime = Time.time;
            }
            else if(shield)
            {
                if(ableToShield)
                {
                    shield = false;
                    shieldEffect.SetActive(true);
                    shielded = true;
                    //ableToPush = false;
                    StartCoroutine(DisableShield(shieldDuration));
                    StartCoroutine(PushTimerForShield());
                }
                else
                {
                    shield = false;
                    Debug.Log("on cooldown or not unlocked");
                }
            }
        }
    }

    // Disables shield after timer seconds (public variable shieldDuration called within PushRelease)
    private IEnumerator DisableShield(float timer)
    {
        yield return new WaitForSeconds(timer);
        ableToPush = true;
        shielded = false;
        shieldEffect.SetActive(false);
    }

    void Awake()
    {
        pushControls = new PlayerControls();
        pushAction = pushControls.Push.Push;
        pushAction.performed += ctx => PushPress();
        pushAction.canceled += ctx => PushRelease();
    }

    // Enable input action map controls.
    private void OnEnable()
    {
        pushAction.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        pushAction.Disable();
    }
}
