using UnityEngine;
using UnityEngine.UI;

public enum PipeTileShape
{
    None,
    Line,
    Corner
}

public enum PipeTileRotation
{
    Degrees0,
    Degrees90,
    Degrees180,
    Degrees270,
}

[ExecuteInEditMode]
public class PipeTileScript : MonoBehaviour
{
    [SerializeField]
    private PipeTileShape _pipeTileShape;

    [SerializeField]
    private PipeTileRotation _pipeTileRotation;

    [SerializeField]
    private Vector2 _gridPosition;

    [SerializeField]
    private bool _isConnected;

    [SerializeField]
    private Sprite _linePipeSprite;

    [SerializeField]
    private Sprite _cornerPipeSprite;

    [SerializeField]
    private Sprite _linePipeFilledSprite;

    [SerializeField]
    private Sprite _cornerPipeFilledSprite;

    private Button _button;
    private Text _text;
    private Image _image;

    private Core.Loggers.ILogger _logger;
    private Core.Mediators.IMessenger _messenger;

    // Use this for initialization
    private void Start()
    {
        _button = GetComponent<Button>();
        _logger = Game.Container?.Resolve<Core.Loggers.ILoggerFactory>()?.Create(this);
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<Text>();

        _messenger = Game.Container?.Resolve<Core.Mediators.IMessenger>();

        UpdateLayout();
    }

    public void SetIsConnected(bool isConnected)
    {
        _isConnected = isConnected;
        UpdateLayout();
    }

    public bool GetIsConnected() => _isConnected;

    public Vector2 GetPosition()
    {
        return _gridPosition;
        //var t = this;

        //var rectTransform = GetComponent<RectTransform>();

        //int xPosition = (int)rectTransform.position.x / 100;
        //int yPosition = (int)Mathf.Abs(rectTransform.position.y / 50);

        //return new Vector2(xPosition, yPosition);
    }

    public PipeTileShape GetShape() => _pipeTileShape;

    public PipeTileRotation GetRotation() => _pipeTileRotation;

    private void UpdateLayout()
    {
        if(_image == null)
            _image = GetComponent<Image>();

        _image.color = new Color(1, 1, 1, 1);
        if (_pipeTileShape == PipeTileShape.None)
        {
            _image.sprite = null;
            _image.color = new Color(1, 1, 1, 0);
        }
        else if (_pipeTileShape == PipeTileShape.Corner)
        {
            if (_isConnected)
                _image.sprite = _cornerPipeFilledSprite;
            else
                _image.sprite = _cornerPipeSprite;
        }
        else if (_pipeTileShape == PipeTileShape.Line)
        {
            if(_isConnected)
                _image.sprite = _linePipeFilledSprite;
            else
                _image.sprite = _linePipeSprite;
        }

        var sprite = _image;
        
        if(_pipeTileRotation == PipeTileRotation.Degrees0)
        {
            sprite.transform.eulerAngles = new Vector3(0, 0, 0);            
        }
        else if (_pipeTileRotation == PipeTileRotation.Degrees90)
        {
            sprite.transform.eulerAngles = new Vector3(0, 0, -90);
        }
        else if (_pipeTileRotation == PipeTileRotation.Degrees180)
        {
            sprite.transform.eulerAngles = new Vector3(0, 0, -180);
        }
        else if (_pipeTileRotation == PipeTileRotation.Degrees270)
        {
            sprite.transform.eulerAngles = new Vector3(0, 0, -270);
        }
    }

    public void OnClick()
    {
        int newPipeRotation = (int)_pipeTileRotation + 1;
        int maxPipeRotations = 3;
        if (newPipeRotation > maxPipeRotations)
            newPipeRotation = 0;

        _pipeTileRotation = (PipeTileRotation)newPipeRotation;

        UpdateLayout();

        _messenger?.Publish(new PipeTileRotatedMessage(this));
    }

    // Update is called once per frame
    private void Update()
    {
        if(Application.isEditor)
            UpdateLayout();
    }

    public override string ToString()
    {
        return _isConnected.ToString() + "" + GetPosition().ToString() + " " + _pipeTileShape + " " + _pipeTileRotation ;
    }
}

public class PipeTileRotatedMessage : Core.Mediators.Message
{
    public PipeTileScript PipeTile { get; }

    public PipeTileRotatedMessage(PipeTileScript sender) : base(sender)
    {
        PipeTile = sender;
    }
}
