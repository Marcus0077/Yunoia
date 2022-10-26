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

    public override void Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        base.Pushed(force, chargeLevel, totalCharges, pusher);
        StartCoroutine(DisplayAnimation());
    }

    protected IEnumerator WaitForAnimation(string boolName)
    {
        do
        {
            yield return null;
        } while (anim.GetBool(boolName)); //animation end behavior is set bool to false? or i can destroy in behavior instead of these coroutines
    }

    public virtual IEnumerator DisplayAnimation()
    {
        anim.SetBool("break", true);
        yield return WaitForAnimation("break"); // have real name
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
