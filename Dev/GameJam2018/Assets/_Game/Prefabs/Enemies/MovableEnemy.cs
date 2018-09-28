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

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void StartMoving()
    {
        _isMoving = true;
    }

    private void Update()
    {
        if (_isMoving)
        {
            if(_moveDirection == MoveDirection.LeftToRight)
                _rigidbody.velocity = new Vector2(_speed, 0);
            else if(_moveDirection == MoveDirection.RightToLeft)
                _rigidbody.velocity = new Vector2(-1 * _speed, 0);
        }
        else
            _rigidbody.velocity = new Vector2(0, 0);
    }
}
