using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimatedSceen1Handler : MonoBehaviour
{
   
    // Use this for initialization
    private void Start()
    {
        StartCoroutine(ToNextSceenAfterDelay());
    }

    private IEnumerator ToNextSceenAfterDelay()
    {

        yield return new WaitForSeconds(15);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
