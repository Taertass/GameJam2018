using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationScene3Handler : MonoBehaviour
{
    private float _time;

    private void Start()
    {
        _time = Time.time;
    }

    private void Update()
    {
        if (Time.time - _time > 12)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
