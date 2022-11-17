using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    // Start is called before the first frame update
public AudioSource impactSound;

   void OncollisionEnter(Collision collision)
   {
    if (collision.relativeVelocity.magnitude > 1)
    {
        impactSound.Play();
    }
   }
}
