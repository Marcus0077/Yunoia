using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleControl : MonoBehaviour
{
    [SerializeField]
    GameObject left, right;
    Vector3 leftPos, rightPos;
    Vector3 oldLeftPos, oldRightPos;
    [SerializeField] // unitDistance: how many unity units should object move for each weight difference in Iris', speed: -, freezeAt: what is the lowest position the scale should go to
    float unitDistance = 1, speed = 1, freezeAtLeft = -1, freezeAtRight = -1, leftMod = 1, rightMod = 1;
    GameObject navHandler;
    [SerializeField] // what axis should the scale move in (currently only y axis is used in game)
    Vector3 axis;
    public float compareValue = 0, frame = -1, freezeAtRightPos, freezeAtLeftPos;
    // Start is called before the first frame update
    void Start()
    {
        frame = -1;
        leftPos = left.transform.position;
        rightPos = right.transform.position;
        oldLeftPos = leftPos;
        oldRightPos = rightPos;
        freezeAtRightPos = oldRightPos.y - freezeAtRight * unitDistance;
        freezeAtLeftPos = oldLeftPos.y - freezeAtLeft * unitDistance;
        compareValue = 0;
        navHandler = GameObject.FindGameObjectWithTag("NavmeshHandler");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if weight was changed on any scale object
        if (compareValue != right.GetComponent<ScaleMeasure>().weight - left.GetComponent<ScaleMeasure>().weight)
        {
            // start at 0 frame to start lerp
            frame = 0;
            compareValue = right.GetComponent<ScaleMeasure>().weight - left.GetComponent<ScaleMeasure>().weight;
            Debug.Log(compareValue);
            oldLeftPos = left.transform.position;
            oldRightPos = right.transform.position;
        }
        if ((right.transform.position.y <= freezeAtRightPos && compareValue >= freezeAtRight) || (left.transform.position.y <= freezeAtLeftPos && compareValue <= -freezeAtLeft))//make it for every axis
        {
            return;
        }
        // Move left and right side until correct position is found for 60/speed frames.
        if (frame * speed <= 60 && frame >= 0)
        {
            // position is calculated using Iris as a unit weight of 1, difference in weight * unitDistance
            left.transform.position = Vector3.Lerp(oldLeftPos, new Vector3(leftPos.x + unitDistance * axis.x * compareValue * leftMod, leftPos.y + unitDistance * axis.y * compareValue * leftMod, leftPos.z + unitDistance * axis.z * compareValue * leftMod), frame / 60f * speed);
            right.transform.position = Vector3.Lerp(oldRightPos, new Vector3(rightPos.x + unitDistance * axis.x * -compareValue * rightMod, rightPos.y + unitDistance * axis.y * -compareValue * rightMod, rightPos.z + unitDistance * axis.z * -compareValue * rightMod), frame / 60f * speed);
            if(navHandler != null)
            {
                //navHandler.GetComponent<NavMeshHandler>().BuildScaleNavMesh();
            }
            frame++;
        }
        else if (frame != -1)
        {
            if (navHandler != null)
            {
                //navHandler.GetComponent<NavMeshHandler>().BuildAllNavMesh();
            }
            frame = -1;
        }
    }
}
