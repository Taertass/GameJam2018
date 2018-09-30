using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsHandler : MonoBehaviour {

    private Core.Loggers.ILogger _logger;
    private Core.Mediators.IMessenger _messenger;

    private void Start()
    {
        _logger = Game.Container?.Resolve<Core.Loggers.ILoggerFactory>()?.Create(this);
        _messenger = Game.Container?.Resolve<Core.Mediators.IMessenger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _messenger?.Publish(new LanderLeftBoundsMessage(this));
        }
    }
}

public class LanderLeftBoundsMessage : Core.Mediators.Message
{
    public LanderLeftBoundsMessage(object sender) : base(sender)
    { }
}
