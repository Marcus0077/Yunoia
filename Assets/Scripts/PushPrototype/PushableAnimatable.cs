using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PushableAnimatable : Pushable
{
    protected Animator anim;
    bool toBeDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override bool Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        Debug.Log(data.animationBool);
        toBeDestroyed = data.doDestroy;
        if (base.Pushed(force, chargeLevel, totalCharges, pusher))
        {
            if(toBeDestroyed)
            {
                GetComponent<Collider>().enabled = false;
            }
            StartCoroutine(DisplayAnimation());
            return true;
        }
        else
        {
            return false;
        }
    }

    protected IEnumerator WaitForAnimation()
    {
        do
        {
            yield return null;
        } while (anim.GetBool(data.animationBool)); //animation end behavior is set bool to false? or i can destroy in behavior instead of these coroutines
    }

    public virtual IEnumerator DisplayAnimation()
    {
        anim.SetBool(data.animationBool, true);
        yield return WaitForAnimation(); // have real name
        if(toBeDestroyed)
        {
            Destroy(gameObject);
        }
    }

    public void AnimationOff()
    {
        anim.SetBool(data.animationBool, false);
    }

    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
