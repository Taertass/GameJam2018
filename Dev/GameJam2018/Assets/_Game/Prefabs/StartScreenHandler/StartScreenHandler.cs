using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenHandler : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
