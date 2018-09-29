using Core.Mediators;
using UnityEngine;

public class FollowCameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private float _targetOffset = 2.10f;

    private float _offset = 3.60f;

    private Transform _transform;
    private Core.Loggers.ILogger _logger;

    private float _startTime;
    private bool isStarted = false;
    private bool isFinished = false;

    private IMessenger _messenger;
    private ISubscriptionToken _liftofToken;
    private ISubscriptionToken _playerEnteredGoalMessageToken;

    private void Start()
    {
        _transform = GetComponent<Transform>();

        Core.Loggers.ILoggerFactory loggerFactory = Game.Container.Resolve<Core.Loggers.ILoggerFactory>();
        _logger = loggerFactory.Create(this);
        if (_target == null)
        {
            _logger.Error("No target found");
        }

        _messenger = Game.Container.Resolve<IMessenger>();

        _liftofToken = _messenger.Subscribe((LiftoffMessage liftoffMessage) =>
        {
            isStarted = true;
            _startTime = Time.time;
        });

        _playerEnteredGoalMessageToken = _messenger.Subscribe((PlayerEnteredGoalMessage playerEnteredGoalMessage) =>
        {
            isFinished = true;
        });

        _startTime = Time.time;
        _transform.position = new Vector3(transform.position.x, _target.position.y + _offset, transform.position.z);
    }

    private void OnDestroy()
    {
        _liftofToken.Dispose();
        _playerEnteredGoalMessageToken.Dispose();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isStarted || isFinished)
        {
            return;
        }

        if (Time.time - _startTime < 1.2)
            return;

        if (_offset > _targetOffset)
            _offset -= 0.9f * Time.deltaTime;

        if (_target != null)
        {
            _transform.position = new Vector3(transform.position.x, _target.position.y + _offset, transform.position.z);
        }
    }
}
