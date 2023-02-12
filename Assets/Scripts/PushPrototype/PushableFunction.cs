using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PushFunction")]
public class PushableFunction : ScriptableObject
{
    [SerializeField]
    float velocity = 1, maxVelocity, dragVelocity, returnDelay;
    [SerializeField]
    int reqCharge = 1, pushNumbers = 0;
    [SerializeField]
    bool activatable, changeColor, destroy, animation, canReturn, constraintX, constraintY, constraintZ, stackPush = true;
    [SerializeField]
    Color color;
    [SerializeField]
    string animationName;

    public float delay
    {
        get
        {
            return returnDelay;
        }
        set
        {
            returnDelay = value;
        }
    }

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

    public bool returning
    {
        get
        {
            return canReturn;
        }
        set
        {
            canReturn = value;
        }
    }

    public bool canStack
    {
        get
        {
            return stackPush;
        }
        set
        {
            stackPush = value;
        }
    }

    public Vector3 constraints
    {
        get
        {
            return new Vector3(constraintX.GetHashCode(),constraintY.GetHashCode(), constraintZ.GetHashCode());
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

    public float drag
    {
        get
        {
            return dragVelocity;
        }
        set
        {
            dragVelocity = value;
        }
    }

    public float maxSpeed
    {
        get
        {
            return maxVelocity;
        }
        set
        {
            maxVelocity = value;
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
            if (destroy && !animation)
            {
                Destroy(pushedObj);
                return;
            }
            if (changeColor)
            {
                pushedObj.GetComponent<Renderer>().material.SetColor("_Color", color);
            }
            if(pushNumbers != 0)
            {
                pushedObj.GetComponent<Pushable>().pushCounter++;
                if(pushedObj.GetComponent<Pushable>().pushCounter >= pushNumbers)
                    pushedObj.GetComponent<Pushable>().capSpeed = false;
            }
            pushedObj.GetComponent<Pushable>().Activate();
        }
    }
}
