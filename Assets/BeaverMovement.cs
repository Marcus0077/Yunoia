using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverMovement : MonoBehaviour
{
        public float speed = 2;
        void Update()
        {
            transform.position = new Vector3(Mathf.PingPong(Time.time * speed, 4), transform.position.y, transform.position.z);
        }

    }
