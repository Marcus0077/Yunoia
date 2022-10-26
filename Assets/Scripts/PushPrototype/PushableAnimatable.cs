using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PushableAnimatable : Pushable
{
    protected Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override bool Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        bool toBeDestroyed = data.doDestroy;
        data.doDestroy = false;
        if (base.Pushed(force, chargeLevel, totalCharges, pusher))
        {
            data.doDestroy = toBeDestroyed;
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
        if(data.doDestroy)
        {
            Destroy(gameObject);
        }
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
