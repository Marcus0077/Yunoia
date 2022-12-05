using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPersist : MonoBehaviour
{
    string curr;
    public static MusicPersist instance;

    public static MusicPersist Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MusicPersist();
            }
            return instance;
        }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
