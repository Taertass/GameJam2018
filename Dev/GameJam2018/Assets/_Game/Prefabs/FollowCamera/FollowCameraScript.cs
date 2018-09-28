using UnityEngine;

public class FollowCameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private float _offset = 3.57f;

    private Transform _transform;
    private Core.Loggers.ILogger _logger;

    private void Start()
    {
        _transform = GetComponent<Transform>();

        Core.Loggers.ILoggerFactory loggerFactory = Game.Container.Resolve<Core.Loggers.ILoggerFactory>();
        _logger = loggerFactory.Create(this);
        if (_target == null)
        {
            _logger.Error("No target found");
        }
            
    }

    // Update is called once per frame
    private void Update()
    {
        if(_target != null)
        {
            _transform.position = new Vector3(transform.position.x, _target.position.y + _offset, transform.position.z);
        }
    }
}
