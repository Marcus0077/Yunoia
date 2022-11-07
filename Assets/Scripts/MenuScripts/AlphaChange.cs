using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaChange : MonoBehaviour
{
    [SerializeField]
    float alphaSelect, alphaDeselect;
    [SerializeField]
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SelectedColor()
    {
        Color color = image.color;
        color.a = alphaSelect;
        image.color = color;
    }

    public void DeselectedColor()
    {
        Color color = image.color;
        color.a = alphaDeselect;
        image.color = color;
        Debug.Log(image.color.a);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
