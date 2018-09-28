using System;
using UnityEngine;

public class StartTriggerScript : MonoBehaviour
{
    [SerializeField]
    private MovableEnemy[] _targets;

    public event EventHandler<EventArgs> PlayerEnteredEvent;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(20, 1));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            foreach(MovableEnemy movableEnemy in _targets)
            {
                movableEnemy.StartMoving();
            }

            PlayerEnteredEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
