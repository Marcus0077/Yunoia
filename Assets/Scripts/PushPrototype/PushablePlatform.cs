using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushablePlatform : Pushable
{
    //Vector3 playerPosition;
    Vector3 prevPosition;

    //void OnTriggerEnter(Collider other)
    //{
    //    GameObject hit = other.gameObject;
    //    if (hit.tag == "Player" || hit.tag == "Clone")
    //    {
    //        Vector3 tempOriginalScale = transform.localScale;
    //        transform.localScale = Vector3.one;
    //        playerPosition = transform.InverseTransformPoint(hit.transform.position);
    //        transform.localScale = tempOriginalScale;
    //        hit.transform.SetParent(transform);
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.tag == "Player" || hit.tag == "Clone")
        {
            prevPosition = transform.position;
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    GameObject hit = other.gameObject;
    //    if (hit.tag == "Player" || hit.tag == "Clone")
    //    {
    //        hit.transform.SetParent(null);
    //    }
    //}

    void OnTriggerStay(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.tag == "Player" || hit.tag == "Clone")
        {
            hit.transform.position -= prevPosition - transform.position;
            prevPosition = transform.position;
        }
    }

    //void FixedUpdate()
    //{
        //if(transform.childCount > 0)
        //{
        //    if (transform.GetChild(0).tag == "Player")
        //    {
        //        BasicMovementPlayer player = transform.GetChild(0).GetComponent<BasicMovementPlayer>();
        //        if (player.isGrounded && player.moveDirection == Vector2.zero)
        //        {
        //            //transform.GetChild(0).transform.position = new Vector3(transform.position.x, transform.GetChild(0).transform.position.y, transform.position.z);
        //            //transform.GetChild(0).transform.position = transform.position + playerPosition;
        //        }
        //        else
        //        {
        //            //playerPosition = transform.InverseTransformPoint(player.transform.position);
        //        }
        //    }
        //    else if (transform.GetChild(0).tag == "Clone")
        //    {
        //        BasicMovementClone player = transform.GetChild(0).GetComponent<BasicMovementClone>();
        //        if (player.isGrounded && player.moveDirection == Vector2.zero)
        //        {
        //            //transform.GetChild(0).transform.position = new Vector3(transform.position.x, transform.GetChild(0).transform.position.y, transform.position.z);
        //            //transform.GetChild(0).transform.position = transform.position + playerPosition;
        //        }
        //        else
        //        {
        //            //playerPosition = transform.InverseTransformPoint(player.transform.position);
        //        }
        //    }
        //}
        //CapSpeed();
    //}
}
