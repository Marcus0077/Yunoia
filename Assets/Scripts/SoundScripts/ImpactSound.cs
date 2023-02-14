using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource impactSound;
    public BasicMovement player;
    public Rigidbody playerRB;
    private bool canPlaySound = true;

    void FixedUpdate()
    {
        if (playerRB.velocity.y < -2 && player.isGrounded && canPlaySound)
        {
            impactSound.Play();
            canPlaySound = false;
            StartCoroutine(impactSoundCooldown());
        }
    }

    private IEnumerator impactSoundCooldown()
    {
        yield return new WaitForSeconds(0.5f);

        canPlaySound = true;
    }
}
