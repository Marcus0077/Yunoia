using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillIcon : MonoBehaviour
{
    [SerializeField]
    TMP_Text cooldownText;
    [SerializeField]
    RawImage cover;
    [SerializeField]
    AbilityType ability;
    enum AbilityType { push, grapple, clone, dash };
    CooldownGrabber cooldown;
    // Start is called before the first frame update
    void Start()
    {
        cooldown = GameObject.FindGameObjectWithTag("Player").GetComponent<CooldownGrabber>();
    }

    // Update is called once per frame
    void Update()
    {
        float value = cooldown.GetCD(ability.ToString());
        if(value > 0)
        {
            cooldownText.text = (value).ToString("F1");
            cover.CrossFadeAlpha(1, 0, true);
        } else
        {
            cooldownText.text = "";
            cover.CrossFadeAlpha(0, 0, true);
        }
    }
}
