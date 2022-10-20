using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dispersible")]
public class Dispersible_Scriptable : ScriptableObject
{
    [SerializeField]
    Color color;
    //[SerializeField]
    
    public Color newColor
    {
        get { return color; }
        private set { color = value; }
    }
}
