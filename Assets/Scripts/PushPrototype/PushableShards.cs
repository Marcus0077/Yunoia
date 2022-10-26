using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableShards : PushableBreakable
{
    [SerializeField]
    GameObject shard;
    [SerializeField]
    int shardCount;
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        for(int i = 0; i < shardCount; i++)
        {
            Vector3 randPosition = (Random.Range(-1f,1f) + transform.position.x) * Vector3.right + (Random.Range(-1f, 1f) + transform.position.y) * Vector3.up + (Random.Range(-1f, 1f) + transform.position.z) * Vector3.forward;
            GameObject shardCopy = Instantiate(shard, randPosition, Quaternion.identity, transform);
            Vector3 direction = shardCopy.transform.position - pusher.transform.position;
            direction = direction.normalized;
            float distance = Vector3.Distance(shardCopy.transform.position, pusher.transform.position);
            shardCopy.GetComponent<Pushable>().Pushed(pusher.GetComponent<AbilityPush>().range/distance * direction, chargeLevel, totalCharges, pusher);
        }
        base.Pushed(force, chargeLevel, totalCharges, pusher);
    }

    public override IEnumerator DisplayAnimation()
    {
        anim.SetBool("break", true);
        GetComponent<Collider>().enabled = false;
        yield return WaitForAnimation("break"); // have real name
        Destroy(gameObject);
    }

    public override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
