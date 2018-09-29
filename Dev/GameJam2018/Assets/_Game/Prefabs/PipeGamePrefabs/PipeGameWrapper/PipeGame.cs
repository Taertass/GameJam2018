using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[ExecuteInEditMode]
public class PipeGame : MonoBehaviour
{
    private Core.Loggers.ILogger _logger;

    private List<PipeTileScript> _pipeTiles;
    private PipeTileScript _startTile;
    private PipeTileScript _endTile;

    private Core.Mediators.IMessenger _messenger;

    // Use this for initialization
    private void Start()
    {
        _pipeTiles = new List<PipeTileScript>();

        _logger = Game.Container?.Resolve<Core.Loggers.ILoggerFactory>()?.Create(this);
        _messenger = Game.Container?.Resolve<Core.Mediators.IMessenger>();

        _pipeTiles.AddRange(GetComponentsInChildren<PipeTileScript>());
        _pipeTiles = _pipeTiles.OrderBy(pt => pt.GetPosition().y).ThenBy(pt => pt.GetPosition().x).ToList();


        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Found {_pipeTiles.Count} tiles");

        _startTile = _pipeTiles.First();
        _endTile = _pipeTiles.Last();

        sb.AppendLine("    Start tile " + _startTile.ToString());
        sb.AppendLine("    End tile " + _endTile.ToString());
        sb.AppendLine("");

        foreach (PipeTileScript pipeTileScript in _pipeTiles)
        {
            sb.AppendLine("    " + pipeTileScript.ToString());
        }

        _logger?.Log(sb.ToString());
        _messenger.Subscribe((PipeTileRotatedMessage pipeTileRotatedMessage) =>
        {
            _logger?.Log("Rotated " + pipeTileRotatedMessage.PipeTile.ToString());
            CheckForGameCompletion();
        });
    }

    // Update is called once per frame
    private void Update()
    {
        if(Application.isEditor)
        {
            CheckForGameCompletion();
        }
    }

    

    private void CheckForGameCompletion()
    {
        _logger?.Log("CheckForGameCompletion");

        bool isConnected = false;

        foreach (PipeTileScript pipeTileScript in _pipeTiles)
        {
            pipeTileScript.SetIsConnected(false);
        }

        PipeTileScript currentPipeTile = _startTile;

        while(currentPipeTile != null)
        {
            currentPipeTile.SetIsConnected(true);

            PipeTileScript connectedPipeTile = null;
            
            PipeTileShape shape = currentPipeTile.GetShape();
            PipeTileRotation rotation = currentPipeTile.GetRotation();

            //Find Connected Pipe
            if (shape == PipeTileShape.Line)
            {
                if (rotation == PipeTileRotation.Degrees0 || rotation == PipeTileRotation.Degrees180)
                {
                    PipeTileScript westTile = GetPipeTileToTheDirection(currentPipeTile, Direction.W);
                    PipeTileScript eastTile = GetPipeTileToTheDirection(currentPipeTile, Direction.E);

                    //Check West
                    if(westTile != null && westTile.GetIsConnected() == false && westTile.GetShape() == PipeTileShape.Line && (westTile.GetRotation() == PipeTileRotation.Degrees0 || westTile.GetRotation() == PipeTileRotation.Degrees180))
                    {
                        connectedPipeTile = westTile;
                    }
                    else if (westTile != null && westTile.GetIsConnected() == false && westTile.GetShape() == PipeTileShape.Corner && westTile.GetRotation() == PipeTileRotation.Degrees90)
                    {
                        connectedPipeTile = westTile;
                    }
                    else if (westTile != null && westTile.GetIsConnected() == false && westTile.GetShape() == PipeTileShape.Corner && westTile.GetRotation() == PipeTileRotation.Degrees180)
                    {
                        connectedPipeTile = westTile;
                    }

                    //Check East
                    else if (eastTile != null && eastTile.GetIsConnected() == false && eastTile.GetShape() == PipeTileShape.Line && (eastTile.GetRotation() == PipeTileRotation.Degrees0 || eastTile.GetRotation() == PipeTileRotation.Degrees180))
                    {
                        connectedPipeTile = eastTile;
                    }
                    else if (eastTile != null && eastTile.GetIsConnected() == false && eastTile.GetShape() == PipeTileShape.Corner && eastTile.GetRotation() == PipeTileRotation.Degrees0)
                    {
                        connectedPipeTile = eastTile;
                    }
                    else if (eastTile != null && eastTile.GetIsConnected() == false && eastTile.GetShape() == PipeTileShape.Corner && eastTile.GetRotation() == PipeTileRotation.Degrees270)
                    {
                        connectedPipeTile = eastTile;
                    }
                }
                else if (rotation == PipeTileRotation.Degrees90 || rotation == PipeTileRotation.Degrees270)
                {
                    PipeTileScript northTile = GetPipeTileToTheDirection(currentPipeTile, Direction.N);
                    PipeTileScript soutTile = GetPipeTileToTheDirection(currentPipeTile, Direction.S);

                    //Check North
                    if (northTile != null && northTile.GetIsConnected() == false && northTile.GetShape() == PipeTileShape.Line && (northTile.GetRotation() == PipeTileRotation.Degrees90 || northTile.GetRotation() == PipeTileRotation.Degrees270))
                    {
                        connectedPipeTile = northTile;
                    }
                    else if (northTile != null && northTile.GetIsConnected() == false && northTile.GetShape() == PipeTileShape.Corner && northTile.GetRotation() == PipeTileRotation.Degrees180)
                    {
                        connectedPipeTile = northTile;
                    }
                    else if (northTile != null && northTile.GetIsConnected() == false && northTile.GetShape() == PipeTileShape.Corner && northTile.GetRotation() == PipeTileRotation.Degrees270)
                    {
                        connectedPipeTile = northTile;
                    }

                    //Check South
                    else if (soutTile != null && soutTile.GetIsConnected() == false && soutTile.GetShape() == PipeTileShape.Line && (soutTile.GetRotation() == PipeTileRotation.Degrees90 || soutTile.GetRotation() == PipeTileRotation.Degrees270))
                    {
                        connectedPipeTile = soutTile;
                    }
                    else if (soutTile != null && soutTile.GetIsConnected() == false && soutTile.GetShape() == PipeTileShape.Corner && soutTile.GetRotation() == PipeTileRotation.Degrees90)
                    {
                        connectedPipeTile = soutTile;
                    }
                    else if (soutTile != null && soutTile.GetIsConnected() == false && soutTile.GetShape() == PipeTileShape.Corner && soutTile.GetRotation() == PipeTileRotation.Degrees0)
                    {
                        connectedPipeTile = soutTile;
                    }
                }
            }
            else if (shape == PipeTileShape.Corner)
            {
               
                if (rotation == PipeTileRotation.Degrees0)
                {
                    PipeTileScript westTile = GetPipeTileToTheDirection(currentPipeTile, Direction.W);
                    PipeTileScript northTile = GetPipeTileToTheDirection(currentPipeTile, Direction.N);

                    //Check westTile
                    if (westTile != null && westTile.GetIsConnected() == false && westTile.GetShape() == PipeTileShape.Line && (westTile.GetRotation() == PipeTileRotation.Degrees0 || westTile.GetRotation() == PipeTileRotation.Degrees180))
                    {
                        connectedPipeTile = westTile;
                    }
                    else if (westTile != null && westTile.GetIsConnected() == false && westTile.GetShape() == PipeTileShape.Corner && westTile.GetRotation() == PipeTileRotation.Degrees90)
                    {
                        connectedPipeTile = westTile;
                    }
                    else if (westTile != null && westTile.GetIsConnected() == false && westTile.GetShape() == PipeTileShape.Corner && westTile.GetRotation() == PipeTileRotation.Degrees180)
                    {
                        connectedPipeTile = westTile;
                    }

                    //Check northTile
                    else if (northTile != null && northTile.GetIsConnected() == false && northTile.GetShape() == PipeTileShape.Line && (northTile.GetRotation() == PipeTileRotation.Degrees90 || northTile.GetRotation() == PipeTileRotation.Degrees270))
                    {
                        connectedPipeTile = northTile;
                    }
                    else if (northTile != null && northTile.GetIsConnected() == false && northTile.GetShape() == PipeTileShape.Corner && northTile.GetRotation() == PipeTileRotation.Degrees270)
                    {
                        connectedPipeTile = northTile;
                    }
                    else if (northTile != null && northTile.GetIsConnected() == false && northTile.GetShape() == PipeTileShape.Corner && northTile.GetRotation() == PipeTileRotation.Degrees180)
                    {
                        connectedPipeTile = northTile;
                    }
                }
                else if (rotation == PipeTileRotation.Degrees90)
                {
                    PipeTileScript northTile = GetPipeTileToTheDirection(currentPipeTile, Direction.N);
                    PipeTileScript eastTile = GetPipeTileToTheDirection(currentPipeTile, Direction.E);

                    //Check northTile
                    if (northTile != null && northTile.GetIsConnected() == false && northTile.GetShape() == PipeTileShape.Line && (northTile.GetRotation() == PipeTileRotation.Degrees90 || northTile.GetRotation() == PipeTileRotation.Degrees270))
                    {
                        connectedPipeTile = northTile;
                    }
                    else if (northTile != null && northTile.GetIsConnected() == false && northTile.GetShape() == PipeTileShape.Corner && northTile.GetRotation() == PipeTileRotation.Degrees270)
                    {
                        connectedPipeTile = northTile;
                    }
                    else if (northTile != null && northTile.GetIsConnected() == false && northTile.GetShape() == PipeTileShape.Corner && northTile.GetRotation() == PipeTileRotation.Degrees180)
                    {
                        connectedPipeTile = northTile;
                    }

                    //Check westTile
                    else if(eastTile != null && eastTile.GetIsConnected() == false && eastTile.GetShape() == PipeTileShape.Line && (eastTile.GetRotation() == PipeTileRotation.Degrees0 || eastTile.GetRotation() == PipeTileRotation.Degrees180))
                    {
                        connectedPipeTile = eastTile;
                    }
                    else if (eastTile != null && eastTile.GetIsConnected() == false && eastTile.GetShape() == PipeTileShape.Corner && eastTile.GetRotation() == PipeTileRotation.Degrees0)
                    {
                        connectedPipeTile = eastTile;
                    }
                    else if (eastTile != null && eastTile.GetIsConnected() == false && eastTile.GetShape() == PipeTileShape.Corner && eastTile.GetRotation() == PipeTileRotation.Degrees270)
                    {
                        connectedPipeTile = eastTile;
                    }
                }
                else if (rotation == PipeTileRotation.Degrees180)
                {
                    PipeTileScript eastTile = GetPipeTileToTheDirection(currentPipeTile, Direction.E);
                    PipeTileScript southTile = GetPipeTileToTheDirection(currentPipeTile, Direction.S);

                    //Check eastTile
                    if (eastTile != null && eastTile.GetIsConnected() == false && eastTile.GetShape() == PipeTileShape.Line && (eastTile.GetRotation() == PipeTileRotation.Degrees0 || eastTile.GetRotation() == PipeTileRotation.Degrees180))
                    {
                        connectedPipeTile = eastTile;
                    }
                    else if (eastTile != null && eastTile.GetIsConnected() == false && eastTile.GetShape() == PipeTileShape.Corner && eastTile.GetRotation() == PipeTileRotation.Degrees270)
                    {
                        connectedPipeTile = eastTile;
                    }
                    else if (eastTile != null && eastTile.GetIsConnected() == false && eastTile.GetShape() == PipeTileShape.Corner && eastTile.GetRotation() == PipeTileRotation.Degrees0)
                    {
                        connectedPipeTile = eastTile;
                    }

                    //Check southTile
                    else if (southTile != null && southTile.GetIsConnected() == false && southTile.GetShape() == PipeTileShape.Line && (southTile.GetRotation() == PipeTileRotation.Degrees90 || southTile.GetRotation() == PipeTileRotation.Degrees270))
                    {
                        connectedPipeTile = southTile;
                    }
                    else if (southTile != null && southTile.GetIsConnected() == false && southTile.GetShape() == PipeTileShape.Corner && southTile.GetRotation() == PipeTileRotation.Degrees90)
                    {
                        connectedPipeTile = southTile;
                    }
                    else if (southTile != null && southTile.GetIsConnected() == false && southTile.GetShape() == PipeTileShape.Corner && southTile.GetRotation() == PipeTileRotation.Degrees0)
                    {
                        connectedPipeTile = southTile;
                    }
                }
                else if (rotation == PipeTileRotation.Degrees270)
                {
                    PipeTileScript southTile = GetPipeTileToTheDirection(currentPipeTile, Direction.S);
                    PipeTileScript westTile = GetPipeTileToTheDirection(currentPipeTile, Direction.W);

                    //Check southTile
                    if (southTile != null && southTile.GetIsConnected() == false && southTile.GetShape() == PipeTileShape.Line && (southTile.GetRotation() == PipeTileRotation.Degrees90 || southTile.GetRotation() == PipeTileRotation.Degrees270))
                    {
                        connectedPipeTile = southTile;
                    }
                    else if (southTile != null && southTile.GetIsConnected() == false && southTile.GetShape() == PipeTileShape.Corner && southTile.GetRotation() == PipeTileRotation.Degrees90)
                    {
                        connectedPipeTile = southTile;
                    }
                    else if (southTile != null && southTile.GetIsConnected() == false && southTile.GetShape() == PipeTileShape.Corner && southTile.GetRotation() == PipeTileRotation.Degrees0)
                    {
                        connectedPipeTile = southTile;
                    }

                    //Check westTile
                    else if (westTile != null && westTile.GetIsConnected() == false && westTile.GetShape() == PipeTileShape.Line && (westTile.GetRotation() == PipeTileRotation.Degrees0 || westTile.GetRotation() == PipeTileRotation.Degrees180))
                    {
                        connectedPipeTile = westTile;
                    }
                    else if (westTile != null && westTile.GetIsConnected() == false && westTile.GetShape() == PipeTileShape.Corner && westTile.GetRotation() == PipeTileRotation.Degrees90)
                    {
                        connectedPipeTile = westTile;
                    }
                    else if (westTile != null && westTile.GetIsConnected() == false && westTile.GetShape() == PipeTileShape.Corner && westTile.GetRotation() == PipeTileRotation.Degrees180)
                    {
                        connectedPipeTile = westTile;
                    }
                }
            }

            //CHECK VICTORY CONDITION
            if (connectedPipeTile == _endTile)
                isConnected = true;

            //Continue
            currentPipeTile = connectedPipeTile;
        }

        if (isConnected)
            _logger?.Log("FULLY CONNECTED!!!");
        else
            _logger?.Log("NOT FULLY CONNECTED");
    }

    private PipeTileScript GetPipeTileToTheDirection(PipeTileScript pipeTile, Direction direction)
    {
        //_pipeTiles.
        Vector2 pipeTilePosition = pipeTile.GetPosition();

        switch (direction)
        {
            case Direction.N:
                return _pipeTiles.FirstOrDefault(pt => pt.GetPosition().x == pipeTilePosition.x && pt.GetPosition().y == pipeTilePosition.y - 1);
            case Direction.E:
                return _pipeTiles.FirstOrDefault(pt => pt.GetPosition().x == pipeTilePosition.x + 1 && pt.GetPosition().y == pipeTilePosition.y);
            case Direction.S:
                return _pipeTiles.FirstOrDefault(pt => pt.GetPosition().x == pipeTilePosition.x && pt.GetPosition().y == pipeTilePosition.y + 1);
            case Direction.W:
                return _pipeTiles.FirstOrDefault(pt => pt.GetPosition().x == pipeTilePosition.x - 1 && pt.GetPosition().y == pipeTilePosition.y);
            default:
                return null;
        }
    }

    public enum Direction
    {
        N,
        E,
        S,
        W
    }
}
