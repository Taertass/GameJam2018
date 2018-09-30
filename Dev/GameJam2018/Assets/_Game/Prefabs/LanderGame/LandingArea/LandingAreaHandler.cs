using UnityEngine;

public class LandingAreaHandler : MonoBehaviour
{

    private Core.Loggers.ILogger _logger;
    private Core.Mediators.IMessenger _messenger;

    private void Start()
    {
        _logger = Game.Container?.Resolve<Core.Loggers.ILoggerFactory>()?.Create(this);
        _messenger = Game.Container?.Resolve<Core.Mediators.IMessenger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _messenger?.Publish(new LanderEnteredLandingAreaMessage(this));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _messenger?.Publish(new LanderExitedLandingAreaMessage(this));
        }
    }


}

public class LanderEnteredLandingAreaMessage : Core.Mediators.Message
{
    public LanderEnteredLandingAreaMessage(object sender) : base(sender)
    { }
}

public class LanderExitedLandingAreaMessage : Core.Mediators.Message
{
    public LanderExitedLandingAreaMessage(object sender) : base(sender)
    { }
}
