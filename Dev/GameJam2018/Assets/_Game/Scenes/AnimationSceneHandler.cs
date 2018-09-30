using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationSceneHandler : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(ToNextSceenAfterDelay());
    }

    private IEnumerator ToNextSceenAfterDelay()
    {
        yield return new WaitForSeconds(18);
        SceneManager.LoadSceneAsync(0);
    }

    private void Update()
    {

    }
}
