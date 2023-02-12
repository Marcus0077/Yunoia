using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TempDestroyer : MonoBehaviour
{
    [SerializeField]
    string nameOfData;
    public List<Pushable> pushables;
    public int count;

    void Start()
    {
        pushables = (from go in new List<Pushable>(FindObjectsOfType<Pushable>()) where go.Data() != null && go.Data().name == nameOfData select go).ToList();
        for(int index = 0; index < pushables.Count; index++)
        {
            TempDestroyerHelper tdh = pushables[index].gameObject.AddComponent<TempDestroyerHelper>();
            tdh.counter = this;
        }
        pushables.Clear();
    }

    void Update()
    {
        if(count <= 0)
        {
            Destroy(gameObject);
        }
    }
}
