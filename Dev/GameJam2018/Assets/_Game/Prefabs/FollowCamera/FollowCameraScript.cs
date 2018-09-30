using Core.Mediators;
using UnityEngine;

public class FollowCameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private float _targetOffset = 2.10f;

    private float _offset = 2.10f; //3.60f;

    private Transform _transform;
    private Core.Loggers.ILogger _logger;

    private float _sceneStartTime;
    private float _startTime;
    private bool isStarted = false;
    private bool isFinished = false;
    private bool isCrashed = false;

    private IMessenger _messenger;
    private ISubscriptionToken _liftofToken;
    private ISubscriptionToken _playerEnteredGoalMessageToken;
    private ISubscriptionToken _playerCrashedMessageToken;

    private float shakeAmount = 0.02f;

    private Vector3 _cameraStartPosition;

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

        _playerCrashedMessageToken = _messenger.Subscribe((PlayerCrashedMessage playerCrashedMessage) =>
        {
            isCrashed = true;
        });

        _sceneStartTime = Time.time;
        //_transform.position = new Vector3(transform.position.x, _target.position.y + _offset, transform.position.z);

        _offset = _transform.position.y - _target.position.y;

        _cameraStartPosition = transform.position;
    }

    private void OnDestroy()
    {
        _liftofToken.Dispose();
        _playerEnteredGoalMessageToken.Dispose();
        _playerCrashedMessageToken.Dispose();
    }

    private bool shouldShake = false;

    // Update is called once per frame
    private void Update()
    {
        if (!shouldShake && Time.time - _sceneStartTime > 6)
        {
            shouldShake = true;
        }
        
        if(isStarted && !isFinished)
        {
            if (Time.time - _startTime > 1.2)
            {
                if (_offset > _targetOffset)
                    _offset -= 0.9f * Time.deltaTime;

                _transform.position = new Vector3(transform.position.x, _target.position.y + _offset, transform.position.z);
            }
        }

        if(shouldShake && !isCrashed)
        {
            if (isStarted)
                _cameraStartPosition = _transform.position; 

            //Add Shake
            _transform.position = _cameraStartPosition + Random.insideUnitSphere * shakeAmount;

            if(!isStarted)
            {
                _offset = _transform.position.y - _target.position.y;
            }
        }
        
    }
}
