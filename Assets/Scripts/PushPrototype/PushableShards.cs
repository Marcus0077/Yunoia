using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableShards : PushableAnimatable
{
    [SerializeField]
    GameObject shard;
    [SerializeField]
    int shardCount;
    [SerializeField]
    float spawnDistance = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    public override bool Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        if(base.Pushed(force, chargeLevel, totalCharges, pusher))
        {
            for (int i = 0; i < shardCount; i++)
            {
                Vector3 randPosition = (Random.Range(-spawnDistance, spawnDistance) + transform.position.x) * Vector3.right + transform.position.y * Vector3.up + (Random.Range(-spawnDistance, spawnDistance) + transform.position.z) * Vector3.forward;
                GameObject shardCopy = Instantiate(shard, randPosition, Quaternion.identity, transform);
                Vector3 direction = shardCopy.transform.position - transform.position; // explode outwards
                //Vector3 direction = shardCopy.transform.position - pusher.transform.position; explode away from push
                direction = direction.normalized;
                float distance = Vector3.Distance(shardCopy.transform.position, pusher.transform.position);
                shardCopy.GetComponent<Pushable>().Pushed(pusher.GetComponent<AbilityPush>().range / distance * direction * shardCopy.GetComponent<Pushable>().pushSpeed * chargeLevel / totalCharges, chargeLevel, totalCharges, pusher);
            }
            return true;
        } else
        {
            return false;
        }
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
