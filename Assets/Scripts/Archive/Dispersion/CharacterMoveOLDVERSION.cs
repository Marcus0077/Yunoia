using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveOLDVERSION : MonoBehaviour
{
    [SerializeField]
    float speed = 5;
    CharacterController character;
    private IEnumerator coroutine;
    Animator anim;
    public Dispersible lightTarget;
    RaycastHit rayHit;
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

    //private IEnumerator Disperse()
    //{
    //    //run animation
    //    anim.SetBool("disperse", true);
    //    yield return WaitForAnimation("disperse");
    //    //raycast
    //    DisperseForward(transform.position, transform.forward);
    //    coroutine = null;
    //    //yield return new WaitForSeconds(2f);
    //}

    public void DisperseForward(Vector3 center, Vector3 direction)
    {
    //    RaycastHit hit;
    //    Vector3 p1 = center; //bottom of capsule
    //    Vector3 p2 = p1 + Vector3.up * 0.1F; //top of capsule
    //    print(p1);
    //    print(p2);
    //    //Debug.DrawLine(p1,p2,Color.white);
    //    RenderVolume(p1, p2, character.radius * 0.5F, direction, 4); // visual for the rays path
    //    if (Physics.CapsuleCast(p1, p2, character.radius * 0.5F, direction, out hit, 4)) //last parameter is distance to raycast
    //    {
    //        if(hit.collider.gameObject.GetComponent<Dispersible>())
    //        {
    //            lightTarget = hit.collider.gameObject.GetComponent<Dispersible>();
    //            rayHit = hit;
    //        }
    //    }
    }

    //void RenderVolume(Vector3 p1, Vector3 p2, float radius, Vector3 dir, float distance)
    //{
    //    Transform shape;
    //    shape = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
    //    Destroy(shape.GetComponent<Collider>());
    //    Vector3 scale = new Vector3();
    //    scale.x = radius * 2;
    //    scale.y = Vector3.Distance(p1, p2) + radius * 2;
    //    scale.z = distance + radius * 2;
    //    shape.localScale = scale;
    //    shape.position = (p1 + p2 + dir.normalized * distance) / 2;
    //    shape.rotation = Quaternion.LookRotation(dir, p2 - p1);
    //    shape.GetComponent<Renderer>().enabled = true;
    //    StartCoroutine(EraseRender(shape)); // erase after 1 second
    //}

    //private IEnumerator EraseRender(Transform shape)
    //{
    //    yield return new WaitForSeconds(1);
    //    Destroy(shape.gameObject);
    //}

    // Update is called once per frame
    void Update()
    {
        if (coroutine == null)
        {
            //if (Input.GetKeyDown("space"))
            //{
            //    coroutine = Disperse();
            //    StartCoroutine(coroutine);
            //}
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
        //if (lightTarget != null)
        //{
        //    //Do some action to the hit object
        //    lightTarget.HitAction(this, rayHit);
        //}
    }
}