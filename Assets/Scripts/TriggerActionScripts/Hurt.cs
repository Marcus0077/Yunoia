using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hurt : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    // Actions to take place upon touching the collider
    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.CompareTag("Player"))
            {
                GameManager.Instance.Player.GetComponent<BasicMovement>().Hurt(parent.transform);
            }
            else if (contact.otherCollider.CompareTag("Clone"))
            {
                GameObject.FindObjectOfType<ExitClone>().despawnClone = true;
            }
        }
    }

    IEnumerator TrackSpeed(ContactPoint point)
    {
        //Vector3 oldpos = GetComponent<Collider>().bounds.center;
        yield return new WaitForFixedUpdate();
        //Vector3 newpos = GetComponent<Collider>().bounds.center;
        //oldpos = new Vector3(oldpos.x, 0, oldpos.z);
        //newpos = new Vector3(newpos.x, 0, newpos.z);
        //float speed = ((newpos - oldpos) * 50f).magnitude;
        Vector3 direction = GameManager.Instance.Player.transform.position - parent.transform.position;//-point.normal;
        direction.Normalize();
        Debug.Log(direction);
        GameManager.Instance.Player.GetComponent<Rigidbody>().AddForce(direction * 100, ForceMode.Acceleration);
    }

    //private IEnumerator FadeThenDie()
    //{
    //    GameManager.Instance.dying = true;
    //    if (GameObject.FindObjectOfType<FadeBlack>() != null)
    //    {
    //        GameObject.FindObjectOfType<FadeBlack>().FadeToBlack(1.5f);
    //    }

    //    yield return new WaitForSeconds(1.5f);
    //    GameObject.FindWithTag("MainCanvas").transform.Find("Lose Screen Object").gameObject.SetActive(true);
    //    GameManager.Instance.dying = false;
    //}
}
