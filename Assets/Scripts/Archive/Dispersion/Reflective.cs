using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflective : Dispersible
{
    Vector3 face, direction;
    public override void HitAction(CharacterMoveOLDVERSION character, RaycastHit rayHit)
    {
        Reflective_Scriptable reflectiveDisp = (disp as Reflective_Scriptable);
        mat.color = reflectiveDisp.newColor;
        face = reflectiveDisp.forward;
        direction = reflectiveDisp.direction;
        Reflect(character, rayHit);
    }

    public void Reflect(CharacterMoveOLDVERSION character, RaycastHit rayHit)
    {
        character.lightTarget = null;
        Vector3 roundedAngle = new Vector3(Mathf.Round(rayHit.normal.x), Mathf.Round(rayHit.normal.y),Mathf.Round(rayHit.normal.z));
        if (face == roundedAngle)
        {
            character.DisperseForward(rayHit.point, direction);
        } else if (direction == roundedAngle)
        {
            character.DisperseForward(rayHit.point, face);
        }
    }
}
