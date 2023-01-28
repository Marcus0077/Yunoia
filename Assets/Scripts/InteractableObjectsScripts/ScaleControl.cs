using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleControl : MonoBehaviour
{
    [SerializeField]
    GameObject left, right;
    Vector3 leftPos, rightPos;
    Vector3 oldLeftPos, oldRightPos;
    [SerializeField]
    float unitDistance = 1, speed = 1;
    [SerializeField]
    Vector3 axis;
    float compareValue = 0, frame;
    // Start is called before the first frame update
    void Start()
    {
        leftPos = left.transform.position;
        rightPos = right.transform.position;
        oldLeftPos = leftPos;
        oldRightPos = rightPos;
        compareValue = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(compareValue != right.GetComponent<ScaleMeasure>().weight - left.GetComponent<ScaleMeasure>().weight)
        {
            frame = 0;
            compareValue = right.GetComponent<ScaleMeasure>().weight - left.GetComponent<ScaleMeasure>().weight;
            oldLeftPos = left.transform.position;
            oldRightPos = right.transform.position;
        }
        if(frame * speed <= 60)
        {
            left.transform.position = Vector3.Lerp(oldLeftPos, new Vector3(leftPos.x + unitDistance * axis.x * compareValue, leftPos.y + unitDistance * axis.y * compareValue, leftPos.z + unitDistance * axis.z * compareValue), frame / 60f * speed);
            right.transform.position = Vector3.Lerp(oldRightPos, new Vector3(rightPos.x + unitDistance * axis.x * -compareValue, rightPos.y + unitDistance * axis.y * -compareValue, rightPos.z + unitDistance * axis.z * -compareValue), frame / 60f * speed);
            frame++;
        }
    }
}
