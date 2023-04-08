using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PushFunction")]
public class PushableFunction : ScriptableObject
{
    [SerializeField] // velocity: push force, maxVelocity: max speed, dragVelocity: friction force, returnDelay: seconds before returning
    float velocity = 1, maxVelocity, dragVelocity, returnDelay;
    [SerializeField] // reqCharge: Charge Level required to push, pushNumbers: number of pushes before maxVelocity is removed
    int reqCharge = 1, pushNumbers = 0;
    // activatable: runs functions inside this class, changeColor: -, destroy: destroys pushed object on completion of all tasks, animation: plays animation
    // canReturn: returns if speed is capped, constraints: -, stackPush: can be pushed while moving from last push
    [SerializeField]
    bool activatable, changeColor, destroy, animation, canReturn, constraintX, constraintY, constraintZ, stackPush = true, turnKinematicOff = false;
    [SerializeField] // color: color for changeColor
    Color color;
    [SerializeField] // animationName: name of the bool used in the animator for specific animation state (connected to default state in a 2 way connection)
    string animationName;
    [SerializeField]
    AudioClip sound;
    [SerializeField]
    GameObject instance, instanceLocation;

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

    public bool kinematicOff
    {
        get
        {
            return turnKinematicOff;
        }
        set
        {
            turnKinematicOff = value;
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
            if (sound != null)
            {
                AudioSource.PlayClipAtPoint(sound, pushedObj.transform.position, GameManager.Instance.GetFloat(Settings.SFX));
            }
            if(instance != null)
            {
                if(instanceLocation != null)
                {
                    Instantiate(instance, instanceLocation.transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(instance, pushedObj.transform.position, Quaternion.identity);
                }
                
            }
            if (!animation)
            {
                if(destroy)
                {
                    Destroy(pushedObj);
                    return;
                }
                if(turnKinematicOff)
                {
                    pushedObj.GetComponent<Rigidbody>().isKinematic = false;
                }
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
