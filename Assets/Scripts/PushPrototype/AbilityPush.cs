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
    bool ableToPush = true;
    [SerializeField]
    bool restored;
    PlayerControls pushControls;
    private InputAction pushAction;
    // Start is called before the first frame update
    void Start()
    {

    }

    void PushTargets(float range)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            Pushable pushedObj = hitCollider.GetComponent<Pushable>();
            if (pushedObj != null)
            {
                Vector3 direction = pushedObj.transform.position - transform.position;
                direction = new Vector3(direction.x, 0, direction.z).normalized;
                float distance = Vector3.Distance(pushedObj.transform.position, transform.position);
                float proximityMultiplier = range / distance;
                //float chargeMultiplier = (range + 1 - minPush) / (float)(maxChargeLevel + 1);
                pushedObj.Pushed(proximityMultiplier * direction, (int)range + 1 - minPush, maxChargeLevel + 1);
            }
                
        }
        RenderVolume(range*2);
    }

    void RenderVolume(float radius)
    {
        Transform shape;
        shape = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        Destroy(shape.GetComponent<Collider>());
        shape.localScale = new Vector3(radius, radius, radius);
        shape.position = transform.position;
        shape.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse"); 
        shape.GetComponent<Renderer>().material.color = new Color(1, 1, 1, .5f);
        shape.GetComponent<Renderer>().enabled = true;
        StartCoroutine(EraseRender(shape)); // erase after 1 second
    }

    private IEnumerator EraseRender(Transform shape)
    {
        yield return new WaitForSeconds(1);
        Destroy(shape.gameObject);
    }

    private IEnumerator PushTimer()
    {
        ableToPush = false;
        yield return new WaitForSeconds(cooldown);
        chargeTime = Time.time; // held before cooldown ended, so start charging right when cooldown ends
        ableToPush = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PushPress()
    {
        if (ableToPush)
        {
            //start animation
            if (!restored)
            {
                PushTargets(minPush);
                StartCoroutine(PushTimer());
            }
        }
        chargeTime = Time.time;
    }

    void PushRelease()
    {
        if (ableToPush)
        {
            //start animation
            if (restored)
            {
                chargeTime = (Time.time - chargeTime) * chargeSpeed + minPush;
                Debug.Log(chargeTime);
                PushTargets(Mathf.Clamp((int)chargeTime, minPush, maxChargeLevel + minPush));
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
