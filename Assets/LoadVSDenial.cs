using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadVSDenial : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(LoadDenial());
    }

    private IEnumerator LoadDenial()
    {
        yield return new WaitForSeconds(61f);

        SceneManager.LoadScene("VSDenial");
    }
}
