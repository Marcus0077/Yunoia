using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class DeathText : MonoBehaviour
{
    [SerializeField]
    [TextArea]
    string[] quotes;
    [SerializeField]
    bool random = false;
    [SerializeField]
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if(random)
        {
            text.text = quotes[Random.Range(0,quotes.Length-1)];
        }
        else
        {
            if(GameManager.Instance.deathIndex >= quotes.Length)
            {
                text.text = quotes[quotes.Length-1];
            }
            else
            {
                text.text = quotes[GameManager.Instance.deathIndex];
                GameManager.Instance.deathIndex++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
