using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDispersible : Dispersible
{
    [SerializeField]
    Vector3 doorStart;
    Door door;
    DispersibleDoor_Scriptable doorDisp;
    new void Start()
    {
        base.Start();
        doorDisp = (disp as DispersibleDoor_Scriptable);
        door = doorDisp.Spawn(transform, doorStart);
    }

    public override void HitAction(CharacterMoveOLDVERSION character, RaycastHit rayHit)
    {
        character.lightTarget = null;
        mat.color = doorDisp.newColor;
        door.Open(doorDisp.duration,doorDisp.speed,doorDisp.direction);
    }
}
