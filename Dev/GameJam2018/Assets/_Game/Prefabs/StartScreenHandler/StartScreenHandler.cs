using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenHandler : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
