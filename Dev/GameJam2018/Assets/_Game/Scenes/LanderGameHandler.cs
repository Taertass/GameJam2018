using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanderGameHandler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _player;

    [SerializeField]
    private GameObject _outOfBoundsMenu;

    [SerializeField]
    private GameObject _crashedMenu;

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private GameObject _landedToHardMenu;

    [SerializeField]
    private GameObject _introTextOverlay;

    private bool _hasLanded = false;

    private Core.Loggers.ILogger _logger;
    private Core.Mediators.IMessenger _messenger;

    private Core.Mediators.ISubscriptionToken _landerEnteredLandingAreaMessageToken;
    private Core.Mediators.ISubscriptionToken _landerExitedLandingAreaMessageToken;
    private Core.Mediators.ISubscriptionToken _landerCrashedMessageToken;
    private Core.Mediators.ISubscriptionToken _landerLeftBoundsMessageToken;

    private void Start()
    {
        _logger = Game.Container?.Resolve<Core.Loggers.ILoggerFactory>()?.Create(this);
        _messenger = Game.Container?.Resolve<Core.Mediators.IMessenger>();

        _landerEnteredLandingAreaMessageToken = _messenger.Subscribe((LanderEnteredLandingAreaMessage LanderEnteredLandingAreaMessage) =>
        {
            _logger?.Log("LanderEnteredLandingAreaMessage");
            CheckVictoryConditions();
        });

        _landerExitedLandingAreaMessageToken = _messenger.Subscribe((LanderExitedLandingAreaMessage LanderEnteredLandingAreaMessage) =>
        {
            _logger?.Log("LanderExitedLandingAreaMessage");
        });

        _landerCrashedMessageToken = _messenger.Subscribe((LanderCrashedMessage landerCrashedMessage) =>
        {
            if(!_hasLanded)
            {
                ShowMenu(_crashedMenu);
                _logger?.Log("LanderCrashedMessage");
            }
        });

        _landerLeftBoundsMessageToken = _messenger.Subscribe((LanderLeftBoundsMessage landerLeftBoundsMessage) =>
        {
            if (!_hasLanded)
            {
                ShowMenu(_outOfBoundsMenu);
                _logger?.Log("LanderLeftBoundsMessage");
            }
        });

        StartCoroutine(StartIntroTextFadeOut());
    }

    private IEnumerator StartIntroTextFadeOut()
    {
        yield return new WaitForSeconds(5);

        CanvasGroup canvasGroup = _introTextOverlay.GetComponent<CanvasGroup>();
        while(canvasGroup != null  && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 1 * Time.deltaTime;
            yield return null;
        }
        _introTextOverlay.SetActive(false);
    }

    private void OnDestroy()
    {
        _landerEnteredLandingAreaMessageToken.Dispose();
        _landerExitedLandingAreaMessageToken.Dispose();
        _landerCrashedMessageToken.Dispose();
        _landerLeftBoundsMessageToken.Dispose();
    }

    private void CheckVictoryConditions()
    {
        float landingSpeed = _player.velocity.magnitude;
        _logger?.Log("Landing Speed " + landingSpeed);

        if(landingSpeed > 1)
        {
            ShowMenu(_landedToHardMenu);
        }
        else
        {
            _messenger?.Publish(new LanderLandedSuccessfullyMessage(this));
            _hasLanded = true;
        }
    }

    private void SetPauseGame(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseMenu.activeSelf)
                HideMenu(_pauseMenu);
            else
                ShowMenu(_pauseMenu);
        }
    }

    private GameObject _activeMenu;

    private void HideActiveMenu()
    {
        if (_activeMenu != null)
            HideMenu(_activeMenu);
    }

    private void ShowMenu(GameObject menu)
    {
        if (menu.activeSelf)
            return;

        SetPauseGame(true);

        _activeMenu = menu;
        menu.SetActive(true);
    }

    private void HideMenu(GameObject menu)
    {
        if (!menu.activeSelf)
            return;

        SetPauseGame(false);
        _activeMenu = null;
        menu.SetActive(false);
    }

    public void RetryLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }

    public void Continue()
    {
        HideActiveMenu();   
    }
}

public class LanderLandedSuccessfullyMessage : Core.Mediators.Message
{
    public LanderLandedSuccessfullyMessage(object sender) : base(sender)
    {

    }
}
