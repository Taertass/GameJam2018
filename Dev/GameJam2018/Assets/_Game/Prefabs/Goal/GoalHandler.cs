using UnityEngine;

public class GoalHandler : MonoBehaviour
{

    private Core.Loggers.ILogger _logger;
    private Core.Mediators.IMessenger _messenger;

    private void Start()
    {
        Core.Loggers.ILoggerFactory loggerFactory = Game.Container.Resolve<Core.Loggers.ILoggerFactory>();
        _logger = loggerFactory.Create(this);

        _messenger = Game.Container.Resolve<Core.Mediators.IMessenger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _logger.Log("Player entered goal");
            _messenger.Publish(new PlayerEnteredGoalMessage(this));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(10, 1));
    }
}
