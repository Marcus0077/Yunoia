using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PushFunction")]
public class PushableFunction : ScriptableObject
{
    [SerializeField]
    float velocity = 1;
    [SerializeField]
    int reqCharge = 1;
    [SerializeField]
    bool activatable, changeColor, destroy, animation;
    [SerializeField]
    Color color;
    [SerializeField]
    string animationName;

    public bool doDestroy
    {
        get
        {
            return destroy;
        }
        set
        {
            destroy = value;
        }
    }

    public float pushSpeed
    { 
        get 
        { 
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }

    public int reqChargeLevel
    {
        get
        {
            return reqCharge;
        }
        set
        {
            reqCharge = value;
        } 
    }

    public string animationBool
    {
        get
        {
            return animationName;
        }
        set
        {
            animationName = value;
        }
    }

    public void OnPush(GameObject pushedObj)
    {
        if(activatable)
        {
            if (destroy)
            {
                Destroy(pushedObj);
                return;
            }
            if (changeColor)
            {
                pushedObj.GetComponent<Renderer>().material.SetColor("_Color", color);
            }
        }
    }
}
