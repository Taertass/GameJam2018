using Core.Mediators;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCrashedMessage : Core.Mediators.Message
{
    public PlayerCrashedMessage(object sender) : base(sender)
    {

    }
}

public class TakeOffGameHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _menu;

    [SerializeField]
    private GameObject _crashMenu;

    private IMessenger _messenger;
    private Core.Loggers.ILogger _logger;

    private ISubscriptionToken _playerEnteredGoalMessageToken;
    private ISubscriptionToken _playerCrashedMessageToken;

    private bool _isPaused;

    private void Start()
    {
        Core.Loggers.ILoggerFactory loggerFactory = Game.Container.Resolve<Core.Loggers.ILoggerFactory>();
        _logger = loggerFactory.Create(this);

        _messenger = Game.Container.Resolve<IMessenger>();

        _playerEnteredGoalMessageToken = _messenger.Subscribe((PlayerEnteredGoalMessage playerEnteredGoalMessage) =>
        {
            _logger.Log("Should change scene");
        });

        _playerCrashedMessageToken = _messenger.Subscribe((PlayerCrashedMessage playerCrashedMessage) =>
        {
            ShowCrashMenu();
        });
    }

    public void OnContinueButtonPressed()
    {
        HideInGameMenu();
    }

    public void OnQuitButtonPressed()
    {
        HideInGameMenu();
        SceneManager.LoadScene(0);
    }

    public void OnRetryButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        _playerEnteredGoalMessageToken.Dispose();
        _playerCrashedMessageToken.Dispose();
    }

    private void ToggelInGameMenu()
    {
        if (_isPaused)
            HideInGameMenu();
        else        
            ShowInGameMenu();
    }

    private void HideInGameMenu()
    {
        if (_isPaused)
        {
            _isPaused = false;
            _menu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void ShowInGameMenu()
    {
        if (!_isPaused)
        {
            _isPaused = true;
            _menu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void HideCrashMenu()
    {
        if (_isPaused)
        {
            _isPaused = false;
            _crashMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void ShowCrashMenu()
    {
        if (!_isPaused)
        {
            _isPaused = true;
            _crashMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggelInGameMenu();
        }
    }
}
