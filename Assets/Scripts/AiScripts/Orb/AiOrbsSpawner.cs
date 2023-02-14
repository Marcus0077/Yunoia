using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiOrbsSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject orb, player;
    [SerializeField]
    int count;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            Bounds bounds = GetComponent<Collider>().bounds;
            for (int i = 0; i < count; i++)
            {
                Vector3 position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));
                Instantiate(orb, position, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
