using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField]
    float speed = 5;
    CharacterController character;
    private IEnumerator coroutine;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private IEnumerator WaitForAnimation(string boolName)
    {
        do
        {
            yield return null;
        } while (anim.GetBool(boolName));
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutine == null)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (move.magnitude > 0)
            {
                anim.SetBool("move", true);
            }
            else
            {
                anim.SetBool("move", false);
            }
            character.Move(move * Time.deltaTime * speed);
        }
    }
}