using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityPush : MonoBehaviour
{
    [SerializeField]
    int maxChargeLevel = 1, minPush = 1;
    [SerializeField]
    float chargeSpeed = 1, cooldown = 1;
    float chargeTime;
    bool ableToPush = true, charging = false;
    [SerializeField]
    public bool restored;
    PlayerControls pushControls;
    private InputAction pushAction;
    public int pushedLevel;
    public float range;
    public float cdRemaining;
    Transform shape;

    [SerializeField] private AudioSource source;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    void PushTargets()
    {
        pushedLevel = (int)range + 1 - minPush;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            Pushable pushedObj = hitCollider.GetComponent<Pushable>();
            if (pushedObj != null)
            {
                Vector3 direction = (pushedObj.transform.position - transform.position).normalized;
                //direction = new Vector3(direction.x, direction.y, direction.z).normalized;
                float distance = Vector3.Distance(pushedObj.transform.position, transform.position);
                float proximityMultiplier = range / distance;
                //float chargeMultiplier = (range + 1 - minPush) / (float)(maxChargeLevel + 1);
                pushedObj.Pushed(proximityMultiplier * direction, pushedLevel, maxChargeLevel + 1, gameObject);//casted to int because chargeLevel should be flat numbers not a float
            }
        }
        if (!restored)
        {
            RenderVolume(range * 2);
        }
    }

    void RenderVolume(float radius)
    {
        shape = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        Destroy(shape.GetComponent<Collider>());
        shape.localScale = new Vector3(radius, radius, radius);
        shape.position = transform.position;
        shape.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        shape.GetComponent<Renderer>().material.color = new Color(1, .3f, .3f, .2f);
        shape.GetComponent<Renderer>().enabled = true;
        StartCoroutine(EraseRender(shape)); // erase after 1 second
    }

    public void DestroyShape()
    {
        if (shape != null)
        {
            Destroy(shape.gameObject);
        }
    }

    private IEnumerator RenderChargeVolume()
    {
        shape = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        Destroy(shape.GetComponent<Collider>());
        shape.position = transform.position;
        shape.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        shape.GetComponent<Renderer>().material.color = new Color(1, 1, 1, .5f);
        shape.GetComponent<Renderer>().enabled = true;
        float radius = minPush;
        float timeCharging = Time.time;
        while (charging)
        {
            timeCharging = (Time.time - chargeTime) * chargeSpeed + minPush;
            //int radius = 2*Mathf.Clamp((int)chargeTime, minPush, maxChargeLevel + minPush);
            radius = Mathf.Clamp(timeCharging, minPush, maxChargeLevel + minPush);
            pushedLevel = (int)radius + 1 - minPush; //different effects like color change or something EXAMPLE COLOR CHANGE:
            shape.GetComponent<Renderer>().material.color = new Color(1, 1f / (2 * pushedLevel), 1f / ( 2* pushedLevel), .2f + (.1f * pushedLevel));
            radius *= 2;
            shape.localScale = new Vector3(radius, radius, radius);
            shape.position = transform.position;
            yield return new WaitForSeconds(Time.deltaTime);
        }
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

    private IEnumerator PushTimer()
    {
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

    void PushPress()
    {
        charging = true;
        if (ableToPush)
        {
            //start animation
            if (!restored)
            {
                range = minPush;
                PushTargets();
                StartCoroutine(PushTimer());
            } else
            {
                chargeTime = Time.time;
                StartCoroutine(RenderChargeVolume());
            }
        }
    }

    void PushRelease()
    {
        charging = false;

        source.Play();

        if (ableToPush)
        {
            //start animation
            if (restored)
            {
                chargeTime = (Time.time - chargeTime) * chargeSpeed + minPush;
                range = Mathf.Clamp(chargeTime, minPush, maxChargeLevel + minPush);
                PushTargets();
                StartCoroutine(PushTimer());
                chargeTime = Time.time;
            }
        }
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
