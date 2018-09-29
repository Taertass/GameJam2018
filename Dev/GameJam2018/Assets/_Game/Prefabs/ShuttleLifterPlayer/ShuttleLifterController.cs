using Core.Mediators;
using UnityEngine;

public class ShuttleLifterController : MonoBehaviour
{
    [SerializeField]
    private float _horizontalSpeed = 100f;

    [SerializeField]
    private float _verticalMaxSpeed = 1f;

    [SerializeField]
    private float _bounds = 5;

    private bool _isAlive = true;

    private float _verticalSpeed = 100f;

    private AudioSource _audioSource;

    private IMessenger _messenger;
    private ISubscriptionToken _liftoffToken;
    private Core.Loggers.ILogger _logger;
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;

    private float _startTime;
    private bool isStarted = false;

    private void Start()
    {
        Core.Loggers.ILoggerFactory loggerFactory = Game.Container.Resolve<Core.Loggers.ILoggerFactory>();
        _logger = loggerFactory.Create(this);

        _audioSource = GetComponent<AudioSource>();

        _messenger = Game.Container.Resolve<IMessenger>();

        _liftoffToken = _messenger.Subscribe((LiftoffMessage liftoffMessage) =>
        {
            _audioSource.Play();
            isStarted = true;
            _startTime = Time.time;
        });

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();

        _startTime = Time.time;

    }

    private void OnDestroy()
    {
        _liftoffToken.Dispose();
    }

    private void Update()
    {
        if(!isStarted )
        {
            return;
        }

        if (Time.time - _startTime < 1.2)
            return;

        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 1);

        if (!_isAlive)
        {
            _rigidbody2D.velocity = new Vector2(0, 0);

            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rigidbody2D.AddForce(new Vector2(-1 * _horizontalSpeed * Time.deltaTime, 0));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _rigidbody2D.AddForce(new Vector2(_horizontalSpeed * Time.deltaTime, 0));
        }

        Vector2 currentPosition = _transform.position;
        if (currentPosition.x > _bounds )
        {
            _logger.Log("Stuck right side");
            _rigidbody2D.velocity = Vector2.zero;
            transform.position = new Vector2(_bounds, currentPosition.y);
        }
        else if(currentPosition.x < (-1 * _bounds))
        {
            _logger.Log("Stuck left side");
            _rigidbody2D.velocity = Vector2.zero;
            transform.position = new Vector2((-1 * _bounds), currentPosition.y);
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            _messenger.Publish(new PlayerCrashedMessage(this));
            _isAlive = false;
        }   
    }
}
