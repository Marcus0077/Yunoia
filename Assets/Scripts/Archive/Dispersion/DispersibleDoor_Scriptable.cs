using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Door")]
public class DispersibleDoor_Scriptable : Dispersible_Scriptable
{
    [SerializeField]
    DoorDisperse door;
    [SerializeField]
    float time, doorSpeed;
    [SerializeField]
    Vector3 dir;
    public float duration
    {
        get { return time; }
        private set { time = value; }
    }
    public float speed
    {
        get { return doorSpeed; }
        private set { doorSpeed = value; }
    }
    public Vector3 direction
    {
        get { return dir; }
        private set { dir = value; }
    }
    public DoorDisperse Spawn(Transform parent, Vector3 position)
    {
        return Instantiate(door, position, parent.rotation, parent) as DoorDisperse;
    }
}