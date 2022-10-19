using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dispersible : MonoBehaviour
{
    protected Material mat;
    [SerializeField]
    protected Dispersible_Scriptable disp;
    // Start is called before the first frame update
    protected void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    public abstract void HitAction(CharacterMoveOLDVERSION character, RaycastHit rayHit);

    // Update is called once per frame
    void Update()
    {
        
    }
}
