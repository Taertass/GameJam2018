using System.Collections;
using UnityEngine;

public class PipeGameHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _introTextOverlay;

    // Use this for initialization
    private void Start()
    {
        StartCoroutine(StartIntroTextFadeOut());
    }

    // Update is called once per frame
    void Update()
    {

    }



    private IEnumerator StartIntroTextFadeOut()
    {
        yield return new WaitForSeconds(5);

        CanvasGroup canvasGroup = _introTextOverlay.GetComponent<CanvasGroup>();
        while (canvasGroup != null && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 1 * Time.deltaTime;
            yield return null;
        }
        _introTextOverlay.SetActive(false);
    }
}
