using Core.Mediators;
using UnityEngine;

public class FollowCameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private float _targetOffset = 2.00f;

    private float _offset = 3.40f;

    private Transform _transform;
    private Core.Loggers.ILogger _logger;

    private float _startTime;
    private bool isStarted = false;

    private Core.Mediators.IMessenger _messenger;
    private Core.Mediators.ISubscriptionToken _liftofToken;

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

        _startTime = Time.time;
        _transform.position = new Vector3(transform.position.x, _target.position.y + _offset, transform.position.z);
    }

    private void OnDestroy()
    {
        _liftofToken.Dispose();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isStarted)
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
