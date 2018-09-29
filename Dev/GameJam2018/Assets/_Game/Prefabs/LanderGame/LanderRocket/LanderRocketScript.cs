using UnityEngine;

public class LanderRocketScript : MonoBehaviour
{    
    private float _speedFactor = 200f;
    private float _maxSpeed = 5f;

    private float _rotationSpeedFactor = 100f;

    private Rigidbody2D _rigidbody;
    private Transform _transform;

    private Core.Loggers.ILogger _logger;


    // Use this for initialization
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();

        _logger = Game.Container?.Resolve<Core.Loggers.ILoggerFactory>()?.Create(this);
    }

    // Update is called once per frame
    private void Update()
    {
        _logger?.Log(transform.forward);

        if (Input.GetKey(KeyCode.Space))
        {
            Vector2 forceToAdd = new Vector2(0, _speedFactor * Time.deltaTime);
            forceToAdd = Rotate(forceToAdd, _rigidbody.rotation);

            _rigidbody.AddForce(forceToAdd);

            
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rigidbody.MoveRotation(_rigidbody.rotation + (_rotationSpeedFactor * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _rigidbody.MoveRotation(_rigidbody.rotation - (_rotationSpeedFactor * Time.deltaTime));
        }

        //Limit Speed
        if (_rigidbody.velocity.magnitude > _maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
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
