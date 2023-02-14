using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicInterpolation : MonoBehaviour 
{
    public Rigidbody body;
 
    public Vector3 startPos;
    public Vector3 endPos;
 
    private Vector3 targetPos;
 
    void Awake() 
    {
        targetPos = startPos;
    }
 
    void FixedUpdate() 
    {
        Vector3 currentPos = transform.position;
 
        if(currentPos.y >= startPos.y) 
        {
            targetPos = endPos;
        }
        else if (currentPos.y <= endPos.y) 
        {
            targetPos = startPos;
        }
 
        Vector3 targetDirection = (targetPos - currentPos).normalized;
        body.MovePosition(currentPos + targetDirection * Time.deltaTime * 2f);
    }
}