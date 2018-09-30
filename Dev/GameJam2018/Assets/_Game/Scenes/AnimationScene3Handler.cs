using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationScene3Handler : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ToNextSceenAfterDelay());
    }

    private IEnumerator ToNextSceenAfterDelay()
    {
        yield return new WaitForSeconds(12);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {

    }
}
