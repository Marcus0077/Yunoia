using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    public Hint[] hints;

    public void DisplayHint()
    {
        FindObjectOfType<HintManager>().OpenHint(hints);
    }
}

    [System.Serializable]
    public class Hint
    {
        public string hint;
    }

