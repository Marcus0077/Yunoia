using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDisperse : MonoBehaviour
{
    bool move = false;
    int flip = -1;
    float time, speed;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator StopMove()
    {
        yield return new WaitForSeconds(time);
        move = false;
    }

    public void Open(float duration, float spd, Vector3 dir)
    {
        move = true;
        flip *= -1;
        time = duration;
        speed = spd;
        direction = dir;
        StartCoroutine(StopMove());

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(move)
        {
            transform.Translate(flip * direction * speed * Time.deltaTime);
        }
    }
}
