using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Reflective")]
public class Reflective_Scriptable : Dispersible_Scriptable
{
    [SerializeField]
    Vector3 forwardAngle, directionAngle;
    public Vector3 forward
    {
        get { return forwardAngle; }
        private set { forwardAngle = value; }
    }
    public Vector3 direction
    {
        get { return directionAngle; }
        private set { directionAngle = value; }
    }
}