using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiddleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(LoadScene));
    }
    private IEnumerator LoadScene()
    {
       
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("SampleScene");
    }

}
