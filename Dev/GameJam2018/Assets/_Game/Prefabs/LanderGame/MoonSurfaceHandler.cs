using UnityEngine;

public class MoonSurfaceHandler : MonoBehaviour
{

    private Core.Loggers.ILogger _logger;
    private Core.Mediators.IMessenger _messenger;

    private void Start()
    {
        _logger = Game.Container?.Resolve<Core.Loggers.ILoggerFactory>()?.Create(this);
        _messenger = Game.Container?.Resolve<Core.Mediators.IMessenger>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _messenger?.Publish(new LanderCrashedMessage(this));
        }
    }
}

public class LanderCrashedMessage : Core.Mediators.Message
{
    public LanderCrashedMessage(object sender) : base(sender)
    { }
}
