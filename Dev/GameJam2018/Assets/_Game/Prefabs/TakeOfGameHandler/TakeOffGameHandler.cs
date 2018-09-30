using Core.Mediators;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCrashedMessage : Core.Mediators.Message
{
    public PlayerCrashedMessage(object sender) : base(sender)
    {

    }
}

public class LiftoffMessage : Core.Mediators.Message
{
    public LiftoffMessage(object sender) : base(sender)
    {

    }
}

public class TakeOffGameHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _menu;

    [SerializeField]
    private GameObject _rigging;

    [SerializeField]
    private GameObject _crashMenu;

    [SerializeField]
    private AudioClip _countdownAudioClip;

    [SerializeField]
    private Text _countdownText;

    [SerializeField]
    private Text _infoText;

    [SerializeField]
    private GameObject _countdownOverlay;

    private IMessenger _messenger;
    private Core.Loggers.ILogger _logger;

    private ISubscriptionToken _playerEnteredGoalMessageToken;
    private ISubscriptionToken _playerCrashedMessageToken;

    private bool _isPaused;

    private AudioSource _audioSource;

    private int _count = 10;

    private float _startTime;

    private void Start()
    {
        _startTime = Time.time;

        _audioSource = GetComponent<AudioSource>();

        Core.Loggers.ILoggerFactory loggerFactory = Game.Container.Resolve<Core.Loggers.ILoggerFactory>();
        _logger = loggerFactory.Create(this);

        _messenger = Game.Container.Resolve<IMessenger>();

        _playerEnteredGoalMessageToken = _messenger.Subscribe((PlayerEnteredGoalMessage playerEnteredGoalMessage) =>
        {
            StartCoroutine(DelayedScreenSwitch());
        });

        _playerCrashedMessageToken = _messenger.Subscribe((PlayerCrashedMessage playerCrashedMessage) =>
        {
            ShowCrashMenu();
            _audioSource.Stop();
        });

        StartCoroutine(CountdownToLiftof());
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        for(int i = 10; i > -1; i--)
        {
            _countdownText.text = i.ToString();

            if(i == 3 && _infoText != null)
            {
                _infoText.text = "";
            }
            
            yield return new WaitForSeconds(1.08f);
        }

        _countdownText.text = "Liftoff";

        yield return new WaitForSeconds(3);

        if (_countdownOverlay != null)
            _countdownOverlay.SetActive(false);

        _countdownText.text = "";
    }

    private IEnumerator CountdownToLiftof()
    {
        _audioSource.clip = _countdownAudioClip;
        _audioSource.Play();

        yield return new WaitForSeconds(11);

        _messenger.Publish(new LiftoffMessage(this));
    }

    private IEnumerator DelayedScreenSwitch()
    {
        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnContinueButtonPressed()
    {
        HideInGameMenu();
    }

    public void OnQuitButtonPressed()
    {
        HideInGameMenu();
        SceneManager.LoadSceneAsync(0);
    }

    public void OnRetryButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
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

        if (Time.time - _startTime > 6)
        {
            float newX = _rigging.transform.localPosition.x - 0.8f * Time.deltaTime;
            if(newX > -2.9)
            {
                _rigging.transform.localPosition = new Vector3(newX, _rigging.transform.localPosition.y);
            }   
        }
    }
}
