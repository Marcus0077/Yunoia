using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class MusicPersist : MonoBehaviour
{
    string curr;
    public static MusicPersist instance;
    AudioSource audioSource;
    [SerializeField]
    SceneMusic<string, AudioClip>[] music;

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
        audioSource = GetComponent<AudioSource>();
        GetMusic(SceneManager.GetActiveScene().name);
        Play();       
    }

    void OnEnabled()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetMusic(scene.name);
    }

    void GetMusic(string key)
    {
        List<AudioClip> clips = (from sceneMusic in music where sceneMusic.SceneName == key select sceneMusic.BGM).ToList();
        if (clips.Count > 0)
        {
            Debug.Log(clips.Count);
            audioSource.clip = clips[0];
        }
        else
        {
            audioSource.clip = null;
        }
        Play();
    }

    void Play()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class SceneMusic<TKey, TValue>
{
    public SceneMusic()
    {
    }

    public SceneMusic(TKey key, TValue value)
    {
        SceneName = key;
        BGM = value;
    }

    public TValue GetClip(TKey key)
    {
        return BGM;
    }

    [field: SerializeField] public TKey SceneName { set; get; }
    [field: SerializeField] public TValue BGM { set; get; }
}