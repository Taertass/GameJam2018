using Core.Mediators;
using UnityEngine;


public enum MoveDirection
{
    LeftToRight,
    RightToLeft
}

public class MovableEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private MoveDirection _moveDirection;

    [SerializeField]
    private bool _isMoving = false;

    private Rigidbody2D _rigidbody;
    private IMessenger _messenger;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _messenger = Game.Container.Resolve<IMessenger>();
    }

    public void StartMoving()
    {
        _isMoving = true;
    }

    private void Update()
    {
        if (_isMoving)
        {
            if (_moveDirection == MoveDirection.LeftToRight)
                _rigidbody.velocity = new Vector2(_speed, 0);
            else if (_moveDirection == MoveDirection.RightToLeft)
                _rigidbody.velocity = new Vector2(-1 * _speed, 0);
        }
        else if (_isDead)
        {
            //Do nothing
        }
        else
            _rigidbody.velocity = new Vector2(0, 0);
    }

    private bool _isDead = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Fire")
        {
            Collider2D collider2D = GetComponent<Collider2D>();
            if(collider2D != null)
                collider2D.enabled = false;

            _isMoving = false;
            _isDead = true;
            _rigidbody.gravityScale = 1;
            _rigidbody.mass = 2;
            _rigidbody.AddTorque(1000f);

        }
        if (collision.tag == "Player")
        {
            _messenger.Publish(new PlayerCrashedMessage(this));
        }
    }
}
