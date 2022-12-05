using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPersist : MonoBehaviour
{
    public static MusicPersist instance;
    string curr;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        curr = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (curr != SceneManager.GetActiveScene().name)
            Destroy(gameObject);
        transform.position = GameObject.FindWithTag("Player").transform.position;
    }
}
