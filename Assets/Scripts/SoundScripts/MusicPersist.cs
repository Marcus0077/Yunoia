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
    bool stop;
    float oldVol;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
        audioSource = GetComponent<AudioSource>();
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
        //GetMusic(scene.name);
    }

    void GetMusic(string key)
    {
        List<AudioClip> clips = (from sceneMusic in music where sceneMusic.SceneName == key select sceneMusic.BGM).ToList();
        if (clips.Count > 0)
        {
            if(audioSource.clip != null)
            {
                if(audioSource.clip.name == clips[0].name)
                {
                    return;
                }
            }
            audioSource.clip = clips[0];
            audioSource.Play();
        }
        else
        {
            //oldVol = audioSource.volume;
            stop = true;
            //audioSource.clip = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(stop && audioSource.isPlaying)
        {
            if(audioSource.volume <= 0.005f)
            {
                audioSource.Stop();
                audioSource.volume = oldVol;
                stop = false;
            }
            audioSource.volume *= 0.95f;
        }
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