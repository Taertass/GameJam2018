using UnityEngine;

public class LanderRocketScript : MonoBehaviour
{    
    private float _speedFactor = 200f;
    private float _maxSpeed = 5f;

    private float _rotationSpeedFactor = 100f;

    [SerializeField]
    private GameObject _fire;

    private Rigidbody2D _rigidbody;
    private Transform _transform;

    private AudioSource _audioSource;

    private Core.Loggers.ILogger _logger;
    private Core.Mediators.IMessenger _messenger;
    private Core.Mediators.ISubscriptionToken _landerLandedSuccessfullyMessageToken;

    private bool _isLanded = false;

    // Use this for initialization
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();

        _audioSource = GetComponent<AudioSource>();

        _logger = Game.Container?.Resolve<Core.Loggers.ILoggerFactory>()?.Create(this);
        _messenger = Game.Container?.Resolve<Core.Mediators.IMessenger>();

        _landerLandedSuccessfullyMessageToken = _messenger.Subscribe((LanderLandedSuccessfullyMessage landerLandedSuccessfullyMessage) =>
        {
            _isLanded = true;
        });
    }

    // Update is called once per frame
    private void Update()
    {
        if(!_isLanded)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }

                if (!_fire.activeSelf)
                {
                    _fire.SetActive(true);
                }

                Vector2 forceToAdd = new Vector2(0, _speedFactor * Time.deltaTime);
                forceToAdd = Rotate(forceToAdd, _rigidbody.rotation);

                _rigidbody.AddForce(forceToAdd);


            }
            else
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.Stop();
                    _audioSource.time = 0;
                }

                if (_fire.activeSelf)
                {
                    _fire.SetActive(false);
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                _rigidbody.MoveRotation(_rigidbody.rotation + (_rotationSpeedFactor * Time.deltaTime));
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                _rigidbody.MoveRotation(_rigidbody.rotation - (_rotationSpeedFactor * Time.deltaTime));
            }

            //Limit Speed
            if (_rigidbody.velocity.magnitude > _maxSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
            }
        }
        else
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
                _audioSource.time = 0;
            }

            if (_fire.activeSelf)
            {
                _fire.SetActive(false);
            }
        }
        
    }

    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
